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
            treeViewMenuStrip = new ContextMenuStrip(components);
            createGameObjectToolStripMenuItem = new ToolStripMenuItem();
            nodeMenuStrip = new ContextMenuStrip(components);
            exportToFileToolStripMenuItem = new ToolStripMenuItem();
            renameGameObjectToolStripMenuItem = new ToolStripMenuItem();
            createGameObjectNodeStripMenuItem = new ToolStripMenuItem();
            editGameObjectToolStripMenuItem = new ToolStripMenuItem();
            removeHierarchyToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            treeViewMenuStrip.SuspendLayout();
            nodeMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, gHEditorToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(5, 2, 0, 2);
            menuStrip1.Size = new Size(709, 24);
            menuStrip1.TabIndex = 2;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, importToolStripMenuItem, saveToolStripMenuItem, saveAsToolStripMenuItem, exitToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(162, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // importToolStripMenuItem
            // 
            importToolStripMenuItem.Name = "importToolStripMenuItem";
            importToolStripMenuItem.Size = new Size(162, 22);
            importToolStripMenuItem.Text = "Import hierarchy";
            importToolStripMenuItem.Click += importToolStripMenuItem_Click;
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(162, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(162, 22);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // exitToolStripMenuItem
            // 
            exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            exitToolStripMenuItem.Size = new Size(162, 22);
            exitToolStripMenuItem.Text = "Exit";
            exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
            // 
            // gHEditorToolStripMenuItem
            // 
            gHEditorToolStripMenuItem.Name = "gHEditorToolStripMenuItem";
            gHEditorToolStripMenuItem.Size = new Size(70, 20);
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
            gameObjectTreeView.AllowDrop = true;
            gameObjectTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            gameObjectTreeView.CheckBoxes = true;
            gameObjectTreeView.ContextMenuStrip = treeViewMenuStrip;
            gameObjectTreeView.HideSelection = false;
            gameObjectTreeView.LabelEdit = true;
            gameObjectTreeView.Location = new Point(10, 23);
            gameObjectTreeView.Margin = new Padding(3, 2, 3, 2);
            gameObjectTreeView.Name = "gameObjectTreeView";
            gameObjectTreeView.Size = new Size(688, 396);
            gameObjectTreeView.TabIndex = 3;
            // 
            // treeViewMenuStrip
            // 
            treeViewMenuStrip.Items.AddRange(new ToolStripItem[] { createGameObjectToolStripMenuItem });
            treeViewMenuStrip.Name = "treeViewMenuStrip";
            treeViewMenuStrip.Size = new Size(178, 26);
            // 
            // createGameObjectToolStripMenuItem
            // 
            createGameObjectToolStripMenuItem.Name = "createGameObjectToolStripMenuItem";
            createGameObjectToolStripMenuItem.Size = new Size(177, 22);
            createGameObjectToolStripMenuItem.Text = "Create GameObject";
            createGameObjectToolStripMenuItem.Click += createGameObjectToolStripMenuItem_Click;
            // 
            // nodeMenuStrip
            // 
            nodeMenuStrip.ImageScalingSize = new Size(20, 20);
            nodeMenuStrip.Items.AddRange(new ToolStripItem[] { exportToFileToolStripMenuItem, renameGameObjectToolStripMenuItem, editGameObjectToolStripMenuItem, createGameObjectNodeStripMenuItem, removeHierarchyToolStripMenuItem });
            nodeMenuStrip.Name = "contextMenuStrip1";
            nodeMenuStrip.Size = new Size(197, 136);
            // 
            // exportToFileToolStripMenuItem
            // 
            exportToFileToolStripMenuItem.Name = "exportToFileToolStripMenuItem";
            exportToFileToolStripMenuItem.Size = new Size(196, 22);
            exportToFileToolStripMenuItem.Text = "Export hierarchy";
            exportToFileToolStripMenuItem.Click += exportToFileToolStripMenuItem_Click;
            // 
            // renameGameObjectToolStripMenuItem
            // 
            renameGameObjectToolStripMenuItem.Name = "renameGameObjectToolStripMenuItem";
            renameGameObjectToolStripMenuItem.ShortcutKeys = Keys.F2;
            renameGameObjectToolStripMenuItem.Size = new Size(196, 22);
            renameGameObjectToolStripMenuItem.Text = "Rename";
            renameGameObjectToolStripMenuItem.Click += renameGameObjectToolStripMenuItem_Click;
            // 
            // createGameObjectNodeStripMenuItem
            // 
            createGameObjectNodeStripMenuItem.Name = "createGameObjectNodeStripMenuItem";
            createGameObjectNodeStripMenuItem.Size = new Size(196, 22);
            createGameObjectNodeStripMenuItem.Text = "Create GameObject";
            createGameObjectNodeStripMenuItem.Click += createGameObjectNodeStripMenuItem_Click;
            // 
            // editGameObjectToolStripMenuItem
            // 
            editGameObjectToolStripMenuItem.Name = "editGameObjectToolStripMenuItem";
            editGameObjectToolStripMenuItem.Size = new Size(196, 22);
            editGameObjectToolStripMenuItem.Text = "Edit";
            editGameObjectToolStripMenuItem.Click += editGameObjectToolStripMenuItem_Click;
            // 
            // removeHierarchyToolStripMenuItem
            // 
            removeHierarchyToolStripMenuItem.Name = "removeHierarchyToolStripMenuItem";
            removeHierarchyToolStripMenuItem.ShortcutKeyDisplayString = "DEL";
            removeHierarchyToolStripMenuItem.Size = new Size(196, 22);
            removeHierarchyToolStripMenuItem.Text = "Remove hierarchy";
            removeHierarchyToolStripMenuItem.Click += removeHierarchyToolStripMenuItem_Click;
            // 
            // MainWindow
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(709, 428);
            Controls.Add(gameObjectTreeView);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Margin = new Padding(3, 2, 3, 2);
            Name = "MainWindow";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GameObjectHierarchyTool";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            treeViewMenuStrip.ResumeLayout(false);
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
        private ToolStripMenuItem removeHierarchyToolStripMenuItem;
        private ContextMenuStrip treeViewMenuStrip;
        private ToolStripMenuItem createGameObjectToolStripMenuItem;
        private ToolStripMenuItem createGameObjectNodeStripMenuItem;
        private ToolStripMenuItem editGameObjectToolStripMenuItem;
    }
}