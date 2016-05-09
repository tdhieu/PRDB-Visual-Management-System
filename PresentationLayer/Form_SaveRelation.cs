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
    public partial class Form_SaveRelation : Form
    {
        public Form_SaveRelation()
        {
            InitializeComponent();
        }

        public string RelationName { get; set; }

        public string SchemeName { get; set; }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                SchemeName = "";
                if (radioButton_NewRelation.Checked)
                {
                    if (txtRelationName.Text == null)
                        MessageBox.Show("Enter new relation name");
                    else if (Resource.relationNames.Contains(txtRelationName.Text))
                        MessageBox.Show("This relation name has already existed");
                    else if (ComboBox_SchemeNames.SelectedItem == null)
                        MessageBox.Show("You have not selected a schema name");
                    else
                    {
                        SchemeName = txtRelationName.Text;
                        RelationName = ComboBox_SchemeNames.SelectedItem.ToString();
                        this.Close();
                    }
                }
                else
                {
                    if (ComboBox_RelationNames.SelectedItem == null)
                        MessageBox.Show("You have not selected a relation");
                    else
                    {
                        RelationName = ComboBox_RelationNames.SelectedItem.ToString();
                        this.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            SchemeName = null;
            RelationName = null;
            this.Close();
        }

        private void Form_SaveRelation_Load(object sender, EventArgs e)
        {
            try
            {
                SchemeName = null;
                RelationName = null;
                List<string> items = Resource.schemeNames;
                foreach (string item in items)
                {
                    ComboBox_SchemeNames.Items.Add(item);
                }
                ComboBox_SchemeNames.SelectedIndex = 0;
                txtRelationName.Focus();
                items = new List<string>();
                items = Resource.relationNames;
                foreach (string item in items) ComboBox_RelationNames.Items.Add(item);

                if (radioButton_NewRelation.Checked)
                {
                    txtRelationName.Enabled = true;
                    ComboBox_SchemeNames.Enabled = true;
                    ComboBox_SchemeNames.Enabled = false;
                }
                else
                {
                    txtRelationName.Enabled = false;
                    ComboBox_SchemeNames.Enabled = false;
                    ComboBox_SchemeNames.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void radioButton_NewRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_NewRelation.Checked)
            {
                txtRelationName.Enabled = true;
                ComboBox_SchemeNames.Enabled = true;
            }
            else
            {
                txtRelationName.Enabled = false;
                ComboBox_SchemeNames.Enabled = false;
            }
        }

        private void radioButton_SelectRelation_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SelectRelation.Checked)
                ComboBox_RelationNames.Enabled = true;
            else ComboBox_RelationNames.Enabled = false;
        }

    }
}
