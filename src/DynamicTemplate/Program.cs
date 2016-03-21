using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DynamicTemplate.Plugin;
using System.Xml.Serialization;
using System.Xml;
using System.IO;

namespace DynamicTemplate
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form1());
            //Application.Run(new TemplateEditorForm());
            //new TemplateChooserForm().ShowDialog();

            string newContent = null;
            if (DialogResult.OK == new DynamicTemplateContentSource().CreateContent(null, ref newContent))
                MessageBox.Show("BEGIN\r\n" + newContent + "\r\nEND");
        }
    }
}