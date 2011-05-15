using System.Linq;
using Xunit;
using System.IO;

namespace CoffeeScript.Editor.Tests
{
    public class ParserTests : IUseFixture<Parser>
    {
        // Create Parser is slow because it loads a JavaScript CoffeeScript compiler!
        // So using SetFixture means we only create it once and reuse the instance in
        // all tests.
        Parser parser;
        public void SetFixture(Parser parser)
        {
            this.parser = parser;
        }

        void AssertToken(Token token, string type, string value, int index)
        {
            Assert.Equal(type, token.Type);
            Assert.Equal(value, token.Value);
            Assert.Equal(index, token.Index);
        }

        [Fact]
        public void Can_parse_string()
        {
            
            var tokens = parser.Parse(@"s = 'Hello, \'World\''").ToArray();
            AssertToken(tokens[0], "IDENTIFIER", "s", 0);
            AssertToken(tokens[1], "=", "=", 2);
            AssertToken(tokens[2], "STRING", @"'Hello, \'World\''", 4);
        }

        [Fact]
        public void Can_parse_simple_expression()
        {
            
            var tokens = parser.Parse("1 + 2").ToArray();

            AssertToken(tokens[0], "NUMBER", "1", 0);
            AssertToken(tokens[1], "+", "+", 2);
            AssertToken(tokens[2], "NUMBER", "2", 4);
        }

        [Fact]
        public void Can_parse_simple_expression_with_same_number_twice()
        {
            
            var tokens = parser.Parse("1 + 1").ToArray();

            AssertToken(tokens[0], "NUMBER", "1", 0);
            AssertToken(tokens[1], "+", "+", 2);
            AssertToken(tokens[2], "NUMBER", "1", 4);
        }

        [Fact]
        public void Can_parse_two_lines_of_code()
        {
            
            var tokens = parser.Parse("x = 1 + 1\ny = 2 + 2").ToArray();

            AssertToken(tokens[0], "IDENTIFIER", "x", 0);
            AssertToken(tokens[1], "=", "=", 2);
            AssertToken(tokens[2], "NUMBER", "1", 4);
            AssertToken(tokens[3], "+", "+", 6);
            AssertToken(tokens[4], "NUMBER", "1", 8);

            AssertToken(tokens[5], "IDENTIFIER", "y", 10);
            AssertToken(tokens[6], "=", "=", 12);
            AssertToken(tokens[7], "NUMBER", "2", 14);
            AssertToken(tokens[8], "+", "+", 16);
            AssertToken(tokens[9], "NUMBER", "2", 18);
        }

        [Fact]
        public void Can_parse_two_lines_of_code_with_carriage_return_line_feed()
        {
            
            var tokens = parser.Parse("x = 1 + 1\r\ny = 2 + 2").ToArray();

            AssertToken(tokens[0], "IDENTIFIER", "x", 0);
            AssertToken(tokens[1], "=", "=", 2);
            AssertToken(tokens[2], "NUMBER", "1", 4);
            AssertToken(tokens[3], "+", "+", 6);
            AssertToken(tokens[4], "NUMBER", "1", 8);

            AssertToken(tokens[5], "IDENTIFIER", "y", 11);
            AssertToken(tokens[6], "=", "=", 13);
            AssertToken(tokens[7], "NUMBER", "2", 15);
            AssertToken(tokens[8], "+", "+", 17);
            AssertToken(tokens[9], "NUMBER", "2", 19);
        }

        [Fact]
        public void Can_parse_object_literal()
        {
            
            var tokens = parser.Parse("obj =\n  a: 1\n  b: 2").ToArray();

            AssertToken(tokens[0], "IDENTIFIER", "obj", 0);
            AssertToken(tokens[1], "=", "=", 4);
            AssertToken(tokens[2], "IDENTIFIER", "a", 8);
            AssertToken(tokens[3], ":", ":", 9);
            AssertToken(tokens[4], "NUMBER", "1", 11);
            AssertToken(tokens[5], "IDENTIFIER", "b", 15);
            AssertToken(tokens[6], ":", ":", 16);
            AssertToken(tokens[7], "NUMBER", "2", 18);

            Assert.True(8 == tokens.Length, "Too many tokens returned.");
        }

        [Fact]
        public void Can_parse_comment()
        {
            
            var tokens = parser.Parse("# a comment").ToArray();
            AssertToken(tokens[0], "COMMENT", "# a comment", 0);
        }

        [Fact]
        public void Can_parse_comment_after_string_containing_hash()
        {
            
            var tokens = parser.Parse("x = 'hello #' # a comment").ToArray();
            AssertToken(tokens[3], "COMMENT", "# a comment", 14);
        }

