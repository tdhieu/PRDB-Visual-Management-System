namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_SaveRelation
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.radioButton_SelectRelation = new System.Windows.Forms.RadioButton();
            this.radioButton_NewRelation = new System.Windows.Forms.RadioButton();
            this.ComboBox_RelationNames = new System.Windows.Forms.ComboBox();
            this.ButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtRelationName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ComboBox_SchemeNames = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // radioButton_SelectRelation
            // 
            this.radioButton_SelectRelation.AutoSize = true;
            this.radioButton_SelectRelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SelectRelation.Location = new System.Drawing.Point(38, 129);
            this.radioButton_SelectRelation.Name = "radioButton_SelectRelation";
            this.radioButton_SelectRelation.Size = new System.Drawing.Size(128, 20);
            this.radioButton_SelectRelation.TabIndex = 61;
            this.radioButton_SelectRelation.Text = "Existing Relation:";
            this.radioButton_SelectRelation.UseVisualStyleBackColor = true;
            this.radioButton_SelectRelation.CheckedChanged += new System.EventHandler(this.radioButton_SelectRelation_CheckedChanged);
            // 
            // radioButton_NewRelation
            // 
            this.radioButton_NewRelation.AutoSize = true;
            this.radioButton_NewRelation.Checked = true;
            this.radioButton_NewRelation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_NewRelation.Location = new System.Drawing.Point(38, 42);
            this.radioButton_NewRelation.Name = "radioButton_NewRelation";
            this.radioButton_NewRelation.Size = new System.Drawing.Size(96, 20);
            this.radioButton_NewRelation.TabIndex = 60;
            this.radioButton_NewRelation.TabStop = true;
            this.radioButton_NewRelation.Text = "New Name:";
            this.radioButton_NewRelation.UseVisualStyleBackColor = true;
            this.radioButton_NewRelation.CheckedChanged += new System.EventHandler(this.radioButton_NewRelation_CheckedChanged);
            // 
            // ComboBox_RelationNames
            // 
            this.ComboBox_RelationNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_RelationNames.Enabled = false;
            this.ComboBox_RelationNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_RelationNames.FormattingEnabled = true;
            this.ComboBox_RelationNames.Location = new System.Drawing.Point(183, 128);
            this.ComboBox_RelationNames.Name = "ComboBox_RelationNames";
            this.ComboBox_RelationNames.Size = new System.Drawing.Size(238, 24);
            this.ComboBox_RelationNames.TabIndex = 59;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel.Appearance.Options.UseFont = true;
            this.ButtonCancel.Location = new System.Drawing.Point(334, 182);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(87, 23);
            this.ButtonCancel.TabIndex = 58;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // ButtonOK
            // 
            this.ButtonOK.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK.Appearance.Options.UseFont = true;
            this.ButtonOK.Location = new System.Drawing.Point(226, 182);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK.TabIndex = 57;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // txtRelationName
            // 
            this.txtRelationName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRelationName.Location = new System.Drawing.Point(183, 41);
            this.txtRelationName.Name = "txtRelationName";
            this.txtRelationName.Size = new System.Drawing.Size(238, 22);
            this.txtRelationName.TabIndex = 56;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.label1.Location = new System.Drawing.Point(55, 85);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 16);
            this.label1.TabIndex = 62;
            this.label1.Text = "Related Scheme:";
            // 
            // ComboBox_SchemeNames
            // 
            this.ComboBox_SchemeNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_SchemeNames.Enabled = false;
            this.ComboBox_SchemeNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_SchemeNames.FormattingEnabled = true;
            this.ComboBox_SchemeNames.Location = new System.Drawing.Point(183, 82);
            this.ComboBox_SchemeNames.Name = "ComboBox_SchemeNames";
            this.ComboBox_SchemeNames.Size = new System.Drawing.Size(238, 24);
            this.ComboBox_SchemeNames.TabIndex = 63;
            // 
            // Form_SaveRelation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(461, 259);
            this.Controls.Add(this.ComboBox_SchemeNames);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.radioButton_SelectRelation);
            this.Controls.Add(this.radioButton_NewRelation);
            this.Controls.Add(this.ComboBox_RelationNames);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.txtRelationName);
            this.Name = "Form_SaveRelation";
            this.Text = "Save Relation...";
            this.Load += new System.EventHandler(this.Form_SaveRelation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_SelectRelation;
        private System.Windows.Forms.RadioButton radioButton_NewRelation;
        private System.Windows.Forms.ComboBox ComboBox_RelationNames;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
        private System.Windows.Forms.TextBox txtRelationName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox ComboBox_SchemeNames;
    }
}