using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Utilities;

namespace CoffeeScript.Editor
{
    static class CoffeeScriptContentType
    {
        [Export]
        [Name("CoffeeScript")]
        [DisplayName("CoffeeScript")]
        [BaseDefinition("text")]
        public static ContentTypeDefinition ContentType = null;

        [Export]
        [ContentType("CoffeeScript")]
        [FileExtension(".coffee")]
        public static FileExtensionToContentTypeDefinition CoffeeFileExtension = null;
    }
}
