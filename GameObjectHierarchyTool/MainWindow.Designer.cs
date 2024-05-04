namespace GameObjectHierarchyTool
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
            components = new System.ComponentModel.Container();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            importToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            exitToolStripMenuItem = new ToolStripMenuItem();
            gHEditorToolStripMenuItem = new ToolStripMenuItem();
            openAssetsDialog = new OpenFileDialog();
            saveAssetsDialog = new SaveFileDialog();
            saveGhDialog = new SaveFileDialog();
            openGhDialog = new OpenFileDialog();
            gameObjectTreeView = new TreeView();
            nodeMenuStrip = new ContextMenuStrip(components);
            exportToFileToolStripMenuItem = new ToolStripMenuItem();
            renameGameObjectToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            nodeMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gHEditorToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(867, 28);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, importToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(201, 26);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(201, 26);
            importToolStripMenuItem.Text = "Import hierarchy";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(201, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(201, 26);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(201, 26);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // gHEditorToolStripMenuItem
            // 
            gHEditorToolStripMenuItem.Name = "gHEditorToolStripMenuItem";
            gHEditorToolStripMenuItem.Size = new Size(88, 24);
            gHEditorToolStripMenuItem.Text = "GH Editor";
            gHEditorToolStripMenuItem.Click += gHEditorToolStripMenuItem_Click;
            // 
            // openAssetsDialog
            // 
            openAssetsDialog.Filter = "Level file|level*|Assets file|*.assets";
            // 
            // saveAssetsDialog
            // 
            saveAssetsDialog.Filter = "Level file|level*|Assets file|*.assets";
            // 
            // saveGhDialog
            // 
            saveGhDialog.DefaultExt = "gh";
            saveGhDialog.Filter = "GameObject Hierarchy file|*.gh";
            // 
            // openGhDialog
            // 
            openGhDialog.DefaultExt = "gh";
            openGhDialog.Filter = "GameObject Hierarchy file|*.gh";
            // 
            // gameObjectTreeView
            // 
            gameObjectTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gameObjectTreeView.CheckBoxes = true;
            gameObjectTreeView.HideSelection = false;
            gameObjectTreeView.LabelEdit = true;
            gameObjectTreeView.Location = new Point(12, 31);
            gameObjectTreeView.Name = "gameObjectTreeView";
            gameObjectTreeView.Size = new Size(843, 498);
            gameObjectTreeView.TabIndex = 3;
            // 
            // nodeMenuStrip
            // 
            nodeMenuStrip.ImageScalingSize = new Size(20, 20);
            nodeMenuStrip.Items.AddRange(new ToolStripItem[] { exportToFileToolStripMenuItem, renameGameObjectToolStripMenuItem });
            nodeMenuStrip.Name = "contextMenuStrip1";
            nodeMenuStrip.Size = new Size(244, 52);
            // 
            // exportToFileToolStripMenuItem
            // 
            exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
            exportToFileToolStripMenuItem.Size = new Size(243, 24);
            exportToFileToolStripMenuItem.Text = "Export hierarchy";
            exportToFileToolStripMenuItem.Click += exportToFileToolStripMenuItem_Click;
            // 
            // renameGameObjectToolStripMenuItem
            // 
            renameGameObjectToolStripMenuItem.Name = "renameGameObjectToolStripMenuItem";
            renameGameObjectToolStripMenuItem.ShortcutKeys = Keys.F2;
            renameGameObjectToolStripMenuItem.Size = new Size(243, 24);
            renameGameObjectToolStripMenuItem.Text = "Rename GameObject";
            renameGameObjectToolStripMenuItem.Click += renameGameObjectToolStripMenuItem_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(867, 541);
            Controls.Add(gameObjectTreeView);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "MainWindow";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameObjectHierarchyTool";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            nodeMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private OpenFileDialog openAssetsDialog;
        private SaveFileDialog saveAssetsDialog;
        private SaveFileDialog saveGhDialog;
        private OpenFileDialog openGhDialog;
        private TreeView gameObjectTreeView;
        private ContextMenuStrip nodeMenuStrip;
        private ToolStripMenuItem exportToFileToolStripMenuItem;
        private ToolStripMenuItem importToolStripMenuItem;
        private ToolStripMenuItem gHEditorToolStripMenuItem;
        private ToolStripMenuItem renameGameObjectToolStripMenuItem;
    }
}