namespace GameObjectHierarchyTool.XResources
{
    partial class CrossResourcesForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            crossResourcesGridView = new DataGridView();
            AssetsFileName = new DataGridViewTextBoxColumn();
            PathId = new DataGridViewTextBoxColumn();
            AssetType = new DataGridViewTextBoxColumn();
            DataSize = new DataGridViewTextBoxColumn();
            EditButtonColumn = new DataGridViewButtonColumn();
            removeCrossAssetsButton = new Button();
            exportCrossAssetButton = new Button();
            saveCrossAssetFileDialog = new SaveFileDialog();
            saveCrossAssetBrowserDialog = new FolderBrowserDialog();
            openFileDialog = new OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)crossResourcesGridView).BeginInit();
            SuspendLayout();
            // 
            // crossResourcesGridView
            // 
            crossResourcesGridView.AllowUserToAddRows = false;
            crossResourcesGridView.AllowUserToDeleteRows = false;
            crossResourcesGridView.AllowUserToResizeRows = false;
            crossResourcesGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            crossResourcesGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            crossResourcesGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            crossResourcesGridView.BackgroundColor = SystemColors.Control;
            crossResourcesGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            crossResourcesGridView.Columns.AddRange(new DataGridViewColumn[] { AssetsFileName, PathId, AssetType, DataSize, EditButtonColumn });
            crossResourcesGridView.Location = new Point(12, 12);
            crossResourcesGridView.Name = "crossResourcesGridView";
            crossResourcesGridView.ReadOnly = true;
            crossResourcesGridView.RowHeadersVisible = false;
            crossResourcesGridView.RowTemplate.Height = 25;
            crossResourcesGridView.RowTemplate.ReadOnly = true;
            crossResourcesGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            crossResourcesGridView.ShowCellToolTips = false;
            crossResourcesGridView.ShowEditingIcon = false;
            crossResourcesGridView.Size = new Size(642, 352);
            crossResourcesGridView.TabIndex = 0;
            // 
            // AssetsFileName
            // 
            AssetsFileName.HeaderText = "Assets File Name";
            AssetsFileName.Name = "AssetsFileName";
            AssetsFileName.ReadOnly = true;
            // 
            // PathId
            // 
            PathId.HeaderText = "PathID";
            PathId.Name = "PathId";
            PathId.ReadOnly = true;
            // 
            // AssetType
            // 
            AssetType.HeaderText = "Asset Type";
            AssetType.Name = "AssetType";
            AssetType.ReadOnly = true;
            // 
            // DataSize
            // 
            DataSize.HeaderText = "Data Size";
            DataSize.Name = "DataSize";
            DataSize.ReadOnly = true;
            // 
            // EditButtonColumn
            // 
            EditButtonColumn.HeaderText = "";
            EditButtonColumn.Name = "EditButtonColumn";
            EditButtonColumn.ReadOnly = true;
            // 
            // removeCrossAssetsButton
            // 
            removeCrossAssetsButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            removeCrossAssetsButton.Location = new Point(12, 415);
            removeCrossAssetsButton.Name = "removeCrossAssetsButton";
            removeCrossAssetsButton.Size = new Size(642, 39);
            removeCrossAssetsButton.TabIndex = 1;
            removeCrossAssetsButton.Text = "Remove selected cross-assets";
            removeCrossAssetsButton.UseVisualStyleBackColor = true;
            removeCrossAssetsButton.Click += removeCrossAssetsButton_Click;
            // 
            // exportCrossAssetButton
            // 
            exportCrossAssetButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            exportCrossAssetButton.Location = new Point(12, 370);
            exportCrossAssetButton.Name = "exportCrossAssetButton";
            exportCrossAssetButton.Size = new Size(642, 39);
            exportCrossAssetButton.TabIndex = 2;
            exportCrossAssetButton.Text = "Export selected cross-assets to the UABEA's (.dat) files";
            exportCrossAssetButton.UseVisualStyleBackColor = true;
            exportCrossAssetButton.Click += exportCrossAssetButton_Click;
            // 
            // saveCrossAssetFileDialog
            // 
            saveCrossAssetFileDialog.DefaultExt = "dat";
            saveCrossAssetFileDialog.Filter = "UABEA's raw dump|*.dat";
            // 
            // CrossResourcesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(666, 466);
            Controls.Add(exportCrossAssetButton);
            Controls.Add(removeCrossAssetsButton);
            Controls.Add(crossResourcesGridView);
            Name = "CrossResourcesForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "CrossResources Form";
            ((System.ComponentModel.ISupportInitialize)crossResourcesGridView).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private DataGridView crossResourcesGridView;
        private Button removeCrossAssetsButton;
        private Button exportCrossAssetButton;
        private SaveFileDialog saveCrossAssetFileDialog;
        private FolderBrowserDialog saveCrossAssetBrowserDialog;
        private DataGridViewTextBoxColumn AssetsFileName;
        private DataGridViewTextBoxColumn PathId;
        private DataGridViewTextBoxColumn AssetType;
        private DataGridViewTextBoxColumn DataSize;
        private DataGridViewButtonColumn EditButtonColumn;
        private OpenFileDialog openFileDialog;
    }
}