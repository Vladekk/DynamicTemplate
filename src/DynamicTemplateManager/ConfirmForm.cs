using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DynamicTemplate;
using System.IO;
using System.Text.RegularExpressions;

namespace DynamicTemplateManager
{
    public partial class ConfirmForm : Form
    {
        private int secondsToEnable = 2;

        public ConfirmForm()
        {
            InitializeComponent();
            UpdateInstallButton();
        }

        public Template Template { get; set; }
        public string TemplatePath { get; set; }

        public string TemplateName
        {
            set
            {
                txtName.Text = Regex.Replace(value, @"\s*\[\d+\]$", "");
            }
        }

        private void UpdateInstallButton()
        {
            if (secondsToEnable > 0)
            {
                btnOK.Text = string.Format("Install ({0})", secondsToEnable--);
                btnOK.Enabled = false;
            }
            else
            {
                btnOK.Text = "Install";
                btnOK.Enabled = true;
                delayTimer.Stop();
            }
        }

        private void delayTimer_Tick(object sender, EventArgs e)
        {
            UpdateInstallButton();
        }

        private void ConfirmForm_Load(object sender, EventArgs e)
        {
            delayTimer.Enabled = true;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (!ValidateChildren())
                return;

            string filePath = TemplatePath;
            string path = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "wlwtemplates");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            string destFile = Path.Combine(path, txtName.Text + ".wlwtemplate");
            if (File.Exists(destFile))
            {
                if (DialogResult.Yes != MessageBox.Show(
                    this,
                    "A template with this name already exists.\r\n\r\nDo you want to replace it?",
                    "Warning",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Exclamation,
                    MessageBoxDefaultButton.Button2))
                {
                    return;
                }

                File.Copy(filePath, destFile, true);
            }
            else
            {
                File.Copy(filePath, destFile, false);
            }

            MessageBox.Show(this, "The template was successfully installed.", "Success");
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtName_Validating(object sender, CancelEventArgs e)
        {
            txtName.Text = txtName.Text.Trim();
            if (txtName.Text.Length == 0)
            {
                toolTip.Show("Name is required",
                    txtName,
                    0,
                    txtName.Height,
                    3000);
                e.Cancel = true;
            }
            else if (txtName.Text.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
            {
                toolTip.Show("Name cannot contain any of these characters:\r\n\\ / : * ? \" < > |",
                    txtName,
                    0,
                    txtName.Height,
                    3000);
                e.Cancel = true;
            }
        }
    }
}
