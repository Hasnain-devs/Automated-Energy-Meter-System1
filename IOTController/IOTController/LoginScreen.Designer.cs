namespace IOTController
{
    partial class LoginScreen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginScreen));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.TxtUsername = new System.Windows.Forms.TextBox();
            this.TxtPassword = new System.Windows.Forms.TextBox();
            this.LoginPicBtn = new System.Windows.Forms.PictureBox();
            this.PicBtnClose = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginPicBtn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBtnClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(457, 452);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(476, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(291, 340);
            this.label1.TabIndex = 1;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Navy;
            this.label2.Location = new System.Drawing.Point(479, 354);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Username";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Navy;
            this.label3.Location = new System.Drawing.Point(479, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Password";
            // 
            // TxtUsername
            // 
            this.TxtUsername.Location = new System.Drawing.Point(482, 369);
            this.TxtUsername.Name = "TxtUsername";
            this.TxtUsername.Size = new System.Drawing.Size(157, 20);
            this.TxtUsername.TabIndex = 4;
            // 
            // TxtPassword
            // 
            this.TxtPassword.Location = new System.Drawing.Point(482, 419);
            this.TxtPassword.Name = "TxtPassword";
            this.TxtPassword.Size = new System.Drawing.Size(157, 20);
            this.TxtPassword.TabIndex = 5;
            this.TxtPassword.UseSystemPasswordChar = true;
            // 
            // LoginPicBtn
            // 
            this.LoginPicBtn.Image = ((System.Drawing.Image)(resources.GetObject("LoginPicBtn.Image")));
            this.LoginPicBtn.Location = new System.Drawing.Point(645, 369);
            this.LoginPicBtn.Name = "LoginPicBtn";
            this.LoginPicBtn.Size = new System.Drawing.Size(100, 60);
            this.LoginPicBtn.TabIndex = 6;
            this.LoginPicBtn.TabStop = false;
            this.LoginPicBtn.Click += new System.EventHandler(this.LoginPicBtn_Click);
            // 
            // PicBtnClose
            // 
            this.PicBtnClose.Image = ((System.Drawing.Image)(resources.GetObject("PicBtnClose.Image")));
            this.PicBtnClose.Location = new System.Drawing.Point(724, 341);
            this.PicBtnClose.Name = "PicBtnClose";
            this.PicBtnClose.Size = new System.Drawing.Size(21, 22);
            this.PicBtnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PicBtnClose.TabIndex = 7;
            this.PicBtnClose.TabStop = false;
            this.PicBtnClose.Click += new System.EventHandler(this.PicBtnClose_Click);
            // 
            // LoginScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 472);
            this.ControlBox = false;
            this.Controls.Add(this.PicBtnClose);
            this.Controls.Add(this.LoginPicBtn);
            this.Controls.Add(this.TxtPassword);
            this.Controls.Add(this.TxtUsername);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.Name = "LoginScreen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginScreen";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoginPicBtn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PicBtnClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox TxtUsername;
        private System.Windows.Forms.TextBox TxtPassword;
        private System.Windows.Forms.PictureBox LoginPicBtn;
        private System.Windows.Forms.PictureBox PicBtnClose;
    }
}