namespace GameObjectHierarchyTool.ComponentEditor
{
    partial class SelectComponentTypeForm
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
            componentTypeGroupBox = new GroupBox();
            componentTypeTextBox = new TextBox();
            okButton = new Button();
            cancelButton = new Button();
            componentTypeGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // componentTypeGroupBox
            // 
            componentTypeGroupBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            componentTypeGroupBox.Controls.Add(componentTypeTextBox);
            componentTypeGroupBox.Location = new Point(12, 12);
            componentTypeGroupBox.Name = "componentTypeGroupBox";
            componentTypeGroupBox.Size = new Size(296, 47);
            componentTypeGroupBox.TabIndex = 0;
            componentTypeGroupBox.TabStop = false;
            componentTypeGroupBox.Text = "Enter component type name/ID:";
            // 
            // componentTypeTextBox
            // 
            componentTypeTextBox.Dock = DockStyle.Fill;
            componentTypeTextBox.Location = new Point(3, 19);
            componentTypeTextBox.Name = "componentTypeTextBox";
            componentTypeTextBox.Size = new Size(290, 23);
            componentTypeTextBox.TabIndex = 0;
            // 
            // okButton
            // 
            okButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            okButton.Location = new Point(152, 71);
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.TabIndex = 1;
            okButton.Text = "OK";
            okButton.UseVisualStyleBackColor = true;
            okButton.Click += okButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(233, 71);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.TabIndex = 2;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // SelectComponentTypeForm
            // 
            AcceptButton = okButton;
            AutoScaleDimensions = new SizeF(96F, 96F);
            AutoScaleMode = AutoScaleMode.Dpi;
            CancelButton = cancelButton;
            ClientSize = new Size(320, 106);
            Controls.Add(cancelButton);
            Controls.Add(okButton);
            Controls.Add(componentTypeGroupBox);
            MinimumSize = new Size(0, 145);
            Name = "SelectComponentTypeForm";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Select Component Type";
            componentTypeGroupBox.ResumeLayout(false);
            componentTypeGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private GroupBox componentTypeGroupBox;
        private TextBox componentTypeTextBox;
        private Button okButton;
        private Button cancelButton;
    }
}