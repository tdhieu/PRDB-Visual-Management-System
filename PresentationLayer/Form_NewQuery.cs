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
    public partial class Form_NewQuery : Form
    {
        public Form_NewQuery()
        {
            InitializeComponent();
        }

        private void ButtonOK_NewQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQueryName.Text == string.Empty)
                    MessageBox.Show("You have not enter a query name!");
                else if (Resource.queryNames.Contains(txtQueryName.Text))
                    MessageBox.Show("The query name has already been used in the Database!");
                else
                {
                    Resource.curQueryName = txtQueryName.Text;
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
        private void ButtonCancel_NewQuery_Click(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            this.Close();
        }

        private void Form_NewQuery_Load(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            txtQueryName.Focus();
        }
    }
}
