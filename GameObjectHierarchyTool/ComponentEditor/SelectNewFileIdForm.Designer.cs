namespace GameObjectHierarchyTool.ComponentEditor
{
    partial class SelectNewFileIdForm
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
            oldFileIdGroupBox = new GroupBox();
            anyCheckBox = new CheckBox();
            oldPathIdUpDown = new NumericUpDown();
            newFileIdGroupBox = new GroupBox();
            newPathIdUpDown = new NumericUpDown();
            okButton = new Button();
            cancelButton = new Button();
            oldFileIdGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)oldPathIdUpDown).BeginInit();
            newFileIdGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)newPathIdUpDown).BeginInit();
            SuspendLayout();
            // 
            // oldFileIdGroupBox
            // 
            oldFileIdGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            oldFileIdGroupBox.Controls.Add(anyCheckBox);
            oldFileIdGroupBox.Controls.Add(oldPathIdUpDown);
            oldFileIdGroupBox.Location = new Point(12, 12);
            oldFileIdGroupBox.Name = "oldFileIdGroupBox";
            oldFileIdGroupBox.Size = new Size(363, 75);
            oldFileIdGroupBox.TabIndex = 0;
            oldFileIdGroupBox.TabStop = false;
            oldFileIdGroupBox.Text = "Old FileID";
            // 
            // anyCheckBox
            // 
            anyCheckBox.AutoSize = true;
            anyCheckBox.Location = new Point(6, 48);
            anyCheckBox.Name = "anyCheckBox";
            anyCheckBox.Size = new Size(47, 19);
            anyCheckBox.TabIndex = 1;
            anyCheckBox.Text = "Any";
            anyCheckBox.UseVisualStyleBackColor = true;
            anyCheckBox.CheckedChanged += anyCheckBox_CheckedChanged;
            // 
            // oldPathIdUpDown
            // 
            oldPathIdUpDown.Dock = DockStyle.Fill;
            oldPathIdUpDown.Location = new Point(3, 19);
            oldPathIdUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            oldPathIdUpDown.Minimum = new decimal(new int[] { int.MinValue, 0, 0, int.MinValue });
            oldPathIdUpDown.Name = "oldPathIdUpDown";
            oldPathIdUpDown.Size = new Size(357, 23);
            oldPathIdUpDown.TabIndex = 0;
            // 
            // newFileIdGroupBox
            // 
            newFileIdGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            newFileIdGroupBox.Controls.Add(newPathIdUpDown);
            newFileIdGroupBox.Location = new Point(12, 93);
            newFileIdGroupBox.Name = "newFileIdGroupBox";
            newFileIdGroupBox.Size = new Size(363, 48);
            newFileIdGroupBox.TabIndex = 1;
            newFileIdGroupBox.TabStop = false;
            newFileIdGroupBox.Text = "New FileID";
            // 
            // newPathIdUpDown
            // 
            newPathIdUpDown.Dock = DockStyle.Fill;
            newPathIdUpDown.Location = new Point(3, 19);
            newPathIdUpDown.Maximum = new decimal(new int[] { int.MaxValue, 0, 0, 0 });
            newPathIdUpDown.Minimum = new decimal(new int[] { int.MinValue, 0, 0, int.MinValue });
            newPathIdUpDown.Name = "newPathIdUpDown";
            newPathIdUpDown.Size = new Size(357, 23);
            newPathIdUpDown.TabIndex = 0;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.Location = new Point(219, 152);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 2;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(300, 152);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 3;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // SelectNewFileIdForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = cancelButton;
            ClientSize = new Size(387, 187);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(newFileIdGroupBox);
            Controls.Add(oldFileIdGroupBox);
            Name = "SelectNewFileIdForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select new File ID";
            oldFileIdGroupBox.ResumeLayout(false);
            oldFileIdGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)oldPathIdUpDown).EndInit();
            newFileIdGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)newPathIdUpDown).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox oldFileIdGroupBox;
        private CheckBox anyCheckBox;
        private NumericUpDown oldPathIdUpDown;
        private GroupBox newFileIdGroupBox;
        private NumericUpDown newPathIdUpDown;
        private Button okButton;
        private Button cancelButton;
    }
}