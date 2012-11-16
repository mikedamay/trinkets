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

        private void ThisDocument_Startup(object sender, System.EventArgs e)
        {
        }

        private void ThisDocument_Shutdown(object sender, System.EventArgs e)
        {
        }

        public void DoStuff()
        {
            MessageBox.Show("mike is here");
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
            int ctr = 0;
            foreach (Word.Paragraph para in Globals.ThisDocument.Paragraphs)
            {
                ctr++;
            }
            MessageBox.Show("Found " + ctr + " paragraphs");
            Word.Paragraph firstPara = this.Paragraphs[1];
            MessageBox.Show(firstPara.Range.Text);

        }
    }
}
