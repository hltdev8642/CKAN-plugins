namespace PartManagerPlugin
{
    partial class PartManagerUI
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.InstalledModsListBox = new System.Windows.Forms.ListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.DisableAllButton = new System.Windows.Forms.Button();
            this.EnableAllButton = new System.Windows.Forms.Button();
            this.PartsGridView = new System.Windows.Forms.DataGridView();
            this.EnabledColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.TitleColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PartNameColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PathColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilterTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.StatsLabel = new System.Windows.Forms.Label();
            this.CraftGroupBox = new System.Windows.Forms.GroupBox();
            this.ScanShipsButton = new System.Windows.Forms.Button();
            this.MissingPartsListBox = new System.Windows.Forms.ListBox();
            this.LookupCkanButton = new System.Windows.Forms.Button();
            this.LookupSpacedockButton = new System.Windows.Forms.Button();
            this.LookupGithubButton = new System.Windows.Forms.Button();
            this.LookupKerbalxButton = new System.Windows.Forms.Button();
            this.CraftStatusLabel = new System.Windows.Forms.Label();
            this.RegexCheckbox = new System.Windows.Forms.CheckBox();
            this.ApplyFilterButton = new System.Windows.Forms.Button();
            this.FilterTypeCombobox = new System.Windows.Forms.ComboBox();
            this.ClearFilterbutton = new System.Windows.Forms.Button();
            this.CraftFilesListBox = new System.Windows.Forms.ListBox();
            this.ScanSelectedButton = new System.Windows.Forms.Button();
            this.CraftFileLabel = new System.Windows.Forms.Label();
            this.MissingPartLabel = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PartsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.InstalledModsListBox);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(293, 568);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Installed mods that contain parts";
            // 
            // InstalledModsListBox
            // 
            this.InstalledModsListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.InstalledModsListBox.FormattingEnabled = true;
            this.InstalledModsListBox.Location = new System.Drawing.Point(6, 19);
            this.InstalledModsListBox.Name = "InstalledModsListBox";
            this.InstalledModsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.InstalledModsListBox.Size = new System.Drawing.Size(281, 538);
            this.InstalledModsListBox.TabIndex = 0;
            this.InstalledModsListBox.SelectedIndexChanged += new System.EventHandler(this.InstalledModsListBox_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.StatsLabel);
            this.groupBox2.Controls.Add(this.DisableAllButton);
            this.groupBox2.Controls.Add(this.EnableAllButton);
            this.groupBox2.Controls.Add(this.PartsGridView);
            this.groupBox2.Location = new System.Drawing.Point(302, 46);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(628, 340);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Mod parts";
            // 
            // DisableAllButton
            // 
            this.DisableAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.DisableAllButton.Location = new System.Drawing.Point(87, 308);
            this.DisableAllButton.Name = "DisableAllButton";
            this.DisableAllButton.Size = new System.Drawing.Size(75, 23);
            this.DisableAllButton.TabIndex = 9;
            this.DisableAllButton.Text = "Disable all";
            this.DisableAllButton.UseVisualStyleBackColor = true;
            this.DisableAllButton.Click += new System.EventHandler(this.DisableAllButton_Click);
            // 
            // StatsLabel
            // 
            this.StatsLabel.AutoSize = true;
            this.StatsLabel.Location = new System.Drawing.Point(168, 313);
            this.StatsLabel.Name = "StatsLabel";
            this.StatsLabel.Size = new System.Drawing.Size(75, 13);
            this.StatsLabel.TabIndex = 10;
            this.StatsLabel.Text = "Parts: 0 total, 0 disabled";
            this.StatsLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // EnableAllButton
            // 
            this.EnableAllButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EnableAllButton.Location = new System.Drawing.Point(6, 308);
            this.EnableAllButton.Name = "EnableAllButton";
            this.EnableAllButton.Size = new System.Drawing.Size(75, 23);
            this.EnableAllButton.TabIndex = 8;
            this.EnableAllButton.Text = "Enable all";
            this.EnableAllButton.UseVisualStyleBackColor = true;
            this.EnableAllButton.Click += new System.EventHandler(this.EnableAllButton_Click);
            // 
            // PartsGridView
            // 
            this.PartsGridView.AllowUserToAddRows = false;
            this.PartsGridView.AllowUserToDeleteRows = false;
            this.PartsGridView.AllowUserToResizeRows = false;
            this.PartsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PartsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.PartsGridView.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.PartsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PartsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.EnabledColumn,
            this.TitleColumn,
            this.PartNameColumn,
            this.PathColumn});
            this.PartsGridView.Location = new System.Drawing.Point(6, 19);
            this.PartsGridView.Name = "PartsGridView";
            this.PartsGridView.RowHeadersVisible = false;
            this.PartsGridView.Size = new System.Drawing.Size(616, 280);
            this.PartsGridView.TabIndex = 0;
            this.PartsGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.PartsGridView_CellValueChanged);
            // 
            // EnabledColumn
            // 
            this.EnabledColumn.HeaderText = "Enabled";
            this.EnabledColumn.Name = "EnabledColumn";
            this.EnabledColumn.Width = 52;
            // 
            // TitleColumn
            // 
            this.TitleColumn.HeaderText = "Title";
            this.TitleColumn.Name = "TitleColumn";
            this.TitleColumn.ReadOnly = true;
            this.TitleColumn.Width = 52;
            // 
            // PartNameColumn
            // 
            this.PartNameColumn.HeaderText = "Part name";
            this.PartNameColumn.Name = "PartNameColumn";
            this.PartNameColumn.ReadOnly = true;
            this.PartNameColumn.Width = 80;
            // 
            // PathColumn
            // 
            this.PathColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.PathColumn.HeaderText = "Path";
            this.PathColumn.Name = "PathColumn";
            this.PathColumn.ReadOnly = true;
            // 
            // CraftGroupBox
            // 
            this.CraftGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CraftGroupBox.Controls.Add(this.CraftStatusLabel);
            this.CraftGroupBox.Controls.Add(this.LookupKerbalxButton);
            this.CraftGroupBox.Controls.Add(this.LookupGithubButton);
            this.CraftGroupBox.Controls.Add(this.LookupSpacedockButton);
            this.CraftGroupBox.Controls.Add(this.LookupCkanButton);
            this.CraftGroupBox.Controls.Add(this.MissingPartLabel);
            this.CraftGroupBox.Controls.Add(this.MissingPartsListBox);
            this.CraftGroupBox.Controls.Add(this.CraftFileLabel);
            this.CraftGroupBox.Controls.Add(this.CraftFilesListBox);
            this.CraftGroupBox.Controls.Add(this.ScanSelectedButton);
            this.CraftGroupBox.Controls.Add(this.ScanShipsButton);
            this.CraftGroupBox.Location = new System.Drawing.Point(302, 392);
            this.CraftGroupBox.Name = "CraftGroupBox";
            this.CraftGroupBox.Size = new System.Drawing.Size(628, 175);
            this.CraftGroupBox.TabIndex = 11;
            this.CraftGroupBox.TabStop = false;
            this.CraftGroupBox.Text = "Craft Scanner";
            // 
            // ScanShipsButton
            // 
            this.ScanShipsButton.Location = new System.Drawing.Point(6, 16);
            this.ScanShipsButton.Name = "ScanShipsButton";
            this.ScanShipsButton.Size = new System.Drawing.Size(140, 23);
            this.ScanShipsButton.TabIndex = 0;
            this.ScanShipsButton.Text = "Scan Ships for Missing Parts";
            this.ScanShipsButton.UseVisualStyleBackColor = true;
            this.ScanShipsButton.Click += new System.EventHandler(this.ScanShipsButton_Click);
            // 
            // ScanSelectedButton
            // 
            this.ScanSelectedButton.Location = new System.Drawing.Point(152, 16);
            this.ScanSelectedButton.Name = "ScanSelectedButton";
            this.ScanSelectedButton.Size = new System.Drawing.Size(100, 23);
            this.ScanSelectedButton.TabIndex = 7;
            this.ScanSelectedButton.Text = "Scan Selected";
            this.ScanSelectedButton.UseVisualStyleBackColor = true;
            this.ScanSelectedButton.Click += new System.EventHandler(this.ScanSelectedButton_Click);
            // 
            // CraftStatusLabel
            // 
            this.CraftStatusLabel.AutoSize = true;
            this.CraftStatusLabel.Location = new System.Drawing.Point(258, 21);
            this.CraftStatusLabel.Name = "CraftStatusLabel";
            this.CraftStatusLabel.Size = new System.Drawing.Size(94, 13);
            this.CraftStatusLabel.TabIndex = 6;
            this.CraftStatusLabel.Text = "Ready (no scan yet)";
            this.CraftStatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CraftFileLabel
            // 
            this.CraftFileLabel.AutoSize = false;
            this.CraftFileLabel.Location = new System.Drawing.Point(6, 42);
            this.CraftFileLabel.Name = "CraftFileLabel";
            this.CraftFileLabel.Size = new System.Drawing.Size(300, 13);
            this.CraftFileLabel.TabIndex = 12;
            this.CraftFileLabel.Text = "Craft Files (select, then click Scan Selected):";
            this.CraftFileLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CraftFilesListBox
            // 
            this.CraftFilesListBox.FormattingEnabled = true;
            this.CraftFilesListBox.Location = new System.Drawing.Point(6, 56);
            this.CraftFilesListBox.Name = "CraftFilesListBox";
            this.CraftFilesListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.CraftFilesListBox.Size = new System.Drawing.Size(300, 82);
            this.CraftFilesListBox.TabIndex = 1;
            this.CraftFilesListBox.SelectedIndexChanged += new System.EventHandler(this.CraftFilesListBox_SelectedIndexChanged);
            // 
            // MissingPartLabel
            // 
            this.MissingPartLabel.AutoSize = false;
            this.MissingPartLabel.Location = new System.Drawing.Point(312, 42);
            this.MissingPartLabel.Name = "MissingPartLabel";
            this.MissingPartLabel.Size = new System.Drawing.Size(152, 13);
            this.MissingPartLabel.TabIndex = 13;
            this.MissingPartLabel.Text = "Missing Parts:";
            this.MissingPartLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MissingPartsListBox
            // 
            this.MissingPartsListBox.FormattingEnabled = true;
            this.MissingPartsListBox.Location = new System.Drawing.Point(312, 56);
            this.MissingPartsListBox.Name = "MissingPartsListBox";
            this.MissingPartsListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.MissingPartsListBox.Size = new System.Drawing.Size(152, 82);
            this.MissingPartsListBox.TabIndex = 8;
            // 
            // LookupCkanButton
            // 
            this.LookupCkanButton.Location = new System.Drawing.Point(470, 45);
            this.LookupCkanButton.Name = "LookupCkanButton";
            this.LookupCkanButton.Size = new System.Drawing.Size(75, 23);
            this.LookupCkanButton.TabIndex = 2;
            this.LookupCkanButton.Text = "CKAN";
            this.LookupCkanButton.UseVisualStyleBackColor = true;
            this.LookupCkanButton.Click += new System.EventHandler(this.LookupCkanButton_Click);
            // 
            // LookupSpacedockButton
            // 
            this.LookupSpacedockButton.Location = new System.Drawing.Point(551, 45);
            this.LookupSpacedockButton.Name = "LookupSpacedockButton";
            this.LookupSpacedockButton.Size = new System.Drawing.Size(75, 23);
            this.LookupSpacedockButton.TabIndex = 3;
            this.LookupSpacedockButton.Text = "Spacedock";
            this.LookupSpacedockButton.UseVisualStyleBackColor = true;
            this.LookupSpacedockButton.Click += new System.EventHandler(this.LookupSpacedockButton_Click);
            // 
            // LookupGithubButton
            // 
            this.LookupGithubButton.Location = new System.Drawing.Point(470, 74);
            this.LookupGithubButton.Name = "LookupGithubButton";
            this.LookupGithubButton.Size = new System.Drawing.Size(75, 23);
            this.LookupGithubButton.TabIndex = 4;
            this.LookupGithubButton.Text = "GitHub";
            this.LookupGithubButton.UseVisualStyleBackColor = true;
            this.LookupGithubButton.Click += new System.EventHandler(this.LookupGithubButton_Click);
            // 
            // LookupKerbalxButton
            // 
            this.LookupKerbalxButton.Location = new System.Drawing.Point(551, 74);
            this.LookupKerbalxButton.Name = "LookupKerbalxButton";
            this.LookupKerbalxButton.Size = new System.Drawing.Size(75, 23);
            this.LookupKerbalxButton.TabIndex = 5;
            this.LookupKerbalxButton.Text = "KerbalX";
            this.LookupKerbalxButton.UseVisualStyleBackColor = true;
            this.LookupKerbalxButton.Click += new System.EventHandler(this.LookupKerbalxButton_Click);
            // 
            // FilterTextBox
            // 
            this.FilterTextBox.Location = new System.Drawing.Point(467, 14);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.Size = new System.Drawing.Size(220, 20);
            this.FilterTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(305, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Filter:";
            // 
            // RegexCheckbox
            // 
            this.RegexCheckbox.AutoSize = true;
            this.RegexCheckbox.Location = new System.Drawing.Point(693, 16);
            this.RegexCheckbox.Name = "RegexCheckbox";
            this.RegexCheckbox.Size = new System.Drawing.Size(57, 17);
            this.RegexCheckbox.TabIndex = 4;
            this.RegexCheckbox.Text = "Regex";
            this.RegexCheckbox.UseVisualStyleBackColor = true;
            // 
            // ApplyFilterButton
            // 
            this.ApplyFilterButton.Location = new System.Drawing.Point(756, 11);
            this.ApplyFilterButton.Name = "ApplyFilterButton";
            this.ApplyFilterButton.Size = new System.Drawing.Size(75, 23);
            this.ApplyFilterButton.TabIndex = 5;
            this.ApplyFilterButton.Text = "Apply filter";
            this.ApplyFilterButton.UseVisualStyleBackColor = true;
            this.ApplyFilterButton.Click += new System.EventHandler(this.ApplyFilterButton_Click);
            // 
            // FilterTypeCombobox
            // 
            this.FilterTypeCombobox.FormattingEnabled = true;
            this.FilterTypeCombobox.Items.AddRange(new object[] {
            "Title",
            "Name",
            "Path"});
            this.FilterTypeCombobox.Location = new System.Drawing.Point(340, 14);
            this.FilterTypeCombobox.Name = "FilterTypeCombobox";
            this.FilterTypeCombobox.Size = new System.Drawing.Size(121, 21);
            this.FilterTypeCombobox.TabIndex = 6;
            this.FilterTypeCombobox.Text = "Path";
            // 
            // ClearFilterbutton
            // 
            this.ClearFilterbutton.Enabled = false;
            this.ClearFilterbutton.Location = new System.Drawing.Point(837, 11);
            this.ClearFilterbutton.Name = "ClearFilterbutton";
            this.ClearFilterbutton.Size = new System.Drawing.Size(75, 23);
            this.ClearFilterbutton.TabIndex = 7;
            this.ClearFilterbutton.Text = "Clear filter";
            this.ClearFilterbutton.UseVisualStyleBackColor = true;
            this.ClearFilterbutton.Click += new System.EventHandler(this.ClearFilterbutton_Click);
            // 
            // PartManagerUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ClearFilterbutton);
            this.Controls.Add(this.FilterTypeCombobox);
            this.Controls.Add(this.ApplyFilterButton);
            this.Controls.Add(this.RegexCheckbox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.FilterTextBox);
            this.Controls.Add(this.CraftGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "PartManagerUI";
            this.Size = new System.Drawing.Size(933, 575);
            this.Load += new System.EventHandler(this.PartManagerUI_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.PartsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ListBox InstalledModsListBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView PartsGridView;
        private System.Windows.Forms.TextBox FilterTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox RegexCheckbox;
        private System.Windows.Forms.Button ApplyFilterButton;
        private System.Windows.Forms.ComboBox FilterTypeCombobox;
        private System.Windows.Forms.Button ClearFilterbutton;
        private System.Windows.Forms.Label StatsLabel;
        private System.Windows.Forms.GroupBox CraftGroupBox;
        private System.Windows.Forms.Button ScanShipsButton;
        private System.Windows.Forms.ListBox MissingPartsListBox;
        private System.Windows.Forms.Button LookupCkanButton;
        private System.Windows.Forms.Button LookupSpacedockButton;
        private System.Windows.Forms.Button LookupGithubButton;
        private System.Windows.Forms.Button LookupKerbalxButton;
        private System.Windows.Forms.Label CraftStatusLabel;
        private System.Windows.Forms.ListBox CraftFilesListBox;
        private System.Windows.Forms.Button ScanSelectedButton;
        private System.Windows.Forms.Label CraftFileLabel;
        private System.Windows.Forms.Label MissingPartLabel;
        private System.Windows.Forms.DataGridViewCheckBoxColumn EnabledColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn TitleColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PartNameColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PathColumn;
        private System.Windows.Forms.Button DisableAllButton;
        private System.Windows.Forms.Button EnableAllButton;
    }
}
