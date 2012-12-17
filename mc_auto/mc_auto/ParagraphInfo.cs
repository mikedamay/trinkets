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
        private const string MC_TECH_STYLE = "mc_tech";
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
            public Strategy(Starter st, Ender en, IsChilder isc, GetLeveler gl)
            {
                DoStart = st;
                DoEnd = en;
                DoIsChild = isc;
                DoGetLevel = gl;
            }
            internal readonly Starter DoStart;
            internal readonly Ender DoEnd;
            internal readonly IsChilder DoIsChild;
            internal readonly GetLeveler DoGetLevel;
        }
        private class StrategyKey
        {
            private static readonly IDictionary<string, bool> paragraphInfos = new Dictionary<string, bool>()
            {
                {MC_TECH_STYLE, true}
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
                return (outlineLevel < BODY_LEVEL && outlineLevel > 0 ? 0x1 : 0x0) << 2;
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
        private delegate void Starter(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate void Ender(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate bool IsChilder(ParagraphInfo pi, ParagraphInfo piParent);
        private delegate int GetLeveler(ParagraphInfo pi);

        private static Starter NoopStart = (xw, pi) => { };
        private static Ender NoopEnd = (xw, pi) => { };
        private static IsChilder TrueIsChild = (p, pp) => true;
        private static IsChilder FalseIsChild = (p, pp) => false;
        private static GetLeveler SuperParent = pi => int.MinValue;
        private static Ender StdEnd = (xw, pi) => xw.WriteEndElement();
        private static IsChilder StdIsChild = (child, parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level;
        private static GetLeveler StdGetLevel = (pi) => (int) pi.Paragraph.OutlineLevel;

        private static IDictionary<StrategyKey, Strategy> _strategyMap 
          = new Dictionary<StrategyKey, Strategy>()
            {
                {new StrategyKey(WrapperType.Root)
                  , new Strategy(st: NoopStart, en: NoopEnd, isc: TrueIsChild, gl: SuperParent )
                }
                ,{new StrategyKey(WrapperType.EndMarker)
                  , new Strategy(st: NoopStart, en: NoopEnd, isc: FalseIsChild, gl: SuperParent )
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
                      , en: StdEnd
                      , isc: StdIsChild
                      , gl: StdGetLevel )
                }
                ,{new StrategyKey(BODY_LEVEL)
                  , new Strategy(
                      st: (xw, pi) =>
                        {
                            xw.WriteStartElement("p");
                            xw.WriteAttributeString("level", pi.Level.ToString());
                            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
                        }

                      , en: StdEnd
                      , isc: StdIsChild
                      , gl: StdGetLevel )
                }
                ,{new StrategyKey(MC_TECH_STYLE, string.Empty)
                  , new Strategy(
                      st: (xw, pi) =>
                        {
                            xw.WriteStartElement("p");
                            xw.WriteStartElement("p");
                            xw.WriteAttributeString("level", pi.Level.ToString());
                            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
                            xw.WriteEndElement();
                        }
                      , en: StdEnd
                      , isc: (child, parent) => (int)parent.Paragraph.Range.ParagraphStyle == child.Paragraph.Range.ParagraphStyle
                      , gl: StdGetLevel )
                }
            };
        public enum WrapperType
        {
            None
            ,EndMarker
            ,Root
        }
        public static ParagraphInfo Create(Word.Paragraph para, Word.Paragraph previousPara)
        {
            return new ParagraphInfo(para, previousPara);
        }
        public static ParagraphInfo Create(WrapperType wt)
        {
            return new ParagraphInfo(wt);
        }

        private readonly WrapperType _wrapperType;

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

        public void Start(System.Xml.XmlWriter xw)
        {
            if (Paragraph != null) System.Diagnostics.Debug.Print("start: " + Paragraph.OutlineLevel.ToString() + " " + MassageXMLString(this.Paragraph.Range.Text));
            _strategy.DoStart(xw, this);
        }
        public void End(System.Xml.XmlWriter xw)
        {

            if ( Paragraph != null ) System.Diagnostics.Debug.Print("end: " + Paragraph.OutlineLevel.ToString() + " " + MassageXMLString(this.Paragraph.Range.Text));
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

        private ParagraphInfo() // allows to configure as struct
        {
            
        }
        private ParagraphInfo(Word.Paragraph para, Word.Paragraph previousPara, WrapperType wt) :this()
        {
            _strategy = _strategyMap.GetValueOrDefault(
              new StrategyKey(wt
              , para == null ? 0 : (int)para.OutlineLevel
              , para == null ? string.Empty : para.Range.ParagraphStyle.NameLocal.ToString()
              , previousPara == null ? string.Empty : previousPara.Range.ParagraphStyle.NameLocal.ToString()), Strategy.Default);
            _wrapperType = wt;
            Paragraph = para;
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
