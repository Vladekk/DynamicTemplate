namespace DynamicTemplate.Plugin
{
    partial class BooleanParamInput
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.chkValue = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.Visible = false;
            // 
            // chkValue
            // 
            this.chkValue.AutoSize = true;
            this.chkValue.Location = new System.Drawing.Point(0, 0);
            this.chkValue.Name = "chkValue";
            this.chkValue.Size = new System.Drawing.Size(80, 17);
            this.chkValue.TabIndex = 1;
            this.chkValue.Text = "checkBox1";
            this.chkValue.UseVisualStyleBackColor = true;
            // 
            // BooleanParamInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.Controls.Add(this.chkValue);
            this.Name = "BooleanParamInput";
            this.Size = new System.Drawing.Size(250, 19);
            this.Controls.SetChildIndex(this.chkValue, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox chkValue;
    }
}
