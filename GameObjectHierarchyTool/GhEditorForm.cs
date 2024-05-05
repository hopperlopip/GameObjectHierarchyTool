using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameObjectHierarchyTool
{
    public partial class GhEditorForm : Form
    {
        bool modified = false;
        string initFormTitle;

        string ghFileName;
        GameObjectHierarchy rootGameObjectHierarchy;
        TreeNode contextMenuStripNode;

        public GhEditorForm(string ghFileName, GameObjectHierarchy rootGameObjectHierarchy)
        {
            InitializeComponent();
            initFormTitle = Text;
            this.ghFileName = ghFileName;
            this.rootGameObjectHierarchy = rootGameObjectHierarchy;
            SetModifiedState(ModifiedState.None);
            RebuildTreeView();
            ghTreeView.AfterCheck += GhTreeView_AfterCheck;
            ghTreeView.MouseUp += GhTreeView_MouseUp;
            ghTreeView.AfterLabelEdit += GhTreeView_AfterLabelEdit;
            ghTreeView.KeyUp += GhTreeView_KeyUp;

            ghTreeView.ItemDrag += GhTreeView_ItemDrag;
            ghTreeView.DragEnter += GhTreeView_DragEnter;
            ghTreeView.DragOver += GhTreeView_DragOver;
            ghTreeView.DragDrop += GhTreeView_DragDrop;
        }

        private void GhTreeView_KeyUp(object? sender, KeyEventArgs e)
        {
            TreeNode node = ghTreeView.SelectedNode;
            if (node == null)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.F2:
                    RenameNode(node);
                    break;
                case Keys.Delete:
                    RemoveNode(node);
                    break;
            }
        }

        enum ModifiedState
        {
            None,
            Modified,
            Saved,
        }

        private void SetModifiedState(ModifiedState state)
        {
            if (state is ModifiedState.Modified)
            {
                modified = true;
                Text = initFormTitle + $" - {Path.GetFileName(ghFileName)} - Modified";
            }
            else if (state is ModifiedState.None or ModifiedState.Saved)
            {
                modified = false;
                Text = initFormTitle + $" - {Path.GetFileName(ghFileName)}";
            }
        }

        private void GhTreeView_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            var hitTest = ghTreeView.HitTest(e.Location);
            if (hitTest.Node != null)
            {
                contextMenuStripNode = hitTest.Node;
            }
        }

        private void RebuildTreeView()
        {
            ghTreeView.Nodes.Clear();
            GameObjectHierarchy gameObjectHierarchy = rootGameObjectHierarchy;
            List<GameObjectHierarchy> gameObjectHierarchies = new List<GameObjectHierarchy>() { gameObjectHierarchy };
            ghTreeView.Nodes.AddRange(BuildNodeTree(gameObjectHierarchies).ToArray());
        }

        private List<TreeNode> BuildNodeTree(List<GameObjectHierarchy> gameObjectHierarchies)
        {
            List<TreeNode> nodeCollection = new();
            for (int i = 0; i < gameObjectHierarchies.Count; i++)
            {
                GameObjectHierarchy gameObjectHierarchy = gameObjectHierarchies[i];
                GameObject gameObject = gameObjectHierarchy.gameObject;
                TreeNode node = new TreeNode(gameObject.name);
                node.Tag = gameObjectHierarchy;
                node.Checked = gameObject.active;
                node.ContextMenuStrip = nodeMenuStrip;
                nodeCollection.Add(node);

                node.Nodes.AddRange(BuildNodeTree(gameObjectHierarchy.children).ToArray());
            }
            return nodeCollection;
        }

        private void GhTreeView_AfterCheck(object? sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            GameObjectHierarchy gameObjectHierarchy = (GameObjectHierarchy)e.Node.Tag;
            gameObjectHierarchy.gameObject.active = e.Node.Checked;
            SetModifiedState(ModifiedState.Modified);
        }

        private void SaveGhFile(string ghFileName, GameObjectHierarchy gameObjectHierarchy)
        {
            try
            {
                File.WriteAllBytes(ghFileName, GameObjectHierarchyFile.Serialize(gameObjectHierarchy));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveGhFile(ghFileName, rootGameObjectHierarchy);
            SetModifiedState(ModifiedState.Saved);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string ghFileName = saveGhDialog.FileName;
            SaveGhFile(ghFileName, rootGameObjectHierarchy);
            SetModifiedState(ModifiedState.Saved);
        }

        private void removeHierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveNode(contextMenuStripNode);
        }

        private void RemoveNode(TreeNode node)
        {
            GameObjectHierarchy gameObjectHierarchy = (GameObjectHierarchy)node.Tag;
            if (gameObjectHierarchy.father == null)
            {
                return;
            }
            gameObjectHierarchy.father.children.Remove(gameObjectHierarchy);
            node.Remove();
            SetModifiedState(ModifiedState.Modified);
        }

        private void RenameNode(TreeNode node)
        {
            node.BeginEdit();
        }

        private void GhTreeView_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            if (e.Label != null)
            {
                GameObjectHierarchy gameObjectHierarchy = (GameObjectHierarchy)e.Node.Tag;
                gameObjectHierarchy.gameObject.name = e.Label;
                SetModifiedState(ModifiedState.Modified);
            }
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameNode(contextMenuStripNode);
        }

        private void GhTreeView_ItemDrag(object? sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void GhTreeView_DragEnter(object? sender, DragEventArgs e)
        {
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (draggedNode == null)
            {
                return;
            }
            draggedNode.ForeColor = Color.Gray;
            e.Effect = e.AllowedEffect;
        }

        private void GhTreeView_DragOver(object? sender, DragEventArgs e)
        {
            ghTreeView.Scroll();
            Point targetPoint = ghTreeView.PointToClient(new Point(e.X, e.Y));
            ghTreeView.SelectedNode = ghTreeView.GetNodeAt(targetPoint);
        }

        private void GhTreeView_DragDrop(object? sender, DragEventArgs e)
        {
            Point targetPoint = ghTreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = ghTreeView.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (targetNode != null && !draggedNode.Equals(targetNode) && !MainWindow.ContainsDraggedNode(draggedNode, targetNode))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    GameObjectHierarchy gameObjectHierarchy = (GameObjectHierarchy)draggedNode.Tag;
                    GameObjectHierarchy newFatherHierarchy = (GameObjectHierarchy)targetNode.Tag;
                    GameObjectHierarchy? oldFatherHierarchy = gameObjectHierarchy.father;
                    if (oldFatherHierarchy != null)
                    {
                        oldFatherHierarchy.children.Remove(gameObjectHierarchy);
                    }
                    newFatherHierarchy.children.Add(gameObjectHierarchy);
                    gameObjectHierarchy.father = newFatherHierarchy;

                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                    SetModifiedState(ModifiedState.Modified);
                }
                targetNode.Expand();
            }

            draggedNode.ForeColor = Color.Empty;
            ghTreeView.SelectedNode = draggedNode;
        }
    }
}
