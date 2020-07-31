﻿using CefSharp;
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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Videoix.Classes;
using Videoix.ManageClasses;
using PDK.Tool;

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
        public ManageRunner(WFCEF wFCEF, ChromiumWebBrowser chromiumWebBrowser, string username, string password)
        {
            Username = username;
            Password = password;
            played = "-1";
            banList = new List<string>();
            f = wFCEF;
            cwb = chromiumWebBrowser;
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
                    .Catch((ex) =>
                    {
                        isLoading = false;
                    })
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
                        f.me.Wait(90 * f.me.m);
                        break;
                    case Pages.Capchta:
                        //not set
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
                    default:
                        break;
                }
            }
        }
        public Pages WhereAmI()
        {
            WaitComplate();

            f.ltb.Log("Were Am I ?");

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
            else
                pages = Pages.None;

            f.ltb.Log("I am from " + pages.ToString());

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

            f.ltb.Log("Request Login");
        }
        public void Main()
        {
            Information();
            f.ltb.Log("Finding Video");
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
                        f.ltb.Log("Counter : " + ++counter);
                        f.me.Wait(250);
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
            currentVideoLink = string.Empty;

            int index = 0;

        reGet:
            var js = $"$($('#loadMoreVideo div a:contains(\"önce\")')[{index}]).attr('href')";
            var taskFirstVideoLink = cwb.EvaluateScriptAsync(js);
            taskFirstVideoLink.Wait();

            currentVideoLink = $@"{baseUri}{taskFirstVideoLink.Result.Result.ToString()}";

            f.ltb.Log("Link : " + currentVideoLink);


            if (banList.Contains(currentVideoLink))
            {
                ++index;
                goto reGet;
            }

            f.ltb.Log("Found video");

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
            f.ltb.Log("Set hasFocus");
        }
        public void VideoIsLoaded()
        {
            int counter = 0;
            while (true)
            {
                PlayVideo();

                var taskIsLoaded = cwb.EvaluateScriptAsync($@"player.getPlayerState()");
                taskIsLoaded.Wait();

                if (taskIsLoaded.Result.Result != null && taskIsLoaded.Result.Result.ToString() == "1")
                {
                    f.ltb.Log("Video Loaded");
                    IsFinish();
                    return;
                }
                else
                {
                    f.ltb.Log("Not played. counter : " + ++counter);
                    if (counter != 10)
                        f.me.Wait(1000);
                    else
                        break;
                }
            }
            f.ltb.Log("Video unloaded");
            banList.Add(currentVideoLink);
        }
        public void PlayVideo()
        {
            var taskPlayVideo = cwb.EvaluateScriptAsync("playing = true; player.playVideo(); player.mute()");
            taskPlayVideo.Wait();
            f.ltb.Log("Play video");
        }
        public void IsFinish()
        {
            f.ltb.Log("Is finish Video ?");
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
                        f.ltb.Log("not finish");
                        played = taskIsPlayed.Result.Result.ToString();
                        f.me.Wait(5000);
                        IsFinish();
                    }
                }
            }
            f.ltb.Log("Video Finish");
        }
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
    class CookieMonster : ICookieVisitor
    {
        readonly List<Tuple<string, string>> cookies = new List<Tuple<string, string>>();
        readonly ManualResetEvent gotAllCookies = new ManualResetEvent(false);

        public bool Visit(Cookie cookie, int count, int total, ref bool deleteCookie)
        {
            cookies.Add(new Tuple<string, string>(cookie.Name, cookie.Value));

            if (count == total - 1)
                gotAllCookies.Set();

            return true;
        }

        public void WaitForAllCookies()
        {
            gotAllCookies.WaitOne();
        }

        public void Dispose()
        {

        }

        public IEnumerable<Tuple<string, string>> NamesValues
        {
            get { return cookies; }
        }
    }
    public enum Pages
    {
        None,
        Capchta,
        Home,
        Login,
        Main,
        Video
    }
}