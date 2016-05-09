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
    public partial class Form_SaveScheme : Form
    {
        public Form_SaveScheme()
        {
            InitializeComponent();
        }

        private void ButtonOK_SaveScheme_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton_NewScheme.Checked)
                {
                    if (txtSchemeName.Text == null)
                        MessageBox.Show("Enter new scheme name!");
                    else if (Resource.schemeNames.Contains(txtSchemeName.Text))
                        MessageBox.Show("This scheme name has already been used!");
                    else
                    {
                        Resource.curSchemeName = txtSchemeName.Text;
                        this.Close();
                    }
                }
                else
                {
                    if (ComboBox_SchemeNames.Items.Count == 0)
                    {
                        Resource.curSchemeName = string.Empty;
                        this.Close();
                    }
                    else
                        if (ComboBox_SchemeNames.SelectedItem == null)
                            MessageBox.Show("You have not selected a scheme!");
                        else
                        {
                            Resource.curSchemeName = ComboBox_SchemeNames.SelectedItem.ToString();
                            this.Close();
                        }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void ButtonCancel_SaveScheme_Click(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            this.Close();
        }

        private void Form_SaveScheme_Load(object sender, EventArgs e)
        {
            try
            {
                List<string> items = Resource.schemeNames;
                foreach (string item in items)
                {
                    ComboBox_SchemeNames.Items.Add(item);
                }

                if (ComboBox_SchemeNames.Items.Count > 0) ComboBox_SchemeNames.SelectedIndex = 0;
                
                txtSchemeName.Focus();
                Resource.curSchemeName = string.Empty;

                if (radioButton_NewScheme.Checked)
                {
                    txtSchemeName.Enabled = true;
                    ComboBox_SchemeNames.Enabled = false;
                }
                else
                {
                    txtSchemeName.Enabled = false;
                    ComboBox_SchemeNames.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void radioButton_NewScheme_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_NewScheme.Checked)
                txtSchemeName.Enabled = true;
            else txtSchemeName.Enabled = false;
        }

        private void radioButton_SelectScheme_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SelectScheme.Checked)
                ComboBox_SchemeNames.Enabled = true;
            else ComboBox_SchemeNames.Enabled = false;
        }
    }
}
