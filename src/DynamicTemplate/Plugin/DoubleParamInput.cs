using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    public partial class DoubleParamInput : DynamicTemplate.Plugin.ParamInput
    {
        public DoubleParamInput()
        {
            InitializeComponent();
        }

        public override object Value
        {
            get
            {
                return double.Parse(txtValue.Text);
            }
        }

        private void txtValue_Validating(object sender, CancelEventArgs e)
        {
            txtValue.Text = txtValue.Text.Trim();
            if (txtValue.Text == "")
            {
                txtValue.Text = "0.0";
            }

            try
            {
                double.Parse(txtValue.Text);
            }
            catch (FormatException)
            {
                e.Cancel = true;
                txtValue.Select();
                txtValue.SelectAll();
                errorToolTip.ToolTipIcon = ToolTipIcon.Warning;
                errorToolTip.ToolTipTitle = "Invalid number";
                errorToolTip.Show("Please enter a valid decimal number (e.g. " + (105.34.ToString()) + ")", txtValue, 5000);
            }
        }
    }
}

