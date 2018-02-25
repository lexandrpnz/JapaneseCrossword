namespace JapaneseСrossword
{
    partial class CreatorForm
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
            this.lblWidth = new System.Windows.Forms.Label();
            this.lblHeigth = new System.Windows.Forms.Label();
            this.numWidth = new System.Windows.Forms.NumericUpDown();
            this.numHeigth = new System.Windows.Forms.NumericUpDown();
            this.grbSize = new System.Windows.Forms.GroupBox();
            this.grbMax = new System.Windows.Forms.GroupBox();
            this.numHorizontal = new System.Windows.Forms.NumericUpDown();
            this.numVertical = new System.Windows.Forms.NumericUpDown();
            this.lblHorizontal = new System.Windows.Forms.Label();
            this.lblVertical = new System.Windows.Forms.Label();
            this.dataHorizontal = new System.Windows.Forms.DataGridView();
            this.dataVertical = new System.Windows.Forms.DataGridView();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreate = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOpen = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeigth)).BeginInit();
            this.grbSize.SuspendLayout();
            this.grbMax.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHorizontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVertical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataHorizontal)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataVertical)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWidth
            // 
            this.lblWidth.AutoSize = true;
            this.lblWidth.Location = new System.Drawing.Point(16, 21);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(52, 13);
            this.lblWidth.TabIndex = 0;
            this.lblWidth.Text = "Ширина :";
            // 
            // lblHeigth
            // 
            this.lblHeigth.AutoSize = true;
            this.lblHeigth.Location = new System.Drawing.Point(16, 47);
            this.lblHeigth.Name = "lblHeigth";
            this.lblHeigth.Size = new System.Drawing.Size(51, 13);
            this.lblHeigth.TabIndex = 1;
            this.lblHeigth.Text = "Высота :";
            // 
            // numWidth
            // 
            this.numWidth.Location = new System.Drawing.Point(74, 19);
            this.numWidth.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numWidth.Name = "numWidth";
            this.numWidth.Size = new System.Drawing.Size(61, 20);
            this.numWidth.TabIndex = 2;
            this.numWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numHeigth
            // 
            this.numHeigth.Location = new System.Drawing.Point(74, 45);
            this.numHeigth.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numHeigth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHeigth.Name = "numHeigth";
            this.numHeigth.Size = new System.Drawing.Size(61, 20);
            this.numHeigth.TabIndex = 3;
            this.numHeigth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // grbSize
            // 
            this.grbSize.Controls.Add(this.numWidth);
            this.grbSize.Controls.Add(this.numHeigth);
            this.grbSize.Controls.Add(this.lblWidth);
            this.grbSize.Controls.Add(this.lblHeigth);
            this.grbSize.Location = new System.Drawing.Point(12, 12);
            this.grbSize.Name = "grbSize";
            this.grbSize.Size = new System.Drawing.Size(150, 78);
            this.grbSize.TabIndex = 4;
            this.grbSize.TabStop = false;
            this.grbSize.Text = "Размер";
            // 
            // grbMax
            // 
            this.grbMax.Controls.Add(this.numHorizontal);
            this.grbMax.Controls.Add(this.numVertical);
            this.grbMax.Controls.Add(this.lblHorizontal);
            this.grbMax.Controls.Add(this.lblVertical);
            this.grbMax.Location = new System.Drawing.Point(168, 12);
            this.grbMax.Name = "grbMax";
            this.grbMax.Size = new System.Drawing.Size(200, 78);
            this.grbMax.TabIndex = 5;
            this.grbMax.TabStop = false;
            this.grbMax.Text = "Максимальное количество чисел:";
            // 
            // numHorizontal
            // 
            this.numHorizontal.Location = new System.Drawing.Point(104, 19);
            this.numHorizontal.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numHorizontal.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numHorizontal.Name = "numHorizontal";
            this.numHorizontal.Size = new System.Drawing.Size(80, 20);
            this.numHorizontal.TabIndex = 4;
            this.numHorizontal.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numVertical
            // 
            this.numVertical.Location = new System.Drawing.Point(104, 45);
            this.numVertical.Maximum = new decimal(new int[] {
            254,
            0,
            0,
            0});
            this.numVertical.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVertical.Name = "numVertical";
            this.numVertical.Size = new System.Drawing.Size(80, 20);
            this.numVertical.TabIndex = 5;
            this.numVertical.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // lblHorizontal
            // 
            this.lblHorizontal.AutoSize = true;
            this.lblHorizontal.Location = new System.Drawing.Point(20, 21);
            this.lblHorizontal.Name = "lblHorizontal";
            this.lblHorizontal.Size = new System.Drawing.Size(78, 13);
            this.lblHorizontal.TabIndex = 2;
            this.lblHorizontal.Text = "Горизонталь :";
            // 
            // lblVertical
            // 
            this.lblVertical.AutoSize = true;
            this.lblVertical.Location = new System.Drawing.Point(31, 45);
            this.lblVertical.Name = "lblVertical";
            this.lblVertical.Size = new System.Drawing.Size(67, 13);
            this.lblVertical.TabIndex = 1;
            this.lblVertical.Text = "Вертикаль :";
            // 
            // dataHorizontal
            // 
            this.dataHorizontal.AllowUserToAddRows = false;
            this.dataHorizontal.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataHorizontal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataHorizontal.Location = new System.Drawing.Point(3, 19);
            this.dataHorizontal.Name = "dataHorizontal";
            this.dataHorizontal.Size = new System.Drawing.Size(224, 208);
            this.dataHorizontal.TabIndex = 6;
            // 
            // dataVertical
            // 
            this.dataVertical.AllowUserToAddRows = false;
            this.dataVertical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataVertical.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataVertical.Location = new System.Drawing.Point(3, 19);
            this.dataVertical.Name = "dataVertical";
            this.dataVertical.Size = new System.Drawing.Size(224, 208);
            this.dataVertical.TabIndex = 7;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(12, 96);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.dataHorizontal);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.dataVertical);
            this.splitContainer1.Size = new System.Drawing.Size(464, 230);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Горизонталь";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Вертикаль";
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(378, 12);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(98, 23);
            this.btnCreate.TabIndex = 9;
            this.btnCreate.Text = "Сформировать";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnForm_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(378, 41);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(98, 23);
            this.btnSave.TabIndex = 10;
            this.btnSave.Text = "Сохранить";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.Location = new System.Drawing.Point(378, 70);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(98, 23);
            this.btnOpen.TabIndex = 11;
            this.btnOpen.Text = "Открыть";
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // CreatorForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(488, 338);
            this.Controls.Add(this.btnCreate);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.btnOpen);
            this.Controls.Add(this.grbMax);
            this.Controls.Add(this.grbSize);
            this.Name = "CreatorForm";
            this.Text = "Новый сканворд";
            ((System.ComponentModel.ISupportInitialize)(this.numWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numHeigth)).EndInit();
            this.grbSize.ResumeLayout(false);
            this.grbSize.PerformLayout();
            this.grbMax.ResumeLayout(false);
            this.grbMax.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numHorizontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVertical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataHorizontal)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataVertical)).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblWidth;
        private System.Windows.Forms.Label lblHeigth;
        private System.Windows.Forms.NumericUpDown numWidth;
        private System.Windows.Forms.NumericUpDown numHeigth;
        private System.Windows.Forms.GroupBox grbSize;
        private System.Windows.Forms.GroupBox grbMax;
        private System.Windows.Forms.NumericUpDown numHorizontal;
        private System.Windows.Forms.NumericUpDown numVertical;
        private System.Windows.Forms.Label lblHorizontal;
        private System.Windows.Forms.Label lblVertical;
        private System.Windows.Forms.DataGridView dataHorizontal;
        private System.Windows.Forms.DataGridView dataVertical;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}