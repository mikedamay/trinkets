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

namespace mc_auto
{
    public partial class ThisDocument
    {
        private string OUTPUT_FILE_EXTENSION = "xml";
        private const string EL_PARA = "p";
        private const string EL_DOC = "doc";
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
            string pathAndFile = System.IO.Path.ChangeExtension(System.IO.Path.Combine(location
              , this.Name), OUTPUT_FILE_EXTENSION);
            WriteDocumentAsXMLFile(this.StoryRanges[Word.WdStoryType.wdMainTextStory], pathAndFile);
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
                int ctr = 1, failed = 0;
                xw.WriteStartElement(EL_DOC);
                foreach (Word.Paragraph para in doc.Paragraphs)
                {
                    string text = para.Range.Text;
                    string styleName = para.Range.ParagraphStyle.NameLocal;
                    xw.WriteStartElement(EL_PARA);
                    xw.WriteAttributeString("style", styleName);
                    xw.WriteAttributeString("tables", para.Range.Tables.Count.ToString());
                    xw.WriteAttributeString("bookmarks", para.Range.Bookmarks.Count.ToString());
                    xw.WriteString(MassageXMLString(text));
                    xw.WriteEndElement();
                    ctr++;
                }
                xw.WriteEndElement();
                xw.WriteEndDocument();
                MessageBox.Show("Found " + ctr + " paragraphs.  " + failed + " to convert to XML");
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
    }
}
