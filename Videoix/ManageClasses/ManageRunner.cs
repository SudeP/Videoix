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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Videoix.Classes;
using Videoix.ManageClasses;
using PDK.Tool;
using System.Diagnostics;

namespace Videoix.ManageClasses
{
    public class ManageRunner
    {
        private readonly List<string> banList;
        private readonly ChromiumWebBrowser cwb;
        private readonly WFCEF f;
        public const string baseUri = "https://www.vidoix.com/";
        public string Username { get; set; }
        public string Password { get; set; }
        public string played, currentVideoLink;
        private readonly List<string> listButtonIds = new List<string>()
        {
            "filterCoins",
            "filterSecond"
        };
        private readonly int maxCount = 8, maxCountOfRefrest = 5;
        private int lastListButtonIdIndex = -1;
        private int countOfRefrest = 0;
        public ManageRunner(WFCEF wFCEF, ChromiumWebBrowser chromiumWebBrowser, string username, string password)
        {
            Username = username;
            Password = password;
            played = "-1";
            banList = new List<string>();
            f = wFCEF;
            cwb = chromiumWebBrowser;
        }
        public void KillMe() => Process.GetCurrentProcess().Kill();
        public string GetListButtonId()
        {
            lastListButtonIdIndex = ++lastListButtonIdIndex % listButtonIds.Count;
            return listButtonIds[lastListButtonIdIndex];
        }
        public void WaitComplate()
        {
            while (true)
            {
                bool isLoading = false;
                f.InvokeRequired(() =>
                {
                    Lambda
                    .Create()
                    .Try(() =>
                    {
                        isLoading = cwb.IsBrowserInitialized ? !cwb.GetBrowser().IsLoading : false;
                    })
                    .Try(() =>
                    {
                        cwb.EvaluateScriptAsync("alert = function() { return; }");
                    })
                    .Catch((ex) =>
                    {
                        isLoading = false;
                    }, 1)
                    .Run();
                });
                if (isLoading)
                    break;
                else
                    f.me.Wait(200);
            }
        }
        public void Forward(string uri)
        {
            cwb.Load(uri);
            WaitComplate();
        }
        public void StartMethod()
        {
            Loop();
        }
        public void Loop()
        {
            while (true)
            {
                var page = WhereAmI();
                switch (page)
                {
                    case Pages.None:
                    case Pages.Capchta:
                        KillMe();
                        break;
                    case Pages.CloudFlare:
                        f.me.Wait(6 * f.me.m);
                        break;
                    case Pages.Home:
                        Forward($@"{baseUri}login");
                        Login();
                        break;
                    case Pages.Login:
                        Login();
                        break;
                    case Pages.Main:
                        Main();
                        break;
                    case Pages.Video:
                        Video();
                        break;
                }
            }
        }
        public Pages WhereAmI()
        {
            WaitComplate();

            var task = cwb.GetSourceAsync();
            task.Wait();
            var html = task.Result;

            Pages pages;
            if (html.Contains("Giriş yap") && html.Contains("Kayıt ol") && html.Contains("Kayıt ol ve kazanmaya başla") && html.Contains("Vidoix nedir ?") && html.Contains("info@vidoix.com") && html.Contains("Anasayfa") && html.Contains("Youtuber"))
                pages = Pages.Home;
            else if (html.Contains("Giriş Yap") && html.Contains("Beni hatırla") && html.Contains("Şifremi unuttum?") && html.Contains("Kullanıcı Adı"))
                pages = Pages.Login;
            else if (html.Contains("Krediye göre sırala") && html.Contains("Saniyeye göre sırala") && html.Contains(Username) && html.Contains("Oturumu kapat"))
                pages = Pages.Main;
            else if (html.Contains("Önerilen videolar") && html.Contains("ytPlayer") && html.Contains("Daha fazla video yükle") && html.Contains("kredi") && html.Contains("Takip Et") && html.Contains("İzleme Durumu"))
                pages = Pages.Video;
            else if (html.Contains("I am human") && html.Contains("Privacy") && html.Contains("Terms") && html.Contains("https://www.hcaptcha.com/privacy") && html.Contains("https://www.hcaptcha.com/terms"))
                pages = Pages.Capchta;
            else if (html.Contains("DDoS protection") && html.Contains("Please allow up to 5 seconds...") && html.Contains("This process is automatic.") && html.Contains("Checking your browser before accessing vidoix.com."))
                pages = Pages.CloudFlare;
            else
                pages = Pages.None;

            return pages;
        }
        public void Login()
        {
            var taskUsername = cwb.EvaluateScriptAsync($@"$('#username').val('{Username}');");
            taskUsername.Wait();

            var taskPassword = cwb.EvaluateScriptAsync($@"$('#password').val('{Password}');");
            taskPassword.Wait();

            var taskSubmit = cwb.EvaluateScriptAsync($@"$('button[type=""submit""]').click();");
            taskSubmit.Wait();
        }
        public void Main()
        {
            LoopIsLoadedVideoDiv(1);
        }
        public void LoopIsLoadedVideoDiv(int counter)
        {
            var isLoaded = false;

            var taskIsLoadedVideoDiv = cwb.EvaluateScriptAsync($@"$('#loadMoreVideo a:first').length");
            taskIsLoadedVideoDiv.Wait();

            Lambda
                .Create()
                .Try(() =>
                {
                    if (taskIsLoadedVideoDiv.Result.Result.ToString() != "0")
                        isLoaded = true;
                })
                .Try(() =>
                {
                    if (isLoaded)
                        SelectVideo();
                    else if (counter != 10)
                    {
                        f.me.Wait(2 * f.me.m);
                        LoopIsLoadedVideoDiv(counter);
                    }
                    else
                    {
                        f.me.Wait(10 * f.me.m);
                        Forward(baseUri);
                    }
                })
                .Run();
        }
        public void SelectVideo()
        {
            ++countOfRefrest;
            if (countOfRefrest == maxCountOfRefrest)
                KillMe();
            currentVideoLink = string.Empty;

            int index = 0;
        reGet:
            f.me.Wait(250);
            if (index == maxCount)
            {
                var id = GetListButtonId();
                var jsList = $@"$('#{id}').click()";
                var taskRefreshVideos = cwb.EvaluateScriptAsync(jsList);
                taskRefreshVideos.Wait();
                return;
            }
            var js = $"$($('#loadMoreVideo div a:contains(\"önce\")')[{index}]).attr('href')";
            var taskFirstVideoLink = cwb.EvaluateScriptAsync(js);
            taskFirstVideoLink.Wait();

            currentVideoLink = $@"{baseUri}{taskFirstVideoLink.Result.Result.ToString()}";

            if (banList.Contains(currentVideoLink))
            {
                ++index;
                goto reGet;
            }

            countOfRefrest = 0;

            Forward(currentVideoLink);
        }
        public void Video()
        {
            SetTrueForHasFocus();
            VideoIsLoaded();
            Forward(baseUri);
        }
        public void SetTrueForHasFocus()
        {
            var taskHasFocus = cwb.EvaluateScriptAsync($@"document.hasFocus = function () {"{"}return true;{"}"}");
            taskHasFocus.Wait();
        }
        public void VideoIsLoaded()
        {
            int counter = 0;
            while (true)
            {
                ++counter;

                PlayVideo();

                var taskIsLoaded = cwb.EvaluateScriptAsync($@"player.getPlayerState()");
                taskIsLoaded.Wait();

                if (taskIsLoaded.Result != null && taskIsLoaded.Result.Result != null && taskIsLoaded.Result.Result.ToString() == "1")
                {
                    IsFinish();
                    return;
                }
                else
                {
                    if (counter != 20)
                        f.me.Wait(2000);
                    else
                        break;
                }
            }
            banList.Add(currentVideoLink);
        }
        public void PlayVideo()
        {
            var taskPlayVideo = cwb.EvaluateScriptAsync("playing = true; player.playVideo(); player.mute()");
            taskPlayVideo.Wait();
        }
        public void IsFinish()
        {
            var taskIsBlock = cwb.EvaluateScriptAsync($@"$('#other_video').css('display') === 'block'");
            taskIsBlock.Wait();
            if ((bool)taskIsBlock.Result.Result == false)
            {
                var taskIsError = cwb.EvaluateScriptAsync($@"$('div.swal2-container').length !== 0");
                taskIsError.Wait();
                if ((bool)taskIsError.Result.Result == false)
                {
                    var taskIsPlayed = cwb.EvaluateScriptAsync("$('#played').text()");
                    taskIsPlayed.Wait();
                    if (taskIsPlayed.Result.Result.ToString() != played)
                    {
                        played = taskIsPlayed.Result.Result.ToString();
                        f.me.Wait(5000);
                        IsFinish();
                    }
                }
            }
        }
        [Obsolete]
        public void Information()
        {
            var cm = cwb.GetCookieManager();
            var visitor = new CookieMonster();
            if (cm.VisitAllCookies(visitor))
                visitor.WaitForAllCookies();
            var sb = new StringBuilder();
            foreach (var nameValue in visitor.NamesValues)
                sb.AppendLine(nameValue.Item1 + " = " + nameValue.Item2);
            MessageBox.Show(sb.ToString());
        }
    }
}