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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PictureViewer = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Filter_tabs = new System.Windows.Forms.TabControl();
            this.LF_filter_tab = new System.Windows.Forms.TabPage();
            this.LF_filtring_panel = new System.Windows.Forms.Panel();
            this.Stop_StepAuto = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.bttn_Filter = new System.Windows.Forms.Button();
            this.porog_txtbx = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtbx_StepAuto = new System.Windows.Forms.NumericUpDown();
            this.chk_AutoFilter = new System.Windows.Forms.CheckBox();
            this.median_filter_tab = new System.Windows.Forms.TabPage();
            this.median_filter_panel = new System.Windows.Forms.Panel();
            this.weigh_dgv = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.median_filter_bttn = new System.Windows.Forms.Button();
            this.chanse_txtbx = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.bttn_Grey = new System.Windows.Forms.Button();
            this.bttn_Noise = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Headers_dgv = new System.Windows.Forms.DataGridView();
            this.Name_adgv = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Value_dgv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.size_dgv = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label4 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.custom_header_bttn = new System.Windows.Forms.Button();
            this.header_bttn = new System.Windows.Forms.Button();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadSpecificToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FilterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LF_filtr_Menu_item = new System.Windows.Forms.ToolStripMenuItem();
            this.Median_3x3_Menu_Item = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PictureViewer)).BeginInit();
            this.panel1.SuspendLayout();
            this.Filter_tabs.SuspendLayout();
            this.LF_filter_tab.SuspendLayout();
            this.LF_filtring_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Stop_StepAuto)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.porog_txtbx)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbx_StepAuto)).BeginInit();
            this.median_filter_tab.SuspendLayout();
            this.median_filter_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.weigh_dgv)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chanse_txtbx)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Headers_dgv)).BeginInit();
            this.panel4.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 254F));
            this.tableLayoutPanel1.Controls.Add(this.PictureViewer, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 24);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 81F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(876, 447);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // PictureViewer
            // 
            this.PictureViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PictureViewer.Location = new System.Drawing.Point(3, 3);
            this.PictureViewer.Name = "PictureViewer";
            this.PictureViewer.Size = new System.Drawing.Size(616, 360);
            this.PictureViewer.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureViewer.TabIndex = 0;
            this.PictureViewer.TabStop = false;
            this.PictureViewer.WaitOnLoad = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.Filter_tabs);
            this.panel1.Controls.Add(this.chanse_txtbx);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.bttn_Grey);
            this.panel1.Controls.Add(this.bttn_Noise);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 369);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(616, 75);
            this.panel1.TabIndex = 1;
            // 
            // Filter_tabs
            // 
            this.Filter_tabs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Filter_tabs.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            this.Filter_tabs.Controls.Add(this.LF_filter_tab);
            this.Filter_tabs.Controls.Add(this.median_filter_tab);
            this.Filter_tabs.ItemSize = new System.Drawing.Size(1, 1);
            this.Filter_tabs.Location = new System.Drawing.Point(111, 0);
            this.Filter_tabs.Name = "Filter_tabs";
            this.Filter_tabs.SelectedIndex = 0;
            this.Filter_tabs.Size = new System.Drawing.Size(444, 78);
            this.Filter_tabs.TabIndex = 11;
            // 
            // LF_filter_tab
            // 
            this.LF_filter_tab.Controls.Add(this.LF_filtring_panel);
            this.LF_filter_tab.Location = new System.Drawing.Point(4, 5);
            this.LF_filter_tab.Name = "LF_filter_tab";
            this.LF_filter_tab.Padding = new System.Windows.Forms.Padding(3);
            this.LF_filter_tab.Size = new System.Drawing.Size(436, 69);
            this.LF_filter_tab.TabIndex = 0;
            this.LF_filter_tab.Text = "lf";
            this.LF_filter_tab.UseVisualStyleBackColor = true;
            // 
            // LF_filtring_panel
            // 
            this.LF_filtring_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LF_filtring_panel.Controls.Add(this.Stop_StepAuto);
            this.LF_filtring_panel.Controls.Add(this.label5);
            this.LF_filtring_panel.Controls.Add(this.bttn_Filter);
            this.LF_filtring_panel.Controls.Add(this.porog_txtbx);
            this.LF_filtring_panel.Controls.Add(this.label2);
            this.LF_filtring_panel.Controls.Add(this.label3);
            this.LF_filtring_panel.Controls.Add(this.txtbx_StepAuto);
            this.LF_filtring_panel.Controls.Add(this.chk_AutoFilter);
            this.LF_filtring_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LF_filtring_panel.Location = new System.Drawing.Point(3, 3);
            this.LF_filtring_panel.Name = "LF_filtring_panel";
            this.LF_filtring_panel.Size = new System.Drawing.Size(430, 63);
            this.LF_filtring_panel.TabIndex = 10;
            // 
            // Stop_StepAuto
            // 
            this.Stop_StepAuto.Location = new System.Drawing.Point(198, 26);
            this.Stop_StepAuto.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.Stop_StepAuto.Name = "Stop_StepAuto";
            this.Stop_StepAuto.Size = new System.Drawing.Size(49, 20);
            this.Stop_StepAuto.TabIndex = 10;
            this.Stop_StepAuto.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(195, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Стоп автофильтра";
            // 
            // bttn_Filter
            // 
            this.bttn_Filter.Location = new System.Drawing.Point(306, 23);
            this.bttn_Filter.Name = "bttn_Filter";
            this.bttn_Filter.Size = new System.Drawing.Size(75, 23);
            this.bttn_Filter.TabIndex = 2;
            this.bttn_Filter.Text = "Фильтр";
            this.bttn_Filter.UseVisualStyleBackColor = true;
            this.bttn_Filter.Click += new System.EventHandler(this.bttn_Filter_Click);
            // 
            // porog_txtbx
            // 
            this.porog_txtbx.Location = new System.Drawing.Point(6, 28);
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
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Порог фильтра";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(93, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(96, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Шаг автофильтра";
            // 
            // txtbx_StepAuto
            // 
            this.txtbx_StepAuto.Location = new System.Drawing.Point(96, 28);
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
            // chk_AutoFilter
            // 
            this.chk_AutoFilter.AutoSize = true;
            this.chk_AutoFilter.Location = new System.Drawing.Point(306, 2);
            this.chk_AutoFilter.Name = "chk_AutoFilter";
            this.chk_AutoFilter.Size = new System.Drawing.Size(111, 17);
            this.chk_AutoFilter.TabIndex = 6;
            this.chk_AutoFilter.Text = "Вкл. автофильтр";
            this.chk_AutoFilter.UseVisualStyleBackColor = true;
            // 
            // median_filter_tab
            // 
            this.median_filter_tab.Controls.Add(this.median_filter_panel);
            this.median_filter_tab.Location = new System.Drawing.Point(4, 5);
            this.median_filter_tab.Name = "median_filter_tab";
            this.median_filter_tab.Padding = new System.Windows.Forms.Padding(3);
            this.median_filter_tab.Size = new System.Drawing.Size(436, 69);
            this.median_filter_tab.TabIndex = 1;
            this.median_filter_tab.Text = "mf";
            this.median_filter_tab.UseVisualStyleBackColor = true;
            // 
            // median_filter_panel
            // 
            this.median_filter_panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.median_filter_panel.Controls.Add(this.weigh_dgv);
            this.median_filter_panel.Controls.Add(this.median_filter_bttn);
            this.median_filter_panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.median_filter_panel.Location = new System.Drawing.Point(3, 3);
            this.median_filter_panel.Name = "median_filter_panel";
            this.median_filter_panel.Size = new System.Drawing.Size(430, 63);
            this.median_filter_panel.TabIndex = 0;
            // 
            // weigh_dgv
            // 
            this.weigh_dgv.AllowUserToAddRows = false;
            this.weigh_dgv.AllowUserToDeleteRows = false;
            this.weigh_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.weigh_dgv.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.weigh_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.weigh_dgv.ColumnHeadersVisible = false;
            this.weigh_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.weigh_dgv.DefaultCellStyle = dataGridViewCellStyle1;
            this.weigh_dgv.Location = new System.Drawing.Point(-1, -1);
            this.weigh_dgv.Name = "weigh_dgv";
            this.weigh_dgv.RowHeadersVisible = false;
            this.weigh_dgv.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.weigh_dgv.Size = new System.Drawing.Size(136, 66);
            this.weigh_dgv.TabIndex = 10;
            this.weigh_dgv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.weigh_dgv_EditingControlShowing);
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Column1";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Column2";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Column3";
            this.Column3.Name = "Column3";
            // 
            // median_filter_bttn
            // 
            this.median_filter_bttn.Location = new System.Drawing.Point(350, 31);
            this.median_filter_bttn.Name = "median_filter_bttn";
            this.median_filter_bttn.Size = new System.Drawing.Size(75, 23);
            this.median_filter_bttn.TabIndex = 9;
            this.median_filter_bttn.Text = "Фильтр";
            this.median_filter_bttn.UseVisualStyleBackColor = true;
            this.median_filter_bttn.Click += new System.EventHandler(this.median_filter_bttn_Click);
            // 
            // chanse_txtbx
            // 
            this.chanse_txtbx.Location = new System.Drawing.Point(3, 6);
            this.chanse_txtbx.Name = "chanse_txtbx";
            this.chanse_txtbx.Size = new System.Drawing.Size(41, 20);
            this.chanse_txtbx.TabIndex = 9;
            this.chanse_txtbx.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "% of noise";
            // 
            // bttn_Grey
            // 
            this.bttn_Grey.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bttn_Grey.Location = new System.Drawing.Point(557, 16);
            this.bttn_Grey.Name = "bttn_Grey";
            this.bttn_Grey.Size = new System.Drawing.Size(75, 55);
            this.bttn_Grey.TabIndex = 3;
            this.bttn_Grey.Text = "Сделать серым";
            this.bttn_Grey.UseVisualStyleBackColor = true;
            this.bttn_Grey.Click += new System.EventHandler(this.bttn_Grey_Click);
            // 
            // bttn_Noise
            // 
            this.bttn_Noise.Location = new System.Drawing.Point(3, 32);
            this.bttn_Noise.Name = "bttn_Noise";
            this.bttn_Noise.Size = new System.Drawing.Size(75, 23);
            this.bttn_Noise.TabIndex = 1;
            this.bttn_Noise.Text = "Шум";
            this.bttn_Noise.UseVisualStyleBackColor = true;
            this.bttn_Noise.Click += new System.EventHandler(this.bttn_Noise_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(625, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(248, 360);
            this.panel2.TabIndex = 13;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.Controls.Add(this.Headers_dgv);
            this.panel3.Location = new System.Drawing.Point(3, 16);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(242, 341);
            this.panel3.TabIndex = 4;
            // 
            // Headers_dgv
            // 
            this.Headers_dgv.AllowUserToAddRows = false;
            this.Headers_dgv.AllowUserToDeleteRows = false;
            this.Headers_dgv.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.Headers_dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Headers_dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Name_adgv,
            this.Value_dgv,
            this.size_dgv});
            this.Headers_dgv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Headers_dgv.Location = new System.Drawing.Point(0, 0);
            this.Headers_dgv.Name = "Headers_dgv";
            this.Headers_dgv.ReadOnly = true;
            this.Headers_dgv.RowHeadersVisible = false;
            this.Headers_dgv.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.Headers_dgv.Size = new System.Drawing.Size(242, 341);
            this.Headers_dgv.TabIndex = 0;
            this.Headers_dgv.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.Headers_dgv_CellValueChanged);
            this.Headers_dgv.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.Headers_dgv_EditingControlShowing);
            // 
            // Name_adgv
            // 
            this.Name_adgv.DataPropertyName = "Name";
            this.Name_adgv.HeaderText = "Параметр";
            this.Name_adgv.Name = "Name_adgv";
            this.Name_adgv.ReadOnly = true;
            this.Name_adgv.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Name_adgv.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.Name_adgv.Width = 83;
            // 
            // Value_dgv
            // 
            this.Value_dgv.DataPropertyName = "Value";
            this.Value_dgv.HeaderText = "Значение";
            this.Value_dgv.Name = "Value_dgv";
            this.Value_dgv.ReadOnly = true;
            this.Value_dgv.Width = 80;
            // 
            // size_dgv
            // 
            this.size_dgv.DataPropertyName = "Size";
            this.size_dgv.HeaderText = "Размерность";
            this.size_dgv.Name = "size_dgv";
            this.size_dgv.ReadOnly = true;
            this.size_dgv.Visible = false;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(75, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Заголовок";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.custom_header_bttn);
            this.panel4.Controls.Add(this.header_bttn);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(625, 369);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(248, 75);
            this.panel4.TabIndex = 14;
            // 
            // custom_header_bttn
            // 
            this.custom_header_bttn.Location = new System.Drawing.Point(85, 5);
            this.custom_header_bttn.Name = "custom_header_bttn";
            this.custom_header_bttn.Size = new System.Drawing.Size(71, 68);
            this.custom_header_bttn.TabIndex = 14;
            this.custom_header_bttn.Text = "Своя структура заголовка";
            this.custom_header_bttn.UseVisualStyleBackColor = true;
            this.custom_header_bttn.Click += new System.EventHandler(this.custom_header_bttn_Click);
            // 
            // header_bttn
            // 
            this.header_bttn.Location = new System.Drawing.Point(162, 5);
            this.header_bttn.Name = "header_bttn";
            this.header_bttn.Size = new System.Drawing.Size(83, 68);
            this.header_bttn.TabIndex = 13;
            this.header_bttn.Text = "Стандартная структура заголовка";
            this.header_bttn.UseVisualStyleBackColor = true;
            this.header_bttn.Click += new System.EventHandler(this.header_bttn_Click);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "Images |*.bmp;*.png;*.jpg;*.jpeg|All files|*.*";
            this.openFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialog_FileOk);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "Image|*.bmp";
            this.saveFileDialog.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog_FileOk);
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.FilterToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(876, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadToolStripMenuItem,
            this.LoadSpecificToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.FileToolStripMenuItem.Text = "Файл";
            // 
            // LoadToolStripMenuItem
            // 
            this.LoadToolStripMenuItem.Name = "LoadToolStripMenuItem";
            this.LoadToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.LoadToolStripMenuItem.Text = "Загрузить";
            this.LoadToolStripMenuItem.Click += new System.EventHandler(this.bttn_Load_Click);
            // 
            // LoadSpecificToolStripMenuItem
            // 
            this.LoadSpecificToolStripMenuItem.Name = "LoadSpecificToolStripMenuItem";
            this.LoadSpecificToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.LoadSpecificToolStripMenuItem.Text = "Загрузить свой формат";
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.SaveToolStripMenuItem.Text = "Сохранить";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.bttn_save_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(207, 22);
            this.SaveAsToolStripMenuItem.Text = "Сохранить свой формат";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.SaveAsToolStripMenuItem_Click);
            // 
            // FilterToolStripMenuItem
            // 
            this.FilterToolStripMenuItem.CheckOnClick = true;
            this.FilterToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LF_filtr_Menu_item,
            this.Median_3x3_Menu_Item});
            this.FilterToolStripMenuItem.Name = "FilterToolStripMenuItem";
            this.FilterToolStripMenuItem.Size = new System.Drawing.Size(125, 20);
            this.FilterToolStripMenuItem.Text = "Метод фильтрации";
            // 
            // LF_filtr_Menu_item
            // 
            this.LF_filtr_Menu_item.Checked = true;
            this.LF_filtr_Menu_item.CheckState = System.Windows.Forms.CheckState.Checked;
            this.LF_filtr_Menu_item.Name = "LF_filtr_Menu_item";
            this.LF_filtr_Menu_item.Size = new System.Drawing.Size(460, 22);
            this.LF_filtr_Menu_item.Text = "Низкочастотная пространственная фильтрация ";
            this.LF_filtr_Menu_item.Click += new System.EventHandler(this.filtr_Menu_item_Click);
            // 
            // Median_3x3_Menu_Item
            // 
            this.Median_3x3_Menu_Item.Name = "Median_3x3_Menu_Item";
            this.Median_3x3_Menu_Item.Size = new System.Drawing.Size(460, 22);
            this.Median_3x3_Menu_Item.Text = "Взвешенный двумерный медианный фильтр с квадратным окном 3х3";
            this.Median_3x3_Menu_Item.Click += new System.EventHandler(this.filtr_Menu_item_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 471);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MinimumSize = new System.Drawing.Size(890, 39);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Обработка фото";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PictureViewer)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.Filter_tabs.ResumeLayout(false);
            this.LF_filter_tab.ResumeLayout(false);
            this.LF_filtring_panel.ResumeLayout(false);
            this.LF_filtring_panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Stop_StepAuto)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.porog_txtbx)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtbx_StepAuto)).EndInit();
            this.median_filter_tab.ResumeLayout(false);
            this.median_filter_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.weigh_dgv)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chanse_txtbx)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Headers_dgv)).EndInit();
            this.panel4.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox PictureViewer;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button bttn_Grey;
        private System.Windows.Forms.Button bttn_Filter;
        private System.Windows.Forms.Button bttn_Noise;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chk_AutoFilter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown txtbx_StepAuto;
        private System.Windows.Forms.NumericUpDown porog_txtbx;
        private System.Windows.Forms.NumericUpDown chanse_txtbx;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem FileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem SaveAsToolStripMenuItem;
        private System.Windows.Forms.Panel LF_filtring_panel;
        private System.Windows.Forms.ToolStripMenuItem FilterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LF_filtr_Menu_item;
        private System.Windows.Forms.NumericUpDown Stop_StepAuto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem Median_3x3_Menu_Item;
        private System.Windows.Forms.TabControl Filter_tabs;
        private System.Windows.Forms.TabPage LF_filter_tab;
        private System.Windows.Forms.TabPage median_filter_tab;
        private System.Windows.Forms.Panel median_filter_panel;
        private System.Windows.Forms.Button median_filter_bttn;
        private System.Windows.Forms.DataGridView weigh_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView Headers_dgv;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button header_bttn;
        private System.Windows.Forms.Button custom_header_bttn;
        private System.Windows.Forms.ToolStripMenuItem LoadSpecificToolStripMenuItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn Name_adgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn Value_dgv;
        private System.Windows.Forms.DataGridViewTextBoxColumn size_dgv;
    }
}

