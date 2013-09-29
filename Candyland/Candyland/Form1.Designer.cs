namespace Candyland
{
    partial class Form1
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
            this.surface = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.surface)).BeginInit();
            this.SuspendLayout();
            // 
            // surface
            // 
            this.surface.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.surface.Location = new System.Drawing.Point(0, 0);
            this.surface.Name = "surface";
            this.surface.Size = new System.Drawing.Size(800, 480);
            this.surface.TabIndex = 0;
            this.surface.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 480);
            this.Controls.Add(this.surface);
            this.Name = "Form1";
            this.Text = "Save the Sweets";
            ((System.ComponentModel.ISupportInitialize)(this.surface)).EndInit();
            this.ResumeLayout(false);

            // Getting rid of the cursor, no idea if doing it this way is good, but seems to work
            this.Cursor.Dispose();
        }

        #endregion

        public System.Windows.Forms.PictureBox surface;

    }
}