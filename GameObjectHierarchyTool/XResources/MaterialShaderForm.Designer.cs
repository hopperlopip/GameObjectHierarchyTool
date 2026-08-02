namespace GameObjectHierarchyTool.XResources
{
    partial class MaterialShaderForm
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
            fileIdUpDown = new NumericUpDown();
            fileIdLabel = new Label();
            pathIdUpDown = new NumericUpDown();
            pathIdLabel = new Label();
            okButton = new Button();
            ((System.ComponentModel.ISupportInitialize)fileIdUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pathIdUpDown).BeginInit();
            SuspendLayout();
            // 
            // fileIdUpDown
            // 
            fileIdUpDown.Location = new Point(63, 12);
            fileIdUpDown.Name = "fileIdUpDown";
            fileIdUpDown.Size = new Size(242, 23);
            fileIdUpDown.TabIndex = 1;
            // 
            // fileIdLabel
            // 
            fileIdLabel.AutoSize = true;
            fileIdLabel.Location = new Point(12, 14);
            fileIdLabel.Name = "fileIdLabel";
            fileIdLabel.Size = new Size(39, 15);
            fileIdLabel.TabIndex = 2;
            fileIdLabel.Text = "FileID:";
            // 
            // pathIdUpDown
            // 
            pathIdUpDown.Location = new Point(63, 41);
            pathIdUpDown.Name = "pathIdUpDown";
            pathIdUpDown.Size = new Size(242, 23);
            pathIdUpDown.TabIndex = 3;
            // 
            // pathIdLabel
            // 
            pathIdLabel.AutoSize = true;
            pathIdLabel.Location = new Point(12, 43);
            pathIdLabel.Name = "pathIdLabel";
            pathIdLabel.Size = new Size(45, 15);
            pathIdLabel.TabIndex = 4;
            pathIdLabel.Text = "PathID:";
            // 
            // okButton
            // 
            okButton.Location = new Point(12, 70);
            okButton.Name = "okButton";
            okButton.Size = new Size(293, 35);
            okButton.TabIndex = 5;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += yesButton_Click;
            // 
            // MaterialShaderForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            ClientSize = new Size(317, 115);
            Controls.Add(okButton);
            Controls.Add(pathIdLabel);
            Controls.Add(pathIdUpDown);
            Controls.Add(fileIdLabel);
            Controls.Add(fileIdUpDown);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MaterialShaderForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New Material Shader Pointer";
            ((System.ComponentModel.ISupportInitialize)fileIdUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)pathIdUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label titleLabel;
        private NumericUpDown fileIdUpDown;
        private Label fileIdLabel;
        private NumericUpDown pathIdUpDown;
        private Label pathIdLabel;
        private Button okButton;
    }
}