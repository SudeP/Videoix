using CsQuery;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Videoix
{
    public partial class WFMain : Form
    {
        public WFMain()
        {
            InitializeComponent();
            Initialize();
        }
        RestClient client;
        RestRequest request;
        RestResponse response;
        private System.Timers.Timer TimerForInformation, TimerForStatu, TimerForStopWatch;
        private bool isRun = false;
        Thread thread;
        int saniye = 0, remaing, loopCount, tryedMaxCount = 5, tryedCount = 0;
        string videoId, youtberId, x;
        Stopwatch stopwatch;
        CheckCoinsModal cc;
        string username, password;
        string logText = "";
        private void IsRun()
        {
            if (isRun)
            {
                stopwatch.Start();
                btnStart.Enabled = !isRun;
                thread.Start();
            }
            else
            {
                btnStart.Enabled = isRun;
                thread.Abort();
            }
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
        public void LogTT(string @string)
        {
            logText += @string + Environment.NewLine;
        }
        public void Write()
        {
            LogTT(Text);
            LogTT(tbxanalys.Text);
            LogTT($@"Bitiş Coin," + tbxCurrentCoin.Text);
            string p = Application.StartupPath + "\\Log";
            if (!Directory.Exists(p))
                Directory.CreateDirectory(p);
            string f = $@"{p}\{DateTime.Now.ToString("yyyy.MM.dd.HH.mm.ss")}.csv";
            StreamWriter streamWriter = new StreamWriter(f, true, System.Text.Encoding.GetEncoding(1254));
            streamWriter.Write(logText);
            streamWriter.Flush();
            streamWriter.Close();
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
            try
            {
                if (InvokeRequired)
                    Invoke(action);
                else
                    action.Invoke();
            }
            catch (Exception)
            {
            }
        }
        private void WFMain_Shown(object sender, EventArgs e)
        {
            stopwatch = new Stopwatch();
        }
        private void WFMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                stopwatch.Stop();
            }
            catch (Exception)
            {
            }
            try
            {
                thread.Abort();
            }
            catch (Exception)
            {

            }
            try
            {
                Write();
            }
            catch (Exception)
            {
            }
        }
        private void WFMain_Load(object sender, EventArgs e)
        {
            username = System.Configuration.ConfigurationManager.AppSettings["username"];
            password = System.Configuration.ConfigurationManager.AppSettings["password"];
            lblusername.Text += username;
            lblpassword.Text += password;
            GoLogin();
            Login();
            if (response.StatusCode == HttpStatusCode.OK)
                TimerForInformation.Enabled = true;
        }
        private void Initialize()
        {
            client = new RestClient("https://www.vidoix.com/")
            {
                UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/84.0.4147.89 Safari/537.36",
                Timeout = -1
            };


            TimerForInformation = new System.Timers.Timer()
            {
                Enabled = false,
                Interval = 5000
            };
            TimerForInformation.Elapsed += TimerForInformation_Elapsed;


            TimerForStatu = new System.Timers.Timer()
            {
                Enabled = false,
                Interval = 3000
            };
            TimerForStatu.Elapsed += TimerForStatu_Elapsed;


            TimerForStopWatch = new System.Timers.Timer()
            {
                Enabled = true,
                Interval = 250
            };
            TimerForStopWatch.Elapsed += TimerForStatu_Elapsed1;

            thread = new Thread(MainWork);
        }
        private void TimerForStatu_Elapsed1(object sender, System.Timers.ElapsedEventArgs e)
        {
            Acction(() =>
            {
                try
                {
                    Text = $@"{stopwatch.Elapsed.Hours} saat {stopwatch.Elapsed.Minutes} dakika {stopwatch.Elapsed.Seconds % 60} saniye {stopwatch.Elapsed.Milliseconds} milisaniye geçti";
                }
                catch (Exception)
                {
                }
            });
        }
        private void TimerForInformation_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Information information = GetInformation();
            if (information != null)
            {
                Acction(() =>
                {
                    if (tbxStartCoin.Text == string.Empty)
                    {
                        LogTT("Başlangıç Coin," + information.Statu.Coins);
                        LogTT($@"saniye,videoId,youtberId,x,remaing,loopCount,earnmoney,passloop");
                        tbxStartCoin.Text = information.Statu.Coins;
                    }
                    tbxCurrentCoin.Text = information.Statu.Coins;
                    tbxanalys.Text = $@"Kazanılan Coin : {int.Parse(tbxCurrentCoin.Text) - int.Parse(tbxStartCoin.Text)}";
                });
            }
        }
        private void GoLogin()
        {
            request = new RestRequest("login", Method.GET);
            Exec();
        }
        private void Login()
        {
            CQ cq = CQ.Create(response.Content);
            request = new RestRequest("login", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.9");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "navigate");
            request.AddHeader("sec-fetch-user", "?1");
            request.AddHeader("sec-fetch-dest", "document");
            request.AddHeader("referer", "https://www.vidoix.com/login");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("token", cq["input[name=\"token\"]"].Get().First().Value);
            request.AddParameter("user", username);
            request.AddParameter("password", password);
            request.AddParameter("connect", "");
            Exec();
        }
        private void GetVideos()
        {
            request = new RestRequest("c/loadVideo", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("accept", "*/*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", "https://www.vidoix.com/");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", "limit=8", ParameterType.RequestBody);
            Exec();
        }
        private Information GetInformation()
        {
            var trequest = new RestRequest("c/userCoinStatu", Method.GET);
            trequest.AddHeader("authority", "www.vidoix.com");
            trequest.AddHeader("accept", "application/json, text/javascript, */*; q=0.01");
            trequest.AddHeader("x-requested-with", "XMLHttpRequest");
            trequest.AddHeader("sec-fetch-site", "same-origin");
            trequest.AddHeader("sec-fetch-mode", "cors");
            trequest.AddHeader("sec-fetch-dest", "empty");
            trequest.AddHeader("referer", "https://www.vidoix.com/");
            trequest.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            SetCookieForRequest();
            var tresponse = client.Execute(trequest) as RestResponse;
            ControlCookieForClient();
            return tresponse.StatusCode == HttpStatusCode.OK ? Information.InformationExt.FromJson(tresponse.Content) : new Information();
        }
        private void GoVideoSite(string uri)
        {
            request = new RestRequest(uri, Method.GET);
            Exec();
        }
        private void Process(string videoId, bool isViewed = false)
        {
            request = new RestRequest("system/process", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("accept", "*/*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", $@"https://www.vidoix.com/youtube/{videoId}");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", $@"data={videoId}&view=" + (isViewed ? "viewed" : ""), ParameterType.RequestBody);
            Exec();
        }
        private void UserStatus(string youtuberId, string videoId)
        {
            request = new RestRequest("c/userFollowStat", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("accept", "*/*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", $@"https://www.vidoix.com/youtube/{videoId}");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", $@"id={youtuberId}", ParameterType.RequestBody);
            Exec();
        }
        private void CheckCoins(string youtuberId, string videoId, string x, string watch_time)
        {
            request = new RestRequest("c/checkCoins", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("accept", "*/*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", $@"https://www.vidoix.com/youtube/{videoId}");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", $@"id={youtuberId}&vidID={videoId}&x={x}&watch_time={watch_time}", ParameterType.RequestBody);
            Exec();
            cc = CheckCoinsModal.CheckCoinsModalExt.FromJson(response.Content);
        }
        private void VideoControl(string videoId)
        {
            request = new RestRequest("c/videoControls", Method.POST);
            request.AddHeader("authority", "www.vidoix.com");
            request.AddHeader("accept", "*/*");
            request.AddHeader("x-requested-with", "XMLHttpRequest");
            request.AddHeader("content-type", "application/x-www-form-urlencoded; charset=UTF-8");
            request.AddHeader("origin", "https://www.vidoix.com");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("referer", $@"https://www.vidoix.com/youtube/{videoId}");
            request.AddHeader("accept-language", "en-GB,en-US;q=0.9,en;q=0.8");
            request.AddParameter("application/x-www-form-urlencoded; charset=UTF-8", $@"controlId={videoId}", ParameterType.RequestBody);
            Exec();
        }
        private void Exec()
        {
            SetCookieForRequest();
            var rst = client.Execute(request);
            if (!(rst is null))
            {
                response = rst as RestResponse;
            }
            ControlCookieForClient();
        }
        private void SetCookieForRequest()
        {
            CookieContainer GetCookieContainer() => new CookieContainer();
            if (client.CookieContainer is null)
                client.CookieContainer = GetCookieContainer();
            var ccl = GetAllCookies(client.CookieContainer).ToList();
            foreach (Cookie cookie in ccl)
                request.AddCookie(cookie.Name, cookie.Value);
        }
        private void BtnStart_Click(object sender, EventArgs e)
        {
            isRun = true;
            IsRun();
        }
        private void BtnStop_Click(object sender, EventArgs e)
        {
            isRun = false;
            IsRun();
        }
        private void ControlCookieForClient()
        {
            var respCookies = response.Cookies.ToList();
            CookieContainer GetCookieContainer() => new CookieContainer();
            if (client.CookieContainer is null)
                client.CookieContainer = GetCookieContainer();
            var ccl = GetAllCookies(client.CookieContainer).ToList();
            for (int respCookiesIndex = 0; respCookiesIndex < respCookies.Count; respCookiesIndex++)
            {
                var rc = respCookies[respCookiesIndex];
                bool isFound = false;
                Cookie occl = new Cookie();
                for (int cclIndex = 0; cclIndex < ccl.Count; cclIndex++)
                {
                    occl = ccl[cclIndex];
                    if (rc.Name == occl.Name)
                    {
                        //response 'den gelen cookie bizde var değişiklik yapıyoruz.
                        isFound = true;
                        client.CookieContainer.Add(new Cookie()
                        {
                            Comment = rc.Comment,
                            CommentUri = rc.CommentUri,
                            Value = rc.Value,
                            Discard = rc.Discard,
                            Domain = rc.Domain,
                            Expired = rc.Expired,
                            Expires = rc.Expires,
                            HttpOnly = rc.HttpOnly,
                            Name = rc.Name,
                            Path = rc.Path,
                            Port = rc.Port,
                            Secure = rc.Secure,
                            Version = rc.Version
                        });
                        break;
                    }
                }
                if (!isFound)
                    //eğer bizdeki cookie responseda yok ise ekle localdekini geri ekle
                    client.CookieContainer.Add(occl);
            }
        }
        public IEnumerable<Cookie> GetAllCookies(CookieContainer c)
        {
            Hashtable k = (Hashtable)c.GetType().GetField("m_domainTable", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            foreach (DictionaryEntry element in k)
            {
                SortedList l = (SortedList)element.Value.GetType().GetField("m_list", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(element.Value);
                foreach (var e in l)
                {
                    var cl = (CookieCollection)((DictionaryEntry)e).Value;
                    foreach (Cookie fc in cl)
                    {
                        yield return fc;
                    }
                }
            }
        }
        private void MainWork()
        {
            while (true)
            {
                int winmoneyCount = 0, passLoopCount = 0;
                try
                {
                    Lw("Get videos");
                    GetVideos();
                    Lw("Get video OK");
                    CQ cq = CQ.Create(response.Content);
                    var link = cq["a:first"].Get().First().GetAttribute("href");
                    Lw("Go video");
                    GoVideoSite(link);
                    Lw("Go video OK");
                    ++tryedCount;
                    if (tryedCount == tryedMaxCount)
                    {
                        System.Diagnostics.Process.GetCurrentProcess().Kill();
                    }
                    if (!response.Content.Contains("Krediye göre sırala"))
                    {
                        tryedCount = 0;
                        saniye = int.Parse(Regex.Match(response.Content, @"var length = [0-9]{1,5};").Value.Replace(";", "").Replace("var length = ", ""));
                        videoId = Regex.Match(response.Content, @"var response = '[0-9]{1,10}';").Value.Replace("';", "").Replace("var response = '", "");
                        youtberId = Regex.Match(response.Content, @"profile/channel/[0-9]{1,10}").Value.Replace("profile/channel/", "");
                        x = Regex.Match(response.Content, @"x: [0-9]{1,10},").Value.Replace("x: ", "").Replace(",", "");
                        remaing = saniye % 15;
                        loopCount = saniye / 15;
                        Lw($@"

saniye:{saniye}
videoId:{videoId}
youtberId:{youtberId}
x:{x}
remaing:{remaing}
loopCount:{loopCount}

");
                        Acction(() =>
                        {
                            tbxcurrentVideo.Text = $@"

saniye:{saniye}
videoId:{videoId}
youtberId:{youtberId}
x:{x}
remaing:{remaing}
loopCount:{loopCount}

";
                        });
                        TimerForStatu.Enabled = true;
                        int getcoinwin()
                        {
                            return int.Parse(x) * 15;
                        }
                        winmoneyCount = getcoinwin();
                        ++passLoopCount;
                        VideoControl(videoId);
                        VideoControl(videoId);
                        Process(videoId, true);
                        Lw("First post OK");
                        for (int i = 0; i < loopCount; i++)
                        {
                            Lw("Wait for 15 seconds");
                            Wait(15 * 1000);
                            Lw("Wait for 15 seconds OK");
                            Lw($@"{(i + 1)}. post");
                            Process(videoId);
                            CheckCoins(youtberId, videoId, x, saniye.ToString());
                            winmoneyCount += getcoinwin();
                            ++passLoopCount;
                            if (cc.IsVideo == 1 || cc.Statu > 0)
                            {
                                Lw("Video finish");
                                break;
                            }
                            Lw($@"{(i + 1)}. post OK");
                        }
                        TimerForStatu.Enabled = false;
                    }
                }
                catch (Exception)
                {
                    TimerForStatu.Enabled = false;
                    Lw("An Error");
                }
                try
                {
                    LogTT($@"{saniye},{videoId},{youtberId},{x},{remaing},{loopCount},{winmoneyCount},{passLoopCount}");
                }
                catch (Exception)
                {
                }

                TimerForStatu.Enabled = false;
                if (!isRun)
                    break;
                Lw("Wait for 2 seconds (Next Video)");
                Wait(2000);
            }
        }
        private void TimerForStatu_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Lw("UserStatus");
            UserStatus(youtberId, videoId);
            Lw("UserStatus OK");
        }
    }
}
