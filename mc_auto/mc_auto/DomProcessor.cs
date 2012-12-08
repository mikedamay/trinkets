
using Word = Microsoft.Office.Interop.Word;

namespace mc_auto
{
    internal class DomProcessor
    {
        private readonly DomIter _domIter;
        private readonly System.Xml.XmlWriter _xmlWriter;
        public DomProcessor(Word.Range document, System.Xml.XmlWriter writer)
        {
            _xmlWriter = writer;
            _domIter = new DomIter(document);
        }
        public void Process()
        {
            ProcessPara();
        }

        private void ProcessPara()
        {
            ParagraphInfo pi = _domIter.Current;
            _domIter.Current.Start(_xmlWriter);
            _domIter.MoveNext();
            while (IsChild(pi, _domIter.Current))
            {
                _domIter.MoveNext();
                Process();
            }
            _domIter.Current.End(_xmlWriter);
        }

        private bool IsChild(ParagraphInfo parent, ParagraphInfo child)
        {
            return !child.IsEndMarker && parent.IsChild(child);
        }
    }
}
