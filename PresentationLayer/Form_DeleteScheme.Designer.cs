namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_DeleteScheme
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
            this.ComboBox_DeleteScheme = new System.Windows.Forms.ComboBox();
            this.ButtonCancel_DeleteScheme = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK_DeleteScheme = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ComboBox_DeleteScheme
            // 
            this.ComboBox_DeleteScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_DeleteScheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_DeleteScheme.FormattingEnabled = true;
            this.ComboBox_DeleteScheme.Location = new System.Drawing.Point(15, 48);
            this.ComboBox_DeleteScheme.Name = "ComboBox_DeleteScheme";
            this.ComboBox_DeleteScheme.Size = new System.Drawing.Size(334, 24);
            this.ComboBox_DeleteScheme.TabIndex = 36;
            // 
            // ButtonCancel_DeleteScheme
            // 
            this.ButtonCancel_DeleteScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel_DeleteScheme.Appearance.Options.UseFont = true;
            this.ButtonCancel_DeleteScheme.Location = new System.Drawing.Point(262, 92);
            this.ButtonCancel_DeleteScheme.Name = "ButtonCancel_DeleteScheme";
            this.ButtonCancel_DeleteScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonCancel_DeleteScheme.TabIndex = 35;
            this.ButtonCancel_DeleteScheme.Text = "Cancel";
            this.ButtonCancel_DeleteScheme.Click += new System.EventHandler(this.ButtonCancel_DeleteScheme_Click);
            // 
            // ButtonOK_DeleteScheme
            // 
            this.ButtonOK_DeleteScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK_DeleteScheme.Appearance.Options.UseFont = true;
            this.ButtonOK_DeleteScheme.Location = new System.Drawing.Point(154, 92);
            this.ButtonOK_DeleteScheme.Name = "ButtonOK_DeleteScheme";
            this.ButtonOK_DeleteScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK_DeleteScheme.TabIndex = 34;
            this.ButtonOK_DeleteScheme.Text = "OK";
            this.ButtonOK_DeleteScheme.Click += new System.EventHandler(this.ButtonOK_DeleteScheme_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 16);
            this.label1.TabIndex = 33;
            this.label1.Text = "Deleted scheme:";
            // 
            // Form_DeleteScheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(365, 148);
            this.Controls.Add(this.ComboBox_DeleteScheme);
            this.Controls.Add(this.ButtonCancel_DeleteScheme);
            this.Controls.Add(this.ButtonOK_DeleteScheme);
            this.Controls.Add(this.label1);
            this.Name = "Form_DeleteScheme";
            this.Text = "Delete Scheme...";
            this.Load += new System.EventHandler(this.Form_DeleteScheme_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBox_DeleteScheme;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel_DeleteScheme;
        private DevExpress.XtraEditors.SimpleButton ButtonOK_DeleteScheme;
        private System.Windows.Forms.Label label1;
    }
}