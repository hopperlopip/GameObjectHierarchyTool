namespace GameObjectHierarchyTransfer
{
    partial class NewNameMessageBox
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
            title = new Label();
            nameTextBox = new TextBox();
            applyButton = new Button();
            skipButton = new Button();
            SuspendLayout();
            // 
            // title
            // 
            title.AutoSize = true;
            title.Location = new Point(12, 9);
            title.Name = "title";
            title.Size = new Size(325, 20);
            title.TabIndex = 0;
            title.Text = "Please type new name for selected GameObject";
            // 
            // nameTextBox
            // 
            nameTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            nameTextBox.BorderStyle = BorderStyle.FixedSingle;
            nameTextBox.Location = new Point(12, 41);
            nameTextBox.Name = "nameTextBox";
            nameTextBox.Size = new Size(415, 27);
            nameTextBox.TabIndex = 1;
            // 
            // applyButton
            // 
            applyButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            applyButton.Location = new Point(307, 79);
            applyButton.Name = "applyButton";
            applyButton.Size = new Size(120, 33);
            applyButton.TabIndex = 2;
            applyButton.Text = "Apply";
            applyButton.UseVisualStyleBackColor = true;
            applyButton.Click += applyButton_Click;
            // 
            // skipButton
            // 
            skipButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            skipButton.Location = new Point(186, 79);
            skipButton.Name = "skipButton";
            skipButton.Size = new Size(115, 33);
            skipButton.TabIndex = 3;
            skipButton.Text = "Skip";
            skipButton.UseVisualStyleBackColor = true;
            skipButton.Click += skipButton_Click;
            // 
            // NewNameMessageBox
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(439, 124);
            ControlBox = false;
            Controls.Add(skipButton);
            Controls.Add(applyButton);
            Controls.Add(nameTextBox);
            Controls.Add(title);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "NewNameMessageBox";
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "New name for GameObject";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label title;
        private TextBox nameTextBox;
        private Button applyButton;
        private Button skipButton;
    }
}