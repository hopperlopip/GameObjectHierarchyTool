using AssetsTools.NET;
using AssetsTools.NET.Extra;

namespace GameObjectHierarchyTool.GameObjectEditor
{
    public partial class GameObjectEditorForm : Form
    {
        GameObjectEditorParams _gameObjectEditorParams;
        public bool applied = false;

        public AssetTypeValueField GameObjectBase
        {
            get => _gameObjectEditorParams.GameObjectBase;
        }

        public GameObjectEditorForm(string gameObjectName, AssetTypeValueField gameObjectBase, long pathId)
        {
            InitializeComponent();
            Text += $" - {gameObjectName}";
            _gameObjectEditorParams = new GameObjectEditorParams(gameObjectBase);

            nameTextBox.Text = _gameObjectEditorParams.Name;
            pathIdTextBox.Text = pathId.ToString();
            layerUpDown.Value = _gameObjectEditorParams.Layer;
            tagUpDown.Value = _gameObjectEditorParams.Tag;
            isActiveCheckBox.Checked = _gameObjectEditorParams.IsActive;
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            applied = true;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nameTextBox_TextChanged(object sender, EventArgs e)
        {
            _gameObjectEditorParams.Name = nameTextBox.Text;
        }

        private void layerUpDown_ValueChanged(object sender, EventArgs e)
        {
            _gameObjectEditorParams.Layer = (uint)layerUpDown.Value;
        }

        private void tagUpDown_ValueChanged(object sender, EventArgs e)
        {
            _gameObjectEditorParams.Tag = (ushort)tagUpDown.Value;
        }

        private void isActiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _gameObjectEditorParams.IsActive = isActiveCheckBox.Checked;
        }
    }
}
