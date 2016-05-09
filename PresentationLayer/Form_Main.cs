using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PRDB_Visual_Management.DataAccessLayer;
using PRDB_Visual_Management.BusinessLogicLayer;
using PRDB_Visual_Management.PresentationLayer;
using PRDB_Visual_Management.ProbSQLCompiler;
using DevExpress.XtraTab;
using System.Timers;

namespace PRDB_Visual_Management
{
    public partial class Form_Main : Form
    {
        public Form_Main()
        {
            InitializeComponent();
        }

        //////////////////////////////////////////////////// Properties Declaration /////////////////////////////////////////  

        #region * Form Declarations

        Form_Connecting frm_CF;
        Form_DeleteQuery frm_DelQuery;
        Form_DeleteRelation frm_DelRelation;
        Form_DeleteScheme frm_Delscheme;
        Form_InputType frm_InpType;
        Form_NewQuery frm_NewQuery;
        Form_NewScheme frm_newScheme;
        Form_NewRelation frm_NewRelation;
        Form_OpenQuery frm_OpenQuery;
        Form_OpenRelation frm_OpenRelation;
        Form_OpenScheme frm_Openscheme;
        Form_RenameDB frm_RenameDB;
        Form_Saving frm_Saving;
        Form_SaveScheme frm_SaveScheme;
        //Form_SaveRelation frm_SaveRelation;
        Form_SaveQuery frm_SaveQuery;
        Form_RenameRelation frm_RenameRelation;
        Form_RenameScheme frm_RenameScheme;
        Form_RenameQuery frm_RenameQuery;

        #endregion

        #region * PRDB objects

        ProbDatabase DB;

        #endregion

        #region * TreeView
        TreeNode NodeDB, NodeScheme, NodeRelation, NodeQuery, CurrentNode, NewNode, ChildNode;
        #endregion

        #region * Class
        clsProcess clsProcess;
        #endregion

        #region * Images
        public struct ImageIndex
        {
            public int UnselectedState;
            public int SelectedState;
        }

        public struct AttributeImageIndex
        {
            public int PrimaryKey;
            public int NonPrimaryKey;
        }
        #endregion

        #region * Variables

        int CurrentRow, CurrentCell;
        ImageIndex DB_ImgIndex, Folder_ImgIndex, Scheme_ImgIndex, Relation_ImgIndex, Query_ImgIndex;
        AttributeImageIndex Attribute_ImgIndex;
        bool validated, flag;
        bool Form_InputType_Opening;
        bool Form_DeleteScheme_Opening;
        bool Form_DeleteRelation_Opening;
        bool Form_DeleteQuery_Opening;
        bool Form_NewScheme_Opening;
        bool Form_NewRelation_Opening;
        bool Form_NewQuery_Opening;
        bool Form_OpenScheme_Opening;
        bool Form_OpenRelation_Opening;
        bool Form_OpenQuery_Opening;
        bool Form_RenameDB_Opening;
        bool Form_RenameQuery_Opening;
        bool Form_RenameRelation_Opening;
        bool Form_RenameScheme_Opening;
        bool Form_SaveScheme_Opening;       
        bool Form_SaveRelation_Opening;
        // bool Form_About_Opening;
        bool Form_SaveQuery_Opening;
        System.Timers.Timer timer;

        #endregion       

        ///////////////////////////////////////////////////// Menu Process /////////////////////////////////////////////////        

        #region * Menu Database

