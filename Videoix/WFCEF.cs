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

namespace Videoix
{
    public partial class WFCEF : Form
    {
        private ChromiumWebBrowser cwb;
        private Thread thread;
        private string username, password, played, limunatiusername, limunatipassword;
        public WFCEF()
        {
            InitializeComponent();
            Init();
        }
        private void Init()
        {
            SetCWB();
            SetThread();
            GetAppValues();
        }
        private void GetAppValues()
        {
            username = System.Configuration.ConfigurationManager.AppSettings["username"];
            password = System.Configuration.ConfigurationManager.AppSettings["password"];
            limunatiusername = System.Configuration.ConfigurationManager.AppSettings["user"];
            limunatipassword = System.Configuration.ConfigurationManager.AppSettings["pass"];
            Text = $@"{username}:{password}";
        }
        private void SetCWB()
        {
            CefSharpSettings.LegacyJavascriptBindingEnabled = true;
            //ProxyClientBrowser("tr");
            var stt = new CefSettings();
            stt.CefCommandLineArgs["autoplay-policy"] = "no-user-gesture-required";
            Cef.Initialize(stt);
            cwb = new ChromiumWebBrowser("https://www.vidoix.com/login")
            {
                Parent = this,
                Dock = DockStyle.Fill
            };
            cwb.SendToBack();
        }
        private void SetThread()
        {
            thread = new Thread(Login);
            thread.SetApartmentState(ApartmentState.STA);
        }
        private void WFCEF_Shown(object sender, EventArgs e)
        {
            WaitComplate();
            thread.Start();
        }
        private void WFCEF_FormClosing(object sender, FormClosingEventArgs e)
        {
            cwb.Load("https://www.vidoix.com/logout");
            WaitComplate();
        }
        public void Lw(string txt)
        {
            Acction(() =>
            {
                if (tbxLog.Text.Length > 5000)
                    tbxLog.Text = string.Empty;
                TextBox log = tbxLog;
                TextBox textBox = log;
                string[] strArray = new string[7];
                strArray[0] = log.Text;
                DateTime now = DateTime.Now;
                strArray[1] = now.ToString("dd MMM");
                strArray[2] = " - ";
                now = DateTime.Now;
                strArray[3] = now.ToString("HH:mm:ss");
                strArray[4] = "  --  ";
                strArray[5] = txt;
                strArray[6] = Environment.NewLine;
                string str = string.Concat(strArray);
                textBox.Text = str;
                tbxLog.SelectionStart = tbxLog.Text.Length;
                tbxLog.ScrollToCaret();
                tbxLog.Refresh();
                Application.DoEvents();
            });
        }
        public void Wait(int sure)
        {
            DateTime dateTime = DateTime.Now.AddMilliseconds((double)sure);
            int num = 0;
            while (DateTime.Now < dateTime)
            {
                if (num % 2 == 0)
                    Application.DoEvents();
                ++num;
            }
        }
        public void Acction(Action action)
        {
            if (InvokeRequired)
                Invoke(action);
            else
                action.Invoke();
        }
        private async Task SetTrueForHasFocus()
        {
            Wait(1000);
            await cwb.EvaluateScriptAsync($@"document.hasFocus = function () {"{"}return true;{"}"}").ContinueWith((jsFirst) =>
            {
                PlayVideo();
            });
        }
        private void PlayVideo()
        {
            Wait(1000);
            cwb.EvaluateScriptAsync("playing = true; player.playVideo(); player.mute()");
        }
        private void WaitComplate()
        {
            while (true)
            {
                bool isLoading = false;
                Acction(() =>
                {
                    isLoading = cwb.IsBrowserInitialized ? !cwb.GetBrowser().IsLoading : false;
                });
                if (isLoading)
                    break;
                else
                    Wait(200);
            }
        }
        private void Login()
        {
            //login sayfasına git
            WaitComplate();//yüklenmesini bekle
            Wait(250);
            cwb.GetSourceAsync().ContinueWith((jsForwardLogin) =>//sayfa yüklenmiş mi kontrolü için
            {
                if (jsForwardLogin.Result.Contains("js-login") && jsForwardLogin.Result.Contains("Kullanıcı Adı"))//sayfa kontrolü
                    cwb.EvaluateScriptAsync($@"$('#username').val('{username}');").ContinueWith((jsUserName) =>//username doldur
                    {
                        cwb.EvaluateScriptAsync($@"$('#password').val('{password}');").ContinueWith((jsPassword) =>//password doldur
                        {
                            cwb.EvaluateScriptAsync($@"$('button[type=""submit""]').click();").ContinueWith((jsSubmit) =>//Giriş Yap 'a tıkla
                            {
                                Wait(500);
                                WaitComplate();//yüklenmesini bekle
                                Wait(500);
                                cwb.GetSourceAsync().ContinueWith((jsIslogin) =>//login olabildik mi ?
                                {
                                    if (jsIslogin.Result.Contains("Krediye göre sırala"))
                                        Loop();//olduk döngüye gir
                                    else
                                    {
                                        MessageBox.Show("Login başarısız ! Program kendini kapatıyor !");
                                        Close();//başarısız kapan
                                    }
                                });
                            });
                        });
                    });
                else
                {
                    MessageBox.Show("Login sayfasına gidilemedi ! Başarısız ! Program kendini kapatıyor !");
                    Close();//başarısız kapan
                }
            });
        }
        private void Loop()
        {
            while (true)
            {
                played = "-1";
                LoopIsLoadedVideoDiv(1).ContinueWith((finishHim) =>
                {
                    GoVideos();
                }).Wait();
            }
        }
        private void GoVideos()
        {
            cwb.Load("https://www.vidoix.com/");
            WaitComplate();
            Wait(250);
        }
        private async Task LoopIsLoadedVideoDiv(int counter)
        {
            bool isLoaded = false;
            await cwb.EvaluateScriptAsync($@"$('#loadMoreVideo a:first').length").ContinueWith((jsIsLoadedVideoDiv) =>
            {
                try
                {
                    if (jsIsLoadedVideoDiv.Result.Result.ToString() != "0")
                    {
                        //videolar yüklendi. İlk videoya git
                        isLoaded = true;
                    }
                }
                catch (Exception)
                {
                }
            }).ContinueWith((task) =>
            {
                if (isLoaded)
                {
                    GoFirstVideo().ContinueWith((finishHim) => { }).Wait();
                }
                else
                {
                    if (counter != 10)
                    {
                        Wait(1000);
                        LoopIsLoadedVideoDiv(++counter).ContinueWith((finishHim) => { }).Wait();
                    }
                }
            });
        }
        private async Task GoFirstVideo()
        {
            await cwb.EvaluateScriptAsync($@"$('#loadMoreVideo a:first').attr('href');").ContinueWith((jsFirstVideoLink) =>
            {
                cwb.Load($@"https://www.vidoix.com/{jsFirstVideoLink.Result.Result.ToString()}");
                WaitComplate();
                Wait(1000);
                StartWatchVideo().ContinueWith((finishHim) => { }).Wait();
            });
        }
        private async Task StartWatchVideo()
        {
            await SetTrueForHasFocus().ContinueWith((finishHim) =>
            {
            });
            await IsFinish().ContinueWith((task) =>
            {
            });
        }
        private async Task IsFinish()
        {
            await cwb.EvaluateScriptAsync($@"$('#other_video').css('display') === 'block'").ContinueWith((jsIsBlock) =>
            {
                if ((bool)jsIsBlock.Result.Result == false)
                {
                    cwb.EvaluateScriptAsync($@"$('div.swal2-container').length !== 0").ContinueWith((isError) =>
                    {
                        if ((bool)isError.Result.Result == false)
                        {
                            cwb.EvaluateScriptAsync("$('#played').text()").ContinueWith((isPlayed) =>
                            {
                                if (isPlayed.Result.Result.ToString() != played)
                                {
                                    played = isPlayed.Result.Result.ToString();
                                    Wait(5000);
                                    IsFinish().ContinueWith((task) => { }).Wait();
                                }
                            }).Wait();
                        }
                    }).Wait();
                }
            });
        }
        private void Button2_Click(object sender, EventArgs e)
        {
            cwb.ShowDevTools();
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            cwb.CloseDevTools();
        }
        private void Button1_Click(object sender, EventArgs e)
        {
            cwb.EvaluateScriptAsync(tbxLog.Text).ContinueWith((js) =>
            {
                Acction(() =>
                {
                    tbxLog.Text += Environment.NewLine + Newtonsoft.Json.JsonConvert.SerializeObject(js.Result);
                });
            });
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
    }
}
