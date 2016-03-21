using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DynamicTemplate.Compiler;

namespace DynamicTemplate.Plugin
{
    public partial class TemplateInputForm : Form
    {
        private Template _template;
        private const int INTRA_CONTROL_PADDING = 8;
        private List<ParamInput> _paramInputControls = new List<ParamInput>();

        public TemplateInputForm(string templateName, Template template)
        {
            _template = template;
            InitializeComponent();
            Text = templateName;

            int top = 0;
            foreach (ArgumentDescription arg in _template.Arguments)
            {
                ParamInput paramInputControl;
                switch (arg.Type)
                {
                    case ArgumentType.TextString:
                    case ArgumentType.HtmlString:
                        paramInputControl = new SingleLineTextboxParamInput(arg.Type == ArgumentType.TextString);
                        break;
                    case ArgumentType.TextStringExtended:
                    case ArgumentType.HtmlStringExtended:
                        paramInputControl = new MultiLineTextboxParamInput(arg.Type == ArgumentType.TextString);
                        break;
                    case ArgumentType.Integer:
                        paramInputControl = new IntegerParamInput();
                        break;
                    case ArgumentType.Double:
                        paramInputControl = new DoubleParamInput();
                        break;
                    case ArgumentType.Boolean:
                        paramInputControl = new BooleanParamInput();
                        break;
                    case ArgumentType.DateTime:
                        paramInputControl = new DateTimeParamInput();
                        break;
                    default:
                        continue;
                }
                paramInputControl.ParamLabel = arg.Label;
                paramInputControl.Left = 0;
                paramInputControl.Top = top;
                top += paramInputControl.Height + INTRA_CONTROL_PADDING;
                pnlInputs.Controls.Add(paramInputControl);
                _paramInputControls.Add(paramInputControl);
            }

            int delta = top - pnlInputs.Height - pnlInputs.AutoScrollMargin.Height * 2 + 10;
            if (delta < 0)
                Height += delta;
        }

        public object[] ParamValues()
        {
            return _paramInputControls.ConvertAll(pi => pi.Value).ToArray();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
                DialogResult = DialogResult.OK;
        }
    }
}