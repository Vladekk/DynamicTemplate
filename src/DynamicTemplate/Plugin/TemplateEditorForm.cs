using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Xml.Serialization;
using System.IO;
using DynamicTemplate.Compiler;
using System.CodeDom.Compiler;

namespace DynamicTemplate.Plugin
{
    public partial class TemplateEditorForm : Form
    {
        private static Dictionary<string, ArgumentType> s_argStrToEnum = new Dictionary<string, ArgumentType>();
        private static Dictionary<ArgumentType, string> s_argEnumToStr = new Dictionary<ArgumentType, string>();
        private LanguageProvider _provider = new CSharpLanguageProvider();

        public TemplateEditorForm()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            // Do this late because otherwise exceptions occasionally occur while loading
            gridArgs.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        public Template Template
        {
            get
            {
                string body = txtBody.Text;
                List<ArgumentDescription> args = new List<ArgumentDescription>();
                foreach (DataGridViewRow row in gridArgs.Rows)
                {
                    if (row.IsNewRow)
                        continue;

                    string identifier = row.Cells[0].Value as string;
                    string rawType = row.Cells[1].Value as string;
                    string label = row.Cells[2].Value as string;
                    ArgumentType type = s_argStrToEnum[rawType];
                    args.Add(new ArgumentDescription(type, identifier, label));
                }

                Template template = new Template();
                template.Arguments = args.ToArray();
                template.Body = body;
                return template;
            }
            set
            {
                txtBody.Text = value.Body;
                ArgumentDescription[] args = value.Arguments;

                gridArgs.Rows.Clear();
                if (args.Length > 0)
                {
                    gridArgs.Rows.Add(args.Length);
                    for (int i = 0; i < args.Length; i++)
                    {
                        DataGridViewRow row = gridArgs.Rows[i];
                        row.Cells[0].Value = args[i].Identifier;
                        row.Cells[1].Value = s_argEnumToStr[args[i].Type];
                        row.Cells[2].Value = args[i].Label;
                    }
                }
            }
        }

        public void LoadFile(string filename)
        {
            Template = Template.Load(filename);
        }

        public void SaveFile(string filename)
        {
            Template.Save(filename);
        }

        static TemplateEditorForm()
        {
            s_argStrToEnum["Text"] = ArgumentType.TextString;
            s_argStrToEnum["Text (Multi-line)"] = ArgumentType.TextStringExtended;
            s_argStrToEnum["HTML"] = ArgumentType.HtmlString;
            s_argStrToEnum["HTML (Multi-line)"] = ArgumentType.HtmlStringExtended;
            s_argStrToEnum["Integer"] = ArgumentType.Integer;
            s_argStrToEnum["Double"] = ArgumentType.Double;
            s_argStrToEnum["Boolean"] = ArgumentType.Boolean;
            s_argStrToEnum["Date/Time"] = ArgumentType.DateTime;

            foreach (KeyValuePair<string, ArgumentType> pair in s_argStrToEnum)
                s_argEnumToStr[pair.Value] = pair.Key;
        }

        private void btnDeleteRow_Click(object sender, EventArgs e)
        {
            if (gridArgs.SelectedRows.Count == 1)
            {
                DataGridViewRow row = gridArgs.SelectedRows[0];
                if (!row.IsNewRow)
                    gridArgs.Rows.Remove(row);
            }
        }

        private void btnRowUp_Click(object sender, EventArgs e)
        {
            if (gridArgs.SelectedRows.Count == 1)
            {
                DataGridViewRow row = gridArgs.SelectedRows[0];
                if (!row.IsNewRow)
                {
                    int rowIndex = gridArgs.Rows.IndexOf(row);
                    if (rowIndex > 0)
                    {
                        DataGridViewRow otherRow = gridArgs.Rows[rowIndex - 1];
                        gridArgs.Rows.RemoveAt(rowIndex - 1);
                        gridArgs.Rows.Insert(rowIndex, otherRow);
                    }
                }
            }
        }

        private void SwapRows(int rowIndex1, int rowIndex2)
        {
            if (rowIndex1 == rowIndex2)
                return;

            if (rowIndex1 > rowIndex2)
            {
                int tmp = rowIndex2;
                rowIndex2 = rowIndex1;
                rowIndex1 = tmp;
            }

            DataGridViewRow row1 = gridArgs.Rows[rowIndex1];
            DataGridViewRow row2 = gridArgs.Rows[rowIndex2];
            gridArgs.Rows.RemoveAt(rowIndex2);
            gridArgs.Rows.RemoveAt(rowIndex1);
            gridArgs.Rows.Insert(rowIndex1, row2);
            gridArgs.Rows.Insert(rowIndex2, row1);
        }

