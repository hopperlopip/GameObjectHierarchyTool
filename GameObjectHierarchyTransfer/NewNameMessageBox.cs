using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameObjectHierarchyTransfer
{
    public partial class NewNameMessageBox : Form
    {
        public string? newName = null;

        public NewNameMessageBox(string? oldName)
        {
            InitializeComponent();
            applyButton.TabIndex = 0;

            if (oldName != null)
            {
                nameTextBox.Text = oldName;
            }
        }

        private void applyButton_Click(object sender, EventArgs e)
        {
            newName = nameTextBox.Text;
            Close();
        }

        private void skipButton_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
