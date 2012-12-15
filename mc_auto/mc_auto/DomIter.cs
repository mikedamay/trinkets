using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace mc_auto
{
    /// <summary>
    /// usage: 
    ///     doSomethingWith(domIter.Current);
    ///     domIter.MoveNext();
    ///     if (domIter.Current.IsEndMarker)
    ///         exit();
    /// 
    /// The iterator is not meant to drive a loop
    /// </summary>
    internal class DomIter
    {
        private readonly Word.Range _document;
        private readonly int _count;

        private int _index;

        public DomIter(Word.Range doc)
        {
            if (doc.Paragraphs.Count == 0)
            {
                throw new InvalidOperationException("Don't know how to handle a document with no paragraphs");
            }
            _count = doc.Paragraphs.Count;
            _document = doc;
            _index = 1;
        }
        public void Reset()
        {
            _index = 1;
        }
        public void MoveNext()
        {
            _index++;
            if (_index > _count + 2)
            {
                throw new InvalidOperationException("Attempt to move iterator after end of document is invalid");
            }
        }
        public void MovePrevious()
        {
            _index--;
            if (_index < 1)
            {
                throw new InvalidOperationException("Attempt to move iterator before start of document is invalid");
            }
        }
        public ParagraphInfo Current
        {
            get
            {
                if (_index > _count + 1 || _index == 0)
                {
                    System.Diagnostics.Debug.Assert(false
                      ,string.Format( "DomIter.Current outside bounds: _index = {0}, _count = {1}"
                      ,_index, _count));
                }
                if (_index > _count)
                {
                    return ParagraphInfo.Create(ParagraphInfo.WrapperType.EndMarker);
                }
                var pi = ParagraphInfo.Create(_document.Paragraphs[_index], _index > 1 ? _document.Paragraphs[_index - 1] : null);
                return pi;
            }
        }

    }
}
