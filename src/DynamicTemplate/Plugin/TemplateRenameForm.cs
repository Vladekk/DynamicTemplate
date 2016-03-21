using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DynamicTemplate.Plugin
{
    public partial class TemplateRenameForm : Form
    {
        public TemplateRenameForm()
        {
            InitializeComponent();
        }

        public event CancelEventHandler NameValidating
        {
            add
            {
                txtName.Validating += value;
            }
            remove
            {
                txtName.Validating -= value;
            }
        }

        public string TemplateName
        {
            get
            {
                return txtName.Text.Trim();
            }
            set { txtName.Text = value; }
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            txtName.Text = txtName.Text.Trim();
            string name = txtName.Text;

            if (name.Length == 0)
            {
                e.Cancel = true;
                txtName.Select();
                ShowErrorMessage("Missing name", "A name is required");
                return;
            }

            char[] badChars = Path.GetInvalidFileNameChars();
            int badIndex;
            if ((badIndex = name.IndexOfAny(badChars)) >= 0)
            {
                e.Cancel = true;
                txtName.Select();
                txtName.Select(badIndex, 1);
                ShowErrorMessage("Invalid name", "The name contains one or more invalid characters");
                return;
            }
        }

        public void ShowErrorMessage(string title, string msg)
        {
            errorToolTip.ToolTipIcon = ToolTipIcon.Warning;
            errorToolTip.ToolTipTitle = title;
            errorToolTip.Show(msg, txtName, 0, txtName.Height, 5000);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateChildren())
                DialogResult = DialogResult.OK;
        }
    }
}