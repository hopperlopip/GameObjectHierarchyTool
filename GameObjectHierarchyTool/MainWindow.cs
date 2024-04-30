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
            List<AssetFileInfo> gameObjectInfos = assetsFile.GetAssetsOfType(AssetClassID.GameObject);
            List<long> rootGameObjectsPathIds = GetRootGameObjectPathIds(gameObjectInfos);
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
                gameObjectNode.ToolTipText = $"PathID: {gameObjectPathId}\r\nFatherPathID: {gameObjectHelper.GetFatherPathId(gameObjectPathId)}";
                gameObjectNode.Tag = gameObjectPathId;
                gameObjectNode.Checked = gameObjectHelper.GetActiveState(gameObjectPathId);
                gameObjectNode.ContextMenuStrip = nodeMenuStrip;
                nodeCollection.Add(gameObjectNode);

                List<long> childrenPathIds = gameObjectHelper.GetChildrenPathIds(gameObjectPathId);
                gameObjectNode.Nodes.AddRange(BuildNodeTree(childrenPathIds).ToArray());
            }
            return nodeCollection;
        }

        private List<long> GetRootGameObjectPathIds(List<AssetFileInfo> gameObjectInfos)
        {
            List<long> rootGameObjectsPathIds = new();
            for (int i = 0; i < gameObjectInfos.Count; i++)
            {
                long gameObjectPathId = gameObjectInfos[i].PathId;
                if (gameObjectHelper.GetFatherPathId(gameObjectPathId) == 0)
                {
                    rootGameObjectsPathIds.Add(gameObjectPathId);
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
            long gameObjectPathID = Convert.ToInt64(contextMenuStripNode.Tag);
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
            long gameObjectPathId = Convert.ToInt64(e.Node.Tag);
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
            GameObjectHierarchy gameObjectHierarchy = GameObjectHierarchyFile.Deserialize(gameObjectHierarchyBytes);
            gameObjectHelper.CreateHierarchy(gameObjectHierarchy, 0);
            SetModifiedState(ModifiedState.Modified);
            RebuildTreeView();
        }

        private void gHEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            GhEditorForm ghEditorForm = new GhEditorForm(openGhDialog.FileName);
            ghEditorForm.Show();
        }
    }
}