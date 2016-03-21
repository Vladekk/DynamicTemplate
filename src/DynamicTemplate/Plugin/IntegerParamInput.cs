using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    public partial class IntegerParamInput : DynamicTemplate.Plugin.ParamInput
    {
        public IntegerParamInput()
        {
            InitializeComponent();
        }

        public override object Value
        {
            get
            {
                return int.Parse(txtValue.Text);
            }
        }

        private void txtValue_Validating(object sender, CancelEventArgs e)
        {
            txtValue.Text = txtValue.Text.Trim();
            if (txtValue.Text == "")
            {
                txtValue.Text = "0";
            }

            try
            {
                int.Parse(txtValue.Text);
            }
            catch (FormatException)
            {
                e.Cancel = true;
                txtValue.Select();
                txtValue.SelectAll();
                errorToolTip.ToolTipIcon = ToolTipIcon.Warning;
                errorToolTip.ToolTipTitle = "Invalid number";
                errorToolTip.Show("Please enter a positive or negative whole number", txtValue, 5000);
            }
        }
    }
}

