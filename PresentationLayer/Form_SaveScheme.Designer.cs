namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_SaveScheme
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
            this.ButtonCancel_SaveScheme = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK_SaveScheme = new DevExpress.XtraEditors.SimpleButton();
            this.txtSchemeName = new System.Windows.Forms.TextBox();
            this.ComboBox_SchemeNames = new System.Windows.Forms.ComboBox();
            this.radioButton_NewScheme = new System.Windows.Forms.RadioButton();
            this.radioButton_SelectScheme = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // ButtonCancel_SaveScheme
            // 
            this.ButtonCancel_SaveScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel_SaveScheme.Appearance.Options.UseFont = true;
            this.ButtonCancel_SaveScheme.Location = new System.Drawing.Point(331, 131);
            this.ButtonCancel_SaveScheme.Name = "ButtonCancel_SaveScheme";
            this.ButtonCancel_SaveScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonCancel_SaveScheme.TabIndex = 19;
            this.ButtonCancel_SaveScheme.Text = "Cancel";
            this.ButtonCancel_SaveScheme.Click += new System.EventHandler(this.ButtonCancel_SaveScheme_Click);
            // 
            // ButtonOK_SaveScheme
            // 
            this.ButtonOK_SaveScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK_SaveScheme.Appearance.Options.UseFont = true;
            this.ButtonOK_SaveScheme.Location = new System.Drawing.Point(223, 131);
            this.ButtonOK_SaveScheme.Name = "ButtonOK_SaveScheme";
            this.ButtonOK_SaveScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK_SaveScheme.TabIndex = 18;
            this.ButtonOK_SaveScheme.Text = "OK";
            this.ButtonOK_SaveScheme.Click += new System.EventHandler(this.ButtonOK_SaveScheme_Click);
            // 
            // txtSchemeName
            // 
            this.txtSchemeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSchemeName.Location = new System.Drawing.Point(180, 31);
            this.txtSchemeName.Name = "txtSchemeName";
            this.txtSchemeName.Size = new System.Drawing.Size(238, 22);
            this.txtSchemeName.TabIndex = 17;
            // 
            // ComboBox_SchemeNames
            // 
            this.ComboBox_SchemeNames.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_SchemeNames.Enabled = false;
            this.ComboBox_SchemeNames.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_SchemeNames.FormattingEnabled = true;
            this.ComboBox_SchemeNames.Location = new System.Drawing.Point(180, 77);
            this.ComboBox_SchemeNames.Name = "ComboBox_SchemeNames";
            this.ComboBox_SchemeNames.Size = new System.Drawing.Size(238, 24);
            this.ComboBox_SchemeNames.TabIndex = 53;
            // 
            // radioButton_NewScheme
            // 
            this.radioButton_NewScheme.AutoSize = true;
            this.radioButton_NewScheme.Checked = true;
            this.radioButton_NewScheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_NewScheme.Location = new System.Drawing.Point(35, 32);
            this.radioButton_NewScheme.Name = "radioButton_NewScheme";
            this.radioButton_NewScheme.Size = new System.Drawing.Size(109, 20);
            this.radioButton_NewScheme.TabIndex = 54;
            this.radioButton_NewScheme.TabStop = true;
            this.radioButton_NewScheme.Text = "New Scheme:";
            this.radioButton_NewScheme.UseVisualStyleBackColor = true;
            this.radioButton_NewScheme.CheckedChanged += new System.EventHandler(this.radioButton_NewScheme_CheckedChanged);
            // 
            // radioButton_SelectScheme
            // 
            this.radioButton_SelectScheme.AutoSize = true;
            this.radioButton_SelectScheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radioButton_SelectScheme.Location = new System.Drawing.Point(35, 78);
            this.radioButton_SelectScheme.Name = "radioButton_SelectScheme";
            this.radioButton_SelectScheme.Size = new System.Drawing.Size(128, 20);
            this.radioButton_SelectScheme.TabIndex = 55;
            this.radioButton_SelectScheme.Text = "Existing Scheme:";
            this.radioButton_SelectScheme.UseVisualStyleBackColor = true;
            this.radioButton_SelectScheme.CheckedChanged += new System.EventHandler(this.radioButton_SelectScheme_CheckedChanged);
            // 
            // Form_SaveScheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(454, 193);
            this.Controls.Add(this.radioButton_SelectScheme);
            this.Controls.Add(this.radioButton_NewScheme);
            this.Controls.Add(this.ComboBox_SchemeNames);
            this.Controls.Add(this.ButtonCancel_SaveScheme);
            this.Controls.Add(this.ButtonOK_SaveScheme);
            this.Controls.Add(this.txtSchemeName);
            this.Name = "Form_SaveScheme";
            this.Text = "Save scheme...";
            this.Load += new System.EventHandler(this.Form_SaveScheme_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ButtonCancel_SaveScheme;
        private DevExpress.XtraEditors.SimpleButton ButtonOK_SaveScheme;
        private System.Windows.Forms.TextBox txtSchemeName;
        private System.Windows.Forms.ComboBox ComboBox_SchemeNames;
        private System.Windows.Forms.RadioButton radioButton_NewScheme;
        private System.Windows.Forms.RadioButton radioButton_SelectScheme;
    }
}