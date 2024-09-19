using AssetsTools.NET;
using AssetsTools.NET.Extra;
using System.ComponentModel;

namespace GameObjectHierarchyTool.ComponentEditor
{
    public partial class ComponentEditorForm : Form
    {
        const string ERROR_TITLE = "Error";
        const string WARNING_TITLE = "Warning";

        readonly AssetsManager _manager;
        readonly AssetsFileInstance _fileInstance;
        readonly List<AssetExternal> _components;
        readonly GameObjectHelper _gameObjectHelper;
        readonly long _gameObjectPathId;
        public bool modified = false;

        public ComponentEditorForm(AssetsManager manager, AssetsFileInstance fileInstance, long gameObjectPathId, string gameObjectName, List<AssetExternal> components)
        {
            InitializeComponent();
            componentListView.KeyDown += ComponentListView_KeyDown;
            _manager = manager;
            _fileInstance = fileInstance;
            _gameObjectPathId = gameObjectPathId;
            _gameObjectHelper = new GameObjectHelper(manager, fileInstance);
            Text += $" - {gameObjectName}";
            _components = components;
            UpdateComponentList();
            componentListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            componentListView.ItemCheck += ComponentListView_ItemCheck;
        }

        private void ComponentListView_KeyDown(object? sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Control && e.KeyCode == Keys.A)
            {
                SelectAllItems();
            }

            if (componentListView.SelectedItems.Count < 1)
                return;

            switch (e.KeyCode)
            {
                case Keys.Delete:
                    removeComponentsToolStripMenuItem.PerformClick();
                    break;
            }
        }

        private void SelectAllItems()
        {
            foreach (ListViewItem item in componentListView.Items)
            {
                item.Selected = true;
            }
        }

        private void UpdateComponentList()
        {
            foreach (var componentExt in _components)
            {
                Component component = new Component(_manager, _fileInstance, componentExt);
                ListViewItem item = CreateListViewItem(component);
                componentListView.Items.Add(item);
            }
        }

        private ListViewItem CreateListViewItem(Component component)
        {
            ListViewItem item = new ListViewItem(new string[] { component.Name, component.TypeId.ToString(), component.PathId.ToString() });
            item.Tag = component;
            item.Checked = component.IsEnabled;
            return item;
        }

        private void ComponentListView_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (componentListView.FocusedItem == null)
                return;

            Component component = (Component)componentListView.Items[e.Index].Tag;
            if (!component.AllowEnable)
            {
                e.NewValue = e.CurrentValue;
                MessageBox.Show($"Can't enable/disable this component ({component.Name}) because it doesn't have the \"m_Enabled\" field.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool enabled = e.NewValue == CheckState.Checked;
            component.IsEnabled = enabled;
            component.SaveChanges();
            modified = true;
        }

        private void enableComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in componentListView.SelectedItems)
            {
                item.Checked = true;
            }
        }

        private void disableComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in componentListView.SelectedItems)
            {
                item.Checked = false;
            }
        }

        private void removeComponentsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in componentListView.SelectedItems)
            {
                Component component = (Component)item.Tag;
                if (component.IsTransform)
                {
                    MessageBox.Show("You shouldn't remove Transform/RectTransform from a GameObject.", WARNING_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    continue;
                }
                _gameObjectHelper.RemoveComponent(component.PathId);
                item.Remove();
                modified = true;
            }
        }

        private void addComponentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectComponentTypeForm selectComponentTypeForm = new SelectComponentTypeForm();
            selectComponentTypeForm.ShowDialog();
            if (selectComponentTypeForm.selectedTypeId == null)
                return;
            AssetClassID componentTypeId = (AssetClassID)selectComponentTypeForm.selectedTypeId;
            string componentTypeIdName = Enum.GetName(typeof(AssetClassID), componentTypeId) ?? string.Empty;
            if (!_gameObjectHelper.HasCldbSpecifiedTypeId(componentTypeId))
            {
                MessageBox.Show($"Couldn't find specified type ({componentTypeIdName}) in the ClassDatabase.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_gameObjectHelper.HasCldbTypeRootNodes(componentTypeId))
            {
                MessageBox.Show("No root nodes were found!\r\n" +
                    $"You can't add this type ({componentTypeIdName}) of an asset.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!_gameObjectHelper.IsValidComponentType(componentTypeId))
            {
                MessageBox.Show("Can't add component because it doesn't have the \"m_GameObject\" field.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (componentTypeId == AssetClassID.Transform || componentTypeId == AssetClassID.RectTransform)
            {
                MessageBox.Show("You shouldn't create new Transform/RectTransform in a GameObject.", WARNING_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            long componentPathId = _gameObjectHelper.AddComponent(_gameObjectPathId, componentTypeId);
            Component component = new Component(_manager, _fileInstance, componentPathId);
            ListViewItem item = CreateListViewItem(component);
            componentListView.ItemCheck -= ComponentListView_ItemCheck;
            componentListView.Items.Add(item);
            componentListView.ItemCheck += ComponentListView_ItemCheck;
            componentListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            modified = true;
        }

        private void changeFileIDsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (componentListView.SelectedItems.Count < 1)
                return;
            SelectNewFileIdForm selectNewFileIdForm = new SelectNewFileIdForm();
            selectNewFileIdForm.ShowDialog();
            if (!selectNewFileIdForm.applied)
                return;
            int? oldPathId = selectNewFileIdForm.oldPathId;
            int newPathId = selectNewFileIdForm.newPathId;
            foreach (ListViewItem item in componentListView.SelectedItems)
            {
                Component component = (Component)item.Tag;
                component.ChangeAllFileIDFields(component.ComponentBase, oldPathId, newPathId);
                component.SaveChanges();
            }
            modified = true;
        }
    }
}
