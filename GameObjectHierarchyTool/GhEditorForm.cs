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
        string ghFileName;
        GameObjectHierarchy rootGameObjectHierarchy;
        TreeNode contextMenuStripNode;

        public GhEditorForm(string ghFileName, GameObjectHierarchy rootGameObjectHierarchy)
        {
            InitializeComponent();
            this.ghFileName = ghFileName;
            this.rootGameObjectHierarchy = rootGameObjectHierarchy;
            RebuildTreeView();
            ghTreeView.AfterCheck += GhTreeView_AfterCheck;
            ghTreeView.MouseUp += GhTreeView_MouseUp;
            ghTreeView.AfterLabelEdit += GhTreeView_AfterLabelEdit;
            ghTreeView.KeyUp += GhTreeView_KeyUp;
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
                case Keys.Delete:
                    RemoveNode(node);
                    break;
                case Keys.F2:
                    RenameNode(node);
                    break;
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
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string ghFileName = saveGhDialog.FileName;
            SaveGhFile(ghFileName, rootGameObjectHierarchy);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
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
            node.Parent.Nodes.Remove(node);
        }

        private void RenameNode(TreeNode node)
        {
            node.BeginEdit();
        }

        private void GhTreeView_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            GameObjectHierarchy gameObjectHierarchy = (GameObjectHierarchy)e.Node.Tag;
            gameObjectHierarchy.gameObject.name = e.Label;
        }

        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameNode(contextMenuStripNode);
        }
    }
}
