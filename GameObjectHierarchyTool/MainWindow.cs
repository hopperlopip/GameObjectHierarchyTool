using AssetsTools.NET.Extra;
using AssetsTools.NET;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GameObjectHierarchyTool
{
    public partial class MainWindow : Form
    {
        bool modified = false;
        string initFormTitle;

        const string ERROR_TITLE = "Error";
        const string WARNING_TITLE = "Warning";
        const string QUESTION_TITLE = "Question";

        AssetsManager manager = new();
        AssetsFileInstance fileInstance;
        AssetsFile assetsFile;
        string assetsPath;
        GameObjectHelper gameObjectHelper;
        TreeNode contextMenuStripNode;

        public MainWindow()
        {
            InitializeComponent();
            initFormTitle = Text;
            FormClosing += MainWindow_FormClosing;
            gameObjectTreeView.AfterCheck += GameObjectTreeView_AfterCheck;
            gameObjectTreeView.MouseUp += GameObjectTreeView_MouseUp;
            gameObjectTreeView.AfterLabelEdit += GameObjectTreeView_AfterLabelEdit;
            gameObjectTreeView.KeyUp += GameObjectTreeView_KeyUp;

            gameObjectTreeView.ItemDrag += GameObjectTreeView_ItemDrag;
            gameObjectTreeView.DragEnter += GameObjectTreeView_DragEnter;
            gameObjectTreeView.DragOver += GameObjectTreeView_DragOver;
            gameObjectTreeView.DragDrop += GameObjectTreeView_DragDrop;
        }

        private void GameObjectTreeView_KeyUp(object? sender, KeyEventArgs e)
        {
            TreeNode node = gameObjectTreeView.SelectedNode;
            if (node == null)
            {
                return;
            }
            switch (e.KeyCode)
            {
                case Keys.F2:
                    RenameNode(node);
                    break;
            }
        }

        private void GameObjectTreeView_MouseUp(object? sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
            {
                return;
            }
            var hitTest = gameObjectTreeView.HitTest(e.Location);
            if (hitTest.Node != null)
            {
                contextMenuStripNode = hitTest.Node;
            }
        }

        private void MainWindow_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (modified == true)
            {
                switch (MessageBox.Show("Would you like to save changes before exit?", QUESTION_TITLE, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                {
                    case DialogResult.Yes:
                        SaveAssetsFile(assetsFile, assetsPath);
                        break;
                    case DialogResult.No:
                        break;
                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        private void LoadAssetsFile(string assetsPath)
        {
            manager.LoadClassPackage("classdata.tpk");
            try
            {
                fileInstance = manager.LoadAssetsFile(assetsPath, loadDeps: true);
            }
            catch
            {
                MessageBox.Show("Your assets file is corrupted", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            assetsFile = fileInstance.file;
            gameObjectHelper = new GameObjectHelper(manager, fileInstance);
            manager.LoadClassDatabaseFromPackage(assetsFile.Metadata.UnityVersion);
            RebuildTreeView();
        }

        private void RebuildTreeView()
        {
            gameObjectTreeView.Nodes.Clear();
            List<long> rootGameObjectsPathIds = GetRootGameObjectPathIds(assetsFile.AssetInfos);
            gameObjectTreeView.Nodes.AddRange(BuildNodeTree(rootGameObjectsPathIds).ToArray());
        }

        private List<TreeNode> BuildNodeTree(List<long> gameObjectPathIds)
        {
            List<TreeNode> nodeCollection = new();
            for (int i = 0; i < gameObjectPathIds.Count; i++)
            {
                long gameObjectPathId = gameObjectPathIds[i];
                string gameObjectName = gameObjectHelper.GetGameObjectName(gameObjectPathId);
                TreeNode gameObjectNode = new TreeNode(gameObjectName);
                gameObjectNode.Tag = gameObjectPathId;
                gameObjectNode.Checked = gameObjectHelper.GetActiveState(gameObjectPathId);
                gameObjectNode.ContextMenuStrip = nodeMenuStrip;
                nodeCollection.Add(gameObjectNode);

                List<long> childrenPathIds = gameObjectHelper.GetChildrenPathIds(gameObjectPathId);
                gameObjectNode.Nodes.AddRange(BuildNodeTree(childrenPathIds).ToArray());
            }
            return nodeCollection;
        }

        private List<long> GetRootGameObjectPathIds(IList<AssetFileInfo> assetInfos)
        {
            List<long> rootGameObjectsPathIds = new();
            foreach (AssetFileInfo assetFileInfo in assetInfos)
            {
                if (assetFileInfo.TypeId == (int)AssetClassID.Transform || assetFileInfo.TypeId == (int)AssetClassID.RectTransform)
                {
                    var transformBase = manager.GetBaseField(fileInstance, assetFileInfo);
                    long fatherGameObjectPathId = transformBase["m_Father.m_PathID"].AsLong;
                    if (fatherGameObjectPathId == 0)
                    {
                        rootGameObjectsPathIds.Add(gameObjectHelper.GetGameObjectPathId(transformBase));
                    }
                }
            }
            return rootGameObjectsPathIds;
        }

        private void SaveAssetsFile(AssetsFile assetsFile, string filePath)
        {
            string tmpAssetsFile = $"{filePath}.tmp";
            using (AssetsFileWriter writer = new AssetsFileWriter(tmpAssetsFile))
            {
                assetsFile.Write(writer);
            }
            assetsFile.Close();
            File.Move(tmpAssetsFile, filePath, true);
            assetsFile.Read(new AssetsFileReader(filePath));
        }

        enum ModifiedState
        {
            Modified,
            Saved,
        }

        private void SetModifiedState(ModifiedState state)
        {
            if (state == ModifiedState.Modified)
            {
                modified = true;
                Text = initFormTitle + $" - {Path.GetFileName(assetsPath)} - Modified";
            }
            else if (state == ModifiedState.Saved)
            {
                modified = false;
                Text = initFormTitle + $" - {Path.GetFileName(assetsPath)}";
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openAssetsDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            assetsPath = openAssetsDialog.FileName;
            if (assetsFile != null)
            {
                manager.UnloadAll();
            }
            LoadAssetsFile(assetsPath);
            SetModifiedState(ModifiedState.Saved);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (assetsFile == null)
            {
                MessageBox.Show("Assets file is not loaded.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SaveAssetsFile(assetsFile, assetsPath);
            SetModifiedState(ModifiedState.Saved);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (assetsFile == null)
            {
                MessageBox.Show("Assets file is not loaded.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (saveAssetsDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            SaveAssetsFile(assetsFile, saveAssetsDialog.FileName);
            assetsPath = saveAssetsDialog.FileName;
            SetModifiedState(ModifiedState.Saved);
        }

        private void exportToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long gameObjectPathID = (long)contextMenuStripNode.Tag;
            string? nameOfGameObject = gameObjectHelper.GetGameObjectName(gameObjectPathID);
            if (nameOfGameObject == null)
            {
                throw new Exception("Field of the GameObject name is null");
            }

            GameObject gameObject = gameObjectHelper.GetGameObject(gameObjectPathID);
            GameObjectHierarchy gameObjectHierarchy = gameObjectHelper.GetHierarchy(gameObject);

            saveGhDialog.FileName = nameOfGameObject;
            if (saveGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            File.WriteAllBytes(saveGhDialog.FileName, GameObjectHierarchyFile.Serialize(gameObjectHierarchy));
        }

        private void GameObjectTreeView_AfterCheck(object? sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                return;
            }
            long gameObjectPathId = (long)e.Node.Tag;
            gameObjectHelper.ChangeActiveState(gameObjectPathId, e.Node.Checked);
            SetModifiedState(ModifiedState.Modified);
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (assetsFile == null)
            {
                return;
            }
            if (openGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            byte[] gameObjectHierarchyBytes = File.ReadAllBytes(openGhDialog.FileName);
            GameObjectHierarchy gameObjectHierarchy;
            try
            {
                gameObjectHierarchy = GameObjectHierarchyFile.Deserialize(gameObjectHierarchyBytes);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            long gameObjectPathId = gameObjectHelper.CreateHierarchy(gameObjectHierarchy, 0);
            List<long> gameObjectPathIds = new List<long> { gameObjectPathId };
            gameObjectTreeView.Nodes.AddRange(BuildNodeTree(gameObjectPathIds).ToArray());
            SetModifiedState(ModifiedState.Modified);
        }

        private void gHEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string ghFileName = openGhDialog.FileName;
            GameObjectHierarchy gameObjectHierarchy;
            try
            {
                gameObjectHierarchy = GameObjectHierarchyFile.Deserialize(File.ReadAllBytes(ghFileName));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            GhEditorForm ghEditorForm = new GhEditorForm(ghFileName, gameObjectHierarchy);
            ghEditorForm.Show();
        }

        private void GameObjectTreeView_AfterLabelEdit(object? sender, NodeLabelEditEventArgs e)
        {
            long gameObjectPathId = (long)e.Node.Tag;
            if (e.Label != null)
            {
                gameObjectHelper.RenameGameObject(gameObjectPathId, e.Label);
                SetModifiedState(ModifiedState.Modified);
            }
        }

        private void RenameNode(TreeNode node)
        {
            node.BeginEdit();
        }

        private void renameGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RenameNode(contextMenuStripNode);
        }

        private void GameObjectTreeView_ItemDrag(object? sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DoDragDrop(e.Item, DragDropEffects.Move);
            }
        }

        private void GameObjectTreeView_DragEnter(object? sender, DragEventArgs e)
        {
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            draggedNode.ForeColor = Color.Gray;
            e.Effect = e.AllowedEffect;
        }

        private void GameObjectTreeView_DragOver(object? sender, DragEventArgs e)
        {
            Point targetPoint = gameObjectTreeView.PointToClient(new Point(e.X, e.Y));
            gameObjectTreeView.SelectedNode = gameObjectTreeView.GetNodeAt(targetPoint);
        }

        private void GameObjectTreeView_DragDrop(object? sender, DragEventArgs e)
        {
            Point targetPoint = gameObjectTreeView.PointToClient(new Point(e.X, e.Y));
            TreeNode targetNode = gameObjectTreeView.GetNodeAt(targetPoint);
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (targetNode == null)
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    long gameObjectPathId = (long)draggedNode.Tag;
                    gameObjectHelper.ChangeGameObjectFather(gameObjectPathId, 0);

                    draggedNode.Remove();
                    gameObjectTreeView.Nodes.Add(draggedNode);
                }
            }
            else if (!draggedNode.Equals(targetNode) && !ContainsDraggedNode(draggedNode, targetNode))
            {
                if (e.Effect == DragDropEffects.Move)
                {
                    long gameObjectPathId = (long)draggedNode.Tag;
                    long newFatherPathId = (long)targetNode.Tag;
                    gameObjectHelper.ChangeGameObjectFather(gameObjectPathId, newFatherPathId);

                    draggedNode.Remove();
                    targetNode.Nodes.Add(draggedNode);
                }

                targetNode.Expand();
            }

            draggedNode.ForeColor = Color.Empty;
            gameObjectTreeView.SelectedNode = draggedNode;
            SetModifiedState(ModifiedState.Modified);
        }

        private bool ContainsDraggedNode(TreeNode draggedNode, TreeNode targedNode)
        {
            TreeNode parent = targedNode.Parent;
            while (parent != null)
            {
                if (draggedNode.Equals(parent))
                {
                    return true;
                }
                parent = parent.Parent;
            }
            return false;
        }
    }
}