        private void CreateNewDatabase()
        {
            try
            {
                SaveFileDialog DialogSave = new SaveFileDialog();                                   // Save dialog
                DialogSave.DefaultExt = "pdb";                                                      // Default extension
                DialogSave.Filter = "Database file (*.pdb)|*.pdb|All files (*.*)|*.*";              // add extension to dialog
                DialogSave.AddExtension = true;                                                     // enable adding extension
                DialogSave.RestoreDirectory = true;                                                 // Restore directory for next time visiting
                DialogSave.Title = "Create new Database...";
                DialogSave.InitialDirectory = clsProcess.GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogSave.SupportMultiDottedExtensions = true;

                if (DialogSave.ShowDialog() == DialogResult.OK)
                {
                    lblStatus.Text = "Creating new Database...";
                    timer.Start();

                    DB = null;
                    TreeView.Nodes.Clear();

                    DB = new ProbDatabase(DialogSave.FileName);
                    Resource.dbName = DB.dbName;
                    Resource.connectionString = DB.connectionString;

                    if (!clsProcess.CreateNewDatabase(DB))
                        throw new Exception("Cannot create new database!");

                    Load_TreeView();
                    ActivateDatabase(true);
                }
                DialogSave.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void OpenDatabase()
        {
            try
            {
                OpenFileDialog DialogOpen = new OpenFileDialog();
                DialogOpen.DefaultExt = "pdb";
                DialogOpen.Filter = "Database file (*.pdb)|*.pdb";
                DialogOpen.AddExtension = true;
                DialogOpen.RestoreDirectory = true;
                DialogOpen.Title = "Open Database...";
                DialogOpen.InitialDirectory = clsProcess.GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogOpen.SupportMultiDottedExtensions = true;

                if (DialogOpen.ShowDialog() == DialogResult.OK)
                {
                    lblStatus.Text = "Opening Database...";
                    timer.Start();
                    TreeView.Nodes.Clear();
                    DB = null;

                    DB = clsProcess.NewDatabase(DialogOpen.FileName);
                    Resource.dbName = DB.dbName;
                    Resource.connectionString = DB.connectionString;

                    Cursor oldCursor = Cursor;
                    Cursor = Cursors.WaitCursor;

                    frm_CF = new Form_Connecting();
                    frm_CF.Show();
                    frm_CF.Refresh();

                    bool success = clsProcess.Connect();
                    success = success && clsProcess.LoadDatabase(DB);

                    frm_CF.Close();
                    Cursor = oldCursor;

                    if (!success)
                    {
                        clsProcess.Dispose();
                        throw new Exception("Cannot connect to the physical Database!");
                    }
                    else
                    {
                        ActivateDatabase(true);
                        Load_TreeView();
                    }
                }
                DialogOpen.Dispose();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SaveDatabase()
        {
            try
            {
                // Record to database
                Cursor oldCursor = Cursor;
                Cursor = Cursors.WaitCursor;

                frm_Saving = new Form_Saving();
                frm_Saving.Show();
                frm_Saving.Refresh();

                clsProcess.DropDatabaseData();
                if (!clsProcess.SaveDatabase(DB))
                {
                    MessageBox.Show("Cannnot save the Database!");
                    lblStatus.Text = "The Database has not been saved!";
                    timer.Start();
                }
                else
                {
                    lblStatus.Text = "The Database has been saved!";
                    timer.Start();
                }

                frm_Saving.Close();
                Cursor = oldCursor;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SaveDatabaseNewName()
        {
            try
            {
                SaveFileDialog DialogSave = new SaveFileDialog();
                DialogSave.DefaultExt = "pdb";                                                      // Default extension
                DialogSave.Filter = "Database file (*.pdb)|*.pdb|All files (*.*)|*.*";              // Ddd extension to dialog
                DialogSave.AddExtension = true;                                                     // Enable adding extension
                DialogSave.RestoreDirectory = true;                                                 // Tu dong phuc hoi duong dan cho lan sau
                DialogSave.Title = "Save as...";
                DialogSave.InitialDirectory = clsProcess.GetRootPath(AppDomain.CurrentDomain.BaseDirectory.ToString());
                DialogSave.SupportMultiDottedExtensions = true;

                if (DialogSave.ShowDialog() == DialogResult.OK)
                {
                    lblStatus.Text = "Saving Database...";
                    timer.Start();

                    DB = null;
                    TreeView.Nodes.Clear();

                    DB = new ProbDatabase(DialogSave.FileName);
                    Resource.dbName = DB.dbName;
                    Resource.connectionString = DB.connectionString;
                    Resource.dbShowName = "DB_" + DB.dbName.ToUpper();

                    NodeDB.Text = Resource.dbShowName;
                    NodeDB.ToolTipText = "Database " + DB.dbName;

                    Cursor oldCursor = Cursor;
                    Cursor = Cursors.WaitCursor;

                    frm_Saving = new Form_Saving();
                    frm_Saving.Show();
                    frm_Saving.Refresh();

                    if (!clsProcess.SaveDatabaseNewName(DB))
                    {
                        MessageBox.Show("Cannnot save the Database!");
                        lblStatus.Text = "The Database has not been saved!";
                        timer.Start();
                    }
                    else
                    {
                        lblStatus.Text = "The Database has been saved!";
                        timer.Start();
                    }

                    frm_Saving.Close();
                    Cursor = oldCursor;
                    Load_TreeView();
                    ActivateDatabase(true);
                }
                DialogSave.Dispose();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CloseDatabase()
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure want to close this Database ?", "Close database " + DB.dbName + "...", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    TreeView.Nodes.Clear();
                    DB = null;
                    ResetObject();
                    ActivateDatabase(false);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuBar_NewDatabase_Click(object sender, EventArgs e)
        {
            CreateNewDatabase();
        }

        private void MenuBar_OpenDatabase_Click(object sender, EventArgs e)
        {
            OpenDatabase();
        }

        private void MenuBar_SaveDatabase_Click(object sender, EventArgs e)
        {
            SaveDatabase();
        }

        private void MenuBar_SaveDatabaseAs_Click(object sender, EventArgs e)
        {
            SaveDatabaseNewName();
        }

        private void MenuBar_CloseDatabase_Click(object sender, EventArgs e)
        {
            CloseDatabase();
        }

        private void MenuBar_Exit_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure want to exit?", "Exit PRDB Visual Management System", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    DB = null;
                    this.Close();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        #region * Menu Scheme

        private void MenuScheme_CreateNew_Click(object sender, EventArgs e)
        {
            ResetSchemePage(true);
            CreateNewScheme();
        }

        void frm_NewScheme_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_NewScheme_Opening = false;
                string schemeName = Resource.curSchemeName;
                if (schemeName != null)
                {
                    ProbScheme newScheme = new ProbScheme(schemeName);
                    DB.schemes.Add(newScheme);
                    TreeNode NewNode = new TreeNode();
                    NewNode.Name = schemeName;
                    NewNode.Text = schemeName;
                    NewNode.ToolTipText = "Scheme " + schemeName;
                    NewNode.ContextMenuStrip = ContextMenu_SchemeNode;
                    NewNode.ImageIndex = Scheme_ImgIndex.UnselectedState;
                    NewNode.SelectedImageIndex = Scheme_ImgIndex.UnselectedState;
                    NodeScheme.Nodes.Add(NewNode);

                    foreach (ProbAttribute attribute in newScheme.attributes)
                    {
                        ChildNode = new TreeNode();
                        ChildNode.Text = attribute.attributeName;
                        ChildNode.Name = attribute.attributeName;
                        ChildNode.ToolTipText = "Attribute " + attribute.attributeName;

                        if (attribute.IsPrimaryKey())
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                        }
                        else
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                        }
                        NewNode.Nodes.Add(ChildNode);
                    }                
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuScheme_SaveScheme_Click(object sender, EventArgs e)
        {
            SaveScheme();
        }

        void frm_SaveScheme_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_SaveScheme_Opening = false;

                // Scheme name taken from Form_SaveScheme
                string schemeName = Resource.curSchemeName;

                if (schemeName != string.Empty)
                {
                    ProbScheme newScheme = DB.GetScheme(schemeName);

                    if (newScheme == null)
                    {
                        newScheme = new ProbScheme(schemeName);
                        DB.schemes.Add(newScheme);

                        // Update to Object Treeview's node
                        TreeNode NewNode = new TreeNode();
                        NewNode.Name = schemeName;
                        NewNode.Text = schemeName;
                        NewNode.ToolTipText = "Scheme " + schemeName;
                        NewNode.ContextMenuStrip = ContextMenu_SchemeNode;
                        NewNode.ImageIndex = Scheme_ImgIndex.UnselectedState;
                        NewNode.SelectedImageIndex = Scheme_ImgIndex.UnselectedState;
                        NodeScheme.Nodes.Add(NewNode);

                        foreach (ProbAttribute attr in newScheme.attributes)
                        {
                            ChildNode = new TreeNode();
                            ChildNode.Text = attr.attributeName;
                            ChildNode.Name = attr.attributeName;
                            ChildNode.ToolTipText = "Attribute " + attr.attributeName;

                            if (attr.IsPrimaryKey())
                            {
                                ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                                ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                            }
                            else
                            {
                                ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                                ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                            }
                            NewNode.Nodes.Add(ChildNode);
                        }
                    }

                    xtraTabDatabase.TabPages[0].Text = "Scheme " + schemeName;
                    xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[0]; ;
                    int nRow = GridViewDesign.Rows.Count - 1;
                    GridViewDesign.CurrentCell = GridViewDesign.Rows[nRow].Cells[0];

                    ProbAttribute attribute;
                    for (int i = 0; i < nRow; i++)
                    {
                        attribute = new ProbAttribute();
                        attribute.primaryKey = Convert.ToBoolean(GridViewDesign.Rows[i].Cells[0].Value);
                        attribute.attributeName = GridViewDesign.Rows[i].Cells[1].Value.ToString();
                        attribute.type.GetDataType(GridViewDesign.Rows[i].Cells[2].Value.ToString());
                        attribute.type.GetDomain(GridViewDesign.Rows[i].Cells[3].Value.ToString());
                        attribute.description = (GridViewDesign.Rows[i].Cells[4].Value == null ? "" : GridViewDesign.Rows[i].Cells[4].Value.ToString());                        
                        newScheme.attributes.Add(attribute);
                    }
                }
                else throw new Exception("The scheme has not a name yet!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuScheme_Open_Click(object sender, EventArgs e)
        {
            ResetSchemePage(true);
            OpenScheme();
        }

        void frm_OpenScheme_Disposed(object sender, EventArgs e)
        {
            try
            {
                string schemeName = Resource.curSchemeName;
                if (schemeName != string.Empty)
                {
                    ProbScheme currentScheme = DB.GetScheme(schemeName);
                    xtraTabDatabase.TabPages[0].Text = "Scheme " + schemeName;
                    xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[0];

                    if (currentScheme.isInherited(DB.relations))
                    {
                        GridViewDesign.Columns[0].ReadOnly = true;
                        GridViewDesign.Columns[1].ReadOnly = true;
                        GridViewDesign.Columns[2].ReadOnly = true;
                    }
                    else
                    {
                        GridViewDesign.Columns[0].ReadOnly = false;
                        GridViewDesign.Columns[1].ReadOnly = false;
                        GridViewDesign.Columns[2].ReadOnly = false;
                    }

                    int i, n = GridViewDesign.Rows.Count - 2;

                    for (i = n; i >= 0; i--)
                        GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);

                    lblDesignRowNumberIndicator.Text = "1 / 1";

                    i = 0;
                    CheckBox chkbox;
                    foreach (ProbAttribute attr in currentScheme.attributes)
                    {
                        GridViewDesign.Rows.Add();
                        chkbox = new CheckBox();
                        chkbox.Checked = attr.primaryKey;
                        GridViewDesign.Rows[i].Cells[0].Value = chkbox.CheckState;
                        GridViewDesign.Rows[i].Cells[1].Value = attr.attributeName;
                        GridViewDesign.Rows[i].Cells[2].Value = attr.type.typeName;
                        GridViewDesign.Rows[i].Cells[3].Value = attr.type.domainString;
                        GridViewDesign.Rows[i].Cells[4].Value = (attr.description != null ? attr.description : null);
                        i++;
                    }
                    GridViewDesign.CurrentCell = GridViewDesign.Rows[i].Cells[0];
                    if (GridViewDesign.CurrentRow != null)
                        lblDesignRowNumberIndicator.Text = (GridViewDesign.CurrentRow.Index + 1).ToString() + " / " + GridViewDesign.Rows.Count.ToString();
                    else lblDesignRowNumberIndicator.Text = "1 / " + GridViewDesign.Rows.Count.ToString();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuScheme_Delete_Click(object sender, EventArgs e)
        {
            DeleteScheme();
        }

        void frm_DelScheme_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_DeleteScheme_Opening = false;
                string schemeName = Resource.curSchemeName;
                if (schemeName != string.Empty)
                {
                    ProbScheme deleteScheme = DB.GetScheme(schemeName);

                    if (InheritedScheme(deleteScheme))
                        throw new Exception("Cannot delete this scheme because it is inherited by some relations!");

                    DialogResult result = new DialogResult();
                    result = MessageBox.Show("Are you sure want to delete this scheme ?", "Delete scheme " + schemeName, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string tabSchemeName = GetSchemeName(xtraTabDatabase.TabPages[0].Text);

                        if (tabSchemeName.Equals(schemeName))
                        {
                            ResetSchemePage(true);
                            xtraTabDatabase.TabPages[0].Text = "Scheme";
                            int n = GridViewDesign.Rows.Count - 2;
                            for (int i = n; i >= 0; i--)
                                GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
                            lblDesignRowNumberIndicator.Text = "1 / 1";
                        }

                        TreeNode DeletedNode = NodeScheme.Nodes[schemeName];
                        DeletedNode.Remove();
                        DB.schemes.Remove(deleteScheme);
                        deleteScheme = null;

                        if (NodeScheme.Nodes.Count == 0)
                            NodeScheme.ImageIndex = NodeScheme.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private bool InheritedScheme(ProbScheme scheme)
        {
            try
            {
                foreach (ProbRelation rel in this.DB.relations)
                    if (rel.scheme.Equals(scheme))
                        return true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
            return false;
        }

        private void MenuScheme_CloseScheme_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSchemePage(true);
                xtraTabDatabase.TabPages[0].Text = "Scheme";
                int n = GridViewDesign.Rows.Count-2;
                for (int i = n; i >= 0; i--)
                    GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
                lblDesignRowNumberIndicator.Text = "1 / 1";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CreateNewScheme()
        {
            try
            {
                if (!Form_NewScheme_Opening)
                {
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Form_NewScheme_Opening = true;
                    Resource.schemeNames = DB.ListOfSchemeName();
                    frm_newScheme = new Form_NewScheme();
                    frm_newScheme.Show();
                }
                frm_newScheme.Disposed += new EventHandler(frm_NewScheme_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private string GetSchemeName(string tabName)
        {
            try
            {
                int index = tabName.IndexOf("Scheme");
                index += 6;
                string schemeName = tabName.Trim().Substring(index);
                schemeName = schemeName.Replace("*", "").Trim();
                return schemeName;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return string.Empty;
            }
        }

        private void SaveScheme()
        {
            try
            {
                if (DB == null) throw new Exception("Cannot find the Database!");

                // If scheme's name on tabScheme does not contain asteroid character
                if (!xtraTabDatabase.TabPages[0].Text.Contains("*")) return;

                // Get scheme name from tabScheme name
                string schemeName = GetSchemeName(xtraTabDatabase.TabPages[0].Text);

                // Save a new created scheme
                if (schemeName == string.Empty)
                {
                    if (!Form_SaveScheme_Opening)
                    {
                        Form_SaveScheme_Opening = true;
                        Resource.schemeNames = DB.ListOfSchemeName();
                        frm_SaveScheme = new Form_SaveScheme();
                        frm_SaveScheme.Show();
                    }
                    frm_SaveScheme.Disposed += new EventHandler(frm_SaveScheme_Disposed);
                }
                // Save an existing scheme
                else
                {
                    ProbScheme currentScheme = DB.GetScheme(schemeName);

                    if (currentScheme == null) 
                        throw new Exception("There is no scheme " + schemeName + " in the Database!");

                    // If the new scheme does not have any attribute, we dont need to save
                    if (GridViewDesign.Rows.Count <= 1) return;

                    xtraTabDatabase.TabPages[0].Text = "Scheme " + schemeName;
                    xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[0];

                    int nRow = GridViewDesign.Rows.Count - 1;
                    currentScheme.attributes.Clear();
                    GridViewDesign.CurrentCell = GridViewDesign.Rows[nRow].Cells[0];

                    ProbAttribute attribute;
                    for (int i = 0; i < nRow; i++)
                    {
                        attribute = new ProbAttribute();
                        attribute.primaryKey = Convert.ToBoolean(GridViewDesign.Rows[i].Cells[0].Value);
                        attribute.attributeName = GridViewDesign.Rows[i].Cells[1].Value.ToString();
                        attribute.type.GetDataType(GridViewDesign.Rows[i].Cells[2].Value.ToString());
                        attribute.type.GetDomain(GridViewDesign.Rows[i].Cells[3].Value.ToString());
                        attribute.description = (GridViewDesign.Rows[i].Cells[4].Value == null ? "" : GridViewDesign.Rows[i].Cells[4].Value.ToString());                        
                        currentScheme.attributes.Add(attribute);
                    }

                    // Update Treeview
                    TreeNode updatedNode = new TreeNode();                    

                    foreach (TreeNode selectedNode in NodeScheme.Nodes)
                        if (selectedNode.Name.Equals(schemeName))
                        {
                            updatedNode = selectedNode;
                            break;
                        }

                    updatedNode.Nodes.Clear();

                    foreach (ProbAttribute attr in currentScheme.attributes)
                    {
                        ChildNode = new TreeNode();
                        ChildNode.Text = attr.attributeName;
                        ChildNode.Name = attr.attributeName;
                        ChildNode.ToolTipText = "Attribute " + attr.attributeName;

                        if (attr.IsPrimaryKey())
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                        }
                        else
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                        }
                        updatedNode.Nodes.Add(ChildNode);
                    }

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void OpenScheme()
        {
            try
            {
                if (!Form_OpenScheme_Opening)
                {
                    Form_OpenScheme_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.schemeNames = DB.ListOfSchemeName();
                    frm_Openscheme = new Form_OpenScheme();
                    frm_Openscheme.Show();                    
                }
                frm_Openscheme.Disposed += new EventHandler(frm_OpenScheme_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void DeleteScheme()
        {
            try
            {
                if (!Form_DeleteScheme_Opening)
                {
                    Form_DeleteScheme_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.schemeNames = DB.ListOfSchemeName();
                    frm_Delscheme = new Form_DeleteScheme();
                    frm_Delscheme.Show();                    
                }
                frm_Delscheme.Disposed += new EventHandler(frm_DelScheme_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        #region * Menu Relation

        private void MenuRelation_New_Click(object sender, EventArgs e)
        {
            ResetRelationPage(true);
            ResetInputValue(true);
            CreateNewRelation();
        }

        private void MenuRelation_SaveRelation_Click(object sender, EventArgs e)
        {
            SaveRelation();
        }

        private void MenuRelation_OpenRelation_Click(object sender, EventArgs e)
        {
            ResetRelationPage(true);
            ResetInputValue(true);
            OpenRelation();
        }       

        private void MenuRelation_Delete_Click(object sender, EventArgs e)
        {
            DeleteRelation();
        }

        private void ClearAllValue()
        {
            GridViewValue.Rows.Clear();
            UpdateValueRowNumber();
            txtMaxProb.Text = "";
            txtMinProb.Text = "";
            txtValue.Text = "";
        }

        private void MenuRelation_CloseRelation_Click(object sender, EventArgs e)
        {
            try
            {
                ResetRelationPage(true);
                ResetInputValue(true);
                xtraTabDatabase.TabPages[1].Text = "Relation";
                GridViewData.Rows.Clear();
                GridViewData.Columns.Clear();
                UpdateDataRowNumber();
                SwitchValueState(true);
                ClearAllValue();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }            
        }

        private void CreateNewRelation()
        {
            try
            {
                if (!Form_NewRelation_Opening)
                {
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    if (DB.schemes.Count == 0) throw new Exception("You must create some schemes first!");
                    Form_NewRelation_Opening = true;
                    Resource.schemeNames = DB.ListOfSchemeName();
                    Resource.relationNames = DB.ListOfRelationName();
                    frm_NewRelation = new Form_NewRelation();
                    frm_NewRelation.Show();
                }
                frm_NewRelation.Disposed += new EventHandler(frm_NewRelation_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_NewRelation_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_NewRelation_Opening = false;
                string relationName = Resource.curRelationName;
                string schemeName = Resource.curSchemeName;

                if (relationName != string.Empty)
                {
                    ProbRelation newRelation = new ProbRelation(relationName);
                    newRelation.scheme = DB.GetScheme(schemeName);
                    DB.relations.Add(newRelation);
                    TreeNode NewNode = new TreeNode();
                    NewNode.Text = relationName;
                    NewNode.Name = relationName;
                    NewNode.ToolTipText = "Relation " + relationName;
                    NewNode.ContextMenuStrip = ContextMenu_RelationNode;
                    NewNode.ImageIndex = Relation_ImgIndex.UnselectedState;
                    NewNode.SelectedImageIndex = Relation_ImgIndex.UnselectedState;
                    NodeRelation.Nodes.Add(NewNode);

                    foreach (ProbAttribute attribute in newRelation.scheme.attributes)
                    {
                        ChildNode = new TreeNode();
                        ChildNode.Text = attribute.attributeName;
                        ChildNode.Name = attribute.attributeName;
                        ChildNode.ToolTipText = "Attribute " + attribute.attributeName;

                        if (attribute.IsPrimaryKey())
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                        }
                        else
                        {
                            ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                            ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                        }
                        NewNode.Nodes.Add(ChildNode);
                    }     
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SaveRelation()
        {
            try
            {
                if (DB == null) throw new Exception("Cannot find the Database!");

                // If relation's name on tabScheme does not contain asteroid character
                if (!xtraTabDatabase.TabPages[1].Text.Contains("*")) return;

                // Get relation name from tabRelation name
                string relationName = GetRelationName(xtraTabDatabase.TabPages[1].Text);

                int nRow, nCol;
                nRow = GridViewData.Rows.Count - 1;
                nCol = GridViewData.Columns.Count;

                ProbRelation saveRelation = DB.GetRelation(relationName);
                saveRelation.tuples.Clear();

                if (GridViewData.Rows.Count <= 1) return;
                GridViewData.CurrentCell = GridViewData.Rows[nRow].Cells[0];

                ProbTuple tuple;
                ProbTriple triple;

                for (int i = 0; i < nRow; i++)
                {
                    tuple = new ProbTuple();
                    for (int j = 0; j < nCol; j++)
                    {
                        if (GridViewData.Rows[i].Cells[j].Value == null)
                            throw new Exception("Null value is not allowed in PRDB!");
                        triple = new ProbTriple(GridViewData.Rows[i].Cells[j].Value.ToString(), saveRelation.scheme.attributes[j].type);
                        tuple.triples.Add(saveRelation.scheme.attributes[j], triple);
                    }
                    saveRelation.tuples.Add(tuple);
                }

                xtraTabDatabase.TabPages[1].Text = "Relation " + relationName;
                xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[1]; ;

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void OpenRelation()
        {
            try
            {
                if (!Form_OpenRelation_Opening)
                {
                    Form_OpenRelation_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.relationNames = DB.ListOfRelationName();
                    frm_OpenRelation = new Form_OpenRelation();
                    frm_OpenRelation.Show();
                }
                frm_OpenRelation.Disposed += new EventHandler(frm_OpenRelation_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_OpenRelation_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_OpenRelation_Opening = false;
                string relationName = Resource.curRelationName;
                if (relationName != string.Empty)
                {
                    ProbRelation openRelation = DB.GetRelation(relationName);

                    xtraTabDatabase.TabPages[1].Text = "Relation " + relationName;
                    xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[1];

                    GridViewData.Rows.Clear();
                    GridViewData.Columns.Clear();

                    int i = 0;
                    foreach (ProbAttribute attr in openRelation.scheme.attributes)
                    {
                        GridViewData.Columns.Add("Column " + i, attr.attributeName);
                        i++;
                    }

                    if (openRelation.tuples.Count > 0)
                    {
                        int nRow = openRelation.tuples.Count;
                        int nCol = openRelation.scheme.attributes.Count;

                        ProbTuple tuple;

                        for (i = 0; i < nRow; i++)      // Assign data for GridViewData
                        {
                            tuple = openRelation.tuples[i];
                            GridViewData.Rows.Add();
                            for (int j = 0; j < nCol; j++)
                                GridViewData.Rows[i].Cells[j].Value = tuple.triples[openRelation.scheme.attributes[j]].GetStrValue();
                        }
                        UpdateDataRowNumber();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void DeleteRelation()
        {
            try
            {
                if (!Form_DeleteRelation_Opening)
                {
                    Form_DeleteRelation_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.relationNames = DB.ListOfRelationName();
                    frm_DelRelation = new Form_DeleteRelation();
                    frm_DelRelation.Show();
                }
                frm_DelRelation.Disposed += new EventHandler(frm_DelRelation_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_DelRelation_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_DeleteRelation_Opening = false;
                string delRelationName = Resource.curRelationName;
                if (delRelationName != null)
                {
                    ProbRelation deleteRelation = DB.GetRelation(delRelationName);

                    DialogResult result = new DialogResult();
                    result = MessageBox.Show("Are you sure want to delete this relation ?", "Delete relation " + delRelationName, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string tabRelationName = GetRelationName(xtraTabDatabase.TabPages[1].Text);
                        if (tabRelationName.Equals(delRelationName))
                        {
                            ResetRelationPage(true);
                            ResetInputValue(true);
                            xtraTabDatabase.TabPages[1].Text = "Relation";
                            GridViewData.Rows.Clear();
                            GridViewData.Columns.Clear();
                            UpdateDataRowNumber();
                        }
                        TreeNode DeletedNode = NodeRelation.Nodes[delRelationName];
                        DeletedNode.Remove();
                        DB.relations.Remove(deleteRelation);
                        deleteRelation = null;

                        if (NodeRelation.Nodes.Count == 0)
                            NodeRelation.ImageIndex = NodeRelation.SelectedImageIndex = Folder_ImgIndex.UnselectedState;

                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private string GetRelationName(string tabName)
        {
            try
            {
                int index = tabName.IndexOf("Relation");
                index += 8;
                string relationName = tabName.Trim().Substring(index);
                relationName = relationName.Replace("*", "").Trim();
                return relationName;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return string.Empty;
            }
        }

        

        #endregion

        #region * Menu Query

        private void MenuQuery_New_Click(object sender, EventArgs e)
        {
            ResetQueryPage(true);
            CreateNewQuery();
        }

        void frm_NewQuery_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_NewQuery_Opening = false;
                string queryName = Resource.curQueryName;
                if (queryName != string.Empty)
                {
                    ProbQuery newQuery = new ProbQuery(queryName);
                    DB.queries.Add(newQuery);
                    TreeNode NewNode = new TreeNode();
                    NewNode.Name = queryName;
                    NewNode.Text = queryName;
                    NewNode.ToolTipText = "Query " + queryName;
                    NewNode.ContextMenuStrip = ContextMenu_QueryNode;
                    NewNode.ImageIndex = Query_ImgIndex.UnselectedState;
                    NewNode.SelectedImageIndex = Query_ImgIndex.UnselectedState;
                    NodeQuery.Nodes.Add(NewNode);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuQuery_Open_Click(object sender, EventArgs e)
        {
            ResetQueryPage(true);
            OpenQuery();
        }

        void frm_OpenQuery_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_OpenQuery_Opening = false;
                string queryName = Resource.curQueryName;
                if (queryName != string.Empty)
                {
                    ProbQuery openQuery = DB.GetQuery(queryName);
                    xtraTabDatabase.TabPages[2].Text = "Query " + queryName;
                    xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[2];
                    rtbQuery.Text = openQuery.queryString;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuQuery_SaveQuery_Click(object sender, EventArgs e)
        {
            SaveQuery();
        }

        void frm_SaveQuery_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_SaveQuery_Opening = false;
                string queryName = Resource.curQueryName;
                if (queryName != string.Empty)
                {
                    ProbQuery newQuery = DB.GetQuery(queryName);
                    if (newQuery == null)
                    {
                        newQuery = new ProbQuery(queryName);
                        DB.queries.Add(newQuery);
                    }
                    newQuery.queryString = rtbQuery.Text;                    
                    TreeNode NewNode = new TreeNode();
                    NewNode.Name = queryName;
                    NewNode.Text = queryName;
                    NewNode.ToolTipText = "Query " + queryName;
                    NewNode.ContextMenuStrip = ContextMenu_QueryNode;
                    NewNode.ImageIndex = Query_ImgIndex.UnselectedState;
                    NewNode.SelectedImageIndex = Query_ImgIndex.UnselectedState;
                    NodeQuery.Nodes.Add(NewNode);
                    xtraTabDatabase.TabPages[2].Text = "Query " + queryName;                    
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuQuery_Delete_Click(object sender, EventArgs e)
        {
            DeleteQuery();
        }

        void frm_DelQuery_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_DeleteQuery_Opening = false;
                string queryName = Resource.curQueryName;
                if (queryName != string.Empty)
                {
                    ProbQuery deleteQuery = DB.GetQuery(queryName);
                    DialogResult result = new DialogResult();
                    result = MessageBox.Show("Are you sure want to delete this query ?", "Delete query " + queryName, MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        string tabQueryName = GetQueryName(xtraTabDatabase.TabPages[2].Text);

                        if (tabQueryName.Equals(queryName))
                        {
                            ResetQueryPage(true);
                            xtraTabDatabase.TabPages[2].Text = "Query";
                            rtbQuery.Text = string.Empty;
                        }
                        TreeNode DeletedNode = NodeQuery.Nodes[queryName];
                        DeletedNode.Remove();
                        DB.queries.Remove(deleteQuery);
                        deleteQuery = null;

                        if (NodeQuery.Nodes.Count == 0)
                            NodeQuery.ImageIndex = NodeQuery.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void MenuQuery_Execute_Click(object sender, EventArgs e)
        {
            ExecuteQuery();
        }

        private void MenuQuery_CloseQuery_Click(object sender, EventArgs e)
        {
            try
            {
                ResetQueryPage(true);
                xtraTabDatabase.TabPages[2].Text = "Query";
                rtbQuery.Text = string.Empty;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CreateNewQuery()
        {
            try
            {
                if (!Form_NewQuery_Opening)
                {
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Form_NewQuery_Opening = true;
                    Resource.queryNames = DB.ListOfQueryName();
                    frm_NewQuery = new Form_NewQuery();
                    frm_NewQuery.Show();
                }
                frm_NewQuery.Disposed += new EventHandler(frm_NewQuery_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void SaveQuery()
        {
            try
            {
                if (DB == null) throw new Exception("Cannot find the Database!");

                // If query's name on tabQuery does not contain asteroid character
                if (!xtraTabDatabase.TabPages[2].Text.Contains("*")) return;

                // Get query name from tabQuery
                string queryName = GetQueryName(xtraTabDatabase.TabPages[2].Text);

                xtraTabDatabase.TabPages[2].Text.Replace("*", "");
                xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[2]; ;

                // Save a new created query
                if (queryName == string.Empty)
                {
                    if (!Form_SaveQuery_Opening)
                    {
                        Form_SaveQuery_Opening = true;
                        Resource.queryNames = DB.ListOfQueryName();
                        frm_SaveQuery = new Form_SaveQuery();
                        frm_SaveQuery.Show();
                    }
                    frm_SaveQuery.Disposed += new EventHandler(frm_SaveQuery_Disposed);
                }
                else
                {
                    ProbQuery saveQuery = DB.GetQuery(queryName);

                    if (saveQuery == null)
                        throw new Exception("There is no query " + queryName + " in the Database!");

                    xtraTabDatabase.TabPages[2].Text = "Query " + queryName;
                    saveQuery.queryString = rtbQuery.Text;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void OpenQuery()
        {
            try
            {
                if (!Form_OpenQuery_Opening)
                {
                    Form_OpenQuery_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.queryNames = DB.ListOfQueryName();
                    frm_OpenQuery = new Form_OpenQuery();
                    frm_OpenQuery.Show();                    
                }
                frm_OpenQuery.Disposed += new EventHandler(frm_OpenQuery_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void DeleteQuery()
        {
            try
            {
                if (!Form_DeleteQuery_Opening)
                {
                    Form_DeleteQuery_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database!");
                    Resource.queryNames = DB.ListOfQueryName();
                    frm_DelQuery = new Form_DeleteQuery();
                    frm_DelQuery.Show();
                }
                frm_DelQuery.Disposed += new EventHandler(frm_DelQuery_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private string GetQueryName(string tabName)
        {
            try
            {
                int index = tabName.IndexOf("Query");
                index += 5;
                string queryName = tabName.Trim().Substring(index);
                queryName = queryName.Replace("*", "").Trim();
                return queryName;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return string.Empty;
            }
        }
        #endregion


        ////////////////////////////////////////////////////  Context Menu Process /////////////////////////////////////////        

        #region * Context Menu Database

        private void CTMenuDB_Rename_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Form_RenameDB_Opening)
                {
                    Form_RenameDB_Opening = true;
                    frm_RenameDB = new Form_RenameDB();
                    frm_RenameDB.Show();
                    frm_RenameDB.Disposed += new EventHandler(frm_RenameDB_Disposed);
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_RenameDB_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_RenameDB_Opening = false;
                string dbName = frm_RenameDB.dbName;
                if (dbName != null)
                {
                    DB.Rename(dbName);

                    Resource.dbName = DB.dbName;
                    Resource.connectionString = DB.connectionString;

                    Resource.dbShowName = "DB_" + (DB.dbName.Remove(DB.dbName.Length - 4)).ToUpper();

                    NodeDB.Text = Resource.dbShowName;
                    NodeDB.ToolTipText = "Database " + Resource.dbShowName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuDB_CloseDB_Click(object sender, EventArgs e)
        {
            CloseDatabase();
        }

        #endregion

        #region * Context Menu Scheme

        private void CTMenuScheme_NewScheme_Click(object sender, EventArgs e)
        {
            ResetSchemePage(true);
            CreateNewScheme();
        }


        private void CTMenuScheme_DelSchemes_Click(object sender, EventArgs e)
        {
            try
            {
                if (DB.relations.Count != 0)
                    throw new Exception("Cannot delete all schemes because some of them are inherited!");

                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete all schemes ?", "Delete All schemes", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ResetSchemePage(true);
                    NodeScheme.Nodes.Clear();
                    DB.schemes.Clear();                    
                    xtraTabDatabase.TabPages[0].Text = "Scheme";
                    int n = GridViewDesign.Rows.Count - 2;
                    for (int i = n; i >= 0; i--)
                        GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
                    lblDesignRowNumberIndicator.Text = "1 / 1";
                    
                    NodeScheme.ImageIndex = NodeScheme.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion 

        #region Context Menu Scheme Node

        private void CTMenuSchNode_OpenScheme_Click(object sender, EventArgs e)
        {
            try
            {
                ResetSchemePage(true);
                string schemeName = CurrentNode.Text;
                TreeView.SelectedNode = CurrentNode;

                xtraTabDatabase.TabPages[0].Text = "Scheme " + schemeName;
                xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[0];

                ProbScheme currentScheme = DB.GetScheme(schemeName);


                if (currentScheme.isInherited(DB.relations))
                {
                    GridViewDesign.Columns[0].ReadOnly = true;
                    GridViewDesign.Columns[1].ReadOnly = true;
                    GridViewDesign.Columns[2].ReadOnly = true;
                }
                else
                {
                    GridViewDesign.Columns[0].ReadOnly = false;
                    GridViewDesign.Columns[1].ReadOnly = false;
                    GridViewDesign.Columns[2].ReadOnly = false;
                }

                int i, n = GridViewDesign.Rows.Count - 2;
                for (i = n; i >= 0; i--)
                    GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);

                lblDesignRowNumberIndicator.Text = "1 / 1";

                i = 0;
                CheckBox chkbox;
                foreach (ProbAttribute attr in currentScheme.attributes)
                {
                    GridViewDesign.Rows.Add();
                    chkbox = new CheckBox();
                    chkbox.Checked = attr.primaryKey;
                    GridViewDesign.Rows[i].Cells[0].Value = chkbox.CheckState;
                    GridViewDesign.Rows[i].Cells[1].Value = attr.attributeName;
                    GridViewDesign.Rows[i].Cells[2].Value = attr.type.typeName;
                    GridViewDesign.Rows[i].Cells[3].Value = attr.type.domainString;
                    GridViewDesign.Rows[i].Cells[4].Value = (attr.description != null ? attr.description : null);
                    i++;
                }

                //GridViewDesign.CurrentCell = GridViewDesign.Rows[i].Cells[0];
                if (GridViewDesign.CurrentRow != null)
                    lblDesignRowNumberIndicator.Text = (GridViewDesign.CurrentRow.Index + 1).ToString() + " / " + GridViewDesign.Rows.Count.ToString();
                else lblDesignRowNumberIndicator.Text = "1 / " + GridViewDesign.Rows.Count.ToString();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuSchNode_DeleteScheme_Click(object sender, EventArgs e)
        {
            try
            {
                string schemeName = CurrentNode.Text;
                ProbScheme deleteScheme = DB.GetScheme(schemeName);

                if (InheritedScheme(deleteScheme))
                    throw new Exception("Cannot delete this scheme because it is inherited by some relations!");

                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete this scheme ?", "Delete scheme " + schemeName, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string tabSchemeName = GetSchemeName(xtraTabDatabase.TabPages[0].Text);

                    if (tabSchemeName.Equals(schemeName))
                    {
                        ResetSchemePage(true);
                        xtraTabDatabase.TabPages[0].Text = "Scheme";
                        int n = GridViewDesign.Rows.Count - 2;
                        for (int i = n; i >= 0; i--)
                            GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
                        lblDesignRowNumberIndicator.Text = "1 / 1";
                    }

                    CurrentNode.Remove();
                    DB.schemes.Remove(deleteScheme);
                    deleteScheme = null;

                    if (NodeScheme.Nodes.Count == 0)
                        NodeScheme.ImageIndex = NodeScheme.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuSchNode_RenameScheme_Click(object sender, EventArgs e)
        {
            try
            {
                string curSchemeName = CurrentNode.Text;
                Resource.curSchemeName = curSchemeName;
                ProbScheme currentScheme = DB.GetScheme(curSchemeName);
                Resource.currentScheme = currentScheme;

                if (InheritedScheme(currentScheme))
                    throw new Exception("Cannot rename this scheme because it is inherited by some relations!");

                RenameScheme();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RenameScheme()
        {
            try
            {
                if (!Form_RenameScheme_Opening)
                {
                    Form_RenameScheme_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database");
                    Resource.schemeNames = DB.ListOfSchemeName();
                    frm_RenameScheme = new Form_RenameScheme();
                    frm_RenameScheme.Show();
                }
                frm_RenameScheme.Disposed += new EventHandler(frm_RenameScheme_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_RenameScheme_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_RenameScheme_Opening = false;
                string newSchemeName = Resource.curSchemeName;
                if (newSchemeName != string.Empty)
                {
                    ProbScheme renamedScheme = Resource.currentScheme;
                    string oldSchemeName = renamedScheme.schemename;
                    renamedScheme.schemename = newSchemeName;
                    string tabSchemeName = GetSchemeName(xtraTabDatabase.TabPages[0].Text);
                    if (tabSchemeName.Equals(oldSchemeName)) xtraTabDatabase.TabPages[0].Text = "Scheme " + newSchemeName;
                    CurrentNode.Name = CurrentNode.Text = newSchemeName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        #region * Context Menu Relation

        private void CTMenuRelation_NewRelation_Click(object sender, EventArgs e)
        {
            ResetRelationPage(true);
            ResetInputValue(true);
            CreateNewRelation();
        }

        private void CTMenuRelation_DeleteRelations_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete all relations ?", "Delete All Relations", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ResetRelationPage(true);
                    ResetInputValue(true);
                    NodeRelation.Nodes.Clear();
                    xtraTabDatabase.TabPages[1].Text = "Relation";
                    GridViewData.Rows.Clear();
                    GridViewData.Columns.Clear();
                    UpdateDataRowNumber();
                    DB.relations.Clear();
                    NodeRelation.ImageIndex = NodeRelation.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        #region Context Menu Relation Node

        private void CTMenuRelNode_OpenRelation_Click(object sender, EventArgs e)
        {
            try
            {
                ResetRelationPage(true);
                ResetInputValue(true);
                string relationName = CurrentNode.Text;
                TreeView.SelectedNode = CurrentNode;

                xtraTabDatabase.TabPages[1].Text = "Relation " + relationName;
                xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[1];

                ProbRelation openRelation = DB.GetRelation(relationName);

                GridViewData.Rows.Clear();
                GridViewData.Columns.Clear();

                int i = 0;
                foreach (ProbAttribute attr in openRelation.scheme.attributes)
                {
                    GridViewData.Columns.Add("Column " + i, attr.attributeName);
                    i++;
                }

                if (openRelation.tuples.Count > 0)
                {
                    int nRow = openRelation.tuples.Count;
                    int nCol = openRelation.scheme.attributes.Count;

                    ProbTuple tuple;

                    for (i = 0; i < nRow; i++)      // Assign data for GridViewData
                    {
                        tuple = openRelation.tuples[i];
                        GridViewData.Rows.Add();
                        for (int j = 0; j < nCol; j++)
                            GridViewData.Rows[i].Cells[j].Value = tuple.triples[openRelation.scheme.attributes[j]].GetStrValue();
                    }
                    UpdateDataRowNumber();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuRelNode_DeleteRelation_Click(object sender, EventArgs e)
        {                     
            try
            {
                string relationName = CurrentNode.Text;
                ProbRelation deleteRelation = DB.GetRelation(relationName);

                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete this relation ?", "Delete relation " + relationName, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string tabRelationName = GetRelationName(xtraTabDatabase.TabPages[1].Text);
                    if (tabRelationName.Equals(relationName))
                    {
                        ResetRelationPage(true);
                        ResetInputValue(true);
                        xtraTabDatabase.TabPages[1].Text = "Relation";
                        GridViewData.Rows.Clear();
                        GridViewData.Columns.Clear();
                        UpdateDataRowNumber();
                    }
                    TreeNode DeletedNode = NodeRelation.Nodes[relationName];
                    DeletedNode.Remove();
                    DB.relations.Remove(deleteRelation);
                    deleteRelation = null;

                    if (NodeRelation.Nodes.Count == 0)
                        NodeRelation.ImageIndex = NodeRelation.SelectedImageIndex = Folder_ImgIndex.UnselectedState;

                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RenameRelation()
        {
            try
            {
                if (!Form_RenameRelation_Opening)
                {
                    Form_RenameRelation_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database");
                    Resource.relationNames = DB.ListOfRelationName();
                    frm_RenameRelation = new Form_RenameRelation();
                    frm_RenameRelation.Show();
                }
                frm_RenameRelation.Disposed += new EventHandler(frm_RenameRelation_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_RenameRelation_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_RenameRelation_Opening = false;
                string newRelationName = Resource.curRelationName;
                if (newRelationName != string.Empty)
                {
                    ProbRelation renamedRelation = Resource.currentRelation;
                    string oldRelationName = renamedRelation.relationname;
                    renamedRelation.relationname = newRelationName;
                    string tabRelationName = GetRelationName(xtraTabDatabase.TabPages[1].Text);
                    if (tabRelationName.Equals(oldRelationName)) xtraTabDatabase.TabPages[0].Text = "Relation " + newRelationName;
                    CurrentNode.Name = CurrentNode.Text = newRelationName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }            
        }

        private void CTMenuRelNode_RenameRelation_Click(object sender, EventArgs e)
        {
            try
            {
                string curRelationName = CurrentNode.Text;
                Resource.curRelationName = curRelationName;
                ProbRelation renamedRelation = DB.GetRelation(curRelationName);
                Resource.currentRelation = renamedRelation;
                RenameRelation();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion        

        #region * Context Menu Query

        private void CTMenuQuery_NewQuery_Click(object sender, EventArgs e)
        {
            ResetQueryPage(true);
            CreateNewQuery();
        }

        private void CTMenuQuery_DeleteQueries_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete all queries ?", "Delete All Queries", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    ResetQueryPage(true);
                    NodeQuery.Nodes.Clear();
                    DB.queries.Clear();
                    xtraTabDatabase.TabPages[2].Text = "Query";
                    rtbQuery.Clear();
                    NodeQuery.ImageIndex = NodeQuery.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuQueryNode_OpenQuery_Click(object sender, EventArgs e)
        {
            try
            {
                ResetQueryPage(true);
                string queryName = CurrentNode.Name;
                TreeView.SelectedNode = CurrentNode;
                ProbQuery openQuery = DB.GetQuery(queryName);
                xtraTabDatabase.TabPages[2].Text = "Query " + queryName;
                xtraTabDatabase.SelectedTabPage = xtraTabDatabase.TabPages[2];
                rtbQuery.Text = openQuery.queryString;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void CTMenuQuery_DeleteQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string queryName = CurrentNode.Name;
                ProbQuery deleteQuery = DB.GetQuery(queryName);

                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to delete this query ?", "Delete query " + queryName, MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    string tabQueryName = GetQueryName(xtraTabDatabase.TabPages[2].Text);
                    if (tabQueryName.Equals(queryName))
                    {
                        ResetQueryPage(true);
                        xtraTabDatabase.TabPages[2].Text = "Query";
                    }

                    TreeNode DeletedNode = NodeQuery.Nodes[queryName];
                    DeletedNode.Remove();
                    DB.queries.Remove(deleteQuery);
                    deleteQuery = null;

                    if (NodeQuery.Nodes.Count == 0)
                        NodeQuery.ImageIndex = NodeQuery.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void RenameQuery()
        {
            try
            {
                if (!Form_RenameQuery_Opening)
                {
                    Form_RenameQuery_Opening = true;
                    if (DB == null) throw new Exception("Cannot find the Database");
                    Resource.queryNames = DB.ListOfQueryName();
                    frm_RenameQuery = new Form_RenameQuery();
                    frm_RenameQuery.Show();
                }
                frm_RenameQuery.Disposed += new EventHandler(frm_RenameQuery_Disposed);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_RenameQuery_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_RenameQuery_Opening = false;
                string newQueryName = Resource.curQueryName;
                if (newQueryName != string.Empty)
                {
                    ProbQuery renamedQuery = Resource.currentQuery;
                    string oldQueryName = renamedQuery.queryName;
                    renamedQuery.queryName = newQueryName;
                    string tabQueryName = GetQueryName(xtraTabDatabase.TabPages[2].Text);
                    if (tabQueryName.Equals(oldQueryName)) xtraTabDatabase.TabPages[2].Text = "Query " + newQueryName;
                    CurrentNode.Name = CurrentNode.Text = newQueryName;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }               
        }

        private void CTMenuQuery_RenameQuery_Click(object sender, EventArgs e)
        {
            try
            {
                string curQueryName = CurrentNode.Text;
                Resource.curQueryName = curQueryName;
                ProbQuery renamedQuery = DB.GetQuery(curQueryName);
                Resource.currentQuery = renamedQuery;
                RenameQuery();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        #region * Context Menu Query Editor

        private void CTMenuQueryEditor_Conj_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Conj_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Conj_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Conj_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Disj_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Disj_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Disj_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Disj_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Diff_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Diff_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Diff_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Diff_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Equal_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Equal_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void CTMenuQueryEditor_Equal_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Equal_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void CTMenuQueryEditor_Inequal_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void CTMenuQueryEditor_Inequal_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Inequal_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Inequal_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LTH_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LTH_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LTH_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LTH_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LET_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LET_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LET_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_LET_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GTH_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GTH_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GTH_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GTH_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GET_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GET_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GET_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_GET_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Join_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Join_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Join_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Join_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Union_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Union_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Union_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Union_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Intersect_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Intersect_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Intersect_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Intersect_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Minus_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Minus_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Minus_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_Minus_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void CTMenuQueryEditor_In_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        int oldCursorPosition, CursorPosition;
        private void CTMenuQueryEditor_In_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_In_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_In_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_NotIn_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_NotIn_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_NotIn_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_NotIn_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void CTMenuQueryEditor_Execute_Click(object sender, EventArgs e)
        {
            ExecuteQuery();
        }

        private void CTMenuQueryEditor_SaveQuery_Click(object sender, EventArgs e)
        {
            SaveQuery();
        }

        private void CTMenuQueryEditor_NewQuery_Click(object sender, EventArgs e)
        {
            ResetQueryPage(true);
        }

        #endregion

        ////////////////////////////////////////////////////////  Toolbar Process //////////////////////////////////////////////        

        #region * System Toolbar

        private void Toolbar_BtnNew_Click(object sender, EventArgs e)
        {
            CreateNewDatabase();
        }

        private void Toolbar_BtnOpen_Click(object sender, EventArgs e)
        {
            OpenDatabase();
        }

        private void Toolbar_BtnSave_Click(object sender, EventArgs e)
        {
            SaveDatabase();
        }

        #endregion

        #region * Editor Toolbar
        private void Toolbar_BtnCut_Click(object sender, EventArgs e)
        {
            rtbQuery.Cut();
        }

        private void Toolbar_BtnCopy_Click(object sender, EventArgs e)
        {
            rtbQuery.Copy();
        }

        private void Toolbar_BtnPaste_Click(object sender, EventArgs e)
        {
            rtbQuery.Paste();
        }

        private void Toolbar_BtnUndo_Click(object sender, EventArgs e)
        {
            rtbQuery.Undo();
        }

        private void Toolbar_BtnRedo_Click(object sender, EventArgs e)
        {
            rtbQuery.Redo();
        }

        #endregion

        #region * Query Processing

        private void ToolbarMenuItem_Conj_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Conj_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Conj_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Conj_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊗";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Disj_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Disj_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Disj_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Disj_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊕";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Diff_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Diff_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Diff_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Diff_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⊖";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Equal_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Equal_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void ToolbarMenuItem_Equal_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Equal_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"==";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void ToolbarMenuItem_Inequal_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";

        }

        private void ToolbarMenuItem_Inequal_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Inequal_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Inequal_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"!=";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LTH_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LTH_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LTH_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LTH_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"<";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LET_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LET_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LET_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_LET_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≤";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GTH_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GTH_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GTH_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GTH_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @">";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GET_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GET_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GET_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_GET_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"≥";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Join_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Join_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Join_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Join_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⨝";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Union_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Union_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Union_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Union_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋃";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊕pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Intersect_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Intersect_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Intersect_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Intersect_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"⋂";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Minus_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Minus_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Minus_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_Minus_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"-";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊖pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @" ";
        }

        private void ToolbarMenuItem_In_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_In_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_In_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_In_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_NotIn_ig_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗ig";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_NotIn_in_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗in";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_NotIn_me_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗me";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void ToolbarMenuItem_NotIn_pc_Click(object sender, EventArgs e)
        {
            // Thêm vào kí tự khoảng cách nếu viết liền
            int startPosition = rtbQuery.SelectionStart;
            if (startPosition > 0 && rtbQuery.Text[startPosition - 1] != ' ') rtbQuery.SelectedText = @" ";

            // Thêm vào kí hiệu toán học
            rtbQuery.SelectedText = @"¬IN";
            rtbQuery.SelectionCharOffset = -5;
            rtbQuery.SelectionFont = new Font("Cambria", 8);
            rtbQuery.SelectedText = @"⊗pc";
            rtbQuery.SelectionFont = new Font("Cambria", 13);
            rtbQuery.SelectionCharOffset = 0;
            rtbQuery.SelectedText = @"(, )[ , ] ";
            rtbQuery.SelectionStart = rtbQuery.SelectionStart - 9;
        }

        private void Toolbar_BtnExecute_Click(object sender, EventArgs e)
        {
            ExecuteQuery();
        }

        private void Toolbar_BtnStop_Click(object sender, EventArgs e)
        {
            clsProcess.StopWorker();
            Toolbar_BtnStop.Enabled = false;
        }

        private void Toolbar_BtnAST_Click(object sender, EventArgs e)
        {
            RenderAbstractSyntaxTree();
        }

        private void ExecuteQuery()
        {
            Toolbar_BtnStop.Enabled = true;

            try
            {
                lblStatus.Text = "Executing...";
                GridViewResult.Rows.Clear();
                GridViewResult.Columns.Clear();
                txtMessage.Text = string.Empty;

                if (rtbQuery.Text == string.Empty) throw new Exception("The query string is empty!");
                QueryExecution.Execute(rtbQuery.Text, this.DB);

                if (QueryExecution.satisfiedRelation == null) throw new Exception("Query process error!");
                if (QueryExecution.satisfiedRelation.tuples.Count <= 0)
                {
                    txtMessage.Text = "No relation satisfies the query";
                    xtraTabResult.SelectedTabPageIndex = 1;
                }
                else
                {
                    ProbRelation relationResult = QueryExecution.satisfiedRelation;
                    foreach (ProbAttribute attribute in relationResult.scheme.attributes)
                        GridViewResult.Columns.Add(attribute.attributeName, attribute.attributeName);

                    int j, i = -1;
                    foreach (ProbTuple tuple in relationResult.tuples)
                    {
                        GridViewResult.Rows.Add();
                        i++; j = -1;
                        foreach (ProbAttribute attribute in relationResult.scheme.attributes)
                            GridViewResult.Rows[i].Cells[++j].Value = tuple.GetTriple(attribute).GetStrValue();
                    }

                    xtraTabResult.SelectedTabPageIndex = 0;
                }
            }
            catch (Exception Ex)
            {
                txtMessage.Text = Ex.Message;
                MessageBox.Show(Ex.Message);
            }

            lblStatus.Text = "Ready";
            Toolbar_BtnStop.Enabled = false;
        }

        private void RenderAbstractSyntaxTree()
        {
            try
            {
                if (rtbQuery.Text == string.Empty) throw new Exception("The query string is empty!");

                string currentDirectory = Resource.currentDirectory;
                Process process = new Process();

                process.StartInfo.FileName = @"java.exe";
                process.StartInfo.WorkingDirectory = currentDirectory;
                process.StartInfo.Arguments = @"-cp .;antlr-4.2-complete.jar org.antlr.v4.runtime.misc.TestRig ProbSQL init -gui";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardInput = true;
                process.Start();

                StreamWriter myStreamWriter = process.StandardInput;

                string query = QueryExecution.StandardizeQuery(rtbQuery.Text);

                myStreamWriter.WriteLine(query);
                myStreamWriter.Close();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void rtbQuery_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string tabQueryName = xtraTabDatabase.TabPages[2].Text;
                if (!tabQueryName.Contains("*"))
                    xtraTabDatabase.TabPages[2].Text = tabQueryName + "*";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        ////////////////////////////////////////////////////////  TreeView Process //////////////////////////////////////////////        

        #region * TreeView

        private void Load_TreeView()
        {
            TreeView.Nodes.Clear();
            Resource.dbShowName = DB.dbName.ToUpper();

            NodeDB = new TreeNode();
            NodeDB.Text = Resource.dbShowName;
            NodeDB.Name = Resource.dbShowName;
            NodeDB.ToolTipText = "Database " + Resource.dbShowName;
            NodeDB.ContextMenuStrip = ContextMenu_Database;
            NodeDB.ImageIndex = DB_ImgIndex.UnselectedState;
            NodeDB.SelectedImageIndex = DB_ImgIndex.SelectedState;
            TreeView.Nodes.Add(NodeDB);

            NodeScheme = new TreeNode();
            NodeScheme.Text = "Schemes";
            NodeScheme.Name = "Schemes";
            NodeScheme.ToolTipText = "Schemes";
            NodeScheme.ContextMenuStrip = ContextMenu_Scheme;
            NodeScheme.ImageIndex = Folder_ImgIndex.UnselectedState;
            NodeScheme.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
            NodeDB.Nodes.Add(NodeScheme);

            NodeRelation = new TreeNode();
            NodeRelation.Text = "Relations";
            NodeRelation.Name = "Relations";
            NodeRelation.ToolTipText = "Relations";
            NodeRelation.ContextMenuStrip = ContextMenu_Relation;
            NodeRelation.ImageIndex = Folder_ImgIndex.UnselectedState;
            NodeRelation.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
            NodeDB.Nodes.Add(NodeRelation);

            NodeQuery = new TreeNode();
            NodeQuery.Text = "Queries";
            NodeQuery.Name = "Queries";
            NodeQuery.ToolTipText = "Queries";
            NodeQuery.ContextMenuStrip = ContextMenu_Query;
            NodeQuery.ImageIndex = Folder_ImgIndex.UnselectedState;
            NodeQuery.SelectedImageIndex = Folder_ImgIndex.UnselectedState;
            NodeDB.Nodes.Add(NodeQuery);

            LoadTreeViewNode();
        }

        private void LoadTreeViewNode()
        {
            foreach (ProbScheme scheme in DB.schemes)
            {
                NewNode = new TreeNode();
                NewNode.Text = scheme.schemename;
                NewNode.Name = scheme.schemename;
                NewNode.ToolTipText = "Scheme " + scheme.schemename;
                NewNode.ContextMenuStrip = ContextMenu_SchemeNode;
                NewNode.ImageIndex = NewNode.SelectedImageIndex = Scheme_ImgIndex.SelectedState;
                NodeScheme.Nodes.Add(NewNode);

                foreach (ProbAttribute attribute in scheme.attributes)
                {
                    ChildNode = new TreeNode();
                    ChildNode.Text = attribute.attributeName;
                    ChildNode.Name = attribute.attributeName;
                    ChildNode.ToolTipText = "Attribute " + attribute.attributeName;

                    if (attribute.IsPrimaryKey())
                    {
                        ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                        ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                    }
                    else
                    {
                        ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                        ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                    }
                    NewNode.Nodes.Add(ChildNode);
                }                
            }

            foreach (ProbRelation relation in DB.relations)
            {
                NewNode = new TreeNode();
                NewNode.Text = relation.relationname;
                NewNode.Name = relation.relationname;
                NewNode.ToolTipText = "Relation " + relation.relationname;
                NewNode.ContextMenuStrip = ContextMenu_RelationNode;
                NewNode.ImageIndex = NewNode.SelectedImageIndex = Relation_ImgIndex.SelectedState;
                NodeRelation.Nodes.Add(NewNode);

                foreach (ProbAttribute attribute in relation.scheme.attributes)
                {
                    ChildNode = new TreeNode();
                    ChildNode.Text = attribute.attributeName;
                    ChildNode.Name = attribute.attributeName;
                    ChildNode.ToolTipText = "Attribute " + attribute.attributeName;

                    if (attribute.IsPrimaryKey())
                    {
                        ChildNode.ImageIndex = Attribute_ImgIndex.PrimaryKey;
                        ChildNode.SelectedImageIndex = Attribute_ImgIndex.PrimaryKey;
                    }
                    else
                    {
                        ChildNode.ImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                        ChildNode.SelectedImageIndex = Attribute_ImgIndex.NonPrimaryKey;
                    }
                    NewNode.Nodes.Add(ChildNode);
                }                
            }

            foreach (ProbQuery query in DB.queries)
            {
                NewNode = new TreeNode();
                NewNode.Text = query.queryName;
                NewNode.Name = query.queryName;
                NewNode.ToolTipText = "Query " + query.queryName;
                NewNode.ContextMenuStrip = ContextMenu_QueryNode;
                NewNode.ImageIndex = Query_ImgIndex.UnselectedState;
                NewNode.SelectedImageIndex = Query_ImgIndex.SelectedState;
                NodeQuery.Nodes.Add(NewNode);
            }
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                CurrentNode = e.Node;

                if (CurrentNode.Parent == NodeDB && !CurrentNode.IsExpanded)
                    CurrentNode.ImageIndex = CurrentNode.SelectedImageIndex = Folder_ImgIndex.UnselectedState;

                if (e.Button == MouseButtons.Right)
                {
                    if (CurrentNode.Parent == NodeScheme || CurrentNode.Parent == NodeRelation || CurrentNode.Parent == NodeQuery)
                        CurrentNode.ContextMenuStrip.Show();
                    else
                    {
                        TreeView.SelectedNode = CurrentNode;
                        TreeView.Focus();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void TreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (e.Node.IsExpanded)
                    if (e.Node == NodeScheme || e.Node == NodeRelation || e.Node == NodeQuery)
                        e.Node.ImageIndex = e.Node.SelectedImageIndex = Folder_ImgIndex.SelectedState;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void TreeView_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                CurrentNode = e.Node;
                rtbQuery.SelectedText = CurrentNode.Name;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }


        #endregion

        ////////////////////////////////////////////////////////  Form Main Process //////////////////////////////////////////////        

        #region * Form Main

        private void LoadImageCollection()
        {
            TreeView.ImageList = ImageList_TreeView;
            DB_ImgIndex.SelectedState = DB_ImgIndex.UnselectedState = 0;
            Folder_ImgIndex.UnselectedState = 1;
            Folder_ImgIndex.SelectedState = 2;
            Scheme_ImgIndex.SelectedState = Scheme_ImgIndex.UnselectedState = 3;
            Relation_ImgIndex.SelectedState = Relation_ImgIndex.UnselectedState = 3;
            Query_ImgIndex.SelectedState = Query_ImgIndex.UnselectedState = 3;
            Attribute_ImgIndex.PrimaryKey = 5;
            Attribute_ImgIndex.NonPrimaryKey = 6;
        }

        public void ResetObject()
        {
            DB = null;
            NodeDB = NodeScheme = NodeRelation = NodeQuery = CurrentNode = NewNode;
        }

        private void Form_Main_Load(object sender, EventArgs e)
        {
            LoadPRDB();
            CompileProbSQL();
            //MessageBox.Show(System.IO.Directory.GetCurrentDirectory());
            //OpenTestDatabase();
        }

        private void OpenTestDatabase()
        {
            try
            {
                TreeView.Nodes.Clear();
                DB = null;

                DB = clsProcess.NewDatabase(@"C:\\Users\\Hieu Tran\\Desktop\\CLINIC DATABASE.pdb");
                Resource.dbName = DB.dbName;
                Resource.connectionString = DB.connectionString;

                bool success = clsProcess.Connect();
                success = success && clsProcess.LoadDatabase(DB);

                if (!success)
                {
                    clsProcess.Dispose();
                    throw new Exception("Cannot connect to the physical Database!");
                }
                else
                {
                    ActivateDatabase(true);
                    Load_TreeView();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            lblStatus.Text = "Ready";
        }

        public void ResetSchemePage(bool state)
        {
            xtraTabDatabase.TabPages[0].Text = "Scheme";
            int n = GridViewDesign.Rows.Count - 2;
            for (int i = n; i >= 0; i--)
                GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
            lblDesignRowNumberIndicator.Text = "1 / 1";
            GridViewDesign.Enabled = state;
            Btn_Design_DeleteRow.Enabled = state;
            Btn_Design_ClearData.Enabled = state;
            Btn_Design_UpdateData.Enabled = state;
        }

        public void ResetRelationPage(bool state)
        {
            xtraTabDatabase.TabPages[1].Text = "Relation";

            GridViewData.Rows.Clear();
            GridViewData.Columns.Clear();

            UpdateDataRowNumber();

            GridViewValue.Rows.Clear();

            UpdateValueRowNumber();
            Btn_Data_DeleteRow.Enabled = state;
            Btn_Data_ClearData.Enabled = state;
            Btn_Data_UpdateData.Enabled = state;

        }

        public void ResetQueryPage(bool state)
        {
            xtraTabDatabase.TabPages[2].Text = "Query";
            rtbQuery.Clear();
            txtMessage.Clear();
            GridViewResult.Rows.Clear();
            GridViewResult.Columns.Clear();
            rtbQuery.Enabled = state;
            xtraTabResult.SelectedTabPage = xtraTabResult.TabPages[0];
        }

        public void ResetInputValue(bool state)
        {
            txtMinProb.Clear();
            txtMaxProb.Clear();
            txtValue.Clear();
            GridViewValue.Rows.Clear();
            UpdateValueRowNumber();

            Checkbox_UUD.Enabled = state;
            Checkbox_UD.Enabled = state;
            txtMinProb.Enabled = state;
            txtMaxProb.Enabled = state;
            btn_Value_AddNewRow.Enabled = state;
            btn_Value_DeleteRow.Enabled = state;
            Btn_Value_ClearData.Enabled = state;
            btn_Value_UpdateValue.Enabled = state;
        }

        private void ResetMenuBar(bool state)
        {
            MenuDB_SaveDatabase.Enabled = state;
            MenuDB_SaveDatabaseAs.Enabled = state;
            MenuDB_CloseDatabase.Enabled = state;
            MenuScheme.Enabled = state;
            MenuRelation.Enabled = state;
            MenuQuery.Enabled = state;
            Toolbar_BtnSave.Enabled = state;
            Toolbar_BtnCut.Enabled = state;
            Toolbar_BtnCopy.Enabled = state;
            Toolbar_BtnPaste.Enabled = state;
            Toolbar_BtnUndo.Enabled = state;
            Toolbar_BtnRedo.Enabled = state;
            Toolbar_BtnConj.Enabled = state;
            Toolbar_BtnDisj.Enabled = state;
            Toolbar_BtnDiff.Enabled = state;
            Toolbar_BtnEqual.Enabled = state;
            Toolbar_BtnExecute.Enabled = state;
            Toolbar_BtnStop.Enabled = false;
            Toolbar_BtnAST.Enabled = state;
            Toolbar_BtnCompare.Enabled = state;
            Toolbar_BtnJoin.Enabled = state;
            Toolbar_BtnUnion.Enabled = state;
            Toolbar_BtnIntersect.Enabled = state;
            Toolbar_BtnMinus.Enabled = state;
            Toolbar_BtnIn.Enabled = state;
        }

        private void ActivateDatabase(bool state)
        {
            ResetSchemePage(state);
            ResetRelationPage(state);
            ResetQueryPage(state);
            ResetInputValue(state);
            ResetMenuBar(state);
        }

        public void LoadPRDB()
        {
            CurrentRow = CurrentCell = 0;
            validated = flag = true;
            //rollbackcell = false;
            xtraTabDatabase.Show();
            xtraTabDatabase.SelectedTabPageIndex = 0;
            LoadImageCollection();
            Toolbar_BtnStop.Enabled = false;
            BindingNavigatorData.Visible = true;
            BindingNavigatorDesign.Visible = true;
            BindingNavigatorValue.Visible = true;
            Form_InputType_Opening = false;
            //Form_About_Opening = false;
            Form_DeleteScheme_Opening = false;
            Form_DeleteRelation_Opening = false;
            Form_DeleteQuery_Opening = false;
            Form_NewScheme_Opening = false;
            Form_NewRelation_Opening = false;
            Form_NewQuery_Opening = false;
            Form_OpenScheme_Opening = false;
            Form_OpenRelation_Opening = false;
            Form_OpenQuery_Opening = false;
            Form_RenameDB_Opening = false;
            Form_RenameQuery_Opening = false;
            Form_RenameRelation_Opening = false;
            Form_RenameScheme_Opening = false;
            Form_SaveScheme_Opening = false;
            //Form_SaveRelation_Opening = false;
            Form_SaveQuery_Opening = false;

            SwitchValueState(true);
            clsProcess = new clsProcess();
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            ActivateDatabase(false);
            Resource.currentDirectory = System.IO.Directory.GetCurrentDirectory();
        }

        private void CompileProbSQL()
        {
            try
            {
                string currentDirectory = Resource.currentDirectory;
                Process process = new Process();

                process.StartInfo.FileName = @"java.exe";
                process.StartInfo.WorkingDirectory = currentDirectory;
                process.StartInfo.Arguments = @" -cp .;antlr-4.2-complete.jar org.antlr.v4.Tool ProbSQL.g4";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();

                process.StartInfo.FileName = @"javac.exe";
                process.StartInfo.WorkingDirectory = currentDirectory;
                process.StartInfo.Arguments = @"ProbSQL*.java";
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.Start();
                process.WaitForExit();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion

        ////////////////////////////////////////////////////////  Gridview Process //////////////////////////////////////////////        

        #region * GridViewDesign

        private void GridViewDesign_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                if (flag) // Đặt biến cờ để tránh sự kiện SelectionChanged lặp lại 2 lần
                {
                    if (GridViewDesign.CurrentRow.Index != CurrentRow)
                        if (ValidateRow(CurrentRow) == false)
                        {

                            flag = false;
                            GridViewDesign.CurrentCell = GridViewDesign.Rows[CurrentRow].Cells[CurrentCell];
                        }
                        else CurrentRow = GridViewDesign.CurrentRow.Index;
                }
                flag = true;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private bool ValidateRow(int RowIndex)
        {
            try
            {
                if (RowIndex >= 0 && RowIndex < GridViewDesign.Rows.Count)
                {
                    bool PrKey = (GridViewDesign.Rows[RowIndex].Cells["PrimaryKey"].Value != null);       // kiểm tra xem field  PrimaryKey đã có giá trị hay chưa
                    bool AttrName = (GridViewDesign.Rows[RowIndex].Cells["ColumnName"].Value != null);    // kiểm tra xem field  AttributeName đã có giá trị hay chưa
                    bool TypeName = (GridViewDesign.Rows[RowIndex].Cells["ColumnType"].Value != null);    // kiểm tra xem field TypeName đã có giá trị hay chưa
                    bool Description = (GridViewDesign.Rows[RowIndex].Cells["ColumnDescription"].Value != null);    // kiểm tra xem field Description đã có giá trị hay chưa
                    // Nếu cả tên thuộc tính và kiểu dữ liệu đã được nhập
                    if (AttrName && TypeName)
                        return true;
                    // Nếu một trong các cột Khóa chính, tên thuộc tính và kiểu dữ liệu đã có giá trị
                    else if (PrKey || AttrName || TypeName || Description)
                    {
                        // Nếu tên thuộc tính và kiểu dữ liệu chưa được nhập
                        if (!AttrName && !TypeName)
                        {
                            MessageBox.Show("Input the attribute name and data type!");
                            CurrentCell = 1;
                            return false;
                        }
                        // Nếu chỉ có tên thuộc tính chưa được nhập
                        else if (!AttrName)
                        {
                            MessageBox.Show("Input the attribute name!");
                            CurrentCell = 1;
                            return false;
                        }
                        // Nếu chỉ có kiểu dữ liệu chưa được nhập
                        else
                        {
                           MessageBox.Show("Select a data type!");
                            CurrentCell = 2;
                            return false;
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

            return true;
        }

        private void GridViewDesign_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ProbScheme currentScheme = GetCurrentScheme();
                if (e.ColumnIndex == 2)
                {
                    if (currentScheme != null && currentScheme.isInherited(DB.relations))
                    {
                        MessageBox.Show("This scheme is being inherited!");
                    }                       
                    else if (!Form_InputType_Opening)
                    {
                        Form_InputType_Opening = true;
                        CurrentCell = e.ColumnIndex;
                        frm_InpType = new Form_InputType();
                        frm_InpType.Show();
                        frm_InpType.Disposed += new EventHandler(frm_InpType_Disposed);
                    }
                }
                else if (e.ColumnIndex == 1)
                {
                    if (currentScheme != null && currentScheme.isInherited(DB.relations))
                    {
                        MessageBox.Show("This scheme is being inherited!");
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        void frm_InpType_Disposed(object sender, EventArgs e)
        {
            try
            {
                Form_InputType_Opening = false;
                if (frm_InpType.TypeName == "")
                    GridViewDesign.Rows[CurrentRow].Cells[CurrentCell].Value = frm_InpType.DataType;
                else GridViewDesign.Rows[CurrentRow].Cells[CurrentCell].Value = frm_InpType.TypeName;

                GridViewDesign.Rows[CurrentRow].Cells[CurrentCell + 1].Value = frm_InpType.Domain;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void GridViewDesign_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                ProbScheme currentScheme = GetCurrentScheme();
                if (currentScheme != null)
                    if (e.ColumnIndex == 1)                    
                        if (currentScheme.isInherited(DB.relations))
                        {
                            MessageBox.Show("This scheme is read only!");
                            GridViewDesign.ClearSelection();
                        }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void GridViewDesign_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (GridViewDesign.CurrentCell.Value != null)
                {
                    if (e.ColumnIndex == 1)
                        for (int i = 0; i < GridViewDesign.Rows.Count - 1; i++)
                            if (GridViewDesign.CurrentCell.Value.ToString().CompareTo(GridViewDesign.Rows[i].Cells[1].Value.ToString()) == 0 && GridViewDesign.CurrentCell.RowIndex != i)
                            {
                                GridViewDesign.ClearSelection();
                                GridViewDesign.CurrentCell.Selected = true;
                                throw new Exception("The attribute's name has already existed on the scheme!");
                            }
                    string tmp = GridViewDesign.CurrentCell.Value.ToString();
                    GridViewDesign.CurrentCell.ToolTipText = tmp;
                }

                string tabSchemeName = xtraTabDatabase.TabPages[0].Text;
                if (!tabSchemeName.Contains("*"))
                    xtraTabDatabase.TabPages[0].Text = tabSchemeName + "*";

            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }       

        private void btn_Design_Home_Click(object sender, EventArgs e)
        {
            if (GridViewDesign.Rows.Count > 1)
            {
                GridViewDesign.CurrentCell = GridViewDesign.Rows[0].Cells[0];
                lblDesignRowNumberIndicator.Text = "1 / " + GridViewDesign.Rows.Count.ToString();
            }
        }

        private void btn_Design_Pre_Click(object sender, EventArgs e)
        {
            if (GridViewDesign.Rows.Count > 1)
            {
                int PreRow = GridViewDesign.CurrentRow.Index - 1;
                PreRow = (PreRow > 0 ? PreRow : 0);
                GridViewDesign.CurrentCell = GridViewDesign.Rows[PreRow].Cells[0];
                lblDesignRowNumberIndicator.Text = (PreRow + 1).ToString() + " / " + GridViewDesign.Rows.Count.ToString();
            }
        }

        private void btn_Design_Next_Click(object sender, EventArgs e)
        {
            if (GridViewDesign.Rows.Count > 1)
            {
                int nRow = GridViewDesign.Rows.Count;
                int NextRow = GridViewDesign.CurrentRow.Index + 1;
                NextRow = (NextRow < nRow - 1 ? NextRow : nRow - 1);
                GridViewDesign.CurrentCell = GridViewDesign.Rows[NextRow].Cells[0];
                lblDesignRowNumberIndicator.Text = (NextRow + 1).ToString() + " / " + GridViewDesign.Rows.Count.ToString();
            }
        }

        private void btn_Design_End_Click(object sender, EventArgs e)
        {
            if (GridViewDesign.Rows.Count > 1)
            {
                int nRow = GridViewDesign.Rows.Count;
                GridViewDesign.CurrentCell = GridViewDesign.Rows[nRow - 1].Cells[0];
                lblDesignRowNumberIndicator.Text = nRow + " / " + nRow;
            }
        }

        private void Btn_Design_DeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewDesign.Rows.Remove(GridViewDesign.CurrentRow);
                lblDesignRowNumberIndicator.Text = GridViewDesign.CurrentRow.Index + 1 + " / " + GridViewDesign.Rows.Count;
            }
            catch { }
        }

        private void Btn_Design_ClearData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to clear all attribute data ?", "Clear All Data", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int n = GridViewDesign.Rows.Count - 2;
                    for (int i = n; i >= 0; i--)
                        GridViewDesign.Rows.Remove(GridViewDesign.Rows[i]);
                    lblDesignRowNumberIndicator.Text = "1 / 1";
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Btn_Design_UpdateData_Click(object sender, EventArgs e)
        {
            SaveScheme();
        }

        private void GridViewDesign_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            if (GridViewDesign.CurrentRow != null)
                lblDesignRowNumberIndicator.Text = (GridViewDesign.CurrentRow.Index + 1) + " / " + GridViewDesign.Rows.Count;
            else lblDesignRowNumberIndicator.Text = "1 / " + GridViewDesign.Rows.Count;
        }

        private void GridViewDesign_Click(object sender, EventArgs e)
        {
            if (GridViewDesign.CurrentRow != null)
                lblDesignRowNumberIndicator.Text = (GridViewDesign.CurrentRow.Index + 1) + " / " + GridViewDesign.Rows.Count;
            else lblDesignRowNumberIndicator.Text = "1 / " + GridViewDesign.Rows.Count;
        }

        private void GridViewDesign_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                ProbScheme currentScheme = GetCurrentScheme();
                if (e.ColumnIndex == 0)
                    if (currentScheme != null)
                        if (currentScheme.isInherited(DB.relations))
                            MessageBox.Show("This scheme is being inherited!");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private ProbScheme GetCurrentScheme()
        {
            try
            {
                string schemeName = GetSchemeName(xtraTabDatabase.TabPages[0].Text);
                if (schemeName == string.Empty) return null;
                return DB.GetScheme(schemeName);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return null;
            }
        }

        #endregion

        #region * GridViewData

        private void btn_Data_Home_Click(object sender, EventArgs e)
        {
            if (GridViewData.Rows.Count > 0)
            {
                GridViewData.CurrentCell = GridViewData.Rows[0].Cells[0];
                lblDataRowNumberIndicator.Text = "1 / " + GridViewData.Rows.Count;
            }
        }

        private void btn_Data_Pre_Click(object sender, EventArgs e)
        {
            if (GridViewData.Rows.Count > 0)
            {
                int PreRow = GridViewData.CurrentRow.Index - 1;
                PreRow = (PreRow > 0 ? PreRow : 0);
                GridViewData.CurrentCell = GridViewData.Rows[PreRow].Cells[0];
                lblDataRowNumberIndicator.Text = (PreRow + 1) + " / " + GridViewData.Rows.Count;
            }
        }

        private void btn_Data_Next_Click(object sender, EventArgs e)
        {
            if (GridViewData.Rows.Count > 0)
            {
                int nRow = GridViewData.Rows.Count;
                int NextRow = GridViewData.CurrentRow.Index + 1;
                NextRow = (NextRow < nRow - 1 ? NextRow : nRow - 1);
                GridViewData.CurrentCell = GridViewData.Rows[NextRow].Cells[0];
                lblDataRowNumberIndicator.Text = (NextRow + 1) + " / " + GridViewData.Rows.Count;
            }
        }

        private void btn_Data_End_Click(object sender, EventArgs e)
        {
            try
            {
                if (GridViewData.Rows.Count > 0)
                {
                    int nRow = GridViewData.Rows.Count;
                    GridViewData.CurrentCell = GridViewData.Rows[nRow - 1].Cells[0];
                    lblDataRowNumberIndicator.Text = nRow + " / " + nRow;
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Btn_Data_ClearData_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = new DialogResult();
                result = MessageBox.Show("Are you sure want to clear all data?", "Clear All Data", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int n = GridViewData.Rows.Count - 2;
                    for (int i = n; i >= 0; i--)
                        GridViewData.Rows.Remove(GridViewData.Rows[i]);
                    UpdateDataRowNumber();
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Btn_Data_DeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewData.Rows.Remove(GridViewData.CurrentRow);
                UpdateDataRowNumber();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void Btn_UpdateData_Click(object sender, EventArgs e)
        {
            SaveRelation();
        }

        private void GridViewData_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            try
            {
                UpdateDataRowNumber();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }

        }

        private void GridViewData_Click(object sender, EventArgs e)
        {
            try
            {
                UpdateDataRowNumber();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void GridViewData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                CurrentCell = e.ColumnIndex;
                CurrentRow = e.RowIndex;

                if (GridViewData.CurrentCell.Value != null)
                {
                    string value = GridViewData.CurrentCell.Value.ToString();
                    if (!DB.isProbTripleValue(value))
                        throw new Exception("Syntax Error! Cannot convert this value to a Probabilistic Triple!");
                    if (!GetCurrentRelation().scheme.attributes[CurrentCell].type.CheckDataType(value))
                        throw new Exception("Attribute value does not match the data type!");
                }
                else throw new Exception("NULL value is unacceptable in PRDB!");

                //string tabRelationName = xtraTabDatabase.TabPages[1].Text;
                //if (!tabRelationName.Contains("*"))
                //    xtraTabDatabase.TabPages[1].Text = tabRelationName + "*";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                //rollbackcell = true;
            }

        }

        private void GridViewData_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string tabRelationName = xtraTabDatabase.TabPages[1].Text;
                if (!tabRelationName.Contains("*"))
                    xtraTabDatabase.TabPages[1].Text = tabRelationName + "*";
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void UpdateDataRowNumber()
        {
            try
            {
                if (GridViewData.Rows.Count == 0)
                    lblDataRowNumberIndicator.Text = "0 / 0";
                else if (GridViewData.CurrentRow != null)
                    lblDataRowNumberIndicator.Text = (GridViewData.CurrentRow.Index + 1) + " / " + GridViewData.Rows.Count;
                else lblDataRowNumberIndicator.Text = "1 / " + GridViewData.Rows.Count;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private ProbRelation GetCurrentRelation()
        {
            try
            {
                string relationName = GetRelationName(xtraTabDatabase.TabPages[1].Text);
                if (relationName == string.Empty) return null;
                return DB.GetRelation(relationName);
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
                return null;
            }
        }

        #endregion

        #region * GridViewValue

        private void SwitchValueState(bool state)
        {
            GridViewValue.Visible = state;
            btn_Value_Home.Enabled = state;
            btn_Value_Pre.Enabled = state;
            btn_Value_Next.Enabled = state;
            btn_Value_End.Enabled = state;
            btn_Value_DeleteRow.Enabled = state;
            btn_Value_AddNewRow.Enabled = state;
            lblValueRowNumberIndicator.Enabled = state;
            Checkbox_UUD.Checked = state;
            Checkbox_UD.Checked = !state;
            label1.Enabled = !state;
            label2.Enabled = !state;
            txtMinProb.Enabled = !state;
            txtMaxProb.Enabled = !state;
            txtValue.Visible = !state;
        }

        private void Checkbox_UD_CheckedChanged(object sender, EventArgs e)
        {
            // Chuyển đổi trạng thái các controls trong khung nhập giá trị
            // Nhập giá trị xác suất theo phân bố đều
            if (Checkbox_UD.Checked) SwitchValueState(false);
        }

        private void Checkbox_UUD_CheckedChanged(object sender, EventArgs e)
        {
            // Chuyển đổi trạng thái các controls trong khung nhập giá trị
            // Nhập giá trị xác suất theo phân bố không đều
            if (Checkbox_UUD.Checked) SwitchValueState(true);
        }

        private string Stdize(string S)     // Chuẩn hóa chuỗi cắt bỏ các dấu , dư thừa
        {
            string R = "";
            int i = 0;
            while (S[i] == ',') i++;
            int k = S.Length - 1;
            while (S[k] == ',') k--;
            for (int j = i; j <= k; j++)
                if (S[j] != ',') R += S[j];
                else if (S[j - 1] != ',') R += S[j];
            return R;
        }

        private void btn_Value_AddNewRow_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewValue.Rows.Add();
                UpdateValueRowNumber();
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btn_Value_DeleteRow_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewValue.Rows.Remove(GridViewValue.CurrentRow);
                UpdateValueRowNumber();
            }
            catch (Exception Ex) 
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void btn_Value_UpdateData_Click(object sender, EventArgs e)
        {
            int UpdateRow, UpdateCell;

            // Nhập bộ ba xác suất theo phân bố không đều
            if (Checkbox_UUD.Checked) 
            {
                try
                {
                    if (GridViewValue.Rows.Count == 0)
                        MessageBox.Show("The value is not entered!");
                    else                    // Lấy tập giá trị từ GridViewValue
                    {
                        int n = GridViewValue.Rows.Count;
                        GridViewValue.CurrentCell = GridViewValue.CurrentRow.Cells[0];
                        GridViewValue.CurrentCell = GridViewValue.CurrentRow.Cells[1];
                        GridViewValue.CurrentCell = GridViewValue.CurrentRow.Cells[2];
                        ProbTriple triple = new ProbTriple();
                        for (int i = 0; i < n; i++)
                        {
                            triple.values.Add(GridViewValue.Rows[i].Cells["ColumnValue"].Value.ToString());
                            triple.minprob.Add(Math.Round(Convert.ToDouble(GridViewValue.Rows[i].Cells["ColumnMinProb"].Value),2));
                            triple.maxprob.Add(Math.Round(Convert.ToDouble(GridViewValue.Rows[i].Cells["ColumnMaxProb"].Value),2));
                        }
                        UpdateRow = GridViewData.CurrentRow.Index;
                        UpdateCell = GridViewData.CurrentCell.ColumnIndex;
                        if (UpdateRow == GridViewData.Rows.Count - 1)
                        {
                            GridViewData.Rows.Add();
                            UpdateDataRowNumber();
                        }
                        GridViewData.CurrentCell = GridViewData.Rows[UpdateRow].Cells[UpdateCell];
                        GridViewData.CurrentCell.Value = triple.GetStrValue();
                    }
                }
                catch (Exception Ex)
                {
                    MessageBox.Show(Ex.Message);
                }
            }
            else // Nhập bộ ba xác suất theo phân bố đều 
            {
                if (txtMinProb.Text == "" || txtMaxProb.Text == "")
                    MessageBox.Show("Sum of MinProb or MaxProb is missing!");
                else if (txtValue.Text == "")
                    MessageBox.Show("Attribute values are not entered!");
                else     // Lấy tập giá trị từ TextBox và phân bố xác suất cho từng giá trị
                {
                    try
                    {
                        double minprob, maxprob;
                        string[] value;
                        value = Stdize(txtValue.Text.Replace("\r\n", ",")).Split(',');
                        for (int i = 0; i < value.Length; i++) value[i] = value[i].Trim();

                        minprob = Convert.ToDouble(txtMinProb.Text);
                        maxprob = Convert.ToDouble(txtMaxProb.Text);
                        int n = value.Length;

                        if (maxprob / n > 1.0)
                            throw new Exception("Upper bound of the value is larger than 1: " + (maxprob / n).ToString());
                        
                        ProbTriple triple = new ProbTriple();
                        for (int i = 0; i < n; i++)
                        {
                            triple.values.Add(value[i]);
                            triple.minprob.Add(Math.Round(minprob / n,2));
                            triple.maxprob.Add(Math.Round(maxprob / n,2));
                        }
                        UpdateRow = GridViewData.CurrentRow.Index;
                        UpdateCell = GridViewData.CurrentCell.ColumnIndex;
                        if (UpdateRow == GridViewData.Rows.Count - 1)
                        {
                            GridViewData.Rows.Add();
                            UpdateDataRowNumber();
                        }
                        GridViewData.CurrentCell = GridViewData.Rows[UpdateRow].Cells[UpdateCell];
                        GridViewData.CurrentCell.Value = triple.GetStrValue();
                    }
                    catch (Exception Ex)
                    {
                        MessageBox.Show(Ex.Message);
                    }
                }
            }
        }

        private void GridViewValue_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (GridViewValue.CurrentCell.Value != null)
            {
                if (e.ColumnIndex > 0)  // Giá trị nhập vào các ô MinProb và MaxProb
                {
                    try
                    {
                        double Prob = Convert.ToDouble(GridViewValue.CurrentCell.Value);
                        if (Prob < 0.0 || Prob > 1.0)
                            MessageBox.Show("Probabilistic value must belong to [0,1]!");
                    }
                    catch
                    {
                        MessageBox.Show("Probabilistic value must be a real number!");
                    }
                }
                else          // Giá trị nhập vào ô Value
                {
                    string StrValue = GridViewValue.CurrentCell.Value.ToString();
                }
            }
        }

        private void Btn_Value_ClearData_Click(object sender, EventArgs e)
        {
            if (Checkbox_UUD.Checked)
            {
                GridViewValue.Rows.Clear();
                UpdateValueRowNumber();
            }
            else
            {
                txtMaxProb.Text = "";
                txtMinProb.Text = "";
                txtValue.Text = "";                
            }
        }

        private void btn_Value_Home_Click(object sender, EventArgs e)
        {
            if (GridViewValue.Rows.Count > 0)
            {
                GridViewValue.CurrentCell = GridViewValue.Rows[0].Cells[0];
                lblValueRowNumberIndicator.Text = "1 / " + GridViewValue.Rows.Count.ToString();
            }
        }

        private void btn_Value_Pre_Click(object sender, EventArgs e)
        {
            if (GridViewValue.Rows.Count > 0)
            {
                int PreRow = GridViewValue.CurrentRow.Index - 1;
                PreRow = (PreRow > 0 ? PreRow : 0);
                GridViewValue.CurrentCell = GridViewValue.Rows[PreRow].Cells[0];
                lblValueRowNumberIndicator.Text = (PreRow + 1).ToString() + " / " + GridViewValue.Rows.Count.ToString();
            }
        }

        private void btn_Value_Next_Click(object sender, EventArgs e)
        {
            if (GridViewValue.Rows.Count > 0)
            {
                int nRow = GridViewValue.Rows.Count;
                int NextRow = GridViewValue.CurrentRow.Index + 1;
                NextRow = (NextRow < nRow - 1 ? NextRow : nRow - 1);
                GridViewValue.CurrentCell = GridViewValue.Rows[NextRow].Cells[0];
                lblValueRowNumberIndicator.Text = (NextRow + 1).ToString() + " / " + GridViewValue.Rows.Count.ToString();
            }
        }

        private void btn_Value_End_Click(object sender, EventArgs e)
        {
            if (GridViewValue.Rows.Count > 0)
            {
                int nRow = GridViewValue.Rows.Count;
                GridViewValue.CurrentCell = GridViewValue.Rows[nRow - 1].Cells[0];
                lblValueRowNumberIndicator.Text = nRow.ToString() + " / " + nRow.ToString();
            }
        }

        private void txtMinProb_Leave(object sender, EventArgs e)
        {
            try
            {
                string strMinProb = txtMinProb.Text.Trim();
                if (strMinProb != "")
                {
                    double minProb = Convert.ToDouble(txtMinProb.Text);
                    if (minProb > 1.0)
                    {
                        MessageBox.Show("Sum of lower bounds must be less than 1!");
                        txtMinProb.Focus();
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        private void GridViewValue_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            UpdateValueRowNumber();
        }

        private void GridViewValue_Click(object sender, EventArgs e)
        {
            UpdateValueRowNumber();
        }

        private void UpdateValueRowNumber()
        {
            try
            {
                if (GridViewValue.Rows.Count == 0)
                    lblValueRowNumberIndicator.Text = "0 / 0";
                else if (GridViewValue.CurrentRow != null)
                    lblValueRowNumberIndicator.Text = (GridViewValue.CurrentRow.Index + 1) + " / " + GridViewValue.Rows.Count;
                else lblValueRowNumberIndicator.Text = "1 / " + GridViewValue.Rows.Count;
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion                

        #region * xtraTabDatabase

        private void xtraTabDatabase_SelectedPageChanging(object sender, TabPageChangingEventArgs e)
        {
            try
            {
                ProbScheme currentScheme = GetCurrentScheme();
                if (currentScheme != null)
                {
                    if (currentScheme.isInherited(DB.relations))
                    {
                        GridViewDesign.Columns[0].ReadOnly = true;
                        GridViewDesign.Columns[1].ReadOnly = true;
                        GridViewDesign.Columns[2].ReadOnly = true;
                    }
                    else
                    {
                        GridViewDesign.Columns[0].ReadOnly = false;
                        GridViewDesign.Columns[1].ReadOnly = false;
                        GridViewDesign.Columns[2].ReadOnly = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }

        #endregion               


    }
}
