using CefSharp;
using CefSharp.WinForms;
using System;
using System.IO;
using System.Windows.Forms;

namespace VidoixOnlyJavaScript
{
    public partial class Runner : Form
    {
        public Runner()
        {
            InitializeComponent();
        }
        ChromiumWebBrowser cwb;
        readonly string baseUri = @"https://www.vidoix.com/";
        string isdebug;
        private void Runner_Load(object sender, EventArgs e)
        {
            isdebug = System.Configuration.ConfigurationManager.AppSettings["isdebug"];
            Text = System.Configuration.ConfigurationManager.AppSettings["title"];

            if (isdebug == "1")
                Size = new System.Drawing.Size(1000, 600);
            else
                Size = new System.Drawing.Size(400, 400);

            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            var stt = new CefSettings();
            stt.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(stt);
            cwb = new ChromiumWebBrowser(baseUri)
            {
                Parent = this,
                Dock = DockStyle.Fill
            };
            cwb.FrameLoadEnd += Cwb_FrameLoadEnd;
            cwb.IsBrowserInitializedChanged += Cwb_IsBrowserInitializedChanged;
            cwb.ConsoleMessage += Cwb_ConsoleMessage;
        }
        private void Cwb_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            if (e.Level != LogSeverity.Info)
                return;

            if (e.Message.StartsWith("//--setQualityToBestLowerQuality"))
            {
                var names = cwb.GetBrowser().GetFrameNames();
                int index = 0;
                while (true)
                {
                    var frame = cwb.GetBrowser().GetFrame(names[index]);
                    if (frame.Url.StartsWith("https://www.youtube.com/"))
                    {
                        frame.EvaluateScriptAsync($@"
var script = document.createElement('script');
script.text = `
{File.ReadAllText("quality.js")
    .Replace("`", @"\`")
    .Replace("${", @"\${")
    .Replace((isdebug == "0" ? "cw(doc" : "1"), (isdebug == "0" ? "//cw(doc" : "1"))}
`;
document.querySelector('body').appendChild(script);").ContinueWith((rst) =>
                                    {
                                        if (rst.Result.Success == false)
                                            Cwb_ConsoleMessage(sender, e);
                                    });
                        GC.Collect();
                        break;
                    }
                    ++index;
                    if (index >= names.Count)
                        break;
                }
            }
        }
        private void Cwb_IsBrowserInitializedChanged(object sender, EventArgs e)
        {
            if (isdebug == "1")
            {
                if (cwb.IsBrowserInitialized)
                {
                    cwb.ShowDevTools();
                }
            }
        }
        private void Cwb_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            if (e.Url.StartsWith(baseUri))
                InsertManage();
        }
        private void InsertManage()
        {
            cwb.ExecuteScriptAsync("var jqueryscript = document.createElement('script'); jqueryscript.src = \"https://cdnjs.cloudflare.com/ajax/libs/jquery/3.5.1/jquery.min.js\";jqueryscript.type=\"text/javascript\";document.body.appendChild(jqueryscript);");
            cwb.EvaluateScriptAsync($@"
var script = document.createElement('script');
script.text = `
{File.ReadAllText("manage.js")
    .Replace("${{username}}", System.Configuration.ConfigurationManager.AppSettings["username"])
    .Replace("${{password}}", System.Configuration.ConfigurationManager.AppSettings["password"])
    .Replace("`", @"\`")
    .Replace("${", @"\${")
    .Replace((isdebug == "0" ? "cw(\"" : "1"), (isdebug == "0" ? "//cw(\"" : "1"))}
`;
jQuery('body').append(script)").ContinueWith((rst) =>
    {
        if (rst.Result.Success == false)
            InsertManage();
    });
            GC.Collect();
        }
    }
}
