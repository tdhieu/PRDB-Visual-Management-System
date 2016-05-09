﻿namespace PRDB_Visual_Management.PresentationLayer
{
    partial class Form_RenameDB
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtDBName = new System.Windows.Forms.TextBox();
            this.ButtonCancel_NewTable = new DevExpress.XtraEditors.SimpleButton();
            this.ButtonOK_NewTable = new DevExpress.XtraEditors.SimpleButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter database name:";
            // 
            // txtDBName
            // 
            this.txtDBName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDBName.Location = new System.Drawing.Point(16, 56);
            this.txtDBName.Name = "txtDBName";
            this.txtDBName.Size = new System.Drawing.Size(350, 22);
            this.txtDBName.TabIndex = 1;
            // 
            // ButtonCancel_NewTable
            // 
            this.ButtonCancel_NewTable.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonCancel_NewTable.Appearance.Options.UseFont = true;
            this.ButtonCancel_NewTable.Location = new System.Drawing.Point(274, 108);
            this.ButtonCancel_NewTable.Name = "ButtonCancel_NewTable";
            this.ButtonCancel_NewTable.Size = new System.Drawing.Size(92, 23);
            this.ButtonCancel_NewTable.TabIndex = 9;
            this.ButtonCancel_NewTable.Text = "Cancel";
            this.ButtonCancel_NewTable.Click += new System.EventHandler(this.ButtonCancel_NewTable_Click);
            // 
            // ButtonOK_NewTable
            // 
            this.ButtonOK_NewTable.Appearance.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ButtonOK_NewTable.Appearance.Options.UseFont = true;
            this.ButtonOK_NewTable.Location = new System.Drawing.Point(165, 108);
            this.ButtonOK_NewTable.Name = "ButtonOK_NewTable";
            this.ButtonOK_NewTable.Size = new System.Drawing.Size(91, 23);
            this.ButtonOK_NewTable.TabIndex = 8;
            this.ButtonOK_NewTable.Text = "OK";
            this.ButtonOK_NewTable.Click += new System.EventHandler(this.ButtonOK_NewTable_Click);
            // 
            // Form_RenameDB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.ClientSize = new System.Drawing.Size(381, 158);
            this.Controls.Add(this.ButtonCancel_NewTable);
            this.Controls.Add(this.ButtonOK_NewTable);
            this.Controls.Add(this.txtDBName);
            this.Controls.Add(this.label1);
            this.Name = "Form_RenameDB";
            this.Text = "Rename Database...";
            this.Load += new System.EventHandler(this.Form_RenameDB_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDBName;
        private DevExpress.XtraEditors.SimpleButton ButtonCancel_NewTable;
        private DevExpress.XtraEditors.SimpleButton ButtonOK_NewTable;
    }
}