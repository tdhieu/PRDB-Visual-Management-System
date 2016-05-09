namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_OpenScheme
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
            this.ComboBox_OpenScheme = new System.Windows.Forms.ComboBox();
            this.ButtonCancel_OpenScheme = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK_OpenScheme = new DevExpress.XtraEditors.SimpleButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ComboBox_OpenScheme
            // 
            this.ComboBox_OpenScheme.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ComboBox_OpenScheme.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ComboBox_OpenScheme.FormattingEnabled = true;
            this.ComboBox_OpenScheme.Location = new System.Drawing.Point(129, 19);
            this.ComboBox_OpenScheme.Name = "ComboBox_OpenScheme";
            this.ComboBox_OpenScheme.Size = new System.Drawing.Size(214, 24);
            this.ComboBox_OpenScheme.TabIndex = 52;
            // 
            // ButtonCancel_OpenScheme
            // 
            this.ButtonCancel_OpenScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel_OpenScheme.Appearance.Options.UseFont = true;
            this.ButtonCancel_OpenScheme.Location = new System.Drawing.Point(205, 93);
            this.ButtonCancel_OpenScheme.Name = "ButtonCancel_OpenScheme";
            this.ButtonCancel_OpenScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonCancel_OpenScheme.TabIndex = 51;
            this.ButtonCancel_OpenScheme.Text = "Cancel";
            this.ButtonCancel_OpenScheme.Click += new System.EventHandler(this.ButtonCancel_OpenScheme_Click);
            // 
            // ButtonOK_OpenScheme
            // 
            this.ButtonOK_OpenScheme.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK_OpenScheme.Appearance.Options.UseFont = true;
            this.ButtonOK_OpenScheme.Location = new System.Drawing.Point(65, 93);
            this.ButtonOK_OpenScheme.Name = "ButtonOK_OpenScheme";
            this.ButtonOK_OpenScheme.Size = new System.Drawing.Size(87, 23);
            this.ButtonOK_OpenScheme.TabIndex = 50;
            this.ButtonOK_OpenScheme.Text = "OK";
            this.ButtonOK_OpenScheme.Click += new System.EventHandler(this.ButtonOK_OpenScheme_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 16);
            this.label1.TabIndex = 49;
            this.label1.Text = "Select a scheme:";
            // 
            // Form_OpenScheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(366, 145);
            this.Controls.Add(this.ComboBox_OpenScheme);
            this.Controls.Add(this.ButtonCancel_OpenScheme);
            this.Controls.Add(this.ButtonOK_OpenScheme);
            this.Controls.Add(this.label1);
            this.Name = "Form_OpenScheme";
            this.Text = "Open Scheme...";
            this.Load += new System.EventHandler(this.Form_OpenScheme_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBox_OpenScheme;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel_OpenScheme;
        private DevExpress.XtraEditors.SimpleButton ButtonOK_OpenScheme;
        private System.Windows.Forms.Label label1;
    }
}