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
    public partial class Form_RenameQuery : Form
    {
        public Form_RenameQuery()
        {
            InitializeComponent();
        }

        private void Form_RenameQuery_Load(object sender, EventArgs e)
        {
            txtQueryName.Text = Resource.curQueryName;
            Resource.curQueryName = string.Empty;
            txtQueryName.Focus();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtQueryName.Text == null)
                    MessageBox.Show("You did not enter a query name!");
                else if (Resource.queryNames.Contains(txtQueryName.Text))
                    MessageBox.Show("This query name has already been used in the Database!");
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

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            Resource.curQueryName = string.Empty;
            this.Close();
        }
    }
}
