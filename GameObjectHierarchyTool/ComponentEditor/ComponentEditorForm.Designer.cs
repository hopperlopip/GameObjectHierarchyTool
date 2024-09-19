namespace GameObjectHierarchyTool.ComponentEditor
{
    partial class ComponentEditorForm
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
            componentListView = new ListView();
            componentColumn = new ColumnHeader();
            typeIdColumn = new ColumnHeader();
            pathIdColumn = new ColumnHeader();
            componentMenuStrip = new ContextMenuStrip(components);
            enableComponentsToolStripMenuItem = new ToolStripMenuItem();
            disableComponentsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            addComponentToolStripMenuItem = new ToolStripMenuItem();
            removeComponentsToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            changeFileIDsToolStripMenuItem = new ToolStripMenuItem();
            componentMenuStrip.SuspendLayout();
            SuspendLayout();
            // 
            // componentListView
            // 
            componentListView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            componentListView.CheckBoxes = true;
            componentListView.Columns.AddRange(new ColumnHeader[] { componentColumn, typeIdColumn, pathIdColumn });
            componentListView.ContextMenuStrip = componentMenuStrip;
            componentListView.FullRowSelect = true;
            componentListView.GridLines = true;
            componentListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            componentListView.Location = new Point(12, 12);
            componentListView.Name = "componentListView";
            componentListView.Size = new Size(461, 351);
            componentListView.TabIndex = 0;
            componentListView.UseCompatibleStateImageBehavior = false;
            componentListView.View = View.Details;
            // 
            // componentColumn
            // 
            componentColumn.Text = "Component";
            componentColumn.Width = 100;
            // 
            // typeIdColumn
            // 
            typeIdColumn.Text = "TypeID";
            typeIdColumn.Width = 100;
            // 
            // pathIdColumn
            // 
            pathIdColumn.Text = "PathID";
            pathIdColumn.Width = 100;
            // 
            // componentMenuStrip
            // 
            componentMenuStrip.Items.AddRange(new ToolStripItem[] { enableComponentsToolStripMenuItem, disableComponentsToolStripMenuItem, toolStripSeparator1, addComponentToolStripMenuItem, removeComponentsToolStripMenuItem, toolStripSeparator2, changeFileIDsToolStripMenuItem });
            componentMenuStrip.Name = "componentMenuStrip";
            componentMenuStrip.Size = new Size(251, 148);
            // 
            // enableComponentsToolStripMenuItem
            // 
            enableComponentsToolStripMenuItem.Name = "enableComponentsToolStripMenuItem";
            enableComponentsToolStripMenuItem.Size = new Size(250, 22);
            enableComponentsToolStripMenuItem.Text = "Enable components";
            enableComponentsToolStripMenuItem.Click += enableComponentsToolStripMenuItem_Click;
            // 
            // disableComponentsToolStripMenuItem
            // 
            disableComponentsToolStripMenuItem.Name = "disableComponentsToolStripMenuItem";
            disableComponentsToolStripMenuItem.Size = new Size(250, 22);
            disableComponentsToolStripMenuItem.Text = "Disable components";
            disableComponentsToolStripMenuItem.Click += disableComponentsToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(247, 6);
            // 
            // addComponentToolStripMenuItem
            // 
            addComponentToolStripMenuItem.Name = "addComponentToolStripMenuItem";
            addComponentToolStripMenuItem.Size = new Size(250, 22);
            addComponentToolStripMenuItem.Text = "Add component";
            addComponentToolStripMenuItem.Click += addComponentToolStripMenuItem_Click;
            // 
            // removeComponentsToolStripMenuItem
            // 
            removeComponentsToolStripMenuItem.Name = "removeComponentsToolStripMenuItem";
            removeComponentsToolStripMenuItem.ShortcutKeyDisplayString = "DEL";
            removeComponentsToolStripMenuItem.Size = new Size(250, 22);
            removeComponentsToolStripMenuItem.Text = "Remove components";
            removeComponentsToolStripMenuItem.Click += removeComponentsToolStripMenuItem_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new Size(247, 6);
            // 
            // changeFileIDsToolStripMenuItem
            // 
            changeFileIDsToolStripMenuItem.Name = "changeFileIDsToolStripMenuItem";
            changeFileIDsToolStripMenuItem.Size = new Size(250, 22);
            changeFileIDsToolStripMenuItem.Text = "Change all FileIDs in components";
            changeFileIDsToolStripMenuItem.Click += changeFileIDsToolStripMenuItem_Click;
            // 
            // ComponentEditorForm
            // 
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(485, 375);
            Controls.Add(componentListView);
            Name = "ComponentEditorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Component Editor";
            componentMenuStrip.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private ListView componentListView;
        private ContextMenuStrip componentMenuStrip;
        private ToolStripMenuItem enableComponentsToolStripMenuItem;
        private ToolStripMenuItem disableComponentsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem removeComponentsToolStripMenuItem;
        private ColumnHeader componentColumn;
        private ColumnHeader pathIdColumn;
        private ToolStripMenuItem addComponentToolStripMenuItem;
        private ColumnHeader typeIdColumn;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem changeFileIDsToolStripMenuItem;
    }
}