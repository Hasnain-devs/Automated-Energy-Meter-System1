namespace IOTController
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lbAnalogMeter1 = new LBSoft.IndustrialCtrls.Meters.LBAnalogMeter();
            this.BtnStartStop = new System.Windows.Forms.Button();
            this.BtnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.DrpCustomerList = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.BtnGenerate = new System.Windows.Forms.Button();
            this.BtnService = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbAnalogMeter1
            // 
            this.lbAnalogMeter1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbAnalogMeter1.AutoSize = true;
            this.lbAnalogMeter1.BodyColor = System.Drawing.Color.LightYellow;
            this.lbAnalogMeter1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbAnalogMeter1.ForeColor = System.Drawing.Color.Coral;
            this.lbAnalogMeter1.Location = new System.Drawing.Point(698, 36);
            this.lbAnalogMeter1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.lbAnalogMeter1.MaxValue = 25D;
            this.lbAnalogMeter1.MeterStyle = LBSoft.IndustrialCtrls.Meters.LBAnalogMeter.AnalogMeterStyle.Circular;
            this.lbAnalogMeter1.MinValue = 0D;
            this.lbAnalogMeter1.Name = "lbAnalogMeter1";
            this.lbAnalogMeter1.NeedleColor = System.Drawing.Color.Red;
            this.lbAnalogMeter1.Renderer = null;
            this.lbAnalogMeter1.ScaleColor = System.Drawing.Color.DarkRed;
            this.lbAnalogMeter1.ScaleDivisions = 6;
            this.lbAnalogMeter1.ScaleSubDivisions = 10;
            this.lbAnalogMeter1.Size = new System.Drawing.Size(212, 188);
            this.lbAnalogMeter1.TabIndex = 2;
            this.lbAnalogMeter1.Value = 0D;
            this.lbAnalogMeter1.ViewGlass = true;
            // 
            // BtnStartStop
            // 
            this.BtnStartStop.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnStartStop.Image = ((System.Drawing.Image)(resources.GetObject("BtnStartStop.Image")));
            this.BtnStartStop.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.BtnStartStop.Location = new System.Drawing.Point(12, 12);
            this.BtnStartStop.Name = "BtnStartStop";
            this.BtnStartStop.Size = new System.Drawing.Size(328, 157);
            this.BtnStartStop.TabIndex = 0;
            this.BtnStartStop.Text = "Start service";
            this.BtnStartStop.UseVisualStyleBackColor = true;
            this.BtnStartStop.Click += new System.EventHandler(this.BtnStartStop_Click);
            // 
            // BtnExit
            // 
            this.BtnExit.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnExit.BackgroundImage")));
            this.BtnExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnExit.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnExit.Location = new System.Drawing.Point(13, 176);
            this.BtnExit.Name = "BtnExit";
            this.BtnExit.Size = new System.Drawing.Size(327, 87);
            this.BtnExit.TabIndex = 1;
            this.BtnExit.Text = "Exit ";
            this.BtnExit.UseVisualStyleBackColor = true;
            this.BtnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(367, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(297, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Apply readings for the selected customer";
            // 
            // DrpCustomerList
            // 
            this.DrpCustomerList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.DrpCustomerList.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DrpCustomerList.FormattingEnabled = true;
            this.DrpCustomerList.Location = new System.Drawing.Point(371, 36);
            this.DrpCustomerList.Name = "DrpCustomerList";
            this.DrpCustomerList.Size = new System.Drawing.Size(292, 28);
            this.DrpCustomerList.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(371, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(293, 74);
            this.label2.TabIndex = 4;
            this.label2.Text = resources.GetString("label2.Text");
            // 
            // BtnGenerate
            // 
            this.BtnGenerate.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnGenerate.BackgroundImage")));
            this.BtnGenerate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.BtnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnGenerate.Location = new System.Drawing.Point(374, 176);
            this.BtnGenerate.Name = "BtnGenerate";
            this.BtnGenerate.Size = new System.Drawing.Size(288, 86);
            this.BtnGenerate.TabIndex = 5;
            this.BtnGenerate.Text = "Single touch Gen Bill";
            this.BtnGenerate.UseVisualStyleBackColor = true;
            this.BtnGenerate.Click += new System.EventHandler(this.BtnGenerate_Click);
            // 
            // BtnService
            // 
            this.BtnService.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnService.BackgroundImage")));
            this.BtnService.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnService.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World, ((byte)(0)));
            this.BtnService.Location = new System.Drawing.Point(355, 468);
            this.BtnService.Name = "BtnService";
            this.BtnService.Size = new System.Drawing.Size(61, 31);
            this.BtnService.TabIndex = 6;
            this.BtnService.Text = "On/Off";
            this.BtnService.UseVisualStyleBackColor = true;
            this.BtnService.Click += new System.EventHandler(this.BtnService_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(668, 242);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(251, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "Live consumption view ( Upated once 1 min )";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(923, 275);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.BtnService);
            this.Controls.Add(this.BtnGenerate);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DrpCustomerList);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BtnExit);
            this.Controls.Add(this.BtnStartStop);
            this.Controls.Add(this.lbAnalogMeter1);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "IOT Auto updater";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnStartStop;
        private System.Windows.Forms.Button BtnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox DrpCustomerList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button BtnGenerate;
        private System.Windows.Forms.Button BtnService;
        private LBSoft.IndustrialCtrls.Meters.LBAnalogMeter lbAnalogMeter1;
        private System.Windows.Forms.Label label3;
    }
}

