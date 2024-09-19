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

namespace GameObjectHierarchyTool.ComponentEditor
{
    public partial class SelectComponentTypeForm : Form
    {
        const string ERROR_TITLE = "Error";

        public AssetClassID? selectedTypeId;

        public SelectComponentTypeForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            string componentTypeText = componentTypeTextBox.Text;
            if (int.TryParse(componentTypeText, out int typeId))
            {
                if (Enum.IsDefined((AssetClassID)typeId))
                    selectedTypeId = (AssetClassID)typeId;
            }
            else if (Enum.TryParse(typeof(AssetClassID), componentTypeText, false, out object? result))
            {
                if (result != null)
                    selectedTypeId = (AssetClassID)result;
            }
            if (selectedTypeId == null)
            {
                MessageBox.Show("Couldn't find this component type.", ERROR_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
