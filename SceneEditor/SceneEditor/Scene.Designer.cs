namespace SceneEditor
{
    partial class SceneGenerator
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addAreaButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.editAreaButton = new System.Windows.Forms.Button();
            this.generateButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 3);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(464, 160);
            this.listBox1.TabIndex = 0;
            // 
            // addAreaButton
            // 
            this.addAreaButton.Location = new System.Drawing.Point(3, 169);
            this.addAreaButton.Name = "addAreaButton";
            this.addAreaButton.Size = new System.Drawing.Size(229, 32);
            this.addAreaButton.TabIndex = 1;
            this.addAreaButton.Text = "Add Area";
            this.addAreaButton.UseVisualStyleBackColor = true;
            this.addAreaButton.Click += new System.EventHandler(this.addAreaButton_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.listBox1);
            this.flowLayoutPanel1.Controls.Add(this.addAreaButton);
            this.flowLayoutPanel1.Controls.Add(this.editAreaButton);
            this.flowLayoutPanel1.Controls.Add(this.generateButton);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(16, 14);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(472, 245);
            this.flowLayoutPanel1.TabIndex = 2;
            // 
            // editAreaButton
            // 
            this.editAreaButton.Location = new System.Drawing.Point(238, 169);
            this.editAreaButton.Name = "editAreaButton";
            this.editAreaButton.Size = new System.Drawing.Size(229, 32);
            this.editAreaButton.TabIndex = 3;
            this.editAreaButton.Text = "Edit Area";
            this.editAreaButton.UseVisualStyleBackColor = true;
            this.editAreaButton.Click += new System.EventHandler(this.editAreaButton_Click);
            // 
            // generateButton
            // 
            this.generateButton.Location = new System.Drawing.Point(3, 207);
            this.generateButton.Name = "generateButton";
            this.generateButton.Size = new System.Drawing.Size(464, 32);
            this.generateButton.TabIndex = 2;
            this.generateButton.Text = "Generate";
            this.generateButton.UseVisualStyleBackColor = true;
            this.generateButton.Click += new System.EventHandler(this.generateButton_Click);
            // 
            // SceneGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 271);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "SceneGenerator";
            this.Text = "SceneGenerator";
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button addAreaButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button generateButton;
        private System.Windows.Forms.Button editAreaButton;
    }
}

