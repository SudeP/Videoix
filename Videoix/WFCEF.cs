using CefSharp;
using CefSharp.WinForms;
using CefSharp.Callback;
using CefSharp.Enums;
using CefSharp.Event;
using CefSharp.Handler;
using CefSharp.Internals;
using CefSharp.Lagacy;
using CefSharp.ModelBinding;
using CefSharp.RenderProcess;
using CefSharp.ResponseFilter;
using CefSharp.SchemeHandler;
using CefSharp.Structs;
using CefSharp.Web;
using CefSharp.WinForms.Internals;
using RestSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Videoix.Classes;
using Videoix.ManageClasses;
using PDK.Tool;

namespace Videoix
{
    public partial class WFCEF : Form
    {
        public WFCEF()
        {
            InitializeComponent();
            Init();
        }
        public string username, password, limunatiusername, limunatipassword;
        public ChromiumWebBrowser cwb;
        public ManageThread mt;
        public ManageSubscriber ms;
        public ManageRunner mr;
        public ManageEnvrionment me;
        public LogTextBox ltb;
        public virtual void Init()
        {
            GetAppValues();
            SetEvents();
            //ProxyClientBrowser("tr");
            SetCWB();
            SetManages();
        }
        private void SetManages()
        {
            ltb = LogTextBox.Create(tbxLog);
            mr = new ManageRunner(this, cwb, username, password);
            mt = ManageThread.Create(mr.StartMethod);
            ms = ManageSubscriber.Create();
            me = new ManageEnvrionment(this);
        }
        private void SetEvents()
        {
            FormClosing += new FormClosingEventHandler(WFCEF_FormClosing);
            Shown += new EventHandler(WFCEF_Shown);
        }
        private void GetAppValues()
        {
            username = System.Configuration.ConfigurationManager.AppSettings["username"];
            password = System.Configuration.ConfigurationManager.AppSettings["password"];
            limunatiusername = System.Configuration.ConfigurationManager.AppSettings["limunatiusername"];
            limunatipassword = System.Configuration.ConfigurationManager.AppSettings["limunatipassword"];
            Text = $@"{username}:{password}";
        }
        private void SetCWB()
        {
            SetCWBSettings();
            cwb = new ChromiumWebBrowser(ManageRunner.baseUri)
            {
                Parent = this,
                Dock = DockStyle.Fill
            };
            cwb.SendToBack();
        }
        private void SetCWBSettings()
        {
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            var stt = new CefSettings();
            stt.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(stt);
        }
        public void ProxyClientBrowser(string countryCode)
        {
            int port = 22225;

            string countryInfo = (countryCode != null ? "-country-" + countryCode.ToLower() : "");
            string sessionInfo = "-session-" + new Random().Next().ToString();

            string username = limunatiusername + countryInfo;
            string password = limunatipassword;

            string login = username + sessionInfo;

            CefSharpSettings.Proxy = new ProxyOptions("zproxy.luminati.io", port.ToString(), login, password);
        }
        private void WFCEF_Shown(object sender, EventArgs e) => mt.Start();
        private void WFCEF_FormClosing(object sender, FormClosingEventArgs e)
        {
            Lambda
                .Create()
                .Try(() =>
                {
                    mt.Abort();
                })
                .Try(() =>
                {
                    cwb.Load($@"{ManageRunner.baseUri}logout");
                })
                .Try(() =>
                {
                    me.Wait(500);
                })
                .Run();
        }
        private void BtnDevOpen_Click(object sender, EventArgs e) => cwb.ShowDevTools();
        private void BtnDevClose_Click(object sender, EventArgs e) => cwb.CloseDevTools();
        private void LogAcKapaToolStripMenuItem_Click(object sender, EventArgs e) => pLog.Visible = !pLog.Visible;
        private void KonsoluAcToolStripMenuItem_Click(object sender, EventArgs e) => pConsole.Visible = !pConsole.Visible;
        private void BtnExecuteJS_Click(object sender, EventArgs e) => cwb.EvaluateScriptAsync(tbxJs.Text).ContinueWith((js) =>
        {
            me.InvokeRequired(() =>
            {
                tbxJs.Text += Environment.NewLine + "Result : " + Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(js.Result);
            });
        });
        private void BtnRestart_Click(object sender, EventArgs e)
        {

        }
    }
}
