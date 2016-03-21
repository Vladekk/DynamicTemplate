using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Web;

namespace DynamicTemplate.Plugin
{
    public partial class MultiLineTextboxParamInput : ParamInput
    {
        private bool _htmlEscape;

        public MultiLineTextboxParamInput(bool htmlEscape)
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
