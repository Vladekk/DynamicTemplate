using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace DynamicTemplate.Plugin
{
    public partial class SingleLineTextboxParamInput : DynamicTemplate.Plugin.ParamInput
    {
        private bool _htmlEscape;

        public SingleLineTextboxParamInput(bool htmlEscape)
        {
            _htmlEscape = htmlEscape;
            InitializeComponent();
        }

        public override object Value
        {
            get
            {
                return _htmlEscape ? HttpUtility.HtmlEncode(txtValue.Text) : txtValue.Text;
            }
        }
    }
}

