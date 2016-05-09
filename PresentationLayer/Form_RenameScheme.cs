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
    public partial class Form_RenameScheme : Form
    {
        public Form_RenameScheme()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (tbxSchemeName.Text == string.Empty)
                    MessageBox.Show("You have not entered a scheme name!");
                else if (Resource.schemeNames.Contains(tbxSchemeName.Text))
                    MessageBox.Show("This scheme name has already been used in the Database!");
                else
                {
                    Resource.curSchemeName = tbxSchemeName.Text;
                    this.Close();
                }

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            this.Close();
        }

        private void Form_RenameScheme_Load(object sender, EventArgs e)
        {
            tbxSchemeName.Text = Resource.curSchemeName;
            Resource.curSchemeName = string.Empty;
            tbxSchemeName.Focus();
        }

    }
}
