using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace CoffeeScript.Editor
{
    static class ClassificationFormats
    {
        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.KEYWORD")]
        [Name("coffeescript.KEYWORD")]
        sealed class CoffeeScriptKeywordFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptKeywordFormat() { ForegroundColor = Colors.Blue; }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.OPERATOR")]
        [Name("coffeescript.OPERATOR")]
        sealed class CoffeeScriptOperatorFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptOperatorFormat() { }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.UNARY")]
        [Name("coffeescript.UNARY")]
        sealed class CoffeeScriptUnaryFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptUnaryFormat() { ForegroundColor = Colors.Blue; }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.COMMENT")]
        [Name("coffeescript.COMMENT")]
        sealed class CoffeeScriptCommentFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptCommentFormat() { ForegroundColor = Colors.DarkGreen; }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.STRING")]
        [Name("coffeescript.STRING")]
        sealed class CoffeeScriptStringFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptStringFormat() { ForegroundColor = Color.FromRgb(163, 21, 21); }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.NUMBER")]
        [Name("coffeescript.NUMBER")]
        sealed class CoffeeScriptNumberFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptNumberFormat() { }
        }

        [Export(typeof(EditorFormatDefinition))]
        [ClassificationType(ClassificationTypeNames = "coffeescript.@")]
        [Name("coffeescript.@")]
        sealed class CoffeeScriptAtFormat : ClassificationFormatDefinition
        {
            public CoffeeScriptAtFormat() { ForegroundColor = Colors.Purple; }
        }
    }
}
