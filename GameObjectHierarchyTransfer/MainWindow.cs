using AssetsTools.NET.Extra;
using AssetsTools.NET;

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
        }

        private void LoadAssetsFile(string assetsPath)
        {
            gameObjectGridView.Rows.Clear();
            manager.LoadClassPackage("classdata.tpk");
            fileInstance = manager.LoadAssetsFile(assetsPath, loadDeps: true);
            assetsFile = fileInstance.file;
            manager.LoadClassDatabaseFromPackage(assetsFile.Metadata.UnityVersion);
            UpdateAssetsFileList();
        }

        private void UpdateAssetsFileList()
        {
            if (assetsFile != null)
            {
                int savedSelectedRowIndex = 0;
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
                }
            }
        }
    }
}