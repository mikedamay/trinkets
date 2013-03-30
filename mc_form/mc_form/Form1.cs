using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Word = Microsoft.Office.Interop.Word;
using mc_auto;

namespace mc_form
{
    public partial class Form1 : Form
    {
        //private string INPUT_FILE_NAME = "mvn_howto.doc";
        private string INPUT_FILE_NAME = "mvn_narrative.doc";
        //private string INPUT_FILE_NAME = "mvn_commentary.doc";
        private string OUTPUT_FILE_EXTENSION = "xml";
        private const string EL_DOC = "doc";

        public Form1()
        {
            InitializeComponent();
        }
        private void Process()
        {
            Word.Document doc = null;
            try
            {
                string location = Directory.GetCurrentDirectory();
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
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            Process();        
        }
    }
}

