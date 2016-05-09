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
    public partial class Form_NewRelation : Form
    {
        public Form_NewRelation()
        {
            InitializeComponent();
        }

        private void Form_NewRelation_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.schemeNames;
            foreach (string item in items)
            {
                ComboBox_NewRelation.Items.Add(item);
            }

            if (ComboBox_NewRelation.Items.Count > 0) ComboBox_NewRelation.SelectedIndex = 0;

            Resource.curSchemeName = string.Empty;
            Resource.curRelationName = string.Empty;
            txtRelationName.Focus();
        }

        private void ButtonOK_NewTable_Click(object sender, EventArgs e)
        {
            if (ComboBox_NewRelation.SelectedItem == null) 
                MessageBox.Show("You have not selected a scheme name!");
            else 
            {
                Resource.curSchemeName = ComboBox_NewRelation.SelectedItem.ToString();

                if (txtRelationName.Text == "")
                    MessageBox.Show("You have not entered a relation name!");
                else if (Resource.relationNames.Contains(txtRelationName.Text))
                    MessageBox.Show("This relation name has already existed in the Database!");
                else
                {
                    Resource.curRelationName = txtRelationName.Text;
                    this.Close();
                }
            }
        }

        private void ButtonCancel_NewTable_Click(object sender, EventArgs e)
        {
            Resource.curRelationName = string.Empty;
            this.Close();
        }
    }
}
