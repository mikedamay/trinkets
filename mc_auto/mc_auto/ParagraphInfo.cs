using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using ExtensionMethods;

namespace mc_auto
{
    internal class ParagraphInfo
    {
        private class Strategy
        {
            internal static readonly Strategy Default = new Strategy(
                    st: (xw, pi) =>
                    {
                        xw.WriteStartElement("p");
                        xw.WriteAttributeString("level", pi.Level.ToString());
                        xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
                    }

                    , en: (xw,pi) => xw.WriteEndElement()
                    , isc: (child,parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level
                    , gl: (pi) => (int)pi.Paragraph.OutlineLevel
            );
            public Strategy(Starter2 st, Ender2 en, IsChilder2 isc, GetLeveler2 gl)
            {
                DoStart = st;
                DoEnd = en;
                DoIsChild = isc;
                DoGetLevel = gl;
            }
            internal readonly Starter2 DoStart;
            internal readonly Ender2 DoEnd;
            internal readonly IsChilder2 DoIsChild;
            internal readonly GetLeveler2 DoGetLevel;
        }
        private class StrategyKey
        {
            private static readonly IDictionary<string, bool> paragraphInfos = new Dictionary<string, bool>()
            {
                {"mc_tech", true}
                ,{"mc_example_text", true}
            };


            private int hash;

            public StrategyKey(WrapperType wt) : this(wt, 0, string.Empty, string.Empty)
            {                
            }
            public StrategyKey(int outlineLevel)
                : this(WrapperType.None, outlineLevel, string.Empty, string.Empty)
            {
            }
            public StrategyKey(string style, string previousStyle)
                : this(WrapperType.None, 0, style, previousStyle)
            {

            }
            public StrategyKey(WrapperType wt, int outlineLevel, string style, string previousStyle)
            {
                hash = Hash(wt) > 0 ? Hash(wt) 
                  : Hash(outlineLevel) > 0 ? Hash(outlineLevel) 
                  : Hash(style, previousStyle);
            }

            private int Hash(string style, string previousStyle)
            {
                int st = (paragraphInfos.GetValueOrDefault(style, false) ? 0x1 : 0x0) << 3;
                int st2 = (paragraphInfos.GetValueOrDefault(previousStyle, false) ? 0x1 : 0x0) << 4;
                return st | st2;
            }

            private int Hash(int outlineLevel)
            {
                return (outlineLevel < BODY_LEVEL ? 0x1 : 0x0) << 2;
            }

            private int Hash(WrapperType wt)
            {
                return wt == WrapperType.EndMarker ? 0x01 : wt == WrapperType.Root ? (0x1<<1) : 0x00;
            }
            // override object.Equals
            public override bool Equals (object obj)
            {
                if (obj == null || GetType() != obj.GetType()) 
                {
                    return false;
                }
        
                return hash == (obj as StrategyKey).hash; 
            }
    
            // override object.GetHashCode
            public override int GetHashCode()
            {
                return hash;
            }
        }
        private delegate void Starter2(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate void Ender2(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate bool IsChilder2(ParagraphInfo pi, ParagraphInfo piParent);
        private delegate int GetLeveler2(ParagraphInfo pi);

        private static IDictionary<StrategyKey, Strategy> _strategyMap 
          = new Dictionary<StrategyKey, Strategy>()
            {
                {new StrategyKey(WrapperType.Root)
                  , new Strategy(st: (xw,pi) => { }, en: (xw,pi) => { }, isc: (p,pp) => true, gl: pi => int.MinValue )
                }
                ,{new StrategyKey(WrapperType.EndMarker)
                  , new Strategy(st: (xw,pi) => { }, en: (xw,pi) => { }, isc: (p,pp) => false, gl: (pi) => int.MinValue )
                }
                ,{new StrategyKey(HEADING_1_LEVEL)
                  , new Strategy(
                      st: (xw, pi) =>
                        {
                            xw.WriteStartElement("p");
                            xw.WriteStartElement("p");
                            xw.WriteAttributeString("level", pi.Level.ToString());
                            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
                            xw.WriteEndElement();
                        }
                      , en: (xw,pi) => xw.WriteEndElement()
                      , isc: (child,parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level
                      , gl: (pi) => (int)pi.Paragraph.OutlineLevel )
                }
                ,{new StrategyKey(BODY_LEVEL)
                  , new Strategy(
                      st: (xw, pi) =>
                        {
                            xw.WriteStartElement("p");
                            xw.WriteAttributeString("level", pi.Level.ToString());
                            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
                        }

                      , en: (xw,pi) => xw.WriteEndElement()
                      , isc: (child,parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level
                      , gl: (pi) => (int)pi.Paragraph.OutlineLevel )
                }
            };
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
        public static ParagraphInfo Create(Word.Paragraph para, Word.Paragraph previousPara)
        {
            return new ParagraphInfo(para, previousPara);
        }
        public static ParagraphInfo Create(WrapperType wt)
        {
            return new ParagraphInfo(wt);
        }

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
        private readonly Strategy _strategy;

        private ParagraphInfo(Word.Paragraph para, Word.Paragraph previousPara)
          : this(para, previousPara, WrapperType.None )
        {
        }
        private ParagraphInfo(WrapperType wrapperType)
          : this(null, null, wrapperType)
        {
        }
/*
        public delegate void Starter(System.Xml.XmlWriter xw);
        public readonly Starter Start;
        public delegate void Ender(System.Xml.XmlWriter xw);
        public readonly Ender End;
        public delegate bool IsChilder(ParagraphInfo pi);
        public readonly IsChilder IsChild;

        private delegate int GetLeveler();
        private readonly GetLeveler GetLevel;
*/

        public void Start(System.Xml.XmlWriter xw)
        {
            _strategy.DoStart(xw, this);
        }
        public void End(System.Xml.XmlWriter xw)
        {
            _strategy.DoEnd(xw, this);
        }
        public bool IsChild(ParagraphInfo child)
        {
            return _strategy.DoIsChild(child, this);
        }
        private int GetLevel()
        {
            return _strategy.DoGetLevel(this);
        }

        private const int HEADING_1_LEVEL = 1;
        private const int BODY_LEVEL = 10;

        private ParagraphInfo(Word.Paragraph para, Word.Paragraph previousPara, WrapperType wt)
        {
            _strategy = _strategyMap.GetValueOrDefault(
              new StrategyKey(wt
              , para == null ? 0 : (int)para.OutlineLevel
              , para == null ? string.Empty : para.Range.ParagraphStyle.NameLocal.ToString()
              , previousPara == null ? string.Empty : previousPara.Range.ParagraphStyle.NameLocal.ToString()), Strategy.Default);
            Paragraph = para;
            _wrapperType = wt;
            return;
#if comment
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
#endif
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
