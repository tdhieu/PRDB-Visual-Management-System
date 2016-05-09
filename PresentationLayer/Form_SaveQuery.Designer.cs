namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_SaveQuery
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
            this.radioButton_SelectQuery = new System.Windows.Forms.RadioButton();
            this.radioButton_NewQuery = new System.Windows.Forms.RadioButton();
            this.ComboBox_QueryNames = new System.Windows.Forms.ComboBox();
            this.ButtonCancel = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK = new DevExpress.XtraEditors.SimpleButton();
            this.txtQueryName = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // radioButton_SelectQuery
            // 
            this.radioButton_SelectQuery.AutoSize = true;
            this.radioButton_SelectQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SelectQuery.Location = new System.Drawing.Point(38, 79);
            this.radioButton_SelectQuery.Name = "radioButton_SelectQuery";
            this.radioButton_SelectQuery.Size = new System.Drawing.Size(114, 20);
            this.radioButton_SelectQuery.TabIndex = 61;
            this.radioButton_SelectQuery.Text = "Existing Query:";
            this.radioButton_SelectQuery.UseVisualStyleBackColor = true;
            this.radioButton_SelectQuery.CheckedChanged += new System.EventHandler(this.radioButton_SelectQuery_CheckedChanged);
            // 
            // radioButton_NewQuery
            // 
            this.radioButton_NewQuery.AutoSize = true;
            this.radioButton_NewQuery.Checked = true;
            this.radioButton_NewQuery.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_NewQuery.Location = new System.Drawing.Point(38, 33);
            this.radioButton_NewQuery.Name = "radioButton_NewQuery";
            this.radioButton_NewQuery.Size = new System.Drawing.Size(95, 20);
            this.radioButton_NewQuery.TabIndex = 60;
            this.radioButton_NewQuery.TabStop = true;
            this.radioButton_NewQuery.Text = "New Query:";
            this.radioButton_NewQuery.UseVisualStyleBackColor = true;
            this.radioButton_NewQuery.CheckedChanged += new System.EventHandler(this.radioButton_NewQuery_CheckedChanged);
            // 
            // ComboBox_QueryNames
            // 
            this.ComboBox_QueryNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_QueryNames.Enabled = false;
            this.ComboBox_QueryNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_QueryNames.FormattingEnabled = true;
            this.ComboBox_QueryNames.Location = new System.Drawing.Point(183, 78);
            this.ComboBox_QueryNames.Name = "ComboBox_QueryNames";
            this.ComboBox_QueryNames.Size = new System.Drawing.Size(238, 24);
            this.ComboBox_QueryNames.TabIndex = 59;
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel.Appearance.Options.UseFont = true;
            this.ButtonCancel.Location = new System.Drawing.Point(334, 132);
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
            this.ButtonOK.Location = new System.Drawing.Point(226, 132);
            this.ButtonOK.Name = "ButtonOK";
            this.ButtonOK.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK.TabIndex = 57;
            this.ButtonOK.Text = "OK";
            this.ButtonOK.Click += new System.EventHandler(this.ButtonOK_Click);
            // 
            // txtQueryName
            // 
            this.txtQueryName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtQueryName.Location = new System.Drawing.Point(183, 32);
            this.txtQueryName.Name = "txtQueryName";
            this.txtQueryName.Size = new System.Drawing.Size(238, 22);
            this.txtQueryName.TabIndex = 56;
            // 
            // Form_SaveQuery
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(465, 209);
            this.Controls.Add(this.radioButton_SelectQuery);
            this.Controls.Add(this.radioButton_NewQuery);
            this.Controls.Add(this.ComboBox_QueryNames);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.ButtonOK);
            this.Controls.Add(this.txtQueryName);
            this.Name = "Form_SaveQuery";
            this.Text = "Save Query...";
            this.Load += new System.EventHandler(this.Form_SaveQuery_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton radioButton_SelectQuery;
        private System.Windows.Forms.RadioButton radioButton_NewQuery;
        private System.Windows.Forms.ComboBox ComboBox_QueryNames;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel;
        private DevExpress.XtraEditors.SimpleButton ButtonOK;
        private System.Windows.Forms.TextBox txtQueryName;
    }
}