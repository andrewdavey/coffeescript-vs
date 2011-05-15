using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace CoffeeScript.Editor
{
    [Export(typeof(IClassifierProvider))]
    [ContentType("CoffeeScript")]
    class CoffeeScriptClassifierProvider : IClassifierProvider
    {
        [Import]
        IClassificationTypeRegistryService ClassificationRegistry = null;

        public IClassifier GetClassifier(ITextBuffer buffer)
        {
            return buffer.Properties.GetOrCreateSingletonProperty(
                () => new Classifier(ClassificationRegistry)
            );
        }
    }
}
