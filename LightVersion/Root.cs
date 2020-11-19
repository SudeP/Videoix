using System;
using System.Windows.Forms;

namespace LightVersion
{
    public partial class Root : Form
    {
        public Root()
        {
            IEVersions._11001.Set();
            InitializeComponent();
            webBrowser = new WebBrowser
            {
                Parent = this,
                Dock = DockStyle.Fill,
                ScriptErrorsSuppressed = true
            };
            webBrowser.SendToBack();
            JavaScriptLoader.JavaScriptsFilePath = "Content\\JavaScripts\\";
            Manage manage = new Manage(webBrowser);
            Shown += (sender1, eventArgs1) =>
            {
                System.Threading.Tasks.Task.Factory.StartNew(manage.Start);
            };
        }
        private readonly WebBrowser webBrowser;

        private void button1_Click(object sender, EventArgs e)
        {
            webBrowser.Document.InvokeScript(@"window.frames[""ytPlayer""].contentWindow.postMessage('setQuality', '*');");
        }
    }
}