        [Fact]
        public void Can_parse_comment_between_other_tokens()
        {
            
            var tokens = parser.Parse("x = 'hello x' # a comment\ny = 1").ToArray();
            AssertToken(tokens[3], "COMMENT", "# a comment", 14);
            AssertToken(tokens[4], "IDENTIFIER", "y", 26);
        }

        [Fact]
        public void Can_parse_comment_between_other_tokens_using_carriage_return_line_feed()
        {
            
            var tokens = parser.Parse("x = 'hello x' # a comment\r\ny = 1").ToArray();
            AssertToken(tokens[3], "COMMENT", "# a comment", 14);
            AssertToken(tokens[4], "IDENTIFIER", "y", 27);
        }

        [Fact]
        public void Can_parse_herecomment()
        {
            
            var tokens = parser.Parse("###\nmy comment\n###").ToArray();
            AssertToken(tokens[0], "HERECOMMENT", "###\nmy comment\n###", 0);
        }

        [Fact]
        public void Can_parse_herecomment_with_carriage_return_line_feed()
        {
            
            var tokens = parser.Parse("###\r\nmy comment\r\n###").ToArray();
            AssertToken(tokens[0], "HERECOMMENT", "###\r\nmy comment\r\n###", 0);
        }

        [Fact]
        public void Can_parse_unfinished_herecomment()
        {
            var tokens = parser.Parse("x=3\n###\nmy comment").ToArray();
            AssertToken(tokens[3], "HERECOMMENT", "###\nmy comment", 4);
        }

        [Fact]
        public void Can_parse_function_call()
        {
            var tokens = parser.Parse("alert 'hello'").ToArray();
            AssertToken(tokens[0], "IDENTIFIER", "alert", 0);
            AssertToken(tokens[1], "STRING", "'hello'", 6);
        }

        [Fact]
        public void Can_parse_function_call_with_parenthesis()
        {
            var tokens = parser.Parse("alert('hello')").ToArray();
            AssertToken(tokens[0], "IDENTIFIER", "alert", 0);
            AssertToken(tokens[1], "STRING", "'hello'", 6);
        }

        [Fact]
        public void Can_parse_string_interpolation()
        {
            var tokens = parser.Parse("x = \"hello #{foo} test\"").ToArray();
            AssertToken(tokens[2], "STRING", "\"hello #{foo} test\"", 4);
        }

        [Fact]
        public void Can_parse_this_property_using_at_syntax()
        {
            var tokens = parser.Parse("@foo = 1").ToArray();
            AssertToken(tokens[0], "@", "@", 0);
            AssertToken(tokens[1], "@", "foo", 1);
        }

        [Fact]
        public void Can_parse_boolean_literal()
        {
            var tokens = parser.Parse("x = yes").ToArray();
            AssertToken(tokens[2], "BOOL", "yes", 4);
        }

        [Fact]
        public void Can_parse_return_statement()
        {
            var tokens = parser.Parse("->\n  return 1").ToArray();
            AssertToken(tokens[1], "RETURN", "return", 5);
        }

        [Fact]
        public void Can_parse_new()
        {
            var tokens = parser.Parse("new Test()").ToArray();
            AssertToken(tokens[0], "UNARY", "new", 0);
        }

        [Fact]
        public void Can_parse_heregex()
        {
            var tokens = parser.Parse("/// \\n #{indent} ///g").ToArray();
        }

        [Fact]
        public void Lexer_problem()
        {
            var code = "\n    if id in JS_KEYWORDS or\n       not forcedIdentifier and id in COFFEE_KEYWORDS\n      tag = id.toUpperCase()\n      if tag is 'WHEN' and @tag() in LINE_BREAK\n        tag = 'LEADING_WHEN'\n      else if tag is 'FOR'\n        @seenFor = yes\n      else if tag is 'UNLESS'\n        tag = 'IF'\n      else if tag in UNARY\n        tag = 'UNARY'\n      else if tag in RELATION\n        if tag isnt 'INSTANCEOF' and @seenFor\n          tag = 'FOR' + tag\n          @seenFor = no\n        else\n          tag = 'RELATION'\n          if @value() is '!'\n            @tokens.pop()\n            id = '!' + id\n";
            parser.Parse(code).ToArray();
        }

        [Fact]
        public void Can_parse_coffee_script_source()
        {
            var source = File.ReadAllText(@"..\..\coffee-script.coffee");
            parser.Parse(source).ToArray();
        }
    }
}
