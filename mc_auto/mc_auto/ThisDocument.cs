using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.Office.Tools.Word;
using Microsoft.VisualStudio.Tools.Applications.Runtime;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using ExtensionMethods;


namespace mc_auto
{
    public partial class ThisDocument
    {
        private string INPUT_FILE_NAME = "mvn_commentary.docm";
        private string OUTPUT_FILE_EXTENSION = "xml";
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
                System.Diagnostics.Stopwatch sw = new Stopwatch();
                sw.Start();
                System.IO.FileStream fs = System.IO.File.OpenWrite(pathAndFile);
                fs.SetLength(0);
                xw = System.Xml.XmlWriter.Create(fs
                  , new System.Xml.XmlWriterSettings { Indent = true, CloseOutput = true });
                xw.WriteStartDocument();
                xw.WriteStartElement( EL_DOC);
                DomProcessor dp = new DomProcessor(doc, xw);
                dp.Process();
                xw.WriteEndElement();
                xw.WriteEndDocument();
                sw.Stop();
                MessageBox.Show("Elapsed time: " + sw.Elapsed.ToString() );
            }
            finally
            {
                if (xw != null)
                {
                    xw.Close();
                }
            }
        }
    }
}
