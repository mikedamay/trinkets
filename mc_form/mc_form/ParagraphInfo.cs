using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Word = Microsoft.Office.Interop.Word;
using ExtensionMethods;

namespace mc_auto
{
    /// <summary>
    /// Each paragraph from the word document is wrapped in a ParagraphInfo object as it is read in.
    /// 
    /// The behaviour of the paragraph and its public methods Start(), End() and IsChild() are determined by
    /// the configuration of its _strategy member to which they are delegated.
    /// When the paragraphInfo object is created its Strategy is
    /// looked up on the _strategeyMap based on the contents such as style and outline level.
    /// </summary>
    internal partial class ParagraphInfo
    {
        private const string MC_TECH_STYLE = "mc_tech";
        private const string STR_PARAGRAPH = "p";
        private const string STR_LEVEL = "level";
        private const string STR_CLASS = "class";
        private const string STR_TABLES = "tables";
        private const string STR_ROW = "row";
        private const string STR_COLUMN = "column";

        private delegate void Starter(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate void Ender(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate bool IsChilder(ParagraphInfo pi, ParagraphInfo piParent);
        private delegate int GetLeveler(ParagraphInfo pi);

        private delegate void AttributeWriter(System.Xml.XmlWriter xw);

        private static readonly Starter NoopStart = (xw, pi) => { };
        private static readonly Ender NoopEnd = (xw, pi) => { };
        private static readonly IsChilder TrueIsChild = (p, pp) => true;
        private static readonly IsChilder FalseIsChild = (p, pp) => false;
        private static readonly GetLeveler SuperParent = pi => int.MinValue;
        private static readonly Ender StdEnd = (xw, pi) => xw.WriteEndElement();
        private static readonly IsChilder StdIsChild = (child, parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level;
        private static readonly GetLeveler StdGetLevel = (pi) => (int)pi.Paragraph.OutlineLevel;
        private static readonly Starter StdStart = (xw, pi) =>
        {
            xw.WriteStartElement(STR_PARAGRAPH);
            xw.WriteAttributeString(STR_LEVEL, pi.Level.ToString());
            xw.WriteAttributeString(STR_CLASS, "my_para_style" ); // pi.Paragraph.Range.ParagraphStyle.NameLocal.ToString());
            GetTableCountAttributeWriter(pi.Paragraph.Range)(xw);
            GetRowAttributeWriter(pi.Paragraph.Range)(xw);
            GetColumnAttributeWriter(pi.Paragraph.Range)(xw);
            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
        };
        private static readonly Starter ParentStart = (xw, pi) =>
        {
            xw.WriteStartElement(STR_PARAGRAPH);
            StdStart(xw, pi);
            xw.WriteEndElement();
        };

        private static IDictionary<StrategyKey, Strategy> _strategyMap 
          = new Dictionary<StrategyKey, Strategy>()
            {
                {new StrategyKey(WrapperType.Root), new Strategy(st: NoopStart, en: NoopEnd, isc: TrueIsChild, gl: SuperParent )}
                ,{new StrategyKey(WrapperType.EndMarker), new Strategy(st: NoopStart, en: NoopEnd, isc: FalseIsChild, gl: SuperParent )}
                ,{new StrategyKey(HEADING_1_LEVEL), new Strategy(st: ParentStart, en: StdEnd, isc: StdIsChild, gl: StdGetLevel )}
                        // this will be used for paragraphs with a outline level other than 1
                //,{new StrategyKey(BODY_LEVEL), new Strategy(st: StdStart, en: StdEnd, isc: StdIsChild, gl: StdGetLevel )}
                ,{new StrategyKey(MC_TECH_STYLE, string.Empty)
                        // this will be used for all other paragraphs
                  , new Strategy(st: ParentStart, en: StdEnd
                      , isc: (child, parent) => false //parent.Paragraph.Range.ParagraphStyle.NameLocal == child.Paragraph.Range.ParagraphStyle.NameLocal
                      , gl: StdGetLevel 
                      )
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
        private int Level {get { return GetLevel(); }}
        private Word.Paragraph Paragraph { get; set; }
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
        private ParagraphInfo(Word.Paragraph para, Word.Paragraph previousPara, WrapperType wt) : this()
        {
            _strategy = _strategyMap.GetValueOrDefault(
              new StrategyKey(wt
              , para == null ? 0 : (int)para.OutlineLevel
              , para == null ? string.Empty : string.Empty //para.Range.ParagraphStyle.NameLocal.ToString()
              , previousPara == null ? string.Empty : string.Empty) // previousPara.Range.ParagraphStyle.NameLocal.ToString())
              , Strategy.Default);
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
        private static AttributeWriter GetTableCountAttributeWriter(Word.Range rng)
        {
            return rng.Tables.Count > 0
                ? (AttributeWriter)((xw) => xw.WriteAttributeString(STR_TABLES, rng.Tables.Count.ToString()))
                : (xw) => { };
        }
        private static AttributeWriter GetRowAttributeWriter(Word.Range rng)
        {
            return rng.Tables.Count > 0 && rng.Cells.Count > 0
                ? (AttributeWriter)((xw) => xw.WriteAttributeString(STR_ROW, rng.Cells[1].Row.Index.ToString()))
                : (xw) => { };
        }
        private static AttributeWriter GetColumnAttributeWriter(Word.Range rng)
        {
            return rng.Tables.Count > 0 && rng.Cells.Count > 0
                ? (AttributeWriter)((xw) => xw.WriteAttributeString(STR_COLUMN, rng.Cells[1].Column.Index.ToString()))
                : (xw) => { };
        }
    }
}
