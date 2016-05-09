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
    public partial class Form_DeleteRelation : Form
    {
        public Form_DeleteRelation()
        {
            InitializeComponent();
        }

        private void Form_DeleteRelation_Load(object sender, EventArgs e)
        {
            List<string> items = Resource.relationNames;
            foreach (string item in items)
            {
                ComboBox_DeleteRelation.Items.Add(item);
            }

            if (ComboBox_DeleteRelation.Items.Count > 0) ComboBox_DeleteRelation.SelectedIndex = 0;
            ComboBox_DeleteRelation.Focus();
        }

        private void ButtonOK_DeleteRelation_Click(object sender, EventArgs e)
        {
            if (ComboBox_DeleteRelation.SelectedItem.ToString().Equals("")) Resource.curRelationName = null;
            else Resource.curRelationName = ComboBox_DeleteRelation.SelectedItem.ToString();
            this.Close();
        }

        private void ButtonCancel_DeleteRelation_Click(object sender, EventArgs e)
        {
            Resource.curRelationName = null;
            this.Close();
        }
    }
}
