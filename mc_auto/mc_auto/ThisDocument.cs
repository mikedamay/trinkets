using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;

namespace ExtensionMethods
{
    public static class MyExtensions
    {
        public static TVALUE GetValueOrDefault<TKEY, TVALUE>(this IDictionary<TKEY, TVALUE> map
          ,TKEY key, TVALUE defaultval)
        {
            TVALUE gotval;
            if (!map.TryGetValue(key, out gotval))
            {
                gotval = defaultval;
            }
            return gotval;
        }
    }
}

namespace mc_auto
{
    using ExtensionMethods;
    public partial class ThisDocument
    {
        private class ParagraphInfo
        {
            public AttributeDefinition AttributeDef {get; private set;}
            public ParagraphInfo()
            {
                AttributeDef = null;
            }
            public ParagraphInfo(AttributeDefinition ad)
            {
                AttributeDef = ad;
            }
        }
        private class AttributeDefinition
        {
            public string Name {get; private set;}
            public object Value {get; private set;}
            public AttributeDefinition(string nm, object vl)
            {
                Name = nm;
                Value = vl;
            }
        }
        private IDictionary<string, ParagraphInfo> paragraphInfos = new Dictionary<string, ParagraphInfo>()
        {
            {"mc_tech", new ParagraphInfo(new AttributeDefinition("group", true ))}
            ,{"mc_example_text", new ParagraphInfo(new AttributeDefinition("group", true ))}
        };

        private string INPUT_FILE_NAME = "mvn_commentary.docm";
        private string OUTPUT_FILE_EXTENSION = "xml";
        private const string EL_PARA = "p";
        private const string EL_DOC = "doc";
        private const string EL_HTML = "html";
        private const string EL_BODY = "body";
        private const string EL_CLASS = "class";
        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
        }


        #region VSTO Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InternalStartup()
        {
            this.button1.Click += new System.EventHandler(this.button1_Click);
            this.Startup += new System.EventHandler(this.ThisDocument_Startup);
            this.Shutdown += new System.EventHandler(this.ThisDocument_Shutdown);

        }

        #endregion

        private void button1_Click(object sender, EventArgs e)
        {
            string location = this.Path;
            string docPathAndFile = System.IO.Path.Combine(location, INPUT_FILE_NAME);
            string xmlPathAndFile = System.IO.Path.ChangeExtension(docPathAndFile, OUTPUT_FILE_EXTENSION);
            Word.Document doc = this.Application.Documents.Open(docPathAndFile);
            WriteDocumentAsXMLFile(doc.StoryRanges[Word.WdStoryType.wdMainTextStory], xmlPathAndFile);
            (doc as Microsoft.Office.Interop.Word._Document).Close(null, null, null);
        }
        private void WriteDocumentAsXMLFile(Word.Range doc, string pathAndFile)
        {
            System.Xml.XmlWriter xw = null;
            try
            {
                System.IO.FileStream fs = System.IO.File.OpenWrite(pathAndFile);
                xw = System.Xml.XmlWriter.Create(fs
                  , new System.Xml.XmlWriterSettings { Indent = true, CloseOutput = true });
                xw.WriteStartDocument();
                int ctr = 1;
                WriteStartElement(xw, EL_DOC);
                foreach (Word.Paragraph para in doc.Paragraphs)
                {
                    string text = para.Range.Text;
                    string styleName = para.Range.ParagraphStyle.NameLocal;
                    WriteStartElement(xw,EL_PARA);
                    WriteAttributeString(xw, EL_CLASS, styleName);
                    if (para.Range.Tables.Count > 0)
                    {
                        WriteAttributeString(xw, "tables", para.Range.Tables.Count.ToString());
                    }
                    if (para.Range.Tables.Count > 0 && para.Range.Cells.Count > 0)
                    {
                        int? row = default(int);
                        int? col = default(int);
                        row = para.Range.Cells[1].Row.Index;
                        col = para.Range.Cells[1].Column.Index;
                        WriteAttributeString(xw, "row", row.ToString());
                        WriteAttributeString(xw, "col", col.ToString());
                    }
                    if (para.Range.Bookmarks.Count > 0)
                    {
                        WriteAttributeString(xw, "bookmark", para.Range.Bookmarks[1].Name);
                    }
                    WriteAttributeString(xw, "level", ((int)para.OutlineLevel).ToString());
                    AttributeDefinition ad = GetParagraphInfo(para).AttributeDef;
                    if (ad != null)
                    {
                        WriteAttributeString(xw, ad.Name, ad.Value.ToString());
                    }
                    WriteString(xw, MassageXMLString(text));
                    WriteEndElement(xw);
                    ctr++;
                }
                WriteEndElement(xw);
                xw.WriteEndDocument();
                MessageBox.Show("Found " + ctr + " paragraphs to convert to XML");
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }
        }
        /// <summary>
        /// see callee
        /// </summary>
        private ParagraphInfo GetParagraphInfo(Word.Paragraph para)
        {
            return GetParagraphInfo(para.OutlineLevel, (string)para.Range.ParagraphStyle.NameLocal);
        }
        /// <summary>
        /// if the paragraph does not specify a level then use the outline level.  styles
        /// such as mc_text have a fake outline level so that they can be put in a div in the
        /// html hierarchy when it is generated
        /// </summary>
        /// <param name="wdOutlineLevel">from the word document</param>
        /// <param name="paragraphStyleName">style as specified in the word document</param>
        /// <returns>level between 1 (heading 1) and 10 (body text)</returns>
        private ParagraphInfo GetParagraphInfo(Word.WdOutlineLevel wdOutlineLevel, string paragraphStyleName)
        {
            return paragraphInfos.GetValueOrDefault(paragraphStyleName, new ParagraphInfo());
        }
        /// <summary>
        /// strips unsightly control characters off the end of paragraphs
        /// </summary>
        /// <param name="text">typically Paragraph.Text from Word</param>
        /// <returns>text minus trailing control characters</returns>
        private string MassageXMLString(string text)
        {
            int reduceBy = 0;
            for (int ii = text.Length - 1; ii >= 0; ii--)
            {
                if ( System.Char.IsControl(text[ii]) )
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
        private void WriteStartElement(System.Xml.XmlWriter xw, string elementName)
        {
            xw.WriteStartElement(elementName);
        }
        private void WriteString(System.Xml.XmlWriter xw, string str)
        {
            xw.WriteString(str);
        }
        private void WriteAttributeString(System.Xml.XmlWriter xw, string attributeName, object attributeValue)
        {
            xw.WriteAttributeString(attributeName, attributeValue.ToString());
        }
        private void WriteEndElement(System.Xml.XmlWriter xw)
        {
            xw.WriteEndElement();
        }
        private static void WriteEndElementS(System.Xml.XmlWriter xw)
        {
            xw.WriteEndElement();
        }
#if comment
        delegate void XmlAction(System.Xml.XmlWriter xw);
        private class XmlActions
        {
            public XmlAction Start;
        }
        XmlAction action = xw => WriteEndElementS(xw);
        private void Create()
        {
            IDictionary<string, XmlActions> actionsMap = new Dictionary<string, XmlActions>()
            {{"any", new XmlActions
            {
                Start = xw => WriteEndElement(xw)
            }}};
            //actionsMap.Add("any", st);
        }
#endif
    }
}
