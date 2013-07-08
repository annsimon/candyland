namespace SceneEditor
{
    partial class LevelGenerator
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
            this.layout = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPosX = new System.Windows.Forms.TextBox();
            this.textBoxPosY = new System.Windows.Forms.TextBox();
            this.textBoxPosZ = new System.Windows.Forms.TextBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxStartMain = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxStartSecondary = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.addStaticButton = new System.Windows.Forms.Button();
            this.editStaticButton = new System.Windows.Forms.Button();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.addDynamicButton = new System.Windows.Forms.Button();
            this.editDynamicButton = new System.Windows.Forms.Button();
            this.acceptButton = new System.Windows.Forms.Button();
            this.importButton = new System.Windows.Forms.Button();
            this.layout.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // layout
            // 
            this.layout.Controls.Add(this.label1);
            this.layout.Controls.Add(this.textBoxID);
            this.layout.Controls.Add(this.label4);
            this.layout.Controls.Add(this.textBoxPosX);
            this.layout.Controls.Add(this.textBoxPosY);
            this.layout.Controls.Add(this.textBoxPosZ);
            this.layout.Controls.Add(this.flowLayoutPanel1);
            this.layout.Controls.Add(this.listBox1);
            this.layout.Controls.Add(this.addStaticButton);
            this.layout.Controls.Add(this.editStaticButton);
            this.layout.Controls.Add(this.listBox2);
            this.layout.Controls.Add(this.addDynamicButton);
            this.layout.Controls.Add(this.editDynamicButton);
            this.layout.Controls.Add(this.importButton);
            this.layout.Controls.Add(this.acceptButton);
            this.layout.Location = new System.Drawing.Point(12, 12);
            this.layout.Name = "layout";
            this.layout.Size = new System.Drawing.Size(496, 542);
            this.layout.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label1.Size = new System.Drawing.Size(18, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "ID";
            // 
            // textBoxID
            // 
            this.textBoxID.Location = new System.Drawing.Point(27, 3);
            this.textBoxID.Name = "textBoxID";
            this.textBoxID.Size = new System.Drawing.Size(25, 20);
            this.textBoxID.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(58, 0);
            this.label4.Name = "label4";
            this.label4.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label4.Size = new System.Drawing.Size(74, 19);
            this.label4.TabIndex = 6;
            this.label4.Text = "Position (x,y,z)";
            // 
            // textBoxPosX
            // 
            this.textBoxPosX.Location = new System.Drawing.Point(138, 3);
            this.textBoxPosX.Name = "textBoxPosX";
            this.textBoxPosX.Size = new System.Drawing.Size(35, 20);
            this.textBoxPosX.TabIndex = 7;
            // 
            // textBoxPosY
            // 
            this.textBoxPosY.Location = new System.Drawing.Point(179, 3);
            this.textBoxPosY.Name = "textBoxPosY";
            this.textBoxPosY.Size = new System.Drawing.Size(37, 20);
            this.textBoxPosY.TabIndex = 8;
            // 
            // textBoxPosZ
            // 
            this.textBoxPosZ.Location = new System.Drawing.Point(222, 3);
            this.textBoxPosZ.Name = "textBoxPosZ";
            this.textBoxPosZ.Size = new System.Drawing.Size(38, 20);
            this.textBoxPosZ.TabIndex = 9;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.label6);
            this.flowLayoutPanel1.Controls.Add(this.textBoxStartMain);
            this.flowLayoutPanel1.Controls.Add(this.label7);
            this.flowLayoutPanel1.Controls.Add(this.textBoxStartSecondary);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 29);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(488, 29);
            this.flowLayoutPanel1.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 0);
            this.label6.Name = "label6";
            this.label6.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label6.Size = new System.Drawing.Size(129, 19);
            this.label6.TabIndex = 22;
            this.label6.Text = "Start MainChar PlatformID";
            // 
            // textBoxStartMain
            // 
            this.textBoxStartMain.Location = new System.Drawing.Point(138, 3);
            this.textBoxStartMain.Name = "textBoxStartMain";
            this.textBoxStartMain.Size = new System.Drawing.Size(64, 20);
            this.textBoxStartMain.TabIndex = 23;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(208, 0);
            this.label7.Name = "label7";
            this.label7.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.label7.Size = new System.Drawing.Size(157, 19);
            this.label7.TabIndex = 24;
            this.label7.Text = "Start SecondaryChar PlatformID";
            // 
            // textBoxStartSecondary
            // 
            this.textBoxStartSecondary.Location = new System.Drawing.Point(371, 3);
            this.textBoxStartSecondary.Name = "textBoxStartSecondary";
            this.textBoxStartSecondary.Size = new System.Drawing.Size(67, 20);
            this.textBoxStartSecondary.TabIndex = 25;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(3, 64);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(490, 225);
            this.listBox1.TabIndex = 10;
            // 
            // addStaticButton
            // 
            this.addStaticButton.Location = new System.Drawing.Point(3, 295);
            this.addStaticButton.Name = "addStaticButton";
            this.addStaticButton.Size = new System.Drawing.Size(243, 32);
            this.addStaticButton.TabIndex = 11;
            this.addStaticButton.Text = "Add Static Object";
            this.addStaticButton.UseVisualStyleBackColor = true;
            this.addStaticButton.Click += new System.EventHandler(this.addStaticButton_Click);
            // 
            // editStaticButton
            // 
            this.editStaticButton.Location = new System.Drawing.Point(252, 295);
            this.editStaticButton.Name = "editStaticButton";
            this.editStaticButton.Size = new System.Drawing.Size(241, 32);
            this.editStaticButton.TabIndex = 13;
            this.editStaticButton.Text = "Edit Static Object";
            this.editStaticButton.UseVisualStyleBackColor = true;
            this.editStaticButton.Click += new System.EventHandler(this.editStaticButton_Click);
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(3, 333);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(490, 95);
            this.listBox2.TabIndex = 14;
            // 
            // addDynamicButton
            // 
            this.addDynamicButton.Location = new System.Drawing.Point(3, 434);
            this.addDynamicButton.Name = "addDynamicButton";
            this.addDynamicButton.Size = new System.Drawing.Size(243, 32);
            this.addDynamicButton.TabIndex = 15;
            this.addDynamicButton.Text = "Add Dynamic Object";
            this.addDynamicButton.UseVisualStyleBackColor = true;
            this.addDynamicButton.Click += new System.EventHandler(this.addDynamicButton_Click);
            // 
            // editDynamicButton
            // 
            this.editDynamicButton.Location = new System.Drawing.Point(252, 434);
            this.editDynamicButton.Name = "editDynamicButton";
            this.editDynamicButton.Size = new System.Drawing.Size(241, 32);
            this.editDynamicButton.TabIndex = 16;
            this.editDynamicButton.Text = "Edit Dynamic Object";
            this.editDynamicButton.UseVisualStyleBackColor = true;
            this.editDynamicButton.Click += new System.EventHandler(this.editDynamicButton_Click);
            // 
            // acceptButton
            // 
            this.acceptButton.Location = new System.Drawing.Point(3, 511);
            this.acceptButton.Name = "acceptButton";
            this.acceptButton.Size = new System.Drawing.Size(490, 32);
            this.acceptButton.TabIndex = 12;
            this.acceptButton.Text = "OK";
            this.acceptButton.UseVisualStyleBackColor = true;
            this.acceptButton.Click += new System.EventHandler(this.acceptButton_Click);
            // 
            // importButton
            // 
            this.importButton.Location = new System.Drawing.Point(3, 472);
            this.importButton.Name = "importButton";
            this.importButton.Size = new System.Drawing.Size(490, 33);
            this.importButton.TabIndex = 23;
            this.importButton.Text = "Import (set level ID first!)";
            this.importButton.UseVisualStyleBackColor = true;
            this.importButton.Click += new System.EventHandler(this.importButton_Click);
            // 
            // LevelGenerator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 566);
            this.Controls.Add(this.layout);
            this.Name = "LevelGenerator";
            this.Text = "SceneGenerator - Edit Level";
            this.layout.ResumeLayout(false);
            this.layout.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel layout;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxPosX;
        private System.Windows.Forms.TextBox textBoxPosY;
        private System.Windows.Forms.TextBox textBoxPosZ;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button addStaticButton;
        private System.Windows.Forms.Button editStaticButton;
        private System.Windows.Forms.Button acceptButton;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Button addDynamicButton;
        private System.Windows.Forms.Button editDynamicButton;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxStartMain;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxStartSecondary;
        private System.Windows.Forms.Button importButton;
    }
}