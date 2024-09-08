using AssetsTools.NET;
using AssetsTools.NET.Extra;

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
                case Keys.Delete:
                    RemoveNode(node);
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
                gameObjectTreeView.SelectedNode = hitTest.Node;
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
            manager.UseQuickLookup = true;
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
            None,
            Modified,
            Saved,
        }

        private void SetModifiedState(ModifiedState state)
        {
            if (state is ModifiedState.Modified)
            {
                modified = true;
                Text = initFormTitle + $" - {Path.GetFileName(assetsPath)} - Modified";
            }
            else if (state is ModifiedState.None or ModifiedState.Saved)
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
            SetModifiedState(ModifiedState.None);
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsAssetsFileLoaded())
                return;
            SaveAssetsFile(assetsFile, assetsPath);
            SetModifiedState(ModifiedState.Saved);
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsAssetsFileLoaded())
                return;
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
            var node = gameObjectTreeView.SelectedNode;
            long gameObjectPathID = (long)node.Tag;
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
            if (!IsAssetsFileLoaded())
                return;
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

        private bool IsAssetsFileLoaded()
        {
            if (assetsFile == null)
            {
                MessageBox.Show("You should open \".assets\" or level file first.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
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
            if (e.Label != null)
            {
                long gameObjectPathId = (long)e.Node.Tag;
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
            RenameNode(gameObjectTreeView.SelectedNode);
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
            if (draggedNode == null)
            {
                return;
            }
            draggedNode.ForeColor = Color.Gray;
            e.Effect = e.AllowedEffect;
        }

        private void GameObjectTreeView_DragOver(object? sender, DragEventArgs e)
        {
            gameObjectTreeView.Scroll();
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
                    SetModifiedState(ModifiedState.Modified);
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
                    SetModifiedState(ModifiedState.Modified);
                }
                targetNode.Expand();
            }

            draggedNode.ForeColor = Color.Empty;
            gameObjectTreeView.SelectedNode = draggedNode;
        }

        public static bool ContainsDraggedNode(TreeNode draggedNode, TreeNode targedNode)
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

        private void removeHierarchyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveNode(gameObjectTreeView.SelectedNode);
        }

        private void RemoveNode(TreeNode node)
        {
            long gameObjectPathId = (long)node.Tag;
            gameObjectHelper.RemoveHierarchy(gameObjectPathId);
            node.Remove();
            SetModifiedState(ModifiedState.Modified);
        }

        private void CreateEmptyGameObject(long gameObjectPathId, long fatherPathId)
        {
            //Creating GameObject
            AssetFileInfo gameObjectInfo = gameObjectHelper.CreateGameObject(gameObjectPathId);
            AssetTypeValueField gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
            gameObjectBase["m_Name"].AsString = "GameObject";
            gameObjectInfo.SetNewData(gameObjectBase);
            var gameObjectEditorForm = new GameObjectEditorForm(gameObjectBase, gameObjectInfo.PathId);
            gameObjectEditorForm.ShowDialog();
            if (gameObjectEditorForm.applied)
            {
                gameObjectInfo.SetNewData(gameObjectEditorForm.GameObjectBase);
            }

            //Creating Transform Component of the GameObject
            long transformPathId = assetsFile.AssetInfos.Count + 1;
            AssetFileInfo transformInfo = gameObjectHelper.CreateTransform(transformPathId);
            AssetTypeValueField transformBase = manager.GetBaseField(fileInstance, transformInfo);
            transformBase["m_LocalRotation.w"].AsFloat = 1f;
            transformBase["m_GameObject.m_PathID"].AsLong = gameObjectPathId;
            transformInfo.SetNewData(transformBase);

            //Editing GameObject because of Transform
            gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
            var components = gameObjectBase["m_Component.Array"];
            components.Children.Clear();
            var newArrayItem = ValueBuilder.DefaultValueFieldFromArrayTemplate(components);
            newArrayItem["component.m_FileID"].AsInt = 0;
            newArrayItem["component.m_PathID"].AsLong = transformPathId;
            components.Children.Add(newArrayItem);
            gameObjectInfo.SetNewData(gameObjectBase);

            //Changing Father Path ID
            gameObjectHelper.ChangeGameObjectFather(gameObjectPathId, fatherPathId);
        }

        private void AddNode(TreeNode? parentNode, long gameObjectPathId, string gameObjectName, bool gameObjectActiveState)
        {
            TreeNode node = new TreeNode(gameObjectName);
            node.Tag = gameObjectPathId;
            node.Checked = gameObjectHelper.GetActiveState(gameObjectPathId);
            node.ContextMenuStrip = nodeMenuStrip;
            if (parentNode != null)
            {
                parentNode.Nodes.Add(node);
            }
            else
            {
                gameObjectTreeView.Nodes.Add(node);
            }
        }

        private void createGameObjectNodeStripMenuItem_Click(object sender, EventArgs e)
        {
            long gameObjectPathId = assetsFile.AssetInfos.Count + 1;
            TreeNode parentNode = gameObjectTreeView.SelectedNode;
            long fatherPathId = (long)parentNode.Tag;
            CreateEmptyGameObject(gameObjectPathId, fatherPathId);
            string gameObjectName = gameObjectHelper.GetGameObjectName(gameObjectPathId);
            bool gameObjectActiveState = gameObjectHelper.GetActiveState(gameObjectPathId);
            AddNode(parentNode, gameObjectPathId, gameObjectName, gameObjectActiveState);
            SetModifiedState(ModifiedState.Modified);
        }

        private void createGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!IsAssetsFileLoaded())
                return;
            long gameObjectPathId = assetsFile.AssetInfos.Count + 1;
            CreateEmptyGameObject(gameObjectPathId, 0);
            string gameObjectName = gameObjectHelper.GetGameObjectName(gameObjectPathId);
            bool gameObjectActiveState = gameObjectHelper.GetActiveState(gameObjectPathId);
            AddNode(null, gameObjectPathId, gameObjectName, gameObjectActiveState);
            SetModifiedState(ModifiedState.Modified);
        }

        private void editGameObjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = gameObjectTreeView.SelectedNode;
            long gameObjectPathId = (long)node.Tag;
            AssetFileInfo gameObjectInfo = assetsFile.GetAssetInfo(gameObjectPathId);
            AssetTypeValueField gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
            GameObjectEditorForm gameObjectEditorForm = new GameObjectEditorForm(gameObjectBase, gameObjectPathId);
            gameObjectEditorForm.ShowDialog();
            if (gameObjectEditorForm.applied)
            {
                gameObjectInfo.SetNewData(gameObjectEditorForm.GameObjectBase);
                node.Text = gameObjectHelper.GetGameObjectName(gameObjectPathId);
                node.Checked = gameObjectHelper.GetActiveState(gameObjectPathId);
                SetModifiedState(ModifiedState.Modified);
            }
        }
    }
}