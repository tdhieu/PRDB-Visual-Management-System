using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace PRDB_Visual_Management.PresentationLayer
{
    public partial class Form_DeleteScheme : Form
    {
        public Form_DeleteScheme()
        {
            InitializeComponent();
        }

        private void Form_DeleteScheme_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.schemeNames;
            foreach (string item in items)
            {
                ComboBox_DeleteScheme.Items.Add(item);
            }
            if (ComboBox_DeleteScheme.Items.Count > 0) ComboBox_DeleteScheme.SelectedIndex = 0;
            ComboBox_DeleteScheme.Focus();
        }

        private void ButtonOK_DeleteScheme_Click(object sender, EventArgs e)
        {
            if (ComboBox_DeleteScheme.SelectedItem == null) this.Close();
            if (ComboBox_DeleteScheme.SelectedItem.ToString().Equals("")) Resource.curSchemeName = string.Empty;
            else Resource.curSchemeName = ComboBox_DeleteScheme.SelectedItem.ToString();
            this.Close();
        }

        private void ButtonCancel_DeleteScheme_Click(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            this.Close();
        }
    }
}
