using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;

namespace mc_auto
{

    internal class ParagraphInfo
    {
        public enum WrapperType
        {
            None
            ,EndMarker
            ,Root
        }
        private enum Grouped
        {
            NotGrouped
            ,FirstInGroup
            , GroupSibling
        }
        public static ParagraphInfo Create(Word.Paragraph para)
        {
            return new ParagraphInfo(para);
        }
        public static ParagraphInfo Create(WrapperType wt)
        {
            return new ParagraphInfo(wt);
        }
        private static IDictionary<string, bool> paragraphInfos = new Dictionary<string, bool>()
        {
            {"mc_tech", true}
            ,{"mc_example_text", true}
        };

        private readonly WrapperType _wrapperType = default(WrapperType);

        public bool IsEndMarker
        {
            get { return _wrapperType == WrapperType.EndMarker; }
        }
        public bool IsRoot
        {
            get { return _wrapperType == WrapperType.Root; }
        }
        public int Level {get { return GetLevel(); }}
        public Word.Paragraph Paragraph { get; private set; }

        private ParagraphInfo(Word.Paragraph para)
          : this(para, WrapperType.None )
        {
        }
        private ParagraphInfo(WrapperType wrapperType)
          : this(null, wrapperType)
        {
        }
        public delegate void Starter(System.Xml.XmlWriter xw);
        public readonly Starter Start;
        public delegate void Ender(System.Xml.XmlWriter xw);
        public readonly Ender End;
        public delegate bool IsChilder(ParagraphInfo pi);
        public readonly IsChilder IsChild;

        private delegate int GetLeveler();
        private readonly GetLeveler GetLevel;

        private const int BODY_LEVEL = 10;

        private ParagraphInfo(Word.Paragraph para, WrapperType wt)
        {
            Paragraph = para;
            _wrapperType = wt;
            if (wt == WrapperType.EndMarker || wt == WrapperType.Root)
            {
                Start = (xw) => { };
                End = (xw) => { };
                if (wt == WrapperType.Root)
                {
                    IsChild = pi => true;
                    GetLevel = () => int.MinValue;
                }
                if (wt == WrapperType.EndMarker)
                {
                    IsChild = pi => false;
                    GetLevel = () => int.MinValue;
                }
                return;
            }
            var parentLevel = (int) para.OutlineLevel;
            if (parentLevel < BODY_LEVEL)
            {
                Start = (xw) =>
                {
                    xw.WriteStartElement("p");
                    xw.WriteStartElement("p");
                    xw.WriteAttributeString("level", this.Level.ToString());
                    xw.WriteString(MassageXMLString(para.Range.Text));
                    xw.WriteEndElement();
                };
            }
            else
            {
                Start = (xw) =>
                {
                    xw.WriteStartElement("p");
                    xw.WriteAttributeString("level", this.Level.ToString());
                    xw.WriteString(MassageXMLString(para.Range.Text));
                };
            }
            End = xw => xw.WriteEndElement();
            GetLevel = () => (int)para.OutlineLevel;
            IsChild = child => parentLevel < (int)child.Level;
        }
        /// <summary>
        /// strips unsightly control characters off the end of paragraphs
        /// </summary>
        /// <param name="text">typically Paragraph.Text from Word</param>
        /// <returns>text minus trailing control characters</returns>
        private static string MassageXMLString(string text)
        {
            int reduceBy = 0;
            for (int ii = text.Length - 1; ii >= 0; ii--)
            {
                if (Char.IsControl(text[ii]))
                {
                    reduceBy++;
                }
                else
                {
                    break;
                }
            }
            return text.Substring(0, text.Length - reduceBy);
        }
    }
}
