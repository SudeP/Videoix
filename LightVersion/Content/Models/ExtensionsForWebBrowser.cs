using mshtml;
using System;
using System.Windows.Forms;

public static class ExtensionsForWebBrowser
{
    public static WebBrowser Redirect(this WebBrowser browser, string url)
        => browser.NotNull(() =>
        {
            browser.Navigate(url);
        });
    public static WebBrowser Completed(this WebBrowser browser, Action<WebBrowserDocumentCompletedEventArgs> action)
        => browser.NotNull(() =>
        {
            void handler(object sender, WebBrowserDocumentCompletedEventArgs args)
            {
                action.Invoke(args);
                browser.DocumentCompleted -= handler;
            }
            browser.DocumentCompleted += handler;
        });
    public static WebBrowser NotNull(this WebBrowser browser, Action action)
    {
        if (browser != null)
            action.Invoke();
        return browser;
    }
    public static void Set(this IEVersions iEVersions)
    {
        var procName = System.Diagnostics.Process.GetCurrentProcess().ProcessName;
        WebBrowserHelper.FixBrowserVersion();
        WebBrowserHelper.FixBrowserVersion(procName);
        WebBrowserHelper.FixBrowserVersion(procName, (int)iEVersions);
    }
    public static void JavaScriptExecute(this HtmlDocument doc, string js)
    {
        HtmlElement head = doc.GetElementsByTagName("head")[0];
        HtmlElement scriptEl = doc.CreateElement("script");
        IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
        element.text = js;
        head.AppendChild(scriptEl);
    }
    public static HtmlWindowCollection Frames(this WebBrowser webBrowser)
        => webBrowser.Document.Window.Frames;
    public static HtmlDocument DocumentInFrames(this HtmlWindow htmlWindow, int index)
        => htmlWindow.Frames[index].GetDocument();
    public static HtmlDocument DocumentInFrames(this HtmlWindow htmlWindow, string name)
        => htmlWindow.Frames[name].GetDocument();
    public static HtmlDocument DocumentInFrames(this WebBrowser webBrowser, int index)
       => index < 0 ?
        webBrowser.Frames()[webBrowser.Frames().Count - index].GetDocument()
        :
        webBrowser.Frames()[index].GetDocument();
    public static HtmlDocument DocumentInFrames(this WebBrowser webBrowser, string name)
       => webBrowser.Frames()[name].GetDocument();
}