using System.Windows.Forms;

public static class JavaScriptLoader
{
    public static string JavaScriptsFilePath { get; set; }
    public static void LoadJavaScriptFile(this HtmlDocument htmlDocument, string javaScriptFileName)
    {
        var path = string.Format("{0}{1}{2}.js",
                                 Application.StartupPath,
                                 JavaScriptsFilePath,
                                 javaScriptFileName);
        string js = System.IO.File.ReadAllText(path);
        htmlDocument.JavaScriptExecute(js);
    }
}
