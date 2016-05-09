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
    public partial class Form_OpenQuery : Form
    {
        public Form_OpenQuery()
        {
            InitializeComponent();
        }

        private void Form_OpenQuery_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.queryNames;
            foreach (string item in items)
            {
                ComboBox_OpenQuery.Items.Add(item);
            }
            
            if (ComboBox_OpenQuery.Items.Count > 0) ComboBox_OpenQuery.SelectedIndex = 0;
            ComboBox_OpenQuery.Focus();
            Resource.curQueryName = string.Empty;
        }

        private void ButtonOK_OpenQuery_Click(object sender, EventArgs e)
        {
            if (ComboBox_OpenQuery.SelectedItem == null)
                MessageBox.Show("You have not selected a query");
            else
            {
                Resource.curQueryName = ComboBox_OpenQuery.SelectedItem.ToString();
                this.Close();
            }
        }

        private void ButtonCancel_OpenQuery_Click(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            this.Close();
        }


    }
}