        private void btnRowDown_Click(object sender, EventArgs e)
        {
            if (gridArgs.SelectedRows.Count == 1)
            {
                DataGridViewRow row = gridArgs.SelectedRows[0];
                if (!row.IsNewRow)
                {
                    int rowIndex = gridArgs.Rows.IndexOf(row);
                    if (rowIndex < gridArgs.Rows.Count - 1)
                    {
                        DataGridViewRow otherRow = gridArgs.Rows[rowIndex + 1];
                        gridArgs.Rows.RemoveAt(rowIndex + 1);
                        gridArgs.Rows.Insert(rowIndex, otherRow);
                    }
                }
            }
        }

        private void gridArgs_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[1].Value = s_argEnumToStr[ArgumentType.TextString];
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (ValidateArgs() && Compile())
                DialogResult = DialogResult.OK;
        }

        private bool Compile()
        {
            UseWaitCursor = true;
            try
            {
                new BodyParser().Parse(Template.Body, Template.Arguments);
                return true;
            }
            catch (TemplateCompilationException ex)
            {
                int sourceLine = ex.Position.Line;
                int sourceCol = ex.Position.Column;
                if (sourceLine >= 0 && sourceCol >= 0)
                {
                    int idx = IndexToPosition.ReverseFind(txtBody.Text, sourceLine, sourceCol);
                    if (idx >= 0)
                    {
                        txtBody.Select(idx, 0);
                        //tooltipPos = TooltipHelper.GetPointFromIndex(txtBody, idx);
                        //tooltipPos.Offset(txtBody.Location);
                        txtBody.Select();
                    }
                }
                string errorTitle = "Compile Error";
                string errorMessage = ex.Message;
                ShowError(errorTitle, errorMessage, OffsetToOrigin(txtBody, new Point(0, txtBody.Height)));
                return false;
            }
            finally
            {
                UseWaitCursor = false;
            }
        }

        private void ShowError(string errorTitle, string errorMessage, Point p)
        {
            toolTip.ToolTipTitle = errorTitle;
            toolTip.ToolTipIcon = ToolTipIcon.Warning;
            toolTip.Show(errorMessage, pnlOrigin, p, 10000);
        }

        private bool ValidateArgs()
        {
            Dictionary<string, DataGridViewRow> usedIdentifiers = new Dictionary<string, DataGridViewRow>();

            foreach (DataGridViewRow row in gridArgs.Rows)
            {
                if (row.IsNewRow)
                    continue;

                DataGridViewCell cell = row.Cells[0];
                if (!ValidateCell(cell, (string)cell.FormattedValue))
                {
                    if (!cell.Displayed)
                    {
                        gridArgs.FirstDisplayedCell = cell;
                    }
                    Rectangle rect = gridArgs.GetCellDisplayRectangle(cell.ColumnIndex, cell.RowIndex, true);
                    ShowError("Error", cell.ErrorText, OffsetToOrigin(gridArgs, new Point(rect.Left, rect.Bottom)));
                    return false;
                }

                string identifier = (string)cell.FormattedValue;
                string normalizedIdentifier = _provider.NormalizeIdentifier(identifier);
                if (normalizedIdentifier == "_selection")
                {
                    foreach (DataGridViewRow row2 in gridArgs.Rows)
                        row2.Selected = row2 == row;
                    ShowError("Error", "_selection is a reserved variable name and cannot be used", OffsetToOrigin(gridArgs, new Point(0, gridArgs.Height)));
                    return false;
                }
                if (usedIdentifiers.ContainsKey(normalizedIdentifier))
                {
                    DataGridViewRow prevRow = usedIdentifiers[normalizedIdentifier];
                    foreach (DataGridViewRow row2 in gridArgs.Rows)
                    {
                        row2.Selected = (row2 == row || row2 == prevRow);
                    }
                    gridArgs.FirstDisplayedScrollingRowIndex = prevRow.Index;
                    ShowError("Error", "Variable names must be unique", OffsetToOrigin(gridArgs, new Point(0, gridArgs.Height)));
                    return false;
                }
                usedIdentifiers.Add(normalizedIdentifier, row);
            }

            return true;
        }

        private void gridArgs_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                DataGridViewCell cell = gridArgs.Rows[e.RowIndex].Cells[0];
                ValidateCell(cell, (string)e.FormattedValue);
            }
        }

        private void gridArgs_RowValidating(object sender, DataGridViewCellCancelEventArgs e)
        {
            DataGridViewCell cell = gridArgs.Rows[e.RowIndex].Cells[0];
            ValidateCell(cell, (string)cell.FormattedValue);
        }

        private bool ValidateCell(DataGridViewCell cell, string identifier)
        {
            if (cell.OwningRow.IsNewRow)
                return true;

            string errorMessage;
            if (!_provider.IsValidIdentifier(identifier, out errorMessage))
            {
                cell.ErrorText = errorMessage;
                return false;
            }
            else
            {
                cell.ErrorText = "";
            }
            return true;
        }

        private Point OffsetToOrigin(Control reference, Point p)
        {
            return pnlOrigin.PointToClient(reference.PointToScreen(p));
        }
    }
}