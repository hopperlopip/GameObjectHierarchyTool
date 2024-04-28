using AssetsTools.NET.Extra;
using AssetsTools.NET;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace GameObjectHierarchyTransfer
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
            Text = string.Format(Text, Application.ProductVersion);
            initFormTitle = Text;
            FormClosing += MainWindow_FormClosing;
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
            gameObjectGridView.Rows.Clear();
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
            UpdateAssetsFileList();
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

        private void UpdateAssetsFileList()
        {
            if (assetsFile != null)
            {
                int savedSelectedRowIndex = 0;
                int savedScroll = gameObjectGridView.VerticalScrollingOffset;
                if (gameObjectGridView.SelectedRows.Count != 0)
                {
                    savedSelectedRowIndex = gameObjectGridView.SelectedRows[0].Index;
                }
                gameObjectGridView.Rows.Clear();
                List<AssetFileInfo> gameObjectInfos = assetsFile.GetAssetsOfType(AssetClassID.GameObject);
                for (int i = 0; i < gameObjectInfos.Count; i++)
                {
                    AssetFileInfo gameObjectInfo = gameObjectInfos[i];
                    AssetTypeValueField gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
                    gameObjectGridView.Rows.Add();
                    gameObjectGridView.Rows[i].Cells[0].Value = gameObjectBase["m_Name"].AsString;
                    gameObjectGridView.Rows[i].Cells[1].Value = gameObjectInfo.PathId;
                }
                if (gameObjectGridView.Rows.Count != 0)
                {
                    gameObjectGridView.Rows[savedSelectedRowIndex].Selected = true;
                    PropertyInfo? verticalOffset = gameObjectGridView.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (verticalOffset != null && savedScroll != 0)
                    {
                        verticalOffset.SetValue(this.gameObjectGridView, savedScroll, null);
                    }
                }
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

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (gameObjectGridView.Rows.Count == 0)
            {
                return;
            }
            long gameObjectPathID = Convert.ToInt64(gameObjectGridView.SelectedRows[0].Cells[1].Value);
            string? nameOfGameObject = gameObjectGridView.SelectedRows[0].Cells[0].Value.ToString();
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

        private void importButton_Click(object sender, EventArgs e)
        {
            if (gameObjectGridView.Rows.Count == 0)
            {
                return;
            }
            if (openGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            byte[] gameObjectHierarchyBytes = File.ReadAllBytes(openGhDialog.FileName);
            GameObjectHierarchy gameObjectHierarchy = GameObjectHierarchyFile.Deserialize(gameObjectHierarchyBytes);

            if (MessageBox.Show("Do you want to put your hierarchy into selected GameObject?", QUESTION_TITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                long gameObjectPathID = Convert.ToInt64(gameObjectGridView.SelectedRows[0].Cells[1].Value);

                gameObjectHelper.CreateHierarchy(gameObjectHierarchy, gameObjectPathID);
            }
            else
            {
                gameObjectHelper.CreateHierarchy(gameObjectHierarchy, 0);
            }

            UpdateAssetsFileList();
            SetModifiedState(ModifiedState.Modified);
        }
    }
}