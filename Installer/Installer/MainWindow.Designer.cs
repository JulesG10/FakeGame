
namespace Installer
{
    partial class MainWindow
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.RessourcesprogressBar = new System.Windows.Forms.ProgressBar();
            this.fileLoadingName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.GamepictureBox = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.GamepictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // RessourcesprogressBar
            // 
            this.RessourcesprogressBar.Location = new System.Drawing.Point(21, 357);
            this.RessourcesprogressBar.Name = "RessourcesprogressBar";
            this.RessourcesprogressBar.Size = new System.Drawing.Size(472, 23);
            this.RessourcesprogressBar.TabIndex = 0;
            // 
            // fileLoadingName
            // 
            this.fileLoadingName.AutoSize = true;
            this.fileLoadingName.Location = new System.Drawing.Point(21, 325);
            this.fileLoadingName.Name = "fileLoadingName";
            this.fileLoadingName.Size = new System.Drawing.Size(0, 15);
            this.fileLoadingName.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(21, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(348, 32);
            this.label2.TabIndex = 2;
            this.label2.Text = "Setting up game dependencies";
            // 
            // GamepictureBox
            // 
            this.GamepictureBox.Location = new System.Drawing.Point(21, 72);
            this.GamepictureBox.Name = "GamepictureBox";
            this.GamepictureBox.Size = new System.Drawing.Size(472, 231);
            this.GamepictureBox.TabIndex = 3;
            this.GamepictureBox.TabStop = false;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 411);
            this.Controls.Add(this.GamepictureBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.fileLoadingName);
            this.Controls.Add(this.RessourcesprogressBar);
            this.Name = "MainWindow";
            this.Text = "GameInstaller";
            this.Load += new System.EventHandler(this.MainWindow_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GamepictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar RessourcesprogressBar;
        private System.Windows.Forms.Label fileLoadingName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox GamepictureBox;
    }
}

