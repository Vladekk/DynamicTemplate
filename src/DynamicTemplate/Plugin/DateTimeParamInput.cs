using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    public partial class DateTimeParamInput : DynamicTemplate.Plugin.ParamInput
    {
        public DateTimeParamInput()
        {
            InitializeComponent();
            dtValue.Value = DateTime.Now;
        }

        public override object Value
        {
            get
            {
                return dtValue.Value;
            }
        }
    }
}

