using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using CoffeeScript.Editor.Properties;
using Jurassic;
using Jurassic.Library;

namespace CoffeeScript.Editor
{
    public class Parser
    {
        readonly ScriptEngine engine;
        readonly HashSet<string> ignoreTokens = new HashSet<string>(new[] {
            "INDENT", "OUTDENT", "{", "}", "TERMINATOR", "CALL_START", "CALL_END", "(", ")"
        });

        readonly Dictionary<string, string[]> aliases = new Dictionary<string, string[]>
        {
            {"==", new[] { "is" }},
            {"!=", new[] { "isnt" }},
            {"===", new[] { "is"}},
            {"!==", new[] { "isnt"}},
            {"||", new[] { "or"} },
            {"&&", new[] { "and" }},
            {"!", new[] { "not" }},
            {"true", new[] { "yes", "on" }},
            {"false", new[] { "no", "off"}}
        };

        public Parser()
        {
            engine = CreateScriptEngineWithCoffeeScriptLoaded();
        }

        public IEnumerable<Token> Parse(string sourceCode)
        {
            var result = engine.Evaluate<ObjectInstance>(
                "(function() { try { return CoffeeScript.tokens(" + EscapeJavaScriptString(sourceCode) + ", {rewrite:false}); } catch (e) { return []; } })()"
            );
            var length = TypeConverter.ToInt32(result.GetPropertyValue("length"));
            
            // We need the index of each token within the source code.
            // So keep track of the current index in this variable.
            var sourceCodeIndex = 0;

            var commentRegex = new Regex(@"(#.*?)([\r\n]|$)");
            var previousType = "";

            for (uint i = 0; i < length; i++)
            {
                var tokenData = (ObjectInstance)result.GetPropertyValue(i);
                var type = TypeConverter.ToString(tokenData.GetPropertyValue(0));

                if (ignoreTokens.Contains(type))
                {
                    continue; // Skip tokens that aren't visible
                }

                var value = TypeConverter.ToString(tokenData.GetPropertyValue(1));
                int index;
                if (type == "HERECOMMENT")
                {
                    var hereCommentTuple = ParseHereComment(value, sourceCodeIndex, sourceCode);
                    value = hereCommentTuple.Item1;
                    index = hereCommentTuple.Item2;
                }
                else if (type == "STRING")
                {
                    index = sourceCode.IndexOf(value, sourceCodeIndex);
                    if (index == -1) // Original string is an interplation e.g. "Hello #{x}"
                    {
                        // Find the full original string in source.
                        var valueWithoutClosingQuote = value.Substring(0, value.Length - 1);
                        index = sourceCode.IndexOf(valueWithoutClosingQuote, sourceCodeIndex);
                        var endIndex = index + 1;
                        var nestLevel = 0;
                        var interpolationCount = 0;
                        while (sourceCode[endIndex] != '"' || nestLevel > 0)
                        {
                            endIndex++;
                            if (sourceCode[endIndex] == '#' && (endIndex < sourceCode.Length - 1) && sourceCode[endIndex + 1] == '{')
                            {
                                nestLevel++;
                                interpolationCount++;
                            }
                            if (sourceCode[endIndex] == '}')
                            {
                                nestLevel--;
                            }
                        }

                        value = sourceCode.Substring(index, endIndex - index + 1);

                        // Need to skip the tokens that form the string interpolation.
                        nestLevel = 0;
                        for (; nestLevel >= 0 && i < length; i++)
                        {
                            var innerTokenData = (ObjectInstance)result.GetPropertyValue(i);
                            var innerType = TypeConverter.ToString(innerTokenData.GetPropertyValue(0));
                            if (innerType == "(") nestLevel++;
                            else if (innerType == ")") nestLevel--;
                        }
                    }
                }
                else
                {
                    var valuesToTry = new List<string>();
                    valuesToTry.Add(value);
                    string[] options;
                    if (aliases.TryGetValue(value, out options))
                    {
                        valuesToTry.AddRange(options);
                    }

                    var closest = valuesToTry.Select(v => new { v, index = sourceCode.IndexOf(v, sourceCodeIndex) })
                        .Where(x => x.index >= 0)
                        .OrderBy(x => x.index)
                        .FirstOrDefault();
                    if (closest == null) throw new Exception("Cannot find '" + value + "' or an alias in source.");
                    value = closest.v;
                    index = closest.index;
                }

                if (type == "IDENTIFIER" && previousType == "@")
                {
                    type = "@";
                }

                // Check for comments between previous token and this token
                var sourceCodeBetweenTokens = sourceCode.Substring(sourceCodeIndex, index - sourceCodeIndex);
                var commentMatches = commentRegex.Matches(sourceCodeBetweenTokens);
                foreach (Match match in commentMatches)
                {
                    yield return new Token
                    {
                        Type = "COMMENT",
                        Value = match.Groups[1].Value,
                        Index = sourceCodeIndex + match.Index
                    };
                }

                yield return new Token { Type = type, Value = value, Index = index };

                sourceCodeIndex = index + value.Length;
                previousType = type;
            }

            // Check for comment at end of file. (CoffeeScript discards single line comments when tokenizing).
            var matches = commentRegex.Matches(sourceCode.Substring(sourceCodeIndex));
            foreach (Match match in matches)
            {
                yield return new Token 
                {
                    Type = "COMMENT",
                    Value = match.Groups[1].Value, 
                    Index = sourceCodeIndex + match.Index 
                };
            }
        }

        Tuple<string, int> ParseHereComment(string value, int sourceCodeIndex, string sourceCode)
        {
            // The value is just the comment text (with newline char wrapper).
            // CoffeeScript tokenizer converts any \r\n between ### and the
            // comment text into just \n.
            // We want to include the '###'s in our returned token.

            // Find the comment text within the source code.
            var commentText = value.Trim('\n');
            var index = sourceCode.IndexOf(commentText, sourceCodeIndex);
            // Step backwards through source code until we've seen ###.
            int count = 0;
            int offset = 0;
            do
            {
                index--;
                offset++;
                if (sourceCode[index] == '#') count++;
            } while (count < 3);
            // Step forwards through source code until we've seen ###.
            count = 0;
            var endIndex = index + offset + commentText.Length;
            while (count < 3 && endIndex < sourceCode.Length)
            {
                if (sourceCode[endIndex] == '#') count++;
                endIndex++;
            }
            if (endIndex < sourceCode.Length)
            {
                value = sourceCode.Substring(index, endIndex - index + 1);
            }
            else
            {
                value = sourceCode.Substring(index, endIndex - index);
            }

            return Tuple.Create(value, index);
        }

        string EscapeJavaScriptString(string sourceCode)
        {
            return "'" + 
                   sourceCode
                       .Replace(@"\", @"\\")
                       .Replace("'", @"\'")
                       .Replace("\r", "\\r")
                       .Replace("\n", "\\n") + 
                   "'";
        }

        ScriptEngine CreateScriptEngineWithCoffeeScriptLoaded()
        {
            var engine = new ScriptEngine();
            engine.Execute(Resources.coffeescript);
            return engine;
        }
    }
}
