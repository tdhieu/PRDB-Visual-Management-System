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
    public partial class Form_RenameRelation : Form
    {
        public Form_RenameRelation()
        {
            InitializeComponent();
        }

        private void ButtonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtRelName.Text == null)
                    MessageBox.Show("You have not entered a relation name!");
                else if (Resource.relationNames.Contains(txtRelName.Text))
                    MessageBox.Show("This relation name has already been used in the Database!");
                else
                {
                    Resource.curRelationName = txtRelName.Text;
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
            Resource.curRelationName = string.Empty;
            this.Close();
        }

        private void Form_RenameRelation_Load(object sender, EventArgs e)
        {
            txtRelName.Text = Resource.curRelationName;
            Resource.curRelationName = string.Empty;
            txtRelName.Focus();
        }
    }
}
