namespace GameObjectHierarchyTool.GameObjectEditor
{
    partial class GameObjectEditorForm
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
            nameGroupBox = new GroupBox();
            nameTextBox = new TextBox();
            layerGroupBox = new GroupBox();
            layerUpDown = new NumericUpDown();
            tagGroupBox = new GroupBox();
            tagUpDown = new NumericUpDown();
            isActiveCheckBox = new CheckBox();
            pathIdGroupBox = new GroupBox();
            pathIdTextBox = new TextBox();
            applyButton = new Button();
            cancelButton = new Button();
            nameGroupBox.SuspendLayout();
            layerGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)layerUpDown).BeginInit();
            tagGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)tagUpDown).BeginInit();
            pathIdGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // nameGroupBox
            // 
            nameGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameGroupBox.Controls.Add(nameTextBox);
            nameGroupBox.Location = new Point(12, 12);
            nameGroupBox.Name = "nameGroupBox";
            nameGroupBox.Size = new Size(354, 46);
            nameGroupBox.TabIndex = 0;
            nameGroupBox.TabStop = false;
            nameGroupBox.Text = "Name";
            // 
            // nameTextBox
            // 
            nameTextBox.Dock = DockStyle.Fill;
            nameTextBox.Location = new Point(3, 19);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(348, 23);
            nameTextBox.TabIndex = 10;
            nameTextBox.TabStop = false;
            nameTextBox.TextChanged += nameTextBox_TextChanged;
            // 
            // layerGroupBox
            // 
            layerGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            layerGroupBox.Controls.Add(layerUpDown);
            layerGroupBox.Location = new Point(12, 116);
            layerGroupBox.Name = "layerGroupBox";
            layerGroupBox.Size = new Size(354, 46);
            layerGroupBox.TabIndex = 1;
            layerGroupBox.TabStop = false;
            layerGroupBox.Text = "Layer";
            // 
            // layerUpDown
            // 
            layerUpDown.Dock = DockStyle.Fill;
            layerUpDown.Location = new Point(3, 19);
            layerUpDown.Maximum = new decimal(new int[] { -1, 0, 0, 0 });
            layerUpDown.Name = "layerUpDown";
            layerUpDown.Size = new Size(348, 23);
            layerUpDown.TabIndex = 0;
            layerUpDown.ValueChanged += layerUpDown_ValueChanged;
            // 
            // tagGroupBox
            // 
            tagGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tagGroupBox.Controls.Add(tagUpDown);
            tagGroupBox.Location = new Point(12, 168);
            tagGroupBox.Name = "tagGroupBox";
            tagGroupBox.Size = new Size(354, 46);
            tagGroupBox.TabIndex = 2;
            tagGroupBox.TabStop = false;
            tagGroupBox.Text = "Tag";
            // 
            // tagUpDown
            // 
            tagUpDown.Dock = DockStyle.Fill;
            tagUpDown.Location = new Point(3, 19);
            tagUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            tagUpDown.Name = "tagUpDown";
            tagUpDown.Size = new Size(348, 23);
            tagUpDown.TabIndex = 0;
            tagUpDown.ValueChanged += tagUpDown_ValueChanged;
            // 
            // isActiveCheckBox
            // 
            isActiveCheckBox.AutoSize = true;
            isActiveCheckBox.Location = new Point(12, 220);
            isActiveCheckBox.Name = "isActiveCheckBox";
            isActiveCheckBox.Size = new Size(67, 19);
            isActiveCheckBox.TabIndex = 3;
            isActiveCheckBox.Text = "IsActive";
            isActiveCheckBox.UseVisualStyleBackColor = true;
            isActiveCheckBox.CheckedChanged += isActiveCheckBox_CheckedChanged;
            // 
            // pathIdGroupBox
            // 
            pathIdGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pathIdGroupBox.Controls.Add(pathIdTextBox);
            pathIdGroupBox.Location = new Point(12, 64);
            pathIdGroupBox.Name = "pathIdGroupBox";
            pathIdGroupBox.Size = new Size(354, 46);
            pathIdGroupBox.TabIndex = 4;
            pathIdGroupBox.TabStop = false;
            pathIdGroupBox.Text = "Path ID";
            // 
            // pathIdTextBox
            // 
            pathIdTextBox.Dock = DockStyle.Fill;
            pathIdTextBox.Location = new Point(3, 19);
            pathIdTextBox.Name = "pathIdTextBox";
            pathIdTextBox.ReadOnly = true;
            pathIdTextBox.Size = new Size(348, 23);
            pathIdTextBox.TabIndex = 0;
            // 
            // applyButton
            // 
            applyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            applyButton.Location = new Point(12, 245);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(354, 40);
            applyButton.TabIndex = 1;
            applyButton.Text = "Apply";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            cancelButton.Location = new Point(12, 291);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(354, 40);
            cancelButton.TabIndex = 5;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // GameObjectEditorForm
            // 
            AcceptButton = applyButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = cancelButton;
            ClientSize = new Size(378, 343);
            Controls.Add(cancelButton);
            Controls.Add(applyButton);
            Controls.Add(pathIdGroupBox);
            Controls.Add(isActiveCheckBox);
            Controls.Add(tagGroupBox);
            Controls.Add(layerGroupBox);
            Controls.Add(nameGroupBox);
            MinimumSize = new Size(0, 382);
            Name = "GameObjectEditorForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "GameObject Editor";
            nameGroupBox.ResumeLayout(false);
            nameGroupBox.PerformLayout();
            layerGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)layerUpDown).EndInit();
            tagGroupBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)tagUpDown).EndInit();
            pathIdGroupBox.ResumeLayout(false);
            pathIdGroupBox.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox nameGroupBox;
        private TextBox nameTextBox;
        private GroupBox layerGroupBox;
        private GroupBox tagGroupBox;
        private CheckBox isActiveCheckBox;
        private GroupBox pathIdGroupBox;
        private TextBox pathIdTextBox;
        private NumericUpDown layerUpDown;
        private NumericUpDown tagUpDown;
        private Button applyButton;
        private Button cancelButton;
    }
}