using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using DynamicTemplate.Compiler;

namespace DynamicTemplate.Plugin
{
    public partial class TemplateChooserForm : Form
    {
        private const string TEMPLATE_EXTENSION = ".wlwtemplate";

        private Template _selectedTemplate;
        private string _selectedTemplateName;
        private int _restoreWidth;

        public TemplateChooserForm()
        {
            InitializeComponent();
        }

        public string SelectedTemplateName
        {
            get { return _selectedTemplateName; }
        }

        public Template SelectedTemplate
        {
            get { return _selectedTemplate; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            RefreshList();

            /*
            _restoreWidth = lstTemplate.Width;
            lstTemplate.Width = btnAdd.Right - lstTemplate.Left;
            */

            _restoreWidth = Width;
            int rightMargin = Width - btnAdd.Right;
            Width = lstTemplate.Right + rightMargin;
            btnAdd.Visible = false;
            btnRemove.Visible = false;
            btnEdit.Visible = false;
            btnRename.Visible = false;
        }

        private void RefreshList()
        {
            lstTemplate.Items.Clear();
            string templatesPath = TemplateDir;
            Directory.CreateDirectory(templatesPath);
            string[] files = Directory.GetFiles(templatesPath, "*" + TEMPLATE_EXTENSION, SearchOption.TopDirectoryOnly);

            Array.Sort<string>(files, StringComparer.CurrentCultureIgnoreCase);

            lstTemplate.BeginUpdate();
            try
            {
                foreach (string file in files)
                {
                    lstTemplate.Items.Add(Path.GetFileNameWithoutExtension(file));
                }
            }
            finally
            {
                lstTemplate.EndUpdate();
            }

            ManageButtons();
        }

        private static string   TemplateDir
        {
            get
            {
                string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string templatesPath = Path.Combine(appData, "WLWTemplates");
                return templatesPath;
            }
        }

        private void lstTemplate_SelectedIndexChanged(object sender, EventArgs e)
        {
            ManageButtons();
        }

        private void ManageButtons()
        {
            int itemsSelected = lstTemplate.SelectedIndices.Count;
            btnOK.Enabled = btnEdit.Enabled = btnRename.Enabled
                = itemsSelected == 1;
            btnRemove.Enabled = itemsSelected > 0;
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                this,
                "Are you sure you want to delete the selected template(s)?",
                "Confirm Delete",
                MessageBoxButtons.OKCancel,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            try
            {
                if (result == DialogResult.OK)
                {
                    string templateDir = TemplateDir;
                    foreach (string file in lstTemplate.SelectedItems)
                    {
                        string filePath = NameToPath(file);
                        if (File.Exists(filePath))
                            File.Delete(filePath);
                    }
                }
            }
            finally
            {
                RefreshList();
                lstTemplate.SelectedItem = null;
            }
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            using (TemplateRenameForm form = new TemplateRenameForm())
            {
                string oldName = (string)lstTemplate.SelectedItem;
                form.TemplateName = oldName;
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    string newName = form.TemplateName;
                    File.Move(NameToPath(oldName), NameToPath(newName));
                    RefreshList();
                    lstTemplate.SelectedItem = newName;
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string name = lstTemplate.SelectedItem as string;
            using (TemplateEditorForm form = new TemplateEditorForm())
            {
                form.Text = string.Format(form.Text, name);
                form.LoadFile(NameToPath(name));
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    form.SaveFile(NameToPath(name));
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name;
            using (TemplateRenameForm form = new TemplateRenameForm())
            {
                form.Text = "New Template";
                form.NameValidating += new CancelEventHandler(validateAddName);
                if (form.ShowDialog(this) != DialogResult.OK)
                    return;
                name = form.TemplateName;
            }

            using (TemplateEditorForm form = new TemplateEditorForm())
            {
                form.Text = string.Format(form.Text, name);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    form.SaveFile(NameToPath(name));
                    RefreshList();
                    lstTemplate.SelectedItem = name;
                }
            }
        }

        void validateAddName(object sender, CancelEventArgs e)
        {
            if (e.Cancel)
                return;
            Control ctrl = (Control)sender;
            TemplateRenameForm form = (TemplateRenameForm)ctrl.FindForm();
            string name = form.TemplateName;
            if (File.Exists(NameToPath(name)))
            {
                e.Cancel = true;
                form.ShowErrorMessage("Name in use", "A template with this name already exists; please choose another name");
            }
        }

        private string NameToPath(string name)
        {
            return Path.Combine(TemplateDir, name + TEMPLATE_EXTENSION);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string name = (string)lstTemplate.SelectedItem;
            string filePath = NameToPath(name);
            Template template;
            using (new WaitCursor())
            {
                template = Template.Load(filePath);
            }
            _selectedTemplateName = name;
            _selectedTemplate = template;
            DialogResult = DialogResult.OK;
        }

        private void lstTemplate_DoubleClick(object sender, EventArgs e)
        {
            if (btnOK.Enabled)
                btnOK_Click(sender, e);
        }

        private void lnkMore_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            lnkMore.Visible = false;
            Width = _restoreWidth;
            btnAdd.Visible = true;
            btnRemove.Visible = true;
            btnEdit.Visible = true;
            btnRename.Visible = true;
        }
    }
}