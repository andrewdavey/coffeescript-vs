using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text;

namespace CoffeeScript.Editor
{
    public class Classifier : IClassifier
    {
        public Classifier(IClassificationTypeRegistryService classificationRegistry)
        {
            this.classificationRegistry = classificationRegistry;
            parser = new Parser();
        }

        readonly IClassificationTypeRegistryService classificationRegistry;
        readonly Parser parser;

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var code = span.Snapshot.GetText();
            
            var tokens = parser.Parse(code);

            return tokens
                .Select(t => ClassifyToken(t, new SnapshotPoint(span.Snapshot, 0)))
                .Where(c => c != null)
                .ToList();
        }

        ClassificationSpan ClassifyToken(Token token, SnapshotPoint start)
        {
            var classificationType = classificationRegistry.GetClassificationType("CoffeeScript." + token.Type);
            
            // We may not have a classification type defined for the token yet.
            if (classificationType == null) return null;

            return new ClassificationSpan(
                new SnapshotSpan(
                    start + token.Index,
                    token.Value.Length
                ),
                classificationType
            );
        }

    }
}
