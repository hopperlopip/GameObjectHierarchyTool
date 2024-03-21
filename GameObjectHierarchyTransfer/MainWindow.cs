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
        const string QUESTION_TITLE = "Question";

        AssetsManager manager = new AssetsManager();
        AssetsFileInstance fileInstance;
        AssetsFile assetsFile;
        string assetsPath;

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
            manager.LoadClassDatabaseFromPackage(assetsFile.Metadata.UnityVersion);
            UpdateAssetsFileList();
        }

        private void SaveAssetsFile(AssetsFile assetsFile, string filePath)
        {
            MemoryStream memoryStream = new MemoryStream();
            using AssetsFileWriter writer = new AssetsFileWriter(memoryStream);
            assetsFile.Write(writer, 0L);
            assetsFile.Close();
            using (FileStream fs = File.OpenWrite(filePath))
            {
                memoryStream.WriteTo(fs);
            }
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
                    PropertyInfo verticalOffset = gameObjectGridView.GetType().GetProperty("VerticalOffset", BindingFlags.NonPublic | BindingFlags.Instance);
                    verticalOffset.SetValue(this.gameObjectGridView, savedScroll, null);
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
            SetModifiedState(ModifiedState.Saved);
        }

        private void exportButton_Click(object sender, EventArgs e)
        {
            if (gameObjectGridView.Rows.Count == 0)
            {
                return;
            }
            long gameObjectPathID = Convert.ToInt64(gameObjectGridView.SelectedRows[0].Cells[1].Value);
            AssetTypeValueField gameObjectBase = manager.GetBaseField(fileInstance, gameObjectPathID);
            AssetFileInfo gameObjectInfo = assetsFile.GetAssetInfo(gameObjectPathID);
            GameObject_Hierarchy_File GhFile = new GameObject_Hierarchy_File();
            GhFile.parentGameObject = gameObjectBase.WriteToByteArray();
            var components = gameObjectBase["m_Component.Array"];
            for (int i = 0; i < components.Children.Count; i++)
            {
                var data = components.Children[i];
                var componentPointer = data["component"];
                var componentExtInfo = manager.GetExtAsset(fileInstance, componentPointer);
                var componentType = componentExtInfo.info.TypeId;
                GhFile.childrenTypeIDs.Add(componentType);
                GhFile.children.Add(componentExtInfo.baseField.WriteToByteArray());
            }
            GH_Worker worker = new();
            saveGhDialog.FileName = $"{gameObjectGridView.SelectedRows[0].Cells[0].Value}";
            if (saveGhDialog.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            File.WriteAllBytes(saveGhDialog.FileName, worker.Serialize(GhFile));
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
            byte[] GhFileBytes = File.ReadAllBytes(openGhDialog.FileName);
            GH_Worker worker = new();
            GameObject_Hierarchy_File GhFile = worker.Deserialize(GhFileBytes);
            int gameObjectPathID = assetsFile.AssetInfos.Count + 1;
            var gameObjectInfo = AssetFileInfo.Create(assetsFile, gameObjectPathID, (int)AssetClassID.GameObject, manager.ClassDatabase, false);
            gameObjectInfo.SetNewData(GhFile.parentGameObject);
            var gameObjectBase = manager.GetBaseField(fileInstance, gameObjectInfo);
            var components = gameObjectBase["m_Component.Array"];
            components.Children.Clear();
            assetsFile.Metadata.AddAssetInfo(gameObjectInfo);
            /*for (int i = 0; i < GhFile.children.Count; i++)
            {
                int compPathID = assetsFile.AssetInfos.Count + 1;
                var compInfo = AssetFileInfo.Create(assetsFile, compPathID, GhFile.childrenTypeIDs[i], manager.ClassDatabase, false);
                compInfo.SetNewData(GhFile.children[i]);
                var compBase = manager.GetBaseField(fileInstance, compInfo);
                assetsFile.Metadata.AddAssetInfo(compInfo);
                components.Children.Add(compBase);
            }*/
            gameObjectInfo.SetNewData(gameObjectBase);
            UpdateAssetsFileList();
            SetModifiedState(ModifiedState.Modified);
        }
    }
}