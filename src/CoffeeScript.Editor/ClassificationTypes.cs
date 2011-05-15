using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace CoffeeScript.Editor
{
    static class ClassificationTypes
    {
        [Export]
        [Name("CoffeeScript")]
        internal static ClassificationTypeDefinition CoffeeScript = null;

        [Export]
        [Name("CoffeeScript.KEYWORD")]
        [DisplayName("Keyword")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition Keyword = null;

        [Export]
        [Name("CoffeeScript.OPERATOR")]
        [DisplayName("Keyword")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition Operator = null;

        [Export]
        [Name("CoffeeScript.UNARY")]
        [DisplayName("Keyword")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Unary = null;

        [Export]
        [Name("CoffeeScript.COMMENT")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition Comment = null;

        [Export]
        [Name("CoffeeScript.STRING")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition String = null;

        [Export]
        [Name("CoffeeScript.NUMBER")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition Number = null;
        

        [Export]
        [Name("CoffeeScript.BOOL")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Bool = null;

        [Export]
        [Name("CoffeeScript.RETURN")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Return = null;

        [Export]
        [Name("CoffeeScript.FOR")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition For = null;

        [Export]
        [Name("CoffeeScript.IF")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition If = null;

        [Export]
        [Name("CoffeeScript.ELSE")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Else = null;

        [Export]
        [Name("CoffeeScript.SWITCH")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Switch = null;

        [Export]
        [Name("CoffeeScript.WHEN")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition When = null;

        [Export]
        [Name("CoffeeScript.THEN")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Then = null;

        [Export]
        [Name("CoffeeScript.OF")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Of = null;

        [Export]
        [Name("CoffeeScript.WHILE")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition While = null;

        [Export]
        [Name("CoffeeScript.NULL")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Null = null;

        [Export]
        [Name("CoffeeScript.TRY")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Try = null;

        [Export]
        [Name("CoffeeScript.CATCH")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Catch = null;

        [Export]
        [Name("CoffeeScript.FINALLY")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Finally = null;

        [Export]
        [Name("CoffeeScript.NEW")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition New = null;

        [Export]
        [Name("CoffeeScript.TYPEOF")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition TypeOf = null;

        [Export]
        [Name("CoffeeScript.DELETE")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Delete = null;

        [Export]
        [Name("CoffeeScript.DO")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Do = null;


        [Export]
        [Name("CoffeeScript.=")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Assign = null;
        [Export]
        [Name("CoffeeScript.->")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Lambda = null;
        [Export]
        [Name("CoffeeScript.MATH")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Math = null;
        [Export]
        [Name("CoffeeScript.LOGIC")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Logic = null;
        [Export]
        [Name("CoffeeScript.COMPARE")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Compare = null;
        [Export]
        [Name("CoffeeScript.COMPOUND_ASSIGN")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition CompoundAssign = null;
        [Export]
        [Name("CoffeeScript.RELATION")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Relation = null;
        [Export]
        [Name("CoffeeScript.SHIFT")]
        [BaseDefinition("CoffeeScript.OPERATOR")]
        internal static ClassificationTypeDefinition Shift = null;


        [Export]
        [Name("CoffeeScript.@")]
        [BaseDefinition("CoffeeScript")]
        internal static ClassificationTypeDefinition At = null;

        [Export]
        [Name("CoffeeScript.CLASS")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Class = null;

        [Export]
        [Name("CoffeeScript.SUPER")]
        [BaseDefinition("CoffeeScript.KEYWORD")]
        internal static ClassificationTypeDefinition Super = null;
    }
}
