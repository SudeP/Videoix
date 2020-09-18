using System;
using System.Windows.Forms;

namespace LightVersion
{
    public partial class Root : Form
    {
        public Root()
        {
            InitializeComponent();
            IEVersions._11001.Set();
            JavaScriptLoader.JavaScriptsFilePath = "Content\\JavaScripts\\";
        }
        private void Root_Load(object sender, EventArgs e)
        {
        }
    }
}