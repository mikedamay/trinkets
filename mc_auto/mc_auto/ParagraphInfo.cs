using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ExtensionMethods;

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
        
        private static IDictionary<string, bool> paragraphInfos = new Dictionary<string, bool>()
        {
            {"mc_tech", true}
            ,{"mc_example_text", true}
        };
        private readonly bool _endMarker;
        private readonly WrapperType _wrapperType = default(WrapperType);
        public AttributeDefinition AttributeDef
        {
            get
            {
                if (paragraphInfos.GetValueOrDefault((string)Paragraph.Range.ParagraphStyle.LocalName, false))
                {
                    return new AttributeDefinition("group", true);
                }
                else
                {
                    return null;
                }
            }
        }

        public bool IsEndMarker
        {
            get { return _wrapperType == WrapperType.EndMarker; }
        }
        public bool IsRoot
        {
            get { return _wrapperType == WrapperType.Root; }
        }
        public Microsoft.Office.Interop.Word.Paragraph Paragraph { get; private set; }
        public ParagraphInfo()
        {
        }
        public ParagraphInfo(AttributeDefinition ad)
        {
        }
        public ParagraphInfo(Microsoft.Office.Interop.Word.Paragraph para)
        {
            Start = StartFunc;
            End = EndFunc;
            IsChild = IsChildFunc;
            _endMarker = para != null;
            Paragraph = para;
        }
        public ParagraphInfo(WrapperType wrapperType)
        {
            Start = StartFunc;
            End = EndFunc;
            IsChild = IsChildFunc;
            _wrapperType = wrapperType;
        }
        public delegate void Starter(System.Xml.XmlWriter xw);
        public readonly Starter Start;
        public delegate void Ender(System.Xml.XmlWriter xw);
        public readonly Ender End;
        public delegate bool IsChilder(ParagraphInfo pi);
        public readonly IsChilder IsChild;

        public void StartFunc(System.Xml.XmlWriter xw)
        {
            if (IsRoot) return;
            xw.WriteStartElement("p");
            xw.WriteString(MassageXMLString(Paragraph.Range.Text));
        }
        public void EndFunc(System.Xml.XmlWriter xw)
        {
            if (IsRoot) return;
            xw.WriteEndElement();
        }

        public bool IsChildFunc(ParagraphInfo child)
        {
            if (IsRoot)
            {
                return true;
            }
            else
            {
                return false;
            }
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
                if (System.Char.IsControl(text[ii]))
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
    internal class AttributeDefinition
    {
        public string Name { get; private set; }
        public object Value { get; private set; }
        public AttributeDefinition(string nm, object vl)
        {
            Name = nm;
            Value = vl;
        }
    }
}
