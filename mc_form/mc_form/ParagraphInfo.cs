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

        private delegate void StartBehaviour(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate void EndBehaviour(System.Xml.XmlWriter xw, ParagraphInfo pi);
        private delegate bool IsChildBehaviour(ParagraphInfo pi, ParagraphInfo piParent);
        private delegate int GetLeveler(ParagraphInfo pi);

        private delegate void AttributeWriter(System.Xml.XmlWriter xw);

        private static readonly StartBehaviour NoopStart = (xw, pi) => { };
        private static readonly EndBehaviour NoopEnd = (xw, pi) => { };
        private static readonly IsChildBehaviour TrueIsChild = (p, pp) => true;
        private static readonly IsChildBehaviour FalseIsChild = (p, pp) => false;
        private static readonly GetLeveler SuperParent = pi => int.MinValue;
        private static readonly EndBehaviour StdEnd = (xw, pi) => xw.WriteEndElement();
        private static readonly IsChildBehaviour StdIsChild = (child, parent) => (int)parent.Paragraph.OutlineLevel < (int)child.Level;
        private static readonly GetLeveler StdGetLevel = (pi) => (int)pi.Paragraph.OutlineLevel;
        private static readonly StartBehaviour StdStart = (xw, pi) =>
        {
            xw.WriteStartElement(STR_PARAGRAPH);
            xw.WriteAttributeString(STR_LEVEL, pi.Level.ToString());
            xw.WriteAttributeString(STR_CLASS, pi.Paragraph.get_Style().NameLocal.ToString());
            GetTableCountAttributeWriter(pi.Paragraph.Range)(xw);
            GetRowAttributeWriter(pi.Paragraph.Range)(xw);
            GetColumnAttributeWriter(pi.Paragraph.Range)(xw);
            xw.WriteString(MassageXMLString(pi.Paragraph.Range.Text));
        };
        private static readonly StartBehaviour ParentStart = (xw, pi) =>
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
                ,{new StrategyKey(MC_TECH_STYLE, string.Empty)
                        // this will be used for all other paragraphs
                  , new Strategy(st: ParentStart, en: StdEnd
                      , isc: (child, parent) => parent.Paragraph.get_Style().NameLocal == child.Paragraph.get_Style().NameLocal
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
              , para == null ? string.Empty : para.get_Style().NameLocal.ToString()
              , previousPara == null ? string.Empty : previousPara.get_Style().NameLocal.ToString())
              , Strategy.Default);
            _wrapperType = wt;
            Paragraph = para;
        }
        /// <summary>
        /// strips unsightly control characters out of of paragraphs.  XML cannot handle them
        /// Most control characters occur at/after the end of the paragraph.  but some such as foot note (introduced by char(2) is 
        /// embedded.
        /// </summary>
        /// <param name="text">typically Paragraph.Text from Word</param>
        /// <returns>text minus trailing and other control characters</returns>
        private static string MassageXMLString(string text)
        {
            StringBuilder sbIn = new StringBuilder(text), sbOut = new StringBuilder(text.Length);
            int jj = 0, ii = 0;
            for (ii = 0; ii < text.Length - 1; ii++)
            {
                if (!Char.IsControl(sbIn[ii]))
                {
                    sbOut.Append(sbIn[ii]);
                    jj++;
                }
            }
            return sbOut.ToString();
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
