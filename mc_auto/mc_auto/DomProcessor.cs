
using System.Collections.Generic;
using Word = Microsoft.Office.Interop.Word;

namespace mc_auto
{
    internal class DomProcessor
    {
        private class ProcessorState
        {
            public ProcessorState(ParagraphInfo parent, ParagraphInfo current)
            {
                this.Parent = parent;
                this.Current = current;
            }
            public readonly ParagraphInfo Parent;
            public readonly ParagraphInfo Current;
        }
        private Stack<ProcessorState> stack = new Stack<ProcessorState>();
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
                    stack.Push(new ProcessorState(parent, null));
                    parent = current;
                    _domIter.MoveNext();
                    current = _domIter.Current;
                }
                else
                {
                    parent.End(_xmlWriter);
                    if (stack.Count == 0)
                        break;
                    ProcessorState ps = stack.Pop();
                    parent = ps.Parent;
                }
            }
        }

        // recursive version takes 13/8 times as long
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
