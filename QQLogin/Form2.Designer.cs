namespace QQLogin
{
    partial class Form2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvContact = new System.Windows.Forms.DataGridView();
            this.GroupFace = new System.Windows.Forms.DataGridViewImageColumn();
            this.GroupGid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GroupTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvContact)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvContact
            // 
            this.dgvContact.AllowUserToAddRows = false;
            this.dgvContact.AllowUserToDeleteRows = false;
            this.dgvContact.AllowUserToResizeColumns = false;
            this.dgvContact.AllowUserToResizeRows = false;
            this.dgvContact.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(248)))), ((int)(((byte)(248)))), ((int)(((byte)(248)))));
            this.dgvContact.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvContact.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgvContact.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvContact.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgvContact.ColumnHeadersVisible = false;
            this.dgvContact.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GroupFace,
            this.GroupGid,
            this.GroupTitle});
            this.dgvContact.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(238)))), ((int)(((byte)(238)))), ((int)(((byte)(238)))));
            this.dgvContact.Location = new System.Drawing.Point(-1, 1);
            this.dgvContact.MultiSelect = false;
            this.dgvContact.Name = "dgvContact";
            this.dgvContact.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.InfoText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvContact.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvContact.RowHeadersVisible = false;
            this.dgvContact.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.Transparent;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(84)))), ((int)(((byte)(84)))), ((int)(((byte)(158)))));
            this.dgvContact.RowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvContact.RowTemplate.Height = 40;
            this.dgvContact.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvContact.Size = new System.Drawing.Size(283, 581);
            this.dgvContact.TabIndex = 12;
            // 
            // GroupFace
            // 
            this.GroupFace.HeaderText = "GroupFace";
            this.GroupFace.Name = "GroupFace";
            this.GroupFace.ReadOnly = true;
            this.GroupFace.Visible = false;
            // 
            // GroupGid
            // 
            this.GroupGid.HeaderText = "GroupGid";
            this.GroupGid.Name = "GroupGid";
            this.GroupGid.ReadOnly = true;
            this.GroupGid.Visible = false;
            // 
            // GroupTitle
            // 
            this.GroupTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.GroupTitle.DefaultCellStyle = dataGridViewCellStyle1;
            this.GroupTitle.HeaderText = "GroupTitle";
            this.GroupTitle.Name = "GroupTitle";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 582);
            this.Controls.Add(this.dgvContact);
            this.Name = "Form2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form2";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvContact)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvContact;
        private System.Windows.Forms.DataGridViewImageColumn GroupFace;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupGid;
        private System.Windows.Forms.DataGridViewTextBoxColumn GroupTitle;
    }
}