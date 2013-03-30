using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Office = Microsoft.Office.Core;
using Word = Microsoft.Office.Interop.Word;
using System.Diagnostics;

namespace mc_auto
{
    public class DocumentController
    {
        private string INPUT_FILE_NAME = "mvn_commentary.docm";
        private string OUTPUT_FILE_EXTENSION = "xml";
        private const string EL_DOC = "doc";
        public void Doc2Xml()
        {
            Word.Document doc = null;
            try
            {
                string location = @"C:\Users\Mike\Documents\GitHub\maven_commentary\mc_auto\mc_auto\bin\Debug";
                //string location = this.GetType().Assembly.Location;
                string docPathAndFile = System.IO.Path.Combine(location, INPUT_FILE_NAME);
                string xmlPathAndFile = System.IO.Path.ChangeExtension(docPathAndFile, OUTPUT_FILE_EXTENSION);
                Word.Application app = new Word.Application();
                doc = app.Documents.Open(docPathAndFile);
                WriteDocumentAsXMLFile(doc.StoryRanges[Word.WdStoryType.wdMainTextStory], xmlPathAndFile);
            }
            finally
            {
                if (doc != null)
                {
                    (doc as Microsoft.Office.Interop.Word._Document).Close(null, null, null);
                }
            }
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
                xw.WriteStartElement(EL_DOC);
                DomProcessor dp = new DomProcessor(doc, xw);
                dp.Process();
                xw.WriteEndElement();
                xw.WriteEndDocument();
                sw.Stop();
                MessageBox.Show("Elapsed time: " + sw.Elapsed.ToString());
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
