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
    public partial class Form_OpenRelation : Form
    {
        public Form_OpenRelation()
        {
            InitializeComponent();
        }

        private void Form_OpenRelation_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.relationNames;
            foreach (string item in items)
            {
                ComboBox_OpenRelation.Items.Add(item);
            }

            if (ComboBox_OpenRelation.Items.Count > 0) ComboBox_OpenRelation.SelectedIndex = 0;
            ComboBox_OpenRelation.Focus();
            Resource.curRelationName = null;
        }

        private void ButtonOK_OpenRelation_Click(object sender, EventArgs e)
        {
            if (ComboBox_OpenRelation.SelectedItem == null)
                MessageBox.Show("You have not selected a relation");
            else
            {
                Resource.curRelationName = ComboBox_OpenRelation.SelectedItem.ToString();
                this.Close();
            }
        }

        private void ButtonCancel_OpenRelation_Click(object sender, EventArgs e)
        {
            Resource.curRelationName = string.Empty;
            this.Close();
        }
    }
}
