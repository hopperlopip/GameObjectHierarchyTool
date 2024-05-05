namespace GameObjectHierarchyTool
{
    partial class GhEditorForm
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
            components = new System.ComponentModel.Container();
            ghTreeView = new TreeView();
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            saveGhDialog = new SaveFileDialog();
            nodeMenuStrip = new ContextMenuStrip(components);
            renameToolStripMenuItem = new ToolStripMenuItem();
            removeHierarchyToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1.SuspendLayout();
            nodeMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ghTreeView
            // 
            ghTreeView.AllowDrop = true;
            ghTreeView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            ghTreeView.CheckBoxes = true;
            ghTreeView.LabelEdit = true;
            ghTreeView.Location = new Point(12, 27);
            ghTreeView.Name = "ghTreeView";
            ghTreeView.Size = new Size(760, 445);
            ghTreeView.TabIndex = 0;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(784, 28);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { saveToolStripMenuItem, saveAsToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(46, 24);
            fileToolStripMenuItem.Text = "File";
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(152, 26);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(152, 26);
            saveAsToolStripMenuItem.Text = "Save As...";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // saveGhDialog
            // 
            saveGhDialog.DefaultExt = "gh";
            saveGhDialog.Filter = "GameObject Hierarchy file|*.gh";
            // 
            // nodeMenuStrip
            // 
            nodeMenuStrip.ImageScalingSize = new Size(20, 20);
            nodeMenuStrip.Items.AddRange(new ToolStripItem[] { renameToolStripMenuItem, removeHierarchyToolStripMenuItem });
            nodeMenuStrip.Name = "contextMenuStrip1";
            nodeMenuStrip.Size = new Size(232, 80);
            // 
            // renameToolStripMenuItem
            // 
            renameToolStripMenuItem.Name = "renameToolStripMenuItem";
            renameToolStripMenuItem.ShortcutKeys = Keys.F2;
            renameToolStripMenuItem.Size = new Size(231, 24);
            renameToolStripMenuItem.Text = "Rename";
            renameToolStripMenuItem.Click += renameToolStripMenuItem_Click;
            // 
            // removeHierarchyToolStripMenuItem
            // 
            removeHierarchyToolStripMenuItem.Name = "removeHierarchyToolStripMenuItem";
            removeHierarchyToolStripMenuItem.ShortcutKeyDisplayString = "DEL";
            removeHierarchyToolStripMenuItem.Size = new Size(231, 24);
            removeHierarchyToolStripMenuItem.Text = "Remove hierarchy";
            removeHierarchyToolStripMenuItem.Click += removeHierarchyToolStripMenuItem_Click;
            // 
            // GhEditorForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(784, 484);
            Controls.Add(ghTreeView);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            Name = "GhEditorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "GH Editor Form";
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            nodeMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TreeView ghTreeView;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem saveAsToolStripMenuItem;
        private SaveFileDialog saveGhDialog;
        private ContextMenuStrip nodeMenuStrip;
        private ToolStripMenuItem removeHierarchyToolStripMenuItem;
        private ToolStripMenuItem renameToolStripMenuItem;
    }
}