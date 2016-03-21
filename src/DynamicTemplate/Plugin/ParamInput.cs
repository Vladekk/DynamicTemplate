using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace DynamicTemplate.Plugin
{
    public partial class ParamInput : UserControl
    {
        public ParamInput()
        {
            InitializeComponent();
        }

        public virtual string ParamLabel
        {
            set { lblName.Text = value; }
        }

        public virtual object Value
        {
            get { return null; }
        }

        private void ParamInput_Load(object sender, EventArgs e)
        {
            lblName.SendToBack();
        }
    }
}
