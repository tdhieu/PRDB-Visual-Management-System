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
    public partial class Form_SaveQuery : Form
    {
        public Form_SaveQuery()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButton_NewQuery.Checked)
                {
                    if (txtQueryName.Text == string.Empty)
                        MessageBox.Show("Enter new query name!");
                    else if (Resource.queryNames.Contains(txtQueryName.Text))
                        MessageBox.Show("This query name has already existed in the Database!");
                    else
                    {
                        Resource.curQueryName = txtQueryName.Text;
                        this.Close();
                    }
                }
                else
                {
                    if (ComboBox_QueryNames.SelectedItem == string.Empty)
                        MessageBox.Show("You have not selected a query!");
                    else
                    {
                        Resource.curQueryName = ComboBox_QueryNames.SelectedItem.ToString();
                        this.Close();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Form_SaveQuery_Load(object sender, EventArgs e)
        {
            try
            {
                List<string> items = Resource.queryNames;
                foreach (string item in items)
                {
                    ComboBox_QueryNames.Items.Add(item);
                }
                if (ComboBox_QueryNames.Items.Count > 0) ComboBox_QueryNames.SelectedIndex = 0;
                txtQueryName.Focus();
                Resource.curQueryName = string.Empty;
                if (radioButton_NewQuery.Checked)
                {
                    txtQueryName.Enabled = true;
                    ComboBox_QueryNames.Enabled = false;
                }
                else
                {
                    txtQueryName.Enabled = false;
                    ComboBox_QueryNames.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            this.Close();
        }

        private void radioButton_SelectQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_SelectQuery.Checked)
            {
                txtQueryName.Enabled = false;
                ComboBox_QueryNames.Enabled = true;
            }
        }

        private void radioButton_NewQuery_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton_NewQuery.Checked)
            {
                txtQueryName.Enabled = true;
                ComboBox_QueryNames.Enabled = false;
            }
        }
    }
}
