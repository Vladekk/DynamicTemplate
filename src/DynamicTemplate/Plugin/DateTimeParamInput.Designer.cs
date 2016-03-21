namespace DynamicTemplate.Plugin
{
    partial class DateTimeParamInput
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
            this.dtValue = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // dtValue
            // 
            this.dtValue.CustomFormat = "M/d/yyyy h:mm:ss tt";
            this.dtValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtValue.Location = new System.Drawing.Point(0, 16);
            this.dtValue.Name = "dtValue";
            this.dtValue.Size = new System.Drawing.Size(171, 20);
            this.dtValue.TabIndex = 1;
            // 
            // DateTimeParamInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.Controls.Add(this.dtValue);
            this.Name = "DateTimeParamInput";
            this.Size = new System.Drawing.Size(250, 36);
            this.Controls.SetChildIndex(this.dtValue, 0);
            this.Controls.SetChildIndex(this.lblName, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker dtValue;
    }
}
