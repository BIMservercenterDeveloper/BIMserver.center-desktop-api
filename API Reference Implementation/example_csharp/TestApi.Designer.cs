namespace example_csharp
{
    partial class TestApi
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
            this.loginBtn = new System.Windows.Forms.Button();
            this.createDBBtn = new System.Windows.Forms.Button();
            this.chkLoginBtn = new System.Windows.Forms.Button();
            this.selectCurrentProjectBtn = new System.Windows.Forms.Button();
            this.selectIfcBtn = new System.Windows.Forms.Button();
            this.selectAssociatedFileBtn = new System.Windows.Forms.Button();
            this.getAbsFilePathBtn = new System.Windows.Forms.Button();
            this.putIFCBtn = new System.Windows.Forms.Button();
            this.putAssociatedBtn = new System.Windows.Forms.Button();
            this.getCurrenProjectPathBtn = new System.Windows.Forms.Button();
            this.checkUpdatedFile = new System.Windows.Forms.Button();
            this.showLoginData = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // loginBtn
            // 
            this.loginBtn.Location = new System.Drawing.Point(12, 53);
            this.loginBtn.Name = "loginBtn";
            this.loginBtn.Size = new System.Drawing.Size(260, 36);
            this.loginBtn.TabIndex = 0;
            this.loginBtn.Text = "Login";
            this.loginBtn.UseVisualStyleBackColor = true;
            this.loginBtn.Click += new System.EventHandler(this.button1_Click);
            // 
            // createDBBtn
            // 
            this.createDBBtn.Location = new System.Drawing.Point(12, 12);
            this.createDBBtn.Name = "createDBBtn";
            this.createDBBtn.Size = new System.Drawing.Size(260, 35);
            this.createDBBtn.TabIndex = 1;
            this.createDBBtn.Text = "Create database";
            this.createDBBtn.UseVisualStyleBackColor = true;
            this.createDBBtn.Click += new System.EventHandler(this.createDBBtn_Click);
            // 
            // chkLoginBtn
            // 
            this.chkLoginBtn.Location = new System.Drawing.Point(13, 96);
            this.chkLoginBtn.Name = "chkLoginBtn";
            this.chkLoginBtn.Size = new System.Drawing.Size(260, 36);
            this.chkLoginBtn.TabIndex = 2;
            this.chkLoginBtn.Text = "Check login status";
            this.chkLoginBtn.UseVisualStyleBackColor = true;
            this.chkLoginBtn.Click += new System.EventHandler(this.chkLoginBtn_Click_1);
            // 
            // selectCurrentProjectBtn
            // 
            this.selectCurrentProjectBtn.AllowDrop = true;
            this.selectCurrentProjectBtn.Location = new System.Drawing.Point(13, 139);
            this.selectCurrentProjectBtn.Name = "selectCurrentProjectBtn";
            this.selectCurrentProjectBtn.Size = new System.Drawing.Size(260, 36);
            this.selectCurrentProjectBtn.TabIndex = 3;
            this.selectCurrentProjectBtn.Text = "Select current project";
            this.selectCurrentProjectBtn.UseVisualStyleBackColor = true;
            this.selectCurrentProjectBtn.Click += new System.EventHandler(this.button2_Click);
            // 
            // selectIfcBtn
            // 
            this.selectIfcBtn.Location = new System.Drawing.Point(12, 182);
            this.selectIfcBtn.Name = "selectIfcBtn";
            this.selectIfcBtn.Size = new System.Drawing.Size(260, 36);
            this.selectIfcBtn.TabIndex = 4;
            this.selectIfcBtn.Text = "Select ifc file from current project";
            this.selectIfcBtn.UseVisualStyleBackColor = true;
            this.selectIfcBtn.Click += new System.EventHandler(this.button3_Click);
            // 
            // selectAssociatedFileBtn
            // 
            this.selectAssociatedFileBtn.Location = new System.Drawing.Point(13, 225);
            this.selectAssociatedFileBtn.Name = "selectAssociatedFileBtn";
            this.selectAssociatedFileBtn.Size = new System.Drawing.Size(260, 36);
            this.selectAssociatedFileBtn.TabIndex = 5;
            this.selectAssociatedFileBtn.Text = "Select associated file from current project";
            this.selectAssociatedFileBtn.UseVisualStyleBackColor = true;
            this.selectAssociatedFileBtn.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // getAbsFilePathBtn
            // 
            this.getAbsFilePathBtn.Location = new System.Drawing.Point(13, 267);
            this.getAbsFilePathBtn.Name = "getAbsFilePathBtn";
            this.getAbsFilePathBtn.Size = new System.Drawing.Size(260, 36);
            this.getAbsFilePathBtn.TabIndex = 6;
            this.getAbsFilePathBtn.Text = "Absolute file paths";
            this.getAbsFilePathBtn.UseVisualStyleBackColor = true;
            this.getAbsFilePathBtn.Click += new System.EventHandler(this.getAbsFilePathBtn_Click);
            // 
            // putIFCBtn
            // 
            this.putIFCBtn.Location = new System.Drawing.Point(13, 309);
            this.putIFCBtn.Name = "putIFCBtn";
            this.putIFCBtn.Size = new System.Drawing.Size(260, 36);
            this.putIFCBtn.TabIndex = 7;
            this.putIFCBtn.Text = "Put ifc file into current project";
            this.putIFCBtn.UseVisualStyleBackColor = true;
            this.putIFCBtn.Click += new System.EventHandler(this.putIFCBtn_Click);
            // 
            // putAssociatedBtn
            // 
            this.putAssociatedBtn.Location = new System.Drawing.Point(13, 351);
            this.putAssociatedBtn.Name = "putAssociatedBtn";
            this.putAssociatedBtn.Size = new System.Drawing.Size(260, 36);
            this.putAssociatedBtn.TabIndex = 8;
            this.putAssociatedBtn.Text = "Put associated file into current project";
            this.putAssociatedBtn.UseVisualStyleBackColor = true;
            this.putAssociatedBtn.Click += new System.EventHandler(this.putAssociatedBtn_Click);
            // 
            // getCurrenProjectPathBtn
            // 
            this.getCurrenProjectPathBtn.Location = new System.Drawing.Point(13, 394);
            this.getCurrenProjectPathBtn.Name = "getCurrenProjectPathBtn";
            this.getCurrenProjectPathBtn.Size = new System.Drawing.Size(260, 36);
            this.getCurrenProjectPathBtn.TabIndex = 9;
            this.getCurrenProjectPathBtn.Text = "Show current project path";
            this.getCurrenProjectPathBtn.UseVisualStyleBackColor = true;
            this.getCurrenProjectPathBtn.Click += new System.EventHandler(this.getCurrenProjectPathBtn_Click);
            // 
            // checkUpdatedFile
            // 
            this.checkUpdatedFile.Location = new System.Drawing.Point(12, 437);
            this.checkUpdatedFile.Name = "checkUpdatedFile";
            this.checkUpdatedFile.Size = new System.Drawing.Size(260, 36);
            this.checkUpdatedFile.TabIndex = 10;
            this.checkUpdatedFile.Text = "Check updated file";
            this.checkUpdatedFile.UseVisualStyleBackColor = true;
            this.checkUpdatedFile.Click += new System.EventHandler(this.checkUpdatedFile_Click);
            // 
            // showLoginData
            // 
            this.showLoginData.Location = new System.Drawing.Point(13, 480);
            this.showLoginData.Name = "showLoginData";
            this.showLoginData.Size = new System.Drawing.Size(260, 36);
            this.showLoginData.TabIndex = 11;
            this.showLoginData.Text = "Show login data";
            this.showLoginData.UseVisualStyleBackColor = true;
            this.showLoginData.Click += new System.EventHandler(this.showLoginData_Click);
            // 
            // TestApi
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 552);
            this.Controls.Add(this.showLoginData);
            this.Controls.Add(this.checkUpdatedFile);
            this.Controls.Add(this.getCurrenProjectPathBtn);
            this.Controls.Add(this.putAssociatedBtn);
            this.Controls.Add(this.putIFCBtn);
            this.Controls.Add(this.getAbsFilePathBtn);
            this.Controls.Add(this.selectAssociatedFileBtn);
            this.Controls.Add(this.selectIfcBtn);
            this.Controls.Add(this.selectCurrentProjectBtn);
            this.Controls.Add(this.chkLoginBtn);
            this.Controls.Add(this.createDBBtn);
            this.Controls.Add(this.loginBtn);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestApi";
            this.Text = "Test API Methods - C#";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button loginBtn;
        private System.Windows.Forms.Button createDBBtn;
        private System.Windows.Forms.Button chkLoginBtn;
        private System.Windows.Forms.Button selectCurrentProjectBtn;
        private System.Windows.Forms.Button selectIfcBtn;
        private System.Windows.Forms.Button selectAssociatedFileBtn;
        private System.Windows.Forms.Button getAbsFilePathBtn;
        private System.Windows.Forms.Button putIFCBtn;
        private System.Windows.Forms.Button putAssociatedBtn;
        private System.Windows.Forms.Button getCurrenProjectPathBtn;
        private System.Windows.Forms.Button checkUpdatedFile;
        private System.Windows.Forms.Button showLoginData;
    }
}

