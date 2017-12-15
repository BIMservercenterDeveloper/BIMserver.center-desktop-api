namespace example_csharp
{
    partial class ShowLoginDataForm
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
            this.userImage = new System.Windows.Forms.PictureBox();
            this.userName = new System.Windows.Forms.Label();
            this.userEmail = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.userImage)).BeginInit();
            this.SuspendLayout();
            // 
            // userImage
            // 
            this.userImage.Location = new System.Drawing.Point(12, 12);
            this.userImage.Name = "userImage";
            this.userImage.Size = new System.Drawing.Size(64, 64);
            this.userImage.TabIndex = 0;
            this.userImage.TabStop = false;
            // 
            // userName
            // 
            this.userName.AutoSize = true;
            this.userName.Location = new System.Drawing.Point(9, 90);
            this.userName.Name = "userName";
            this.userName.Size = new System.Drawing.Size(35, 13);
            this.userName.TabIndex = 1;
            this.userName.Text = "label1";
            // 
            // userEmail
            // 
            this.userEmail.AutoSize = true;
            this.userEmail.Location = new System.Drawing.Point(9, 113);
            this.userEmail.Name = "userEmail";
            this.userEmail.Size = new System.Drawing.Size(35, 13);
            this.userEmail.TabIndex = 2;
            this.userEmail.Text = "label2";
            // 
            // ShowLoginDataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 262);
            this.Controls.Add(this.userEmail);
            this.Controls.Add(this.userName);
            this.Controls.Add(this.userImage);
            this.Name = "ShowLoginDataForm";
            this.Text = "ShowLoginDataForm";
            ((System.ComponentModel.ISupportInitialize)(this.userImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox userImage;
        private System.Windows.Forms.Label userName;
        private System.Windows.Forms.Label userEmail;
    }
}