using mshtml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

public static class ExtensionsForWebBrowser
{
    public static WebBrowser Redirect(this WebBrowser browser, string url)
        => browser.NotNull(() =>
        {
            browser.Navigate(url);
        });
    public static WebBrowser OnRedirect(this WebBrowser browser, Action<WebBrowserNavigatedEventArgs> action)
        => browser.NotNull(() =>
        {
            void handler(object sender, WebBrowserNavigatedEventArgs args)
            {
                action.Invoke(args);
            }
            browser.Navigated += handler;
        });
    public static WebBrowser OnCompleted(this WebBrowser browser, Action<WebBrowserDocumentCompletedEventArgs> action)
        => browser.NotNull(() =>
        {
            void handler(object sender, WebBrowserDocumentCompletedEventArgs args)
            {
                action.Invoke(args);
            }
            browser.DocumentCompleted += handler;
        });
    public static WebBrowser OnError(this WebBrowser browser, Action<HtmlElementErrorEventArgs> action)
        => browser.NotNull(() =>
        {
            void handler(object sender, HtmlElementErrorEventArgs args)
            {
                action.Invoke(args);
            }
            if (browser != null && browser.Document != null && browser.Document.Window != null)
                browser.Document.Window.Error += handler;
        });
    public static WebBrowser NotNull(this WebBrowser browser, Action action)
    {
        if (browser != null)
            action.Invoke();
        return browser;
    }
    public static void JavaScriptExecuteInHead(this HtmlDocument doc, string js)
    {
        HtmlElement head = doc.GetElementsByTagName("head")[0];
        HtmlElement scriptEl = doc.CreateElement("script");
        IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
        element.text = js;
        head.AppendChild(scriptEl);
    }
    public static void JavaScriptExecuteInBody(this HtmlDocument doc, string js)
    {
        HtmlElement body = doc.GetElementsByTagName("body")[0];
        HtmlElement scriptEl = doc.CreateElement("script");
        IHTMLScriptElement element = (IHTMLScriptElement)scriptEl.DomElement;
        element.text = js;
        body.AppendChild(scriptEl);
    }
    public static List<HtmlWindow> Frames(this WebBrowser webBrowser)
        => webBrowser.Document.Window.Frames.Cast<HtmlWindow>().ToList();
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
       => webBrowser.Document.Window.Frames[name].GetDocument();
    public static string GetHead(this WebBrowser webBrowser) => webBrowser.Document.GetElementsByTagName("head")[0].OuterHtml;
    public static string GetBody(this WebBrowser webBrowser) => webBrowser.Document.GetElementsByTagName("body")[0].OuterHtml;
    public static string GetHtml(this WebBrowser webBrowser) => webBrowser.Document.GetElementsByTagName("html")[0].OuterHtml;
    public static string GetHead(this HtmlWindow htmlWindow) => htmlWindow.Document.GetElementsByTagName("head")[0].OuterHtml;
    public static string GetBody(this HtmlWindow htmlWindow) => htmlWindow.Document.GetElementsByTagName("body")[0].OuterHtml;
    public static string GetHtml(this HtmlWindow htmlWindow) => htmlWindow.Document.GetElementsByTagName("html")[0].OuterHtml;
    public static string GetHead(this HtmlDocument htmlDocument) => htmlDocument.GetElementsByTagName("head")[0].OuterHtml;
    public static string GetBody(this HtmlDocument htmlDocument) => htmlDocument.GetElementsByTagName("body")[0].OuterHtml;
    public static string GetHtml(this HtmlDocument htmlDocument) => htmlDocument.GetElementsByTagName("html")[0].OuterHtml;
}