namespace GotYouDataBase
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            btnMon = new Button();
            btnExp = new Button();
            btnClr = new Button();
            btnSrch = new Button();
            btnFil = new Button();
            btnSave = new Button();
            dataGridView1 = new DataGridView();
            label1 = new Label();
            label2 = new Label();
            textBox1 = new TextBox();
            groupBox1 = new GroupBox();
            groupBox3 = new GroupBox();
            chkCh = new CheckBox();
            chkRe = new CheckBox();
            chkCre = new CheckBox();
            chkDel = new CheckBox();
            tableLayoutPanel1 = new TableLayoutPanel();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            SuspendLayout();
            // 
            // btnMon
            // 
            btnMon.Location = new Point(59, 85);
            btnMon.Name = "btnMon";
            btnMon.Size = new Size(94, 29);
            btnMon.TabIndex = 0;
            btnMon.Text = "...";
            btnMon.UseVisualStyleBackColor = true;
            btnMon.Click += btnMon_Click;
            // 
            // btnExp
            // 
            btnExp.Location = new Point(368, 85);
            btnExp.Name = "btnExp";
            btnExp.Size = new Size(94, 29);
            btnExp.TabIndex = 1;
            btnExp.Text = "Export log";
            btnExp.UseVisualStyleBackColor = true;
            btnExp.Click += btnExp_Click;
            // 
            // btnClr
            // 
            btnClr.Location = new Point(667, 85);
            btnClr.Name = "btnClr";
            btnClr.Size = new Size(94, 29);
            btnClr.TabIndex = 2;
            btnClr.Text = "Clear";
            btnClr.UseVisualStyleBackColor = true;
            btnClr.Click += btnClr_Click;
            // 
            // btnSrch
            // 
            btnSrch.Location = new Point(967, 85);
            btnSrch.Name = "btnSrch";
            btnSrch.Size = new Size(94, 29);
            btnSrch.TabIndex = 3;
            btnSrch.Text = "Search";
            btnSrch.UseVisualStyleBackColor = true;
            // 
            // btnFil
            // 
            btnFil.Location = new Point(287, 53);
            btnFil.Name = "btnFil";
            btnFil.Size = new Size(94, 29);
            btnFil.TabIndex = 4;
            btnFil.Text = "Filter";
            btnFil.UseVisualStyleBackColor = true;
            btnFil.Click += btnFil_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.None;
            btnSave.Location = new Point(1207, 804);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(94, 29);
            btnSave.TabIndex = 5;
            btnSave.Text = "Save";
            btnSave.UseVisualStyleBackColor = true;
            btnSave.Click += btnSave_Click_1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            tableLayoutPanel1.SetColumnSpan(dataGridView1, 2);
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(3, 163);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersWidth = 51;
            dataGridView1.Size = new Size(1666, 580);
            dataGridView1.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(406, 35);
            label1.Name = "label1";
            label1.Size = new Size(149, 20);
            label1.TabIndex = 7;
            label1.Text = "Currently Monitoring:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 10.2F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.Brown;
            label2.Location = new Point(561, 33);
            label2.Name = "label2";
            label2.Size = new Size(24, 23);
            label2.TabIndex = 8;
            label2.Text = "??";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(924, 35);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(177, 27);
            textBox1.TabIndex = 9;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.None;
            tableLayoutPanel1.SetColumnSpan(groupBox1, 2);
            groupBox1.Controls.Add(btnSrch);
            groupBox1.Controls.Add(textBox1);
            groupBox1.Controls.Add(btnClr);
            groupBox1.Controls.Add(btnMon);
            groupBox1.Controls.Add(btnExp);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label2);
            groupBox1.Location = new Point(266, 17);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(1140, 125);
            groupBox1.TabIndex = 10;
            groupBox1.TabStop = false;
            // 
            // groupBox3
            // 
            groupBox3.Anchor = AnchorStyles.None;
            groupBox3.Controls.Add(chkCh);
            groupBox3.Controls.Add(chkRe);
            groupBox3.Controls.Add(chkCre);
            groupBox3.Controls.Add(chkDel);
            groupBox3.Controls.Add(btnFil);
            groupBox3.Location = new Point(205, 751);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(426, 135);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            // 
            // chkCh
            // 
            chkCh.AutoSize = true;
            chkCh.Location = new Point(162, 82);
            chkCh.Name = "chkCh";
            chkCh.Size = new Size(90, 24);
            chkCh.TabIndex = 8;
            chkCh.Text = "Changed";
            chkCh.UseVisualStyleBackColor = true;
            // 
            // chkRe
            // 
            chkRe.AutoSize = true;
            chkRe.Location = new Point(22, 82);
            chkRe.Name = "chkRe";
            chkRe.Size = new Size(94, 24);
            chkRe.TabIndex = 7;
            chkRe.Text = "Renamed";
            chkRe.UseVisualStyleBackColor = true;
            // 
            // chkCre
            // 
            chkCre.AutoSize = true;
            chkCre.Location = new Point(162, 36);
            chkCre.Name = "chkCre";
            chkCre.Size = new Size(83, 24);
            chkCre.TabIndex = 6;
            chkCre.Text = "Created";
            chkCre.UseVisualStyleBackColor = true;
            // 
            // chkDel
            // 
            chkDel.AutoSize = true;
            chkDel.Location = new Point(22, 37);
            chkDel.Name = "chkDel";
            chkDel.Size = new Size(84, 24);
            chkDel.TabIndex = 5;
            chkDel.Text = "Deleted";
            chkDel.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Controls.Add(btnSave, 1, 2);
            tableLayoutPanel1.Controls.Add(groupBox3, 0, 2);
            tableLayoutPanel1.Controls.Add(dataGridView1, 0, 1);
            tableLayoutPanel1.Controls.Add(groupBox1, 0, 0);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 21.4477215F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 78.55228F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 144F));
            tableLayoutPanel1.Size = new Size(1672, 891);
            tableLayoutPanel1.TabIndex = 12;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1672, 891);
            Controls.Add(tableLayoutPanel1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            tableLayoutPanel1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Button btnMon;
        private Button btnExp;
        private Button btnClr;
        private Button btnSrch;
        private Button btnFil;
        private Button btnSave;
        private DataGridView dataGridView1;
        private Label label1;
        private Label label2;
        private TextBox textBox1;
        private GroupBox groupBox1;
        private GroupBox groupBox3;
        private CheckBox chkCh;
        private CheckBox chkRe;
        private CheckBox chkCre;
        private CheckBox chkDel;
        private TableLayoutPanel tableLayoutPanel1;
    }
}
