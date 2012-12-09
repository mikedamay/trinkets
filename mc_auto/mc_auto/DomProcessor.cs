
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
            ProcessPara( ParagraphInfo.Create(ParagraphInfo.WrapperType.Root));
        }

        private void ProcessPara(ParagraphInfo parent)
        {
            parent.Start(_xmlWriter);
            _domIter.MoveNext();
            while (IsChild(parent, _domIter.Current))
            {
                ProcessPara(_domIter.Current);
            }
            parent.End(_xmlWriter);
        }

        private bool IsChild(ParagraphInfo parent, ParagraphInfo child)
        {
            return !child.IsEndMarker && parent.IsChild(child);
        }
    }
}
