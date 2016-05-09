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
    public partial class Form_DeleteQuery : Form
    {
        public Form_DeleteQuery()
        {
            InitializeComponent();
        }

        private void Form_DeleteQuery_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.queryNames;
            foreach (string item in items)
            {
                ComboBox_DeleteQuery.Items.Add(item);
            }
            
            if (ComboBox_DeleteQuery.Items.Count > 0) ComboBox_DeleteQuery.SelectedIndex = 0;
            ComboBox_DeleteQuery.Focus();
        }

        private void ButtonOK_DeleteQuery_Click(object sender, EventArgs e)
        {
            if (ComboBox_DeleteQuery.SelectedItem.ToString().Equals("")) Resource.curQueryName = string.Empty;
            else Resource.curQueryName = ComboBox_DeleteQuery.SelectedItem.ToString();
            this.Close();
        }

        private void ButtonCancel_DeleteQuery_Click(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            this.Close();
        }
    }
}
