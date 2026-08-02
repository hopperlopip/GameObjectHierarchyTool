using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameObjectHierarchyTool.XResources
{
    public partial class CrossResourcesForm : Form
    {
        public bool modified = false;
        private CrossResources _crossResources;

        public CrossResourcesForm(CrossResources crossResources)
        {
            InitializeComponent();
            _crossResources = crossResources;
            ImportCrossResources();
        }

        private void ImportCrossResources()
        {
            foreach (CrossAsset crossAsset in _crossResources.crossAssets)
            {
                int rowIndex = crossResourcesGridView.Rows.Add(crossAsset.resourceName, crossAsset.pathId, crossAsset.typeId, crossAsset.data.Length);
                DataGridViewRow row = crossResourcesGridView.Rows[rowIndex];
                DataGridViewButtonCellEx buttonCell = new DataGridViewButtonCellEx();
                row.Cells[4] = buttonCell;
                buttonCell.Value = $"Edit asset data";
                buttonCell.OnClickEvent = () => { EditCrossAsset(row); };
                row.Tag = crossAsset;
            }
        }

        private void EditCrossAsset(DataGridViewRow row)
        {
            if (row.Tag == null)
                return;
            CrossAsset crossAsset = (CrossAsset)row.Tag;
            if (openFileDialog.ShowDialog() != DialogResult.Cancel)
            {
                string filePath = openFileDialog.FileName;
                if (!File.Exists(filePath))
                    return;
                byte[] newAssetData = File.ReadAllBytes(filePath);
                crossAsset.data = newAssetData;
                if (crossAsset.HasExternalData)
                {
                    if (MessageBox.Show("The cross-asset has additional data, would you like to replace it?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        if (openFileDialog.ShowDialog() != DialogResult.Cancel)
                        {
                            string externalDataFilePath = openFileDialog.FileName;
                            if (File.Exists(externalDataFilePath))
                            {
                                byte[] externalDataBytes = File.ReadAllBytes(externalDataFilePath);
                                crossAsset.externalData = externalDataBytes;
                            }
                        }
                    }
                }
                int crossAssetIndex = _crossResources.crossAssets.IndexOf(crossAsset);
                if (crossAssetIndex == -1)
                    return;
                _crossResources.crossAssets[crossAssetIndex] = crossAsset;
                row.Tag = crossAsset;
                row.Cells[3].Value = crossAsset.data.Length;
                modified = true;
            }
        }

        private void RemoveSelectedCrossAssets()
        {
            foreach (DataGridViewRow selectedRow in crossResourcesGridView.SelectedRows)
            {
                if (selectedRow.Tag == null)
                    continue;
                CrossAsset selectedCrossAsset = (CrossAsset)selectedRow.Tag;
                _crossResources.crossAssets.Remove(selectedCrossAsset);
                crossResourcesGridView.Rows.Remove(selectedRow);
                modified = true;
            }
        }

        private void ExportSelectedCrossAssets()
        {
            if (crossResourcesGridView.SelectedRows.Count == 0)
                return;
            
            if (crossResourcesGridView.SelectedRows.Count == 1)
            {
                DataGridViewRow selectedRow = crossResourcesGridView.SelectedRows[0];
                if (selectedRow.Tag == null)
                    return;
                CrossAsset crossAsset = (CrossAsset)selectedRow.Tag;
                saveCrossAssetFileDialog.FileName = $"{crossAsset.typeId}-{crossAsset.resourceName}-{crossAsset.pathId}.dat";
                if (saveCrossAssetFileDialog.ShowDialog() != DialogResult.Cancel)
                {
                    string filePath = saveCrossAssetFileDialog.FileName;
                    File.WriteAllBytes(filePath, crossAsset.data);
                }
                return;
            }
            if (saveCrossAssetBrowserDialog.ShowDialog() != DialogResult.Cancel)
            {
                foreach (DataGridViewRow selectedRow in crossResourcesGridView.SelectedRows)
                {
                    if (selectedRow.Tag == null)
                        continue;
                    CrossAsset crossAsset = (CrossAsset)selectedRow.Tag;
                    string crossAssetFileName = $"{crossAsset.typeId}-{crossAsset.resourceName}-{crossAsset.pathId}.dat";
                    string dirPath = saveCrossAssetBrowserDialog.SelectedPath;
                    string filePath = Path.Combine(dirPath, crossAssetFileName);
                    File.WriteAllBytes(filePath, crossAsset.data);
                }
            }
        }

        private void removeCrossAssetsButton_Click(object sender, EventArgs e)
        {
            RemoveSelectedCrossAssets();
        }

        private void exportCrossAssetButton_Click(object sender, EventArgs e)
        {
            ExportSelectedCrossAssets();
        }
    }
}
