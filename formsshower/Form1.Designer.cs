﻿namespace formsshower
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PictureViewer = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chanse_txtbx = new System.Windows.Forms.NumericUpDown();
            this.txtbx_StepAuto = new System.Windows.Forms.NumericUpDown();
            this.porog_txtbx = new System.Windows.Forms.NumericUpDown();
            this.chk_AutoFilter = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.bttn_Grey = new System.Windows.Forms.Button();
            this.bttn_Filter = new System.Windows.Forms.Button();
            this.bttn_Noise = new System.Windows.Forms.Button();
            this.bttn_Load = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.bttn_save = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureViewer)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chanse_txtbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbx_StepAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.porog_txtbx)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.PictureViewer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 45F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(874, 469);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PictureViewer
            // 
            this.PictureViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureViewer.Location = new System.Drawing.Point(3, 3);
            this.PictureViewer.Name = "PictureViewer";
            this.PictureViewer.Size = new System.Drawing.Size(868, 418);
            this.PictureViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureViewer.TabIndex = 0;
            this.PictureViewer.TabStop = false;
            this.PictureViewer.WaitOnLoad = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chanse_txtbx);
            this.panel1.Controls.Add(this.txtbx_StepAuto);
            this.panel1.Controls.Add(this.porog_txtbx);
            this.panel1.Controls.Add(this.chk_AutoFilter);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bttn_Grey);
            this.panel1.Controls.Add(this.bttn_Filter);
            this.panel1.Controls.Add(this.bttn_Noise);
            this.panel1.Controls.Add(this.bttn_save);
            this.panel1.Controls.Add(this.bttn_Load);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 427);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(868, 39);
            this.panel1.TabIndex = 1;
            // 
            // chanse_txtbx
            // 
            this.chanse_txtbx.Location = new System.Drawing.Point(171, 17);
            this.chanse_txtbx.Name = "chanse_txtbx";
            this.chanse_txtbx.Size = new System.Drawing.Size(41, 20);
            this.chanse_txtbx.TabIndex = 9;
            this.chanse_txtbx.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // txtbx_StepAuto
            // 
            this.txtbx_StepAuto.Location = new System.Drawing.Point(692, 17);
            this.txtbx_StepAuto.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.txtbx_StepAuto.Name = "txtbx_StepAuto";
            this.txtbx_StepAuto.Size = new System.Drawing.Size(41, 20);
            this.txtbx_StepAuto.TabIndex = 8;
            this.txtbx_StepAuto.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // porog_txtbx
            // 
            this.porog_txtbx.Location = new System.Drawing.Point(563, 16);
            this.porog_txtbx.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.porog_txtbx.Name = "porog_txtbx";
            this.porog_txtbx.Size = new System.Drawing.Size(49, 20);
            this.porog_txtbx.TabIndex = 7;
            this.porog_txtbx.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // chk_AutoFilter
            // 
            this.chk_AutoFilter.AutoSize = true;
            this.chk_AutoFilter.Location = new System.Drawing.Point(616, 16);
            this.chk_AutoFilter.Name = "chk_AutoFilter";
            this.chk_AutoFilter.Size = new System.Drawing.Size(70, 17);
            this.chk_AutoFilter.TabIndex = 6;
            this.chk_AutoFilter.Text = "AutoFilter";
            this.chk_AutoFilter.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(678, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "AutoFilter Step";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(560, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Filter gain";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(168, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "% of noise";
            // 
            // bttn_Grey
            // 
            this.bttn_Grey.Location = new System.Drawing.Point(296, 16);
            this.bttn_Grey.Name = "bttn_Grey";
            this.bttn_Grey.Size = new System.Drawing.Size(75, 23);
            this.bttn_Grey.TabIndex = 3;
            this.bttn_Grey.Text = "Grey";
            this.bttn_Grey.UseVisualStyleBackColor = true;
            this.bttn_Grey.Click += new System.EventHandler(this.bttn_Grey_Click);
            // 
            // bttn_Filter
            // 
            this.bttn_Filter.Location = new System.Drawing.Point(482, 14);
            this.bttn_Filter.Name = "bttn_Filter";
            this.bttn_Filter.Size = new System.Drawing.Size(75, 23);
            this.bttn_Filter.TabIndex = 2;
            this.bttn_Filter.Text = "Filter";
            this.bttn_Filter.UseVisualStyleBackColor = true;
            this.bttn_Filter.Click += new System.EventHandler(this.bttn_Filter_Click);
            // 
            // bttn_Noise
            // 
            this.bttn_Noise.Location = new System.Drawing.Point(88, 16);
            this.bttn_Noise.Name = "bttn_Noise";
            this.bttn_Noise.Size = new System.Drawing.Size(75, 23);
            this.bttn_Noise.TabIndex = 1;
            this.bttn_Noise.Text = "Noise";
            this.bttn_Noise.UseVisualStyleBackColor = true;
            this.bttn_Noise.Click += new System.EventHandler(this.bttn_Noise_Click);
            // 
            // bttn_Load
            // 
            this.bttn_Load.Location = new System.Drawing.Point(0, 16);
            this.bttn_Load.Name = "bttn_Load";
            this.bttn_Load.Size = new System.Drawing.Size(75, 23);
            this.bttn_Load.TabIndex = 0;
            this.bttn_Load.Text = "Load";
            this.bttn_Load.UseVisualStyleBackColor = true;
            this.bttn_Load.Click += new System.EventHandler(this.bttn_Load_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Images |*.bmp;*.png;*.jpg;*.jpeg|All files|*.*";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // bttn_save
            // 
            this.bttn_save.Location = new System.Drawing.Point(770, 14);
            this.bttn_save.Name = "bttn_save";
            this.bttn_save.Size = new System.Drawing.Size(75, 23);
            this.bttn_save.TabIndex = 0;
            this.bttn_save.Text = "Save";
            this.bttn_save.UseVisualStyleBackColor = true;
            this.bttn_save.Click += new System.EventHandler(this.bttn_save_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Image|*.bmp";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 469);
            this.Controls.Add(this.tableLayoutPanel1);
            this.MinimumSize = new System.Drawing.Size(890, 0);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Баловство всё это";
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureViewer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chanse_txtbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbx_StepAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.porog_txtbx)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox PictureViewer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bttn_Grey;
        private System.Windows.Forms.Button bttn_Filter;
        private System.Windows.Forms.Button bttn_Noise;
        private System.Windows.Forms.Button bttn_Load;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_AutoFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txtbx_StepAuto;
        private System.Windows.Forms.NumericUpDown porog_txtbx;
        private System.Windows.Forms.NumericUpDown chanse_txtbx;
        private System.Windows.Forms.Button bttn_save;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

