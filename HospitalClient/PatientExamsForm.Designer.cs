namespace HospitalClient
{
    partial class PatientExamsForm
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
            this.cmbExam = new System.Windows.Forms.ComboBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.txtExamResult = new System.Windows.Forms.TextBox();
            this.btnAddExam = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.btnRefreshExams = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbExam
            // 
            this.cmbExam.FormattingEnabled = true;
            this.cmbExam.Location = new System.Drawing.Point(12, 168);
            this.cmbExam.Name = "cmbExam";
            this.cmbExam.Size = new System.Drawing.Size(208, 21);
            this.cmbExam.TabIndex = 0;
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(247, 168);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(208, 21);
            this.cmbCategory.TabIndex = 1;
            // 
            // txtExamResult
            // 
            this.txtExamResult.Location = new System.Drawing.Point(12, 276);
            this.txtExamResult.Multiline = true;
            this.txtExamResult.Name = "txtExamResult";
            this.txtExamResult.Size = new System.Drawing.Size(443, 114);
            this.txtExamResult.TabIndex = 2;
            // 
            // btnAddExam
            // 
            this.btnAddExam.Location = new System.Drawing.Point(12, 195);
            this.btnAddExam.Name = "btnAddExam";
            this.btnAddExam.Size = new System.Drawing.Size(132, 23);
            this.btnAddExam.TabIndex = 3;
            this.btnAddExam.Text = "Προσθήκη εξέτασης";
            this.btnAddExam.UseVisualStyleBackColor = true;
            this.btnAddExam.Click += new System.EventHandler(this.btnAddExam_Click);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 12);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.Size = new System.Drawing.Size(776, 150);
            this.dataGridView2.TabIndex = 4;
            // 
            // btnRefreshExams
            // 
            this.btnRefreshExams.Location = new System.Drawing.Point(247, 195);
            this.btnRefreshExams.Name = "btnRefreshExams";
            this.btnRefreshExams.Size = new System.Drawing.Size(180, 23);
            this.btnRefreshExams.TabIndex = 5;
            this.btnRefreshExams.Text = "Ανανέωση λίστας εξετάσεων";
            this.btnRefreshExams.UseVisualStyleBackColor = true;
            this.btnRefreshExams.Click += new System.EventHandler(this.btnRefreshExams_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 260);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(142, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Αποτελεσματα εξετάσεων";
            // 
            // PatientExamsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnRefreshExams);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.btnAddExam);
            this.Controls.Add(this.txtExamResult);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.cmbExam);
            this.Name = "PatientExamsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "PatientExamsForm";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbExam;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.TextBox txtExamResult;
        private System.Windows.Forms.Button btnAddExam;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btnRefreshExams;
        private System.Windows.Forms.Label label1;
    }
}