namespace Videoix
{
    partial class WFCEF
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
            this.tbxJs = new System.Windows.Forms.TextBox();
            this.btnExecuteJS = new System.Windows.Forms.Button();
            this.btnDevOpen = new System.Windows.Forms.Button();
            this.btnDevClose = new System.Windows.Forms.Button();
            this.btnRestart = new System.Windows.Forms.Button();
            this.pConsole = new System.Windows.Forms.Panel();
            this.formStrip = new System.Windows.Forms.MenuStrip();
            this.konsoluAçToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.logAçKapaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pLog = new System.Windows.Forms.Panel();
            this.tbxLog = new System.Windows.Forms.TextBox();
            this.pConsole.SuspendLayout();
            this.formStrip.SuspendLayout();
            this.pLog.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxJs
            // 
            this.tbxJs.Location = new System.Drawing.Point(3, 3);
            this.tbxJs.Multiline = true;
            this.tbxJs.Name = "tbxJs";
            this.tbxJs.Size = new System.Drawing.Size(237, 198);
            this.tbxJs.TabIndex = 0;
            // 
            // btnExecuteJS
            // 
            this.btnExecuteJS.Location = new System.Drawing.Point(246, 3);
            this.btnExecuteJS.Name = "btnExecuteJS";
            this.btnExecuteJS.Size = new System.Drawing.Size(104, 46);
            this.btnExecuteJS.TabIndex = 1;
            this.btnExecuteJS.Text = "Execute JS";
            this.btnExecuteJS.UseVisualStyleBackColor = true;
            this.btnExecuteJS.Click += new System.EventHandler(this.BtnExecuteJS_Click);
            // 
            // btnDevOpen
            // 
            this.btnDevOpen.Location = new System.Drawing.Point(246, 55);
            this.btnDevOpen.Name = "btnDevOpen";
            this.btnDevOpen.Size = new System.Drawing.Size(104, 46);
            this.btnDevOpen.TabIndex = 2;
            this.btnDevOpen.Text = "Dev Aç";
            this.btnDevOpen.UseVisualStyleBackColor = true;
            this.btnDevOpen.Click += new System.EventHandler(this.BtnDevOpen_Click);
            // 
            // btnDevClose
            // 
            this.btnDevClose.Location = new System.Drawing.Point(246, 107);
            this.btnDevClose.Name = "btnDevClose";
            this.btnDevClose.Size = new System.Drawing.Size(104, 46);
            this.btnDevClose.TabIndex = 3;
            this.btnDevClose.Text = "Dev Kapa";
            this.btnDevClose.UseVisualStyleBackColor = true;
            this.btnDevClose.Click += new System.EventHandler(this.BtnDevClose_Click);
            // 
            // btnRestart
            // 
            this.btnRestart.Location = new System.Drawing.Point(246, 159);
            this.btnRestart.Name = "btnRestart";
            this.btnRestart.Size = new System.Drawing.Size(104, 46);
            this.btnRestart.TabIndex = 4;
            this.btnRestart.Text = "Baştan Başla";
            this.btnRestart.UseVisualStyleBackColor = true;
            this.btnRestart.Click += new System.EventHandler(this.BtnRestart_Click);
            // 
            // pConsole
            // 
            this.pConsole.Controls.Add(this.tbxJs);
            this.pConsole.Controls.Add(this.btnRestart);
            this.pConsole.Controls.Add(this.btnExecuteJS);
            this.pConsole.Controls.Add(this.btnDevClose);
            this.pConsole.Controls.Add(this.btnDevOpen);
            this.pConsole.Location = new System.Drawing.Point(0, 27);
            this.pConsole.Name = "pConsole";
            this.pConsole.Size = new System.Drawing.Size(355, 212);
            this.pConsole.TabIndex = 6;
            this.pConsole.Visible = false;
            // 
            // formStrip
            // 
            this.formStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.konsoluAçToolStripMenuItem,
            this.logAçKapaToolStripMenuItem});
            this.formStrip.Location = new System.Drawing.Point(0, 0);
            this.formStrip.Name = "formStrip";
            this.formStrip.Size = new System.Drawing.Size(1119, 24);
            this.formStrip.TabIndex = 7;
            this.formStrip.Text = "menuStrip1";
            // 
            // konsoluAçToolStripMenuItem
            // 
            this.konsoluAçToolStripMenuItem.Name = "konsoluAçToolStripMenuItem";
            this.konsoluAçToolStripMenuItem.Size = new System.Drawing.Size(110, 20);
            this.konsoluAçToolStripMenuItem.Text = "Konsolu Aç/Kapa";
            this.konsoluAçToolStripMenuItem.Click += new System.EventHandler(this.KonsoluAcToolStripMenuItem_Click);
            // 
            // logAçKapaToolStripMenuItem
            // 
            this.logAçKapaToolStripMenuItem.Name = "logAçKapaToolStripMenuItem";
            this.logAçKapaToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.logAçKapaToolStripMenuItem.Text = "Log Aç/Kapa";
            this.logAçKapaToolStripMenuItem.Click += new System.EventHandler(this.LogAcKapaToolStripMenuItem_Click);
            // 
            // pLog
            // 
            this.pLog.Controls.Add(this.tbxLog);
            this.pLog.Location = new System.Drawing.Point(3, 245);
            this.pLog.Name = "pLog";
            this.pLog.Size = new System.Drawing.Size(662, 495);
            this.pLog.TabIndex = 7;
            this.pLog.Visible = false;
            // 
            // tbxLog
            // 
            this.tbxLog.BackColor = System.Drawing.Color.Black;
            this.tbxLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbxLog.ForeColor = System.Drawing.Color.White;
            this.tbxLog.Location = new System.Drawing.Point(0, 0);
            this.tbxLog.Multiline = true;
            this.tbxLog.Name = "tbxLog";
            this.tbxLog.ReadOnly = true;
            this.tbxLog.Size = new System.Drawing.Size(662, 495);
            this.tbxLog.TabIndex = 0;
            // 
            // WFCEF
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1119, 864);
            this.Controls.Add(this.pLog);
            this.Controls.Add(this.pConsole);
            this.Controls.Add(this.formStrip);
            this.MainMenuStrip = this.formStrip;
            this.Name = "WFCEF";
            this.Text = "WFCEF";
            this.pConsole.ResumeLayout(false);
            this.pConsole.PerformLayout();
            this.formStrip.ResumeLayout(false);
            this.formStrip.PerformLayout();
            this.pLog.ResumeLayout(false);
            this.pLog.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxJs;
        private System.Windows.Forms.Button btnExecuteJS;
        private System.Windows.Forms.Button btnDevOpen;
        private System.Windows.Forms.Button btnDevClose;
        private System.Windows.Forms.Button btnRestart;
        private System.Windows.Forms.Panel pConsole;
        private System.Windows.Forms.MenuStrip formStrip;
        private System.Windows.Forms.ToolStripMenuItem konsoluAçToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem logAçKapaToolStripMenuItem;
        private System.Windows.Forms.Panel pLog;
        private System.Windows.Forms.TextBox tbxLog;
    }
}