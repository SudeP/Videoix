using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LightVersion
{
    public class Manage
    {
        private readonly WebBrowser wb;
        private readonly string mainUrl, youtubeUrl, username, password;
        public Manage(WebBrowser webBrowser)
        {
            youtubeUrl = System.Configuration.ConfigurationManager.AppSettings["youtubeUrl"];
            mainUrl = System.Configuration.ConfigurationManager.AppSettings["mainUrl"];
            username = System.Configuration.ConfigurationManager.AppSettings["username"];
            password = System.Configuration.ConfigurationManager.AppSettings["password"];
            wb = webBrowser;
        }
        public void Start()
        {
            wb
                .OnRedirect((args) =>
                {
                    if (args.Url.OriginalString.StartsWith(mainUrl))
                    {
                        wb.Document.LoadJavaScriptFileInHead("include");
                    }
                })
                .OnCompleted((args) =>
                {
                    if (args.Url.OriginalString.StartsWith(mainUrl))
                    {
                        wb.Document.LoadJavaScriptFileInBody("manage",
                                                               true,
                                                               "${{username}}|" + username,
                                                               "${{password}}|" + password);
                    }
                    else if (args.Url.OriginalString.StartsWith(youtubeUrl))
                    {
                        var ytPlayer = wb.Document.Window.DocumentInFrames("ytPlayer");
                        ytPlayer.LoadJavaScriptFileInBody("quality");
                        //var html = ytPlayer.GetHtml();
                    }
                })
                .Redirect(mainUrl);
        }
    }
}
