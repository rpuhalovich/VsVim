﻿using EditorUtils;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.Text;
using System.Collections.ObjectModel;
using Microsoft.VisualStudio.Text.Classification;

namespace Vim.UI.Wpf.Implementation.Directory
{
    internal sealed class DirectoryTagger : IBasicTaggerSource<IClassificationTag>
    {
        private readonly ITextBuffer _textBuffer;
        private readonly IClassificationTag _classificationTag;
        private EventHandler _changed;

        internal DirectoryTagger(ITextBuffer textBuffer, IClassificationType classificationType)
        {
            _textBuffer = textBuffer;
            _classificationTag = new ClassificationTag(classificationType);
        }

        private ReadOnlyCollection<ITagSpan<IClassificationTag>> GetTags(SnapshotSpan span)
        {
            var lineSpan = SnapshotLineRangeUtil.CreateForSpan(span);
            var snapshot = span.Snapshot;
            var list = new List<ITagSpan<IClassificationTag>>();
            foreach (var line in lineSpan.Lines)
            {
                if (line.Length == 0)
                {
                    continue;
                }

                if (snapshot[line.End.Position - 1] == '/')
                {
                    var directorySpan = new SnapshotSpan(line.Start, line.Length - 1);
                    list.Add(new TagSpan<IClassificationTag>(directorySpan, _classificationTag));
                }
            }

            return list.ToReadOnlyCollectionShallow();
        }

        #region IBasicTaggerSource<TextMarkerTag>

        ITextSnapshot IBasicTaggerSource<IClassificationTag>.TextSnapshot
        {
            get { return _textBuffer.CurrentSnapshot; }
        }

        event EventHandler IBasicTaggerSource<IClassificationTag>.Changed
        {
            add { _changed += value; }
            remove { _changed -= value; }
        }

        ReadOnlyCollection<ITagSpan<IClassificationTag>> IBasicTaggerSource<IClassificationTag>.GetTags(SnapshotSpan span)
        {
            return GetTags(span);
        }

        #endregion
    }
}
