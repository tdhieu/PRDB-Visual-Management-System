using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using PRDB_Visual_Management.DataAccessLayer;
using System.Windows.Forms;

namespace PRDB_Visual_Management.PresentationLayer
{
    public partial class Form_NewScheme : Form
    {
        public Form_NewScheme()
        {
            InitializeComponent();
        }

        private void ButtonOK_NewTable_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSchemeName.Text == string.Empty)
                    MessageBox.Show("You have not entered a schema name");
                else if (Resource.schemeNames.Contains(txtSchemeName.Text))
                    MessageBox.Show("This scheme name has already existed in the database");
                else
                {
                    Resource.curSchemeName = txtSchemeName.Text;
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void ButtonCancel_NewTable_Click(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            this.Close();
        }

        private void Form_NewScheme_Load(object sender, EventArgs e)
        {
            Resource.curSchemeName = string.Empty;
            txtSchemeName.Focus();
        }

    }
}
