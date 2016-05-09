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
    public partial class Form_OpenScheme : Form
    {
        public Form_OpenScheme()
        {
            InitializeComponent();
        }

        private void Form_OpenScheme_Load(object sender, EventArgs e)
        {            
            List<string> items = Resource.schemeNames;
            foreach (string item in items)
            {
                ComboBox_OpenScheme.Items.Add(item);
            }

            if (ComboBox_OpenScheme.Items.Count > 0) ComboBox_OpenScheme.SelectedIndex = 0;
            ComboBox_OpenScheme.Focus();
            Resource.curSchemeName = string.Empty;
        }

        private void ButtonOK_OpenScheme_Click(object sender, EventArgs e)
        {
            if (ComboBox_OpenScheme.SelectedItem == null)
                MessageBox.Show("You have not selected a scheme");
            else
            {
                Resource.curSchemeName = ComboBox_OpenScheme.SelectedItem.ToString();
                this.Close();
            }
        }

        private void ButtonCancel_OpenScheme_Click(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            this.Close();
        }
    }
}
