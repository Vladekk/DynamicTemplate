using System;
using System.Collections.Generic;
using System.Text;

using System.Windows.Forms;
using DynamicTemplate.Compiler;
using DynamicTemplate.Plugin;
using System.Threading;
using OpenLiveWriter.Api;

namespace DynamicTemplate
{
    [WriterPlugin("233830C2-4EAE-482b-9A2B-8FA70AE6309B", "Dynamic Template", ImagePath="Icon.png", 
        PublisherUrl="http://www.joecheng.com/code/DynamicTemplate/",
        Description="Easily insert oft-used snippets of HTML or text.")]
    [InsertableContentSource("Template")]
    public class DynamicTemplateContentSource : ContentSource
    {
        public override DialogResult CreateContent(
            IWin32Window dialogOwner, 
            ref string newContent)
        {
            string templateName;
            Template template;

            using (TemplateChooserForm form = new TemplateChooserForm())
            {
                if (form.ShowDialog(dialogOwner) == DialogResult.Cancel)
                    return DialogResult.Cancel;
                templateName = form.SelectedTemplateName;
                template = form.SelectedTemplate;
            }

            object[] parameters;
            if (template.Arguments.Length == 0)
            {
                parameters = new object[0];
            }
            else
            {
                using (TemplateInputForm form = new TemplateInputForm(templateName, template))
                {
                    if (form.ShowDialog(dialogOwner) == DialogResult.Cancel)
                        return DialogResult.Cancel;
                    parameters = form.ParamValues();
                }
            }

            try
            {
                using (new WaitCursor())
                {
                    newContent = new BodyParser().Parse(template.Body, template.Arguments)(newContent, parameters);
                }
            }
            catch (TemplateCompilationException tce)
            {
                throw new ContentCreationException("Template Error", tce.Message);
            }

            return DialogResult.OK;
        }
    }
}
