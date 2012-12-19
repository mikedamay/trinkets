
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;

namespace mc_auto
{
    /// <summary>
    /// call domProcessor.Process() to iterate through the range from a word document and write as XML
    /// </summary>
    internal class DomProcessor
    {
        private readonly Stack<ParagraphInfo> _stack = new Stack<ParagraphInfo>();
        private readonly DomIter _domIter;
        private readonly System.Xml.XmlWriter _xmlWriter;
        public DomProcessor(Word.Range document, System.Xml.XmlWriter writer)
        {
            _xmlWriter = writer;
            _domIter = new DomIter(document);
        }
        public void Process()
        {
            ProcessPara( ParagraphInfo.Create(ParagraphInfo.WrapperType.Root));
        }

        private void ProcessPara(ParagraphInfo startPI)
        {
            ParagraphInfo parent = startPI;
            _domIter.MoveNext();
            ParagraphInfo current = _domIter.Current;
            while (true)
            {
                if (IsChild(parent, current))
                {
                    current.Start(_xmlWriter);
                    _stack.Push(parent);
                    parent = current;
                    _domIter.MoveNext();
                    current = _domIter.Current;
                }
                else
                {
                    parent.End(_xmlWriter);
                    if (_stack.Count == 0)
                    {
                        System.Diagnostics.Debug.Assert(current.IsEndMarker);
                        break;
                    }
                    parent = _stack.Pop();
                }
            }
        }

        // recursive version takes 13/8 times as long as stack version above
        private void ProcessParaRecursive(ParagraphInfo parent)
        {
            ParagraphInfo current;
            parent.Start(_xmlWriter);
            _domIter.MoveNext();
            while (IsChild(parent, (current = _domIter.Current)))
            {
                ProcessPara(current);
            }
            parent.End(_xmlWriter);
        }

        private bool IsChild(ParagraphInfo parent, ParagraphInfo child)
        {
            return !child.IsEndMarker && parent.IsChild(child);
        }
    }
}
