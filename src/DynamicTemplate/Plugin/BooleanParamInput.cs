using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    public partial class BooleanParamInput : DynamicTemplate.Plugin.ParamInput
    {
        public BooleanParamInput()
        {
            InitializeComponent();
        }

        public override string ParamLabel
        {
            set
            {
                chkValue.Text = value;
                base.ParamLabel = value;
            }
        }

        public override object Value
        {
            get
            {
                return chkValue.Checked;
            }
        }
    }
}

