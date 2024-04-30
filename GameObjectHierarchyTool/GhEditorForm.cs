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

        public GhEditorForm(string ghFileName)
        {
            InitializeComponent();
            this.ghFileName = ghFileName;
            RebuildTreeView();
            ghTreeView.AfterCheck += GhTreeView_AfterCheck;
        }

        private GameObjectHierarchy GetGameObjectHierarchy(string ghFileName)
        {
            return GameObjectHierarchyFile.Deserialize(File.ReadAllBytes(ghFileName));
        }

        private void RebuildTreeView()
        {
            ghTreeView.Nodes.Clear();
            GameObjectHierarchy gameObjectHierarchy = GetGameObjectHierarchy(ghFileName);
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
    }
}
