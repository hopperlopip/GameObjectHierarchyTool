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
    public partial class SelectNewFileIdForm : Form
    {
        public int? oldPathId;
        public int newPathId;
        public bool applied;

        public SelectNewFileIdForm()
        {
            InitializeComponent();
        }

        private void anyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            oldPathIdUpDown.Enabled = !anyCheckBox.Checked;
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            if (anyCheckBox.Checked)
                oldPathId = null;
            else
                oldPathId = (int)oldPathIdUpDown.Value;
            newPathId = (int)newPathIdUpDown.Value;
            applied = true;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
