namespace Videoix
{
    partial class WFMain
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
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tbxanalys = new System.Windows.Forms.TextBox();
            this.tbxStartCoin = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbxCurrentCoin = new System.Windows.Forms.TextBox();
            this.tbxcurrentVideo = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbxLog
            // 
            this.tbxLog.Font = new System.Drawing.Font("Consolas", 11F);
            this.tbxLog.Location = new System.Drawing.Point(16, 17);
            this.tbxLog.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ReadOnly = true;
            this.tbxLog.Size = new System.Drawing.Size(883, 527);
            this.tbxLog.TabIndex = 0;
            // 
            // btnStart
            // 
            this.btnStart.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.btnStart.Location = new System.Drawing.Point(908, 17);
            this.btnStart.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(293, 69);
            this.btnStart.TabIndex = 1;
            this.btnStart.Text = "Başla";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.BtnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Enabled = false;
            this.btnStop.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Bold);
            this.btnStop.Location = new System.Drawing.Point(908, 94);
            this.btnStop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(293, 69);
            this.btnStop.TabIndex = 2;
            this.btnStop.Text = "Durdur";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // tbxanalys
            // 
            this.tbxanalys.Font = new System.Drawing.Font("Consolas", 11F);
            this.tbxanalys.Location = new System.Drawing.Point(908, 235);
            this.tbxanalys.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tbxanalys.Multiline = true;
            this.tbxanalys.Name = "tbxanalys";
            this.tbxanalys.ReadOnly = true;
            this.tbxanalys.Size = new System.Drawing.Size(292, 156);
            this.tbxanalys.TabIndex = 3;
            // 
            // tbxStartCoin
            // 
            this.tbxStartCoin.Location = new System.Drawing.Point(910, 203);
            this.tbxStartCoin.Name = "tbxStartCoin";
            this.tbxStartCoin.ReadOnly = true;
            this.tbxStartCoin.Size = new System.Drawing.Size(122, 25);
            this.tbxStartCoin.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(906, 182);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Başlangıç : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1059, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 18);
            this.label2.TabIndex = 7;
            this.label2.Text = "Şuan : ";
            // 
            // tbxCurrentCoin
            // 
            this.tbxCurrentCoin.Location = new System.Drawing.Point(1062, 203);
            this.tbxCurrentCoin.Name = "tbxCurrentCoin";
            this.tbxCurrentCoin.ReadOnly = true;
            this.tbxCurrentCoin.Size = new System.Drawing.Size(122, 25);
            this.tbxCurrentCoin.TabIndex = 6;
            // 
            // tbxcurrentVideo
            // 
            this.tbxcurrentVideo.Font = new System.Drawing.Font("Consolas", 11F);
            this.tbxcurrentVideo.Location = new System.Drawing.Point(907, 399);
            this.tbxcurrentVideo.Margin = new System.Windows.Forms.Padding(4);
            this.tbxcurrentVideo.Multiline = true;
            this.tbxcurrentVideo.Name = "tbxcurrentVideo";
            this.tbxcurrentVideo.ReadOnly = true;
            this.tbxcurrentVideo.Size = new System.Drawing.Size(292, 145);
            this.tbxcurrentVideo.TabIndex = 8;
            // 
            // WFMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1217, 562);
            this.Controls.Add(this.tbxcurrentVideo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbxCurrentCoin);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbxStartCoin);
            this.Controls.Add(this.tbxanalys);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.tbxLog);
            this.Font = new System.Drawing.Font("Consolas", 11F);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "WFMain";
            this.Text = "WFMain";
            this.Load += new System.EventHandler(this.WFMain_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxLog;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox tbxanalys;
        private System.Windows.Forms.TextBox tbxStartCoin;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbxCurrentCoin;
        private System.Windows.Forms.TextBox tbxcurrentVideo;
    }
}