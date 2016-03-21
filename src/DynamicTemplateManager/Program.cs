using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using DynamicTemplate;

namespace DynamicTemplateManager
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            if (args.Length == 0)
            {
                ConfirmForm cfdebug = new ConfirmForm();
                cfdebug.TemplateName = "Teh Best Template Evar";
                cfdebug.ShowDialog();
                return;
            }
#endif

            if (args.Length != 1)
                return;

            string filePath = Path.GetFullPath(args[0]);

            if (!File.Exists(filePath))
                return;

            Template template = Template.Load(filePath);

            ConfirmForm cf = new ConfirmForm()
            {
                TemplateName = Path.GetFileNameWithoutExtension(filePath),
                Template = template,
                TemplatePath = filePath
            };
            cf.ShowDialog();
        }
    }
}
