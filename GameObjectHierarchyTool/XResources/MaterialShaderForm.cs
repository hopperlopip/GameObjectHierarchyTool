namespace GameObjectHierarchyTool.XResources
{
    public partial class MaterialShaderForm : Form
    {
        public AssetPptr ShaderPptr => new AssetPptr((int)fileIdUpDown.Value, (long)pathIdUpDown.Value);

        public MaterialShaderForm()
        {
            InitializeComponent();
            fileIdUpDown.Maximum = int.MaxValue;
            pathIdUpDown.Maximum = long.MaxValue;
        }

        private void yesButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    }
}
