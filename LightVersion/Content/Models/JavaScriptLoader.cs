using System.Windows.Forms;

public static class JavaScriptLoader
{
    public static string JavaScriptsFilePath { get; set; }
    public static void LoadJavaScriptFileInHead(this HtmlDocument htmlDocument,
                                          string javaScriptFileName,
                                          bool isDebug = false,
                                          params string[] vs)
    {
        var path = string.Format("{0}\\{1}{2}.js",
                                 Application.StartupPath,
                                 JavaScriptsFilePath,
                                 javaScriptFileName);
        string js = System.IO.File.ReadAllText(path);
        if (isDebug)
            js = js.Replace("cw(\"", "//cw(\"");
        foreach (var replaceValues in vs)
            js = js.Replace(replaceValues.Split('|')[0], replaceValues.Split('|')[1]);
        htmlDocument.JavaScriptExecuteInHead(js);
    }
    public static void LoadJavaScriptFileInBody(this HtmlDocument htmlDocument,
                                          string javaScriptFileName,
                                          bool isDebug = false,
                                          params string[] vs)
    {
        var path = string.Format("{0}\\{1}{2}.js",
                                 Application.StartupPath,
                                 JavaScriptsFilePath,
                                 javaScriptFileName);
        string js = System.IO.File.ReadAllText(path);
        if (isDebug)
            js = js.Replace("cw(\"", "//cw(\"");
        foreach (var replaceValues in vs)
            js = js.Replace(replaceValues.Split('|')[0], replaceValues.Split('|')[1]);
        htmlDocument.JavaScriptExecuteInBody(js);
    }
}
