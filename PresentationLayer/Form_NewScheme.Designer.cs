namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_NewScheme
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
            this.ButtonCancel_NewSchema = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK_NewSchema = new DevExpress.XtraEditors.SimpleButton();
            this.txtSchemeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonCancel_NewSchema
            // 
            this.ButtonCancel_NewSchema.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel_NewSchema.Appearance.Options.UseFont = true;
            this.ButtonCancel_NewSchema.Location = new System.Drawing.Point(265, 89);
            this.ButtonCancel_NewSchema.Name = "ButtonCancel_NewSchema";
            this.ButtonCancel_NewSchema.Size = new System.Drawing.Size(87, 23);
            this.ButtonCancel_NewSchema.TabIndex = 15;
            this.ButtonCancel_NewSchema.Text = "Cancel";
            this.ButtonCancel_NewSchema.Click += new System.EventHandler(this.ButtonCancel_NewTable_Click);
            // 
            // ButtonOK_NewSchema
            // 
            this.ButtonOK_NewSchema.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK_NewSchema.Appearance.Options.UseFont = true;
            this.ButtonOK_NewSchema.Location = new System.Drawing.Point(157, 89);
            this.ButtonOK_NewSchema.Name = "ButtonOK_NewSchema";
            this.ButtonOK_NewSchema.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK_NewSchema.TabIndex = 14;
            this.ButtonOK_NewSchema.Text = "OK";
            this.ButtonOK_NewSchema.Click += new System.EventHandler(this.ButtonOK_NewTable_Click);
            // 
            // txtSchemeName
            // 
            this.txtSchemeName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSchemeName.Location = new System.Drawing.Point(18, 46);
            this.txtSchemeName.Name = "txtSchemeName";
            this.txtSchemeName.Size = new System.Drawing.Size(334, 22);
            this.txtSchemeName.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 16);
            this.label1.TabIndex = 12;
            this.label1.Text = "Scheme name:";
            // 
            // Form_NewScheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(370, 142);
            this.Controls.Add(this.ButtonCancel_NewSchema);
            this.Controls.Add(this.ButtonOK_NewSchema);
            this.Controls.Add(this.txtSchemeName);
            this.Controls.Add(this.label1);
            this.Name = "Form_NewScheme";
            this.Text = "Create New Scheme...";
            this.Load += new System.EventHandler(this.Form_NewScheme_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.SimpleButton ButtonCancel_NewSchema;
        private DevExpress.XtraEditors.SimpleButton ButtonOK_NewSchema;
        private System.Windows.Forms.TextBox txtSchemeName;
        private System.Windows.Forms.Label label1;
    }
}