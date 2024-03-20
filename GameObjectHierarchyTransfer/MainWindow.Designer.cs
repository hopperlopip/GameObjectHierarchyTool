namespace GameObjectHierarchyTransfer
{
    partial class MainWindow
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
            gameObjectGridView = new DataGridView();
            NameColumn = new DataGridViewTextBoxColumn();
            PathIDColumn = new DataGridViewTextBoxColumn();
            splitContainer1 = new SplitContainer();
            exportButton = new Button();
            importButton = new Button();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)gameObjectGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            menuStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // gameObjectGridView
            // 
            gameObjectGridView.AllowUserToAddRows = false;
            gameObjectGridView.AllowUserToDeleteRows = false;
            gameObjectGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gameObjectGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gameObjectGridView.BackgroundColor = SystemColors.Control;
            gameObjectGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            gameObjectGridView.Columns.AddRange(new DataGridViewColumn[] { NameColumn, PathIDColumn });
            gameObjectGridView.Location = new Point(12, 27);
            gameObjectGridView.MultiSelect = false;
            gameObjectGridView.Name = "gameObjectGridView";
            gameObjectGridView.ReadOnly = true;
            gameObjectGridView.RowHeadersVisible = false;
            gameObjectGridView.RowHeadersWidth = 51;
            gameObjectGridView.RowTemplate.Height = 29;
            gameObjectGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            gameObjectGridView.Size = new Size(752, 399);
            gameObjectGridView.TabIndex = 0;
            // 
            // NameColumn
            // 
            NameColumn.HeaderText = "Name";
            NameColumn.MinimumWidth = 6;
            NameColumn.Name = "NameColumn";
            NameColumn.ReadOnly = true;
            // 
            // PathIDColumn
            // 
            PathIDColumn.HeaderText = "PathID";
            PathIDColumn.MinimumWidth = 6;
            PathIDColumn.Name = "PathIDColumn";
            PathIDColumn.ReadOnly = true;
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.IsSplitterFixed = true;
            splitContainer1.Location = new Point(12, 432);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(exportButton);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(importButton);
            splitContainer1.Size = new Size(752, 56);
            splitContainer1.SplitterDistance = 376;
            splitContainer1.TabIndex = 1;
            // 
            // exportButton
            // 
            exportButton.Dock = DockStyle.Fill;
            exportButton.Location = new Point(0, 0);
            exportButton.Name = "exportButton";
            exportButton.Size = new Size(376, 56);
            exportButton.TabIndex = 0;
            exportButton.Text = "Export Hierarchy";
            exportButton.UseVisualStyleBackColor = true;
            // 
            // importButton
            // 
            importButton.Dock = DockStyle.Fill;
            importButton.Location = new Point(0, 0);
            importButton.Name = "importButton";
            importButton.Size = new Size(372, 56);
            importButton.TabIndex = 0;
            importButton.Text = "Import Hierarchy";
            importButton.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(776, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(224, 26);
            openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(224, 26);
            saveToolStripMenuItem.Text = "Save";
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(224, 26);
            saveAsToolStripMenuItem.Text = "Save As...";
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(224, 26);
            exitToolStripMenuItem.Text = "Exit";
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(776, 500);
            Controls.Add(splitContainer1);
            Controls.Add(gameObjectGridView);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            ShowIcon = false;
            Text = "GameObjectHierarchyTransfer";
            ((System.ComponentModel.ISupportInitialize)gameObjectGridView).EndInit();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private DataGridView gameObjectGridView;
        private DataGridViewTextBoxColumn NameColumn;
        private DataGridViewTextBoxColumn PathIDColumn;
        private SplitContainer splitContainer1;
        private Button exportButton;
        private Button importButton;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
    }
}