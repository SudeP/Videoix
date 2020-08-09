using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Videoix.ManageClasses
{
    public class LogTextBox
    {
        public TextBox TextBox { get; set; }
        private LogTextBox()
        {

        }
        public LogTextBox SetTextBox(TextBox textBox)
        {
            if (textBox is null)
                throw new Exception("textBox is null! Not Allow! Required");

            TextBox = textBox;

            return this;
        }
        public void OnLog(object message, EventArgs e)
        {
            ControlInvokeRequired(() => OnLog(message.ToString()));
        }
        public void Log(string message)
        {
            ControlInvokeRequired(() => OnLog(message.ToString()));
        }
        private void OnLog(string message)
        {
            if (TextBox.Text.Length > 5000)
                LogClear();

            var dt = DateTime.Now;

            TextBox.Text += $@"{dt.ToString("yyyy.MM.dd HH:mm:ss")}___{message}{Environment.NewLine}";

            TextBox.SelectionStart = TextBox.Text.Length;

            TextBox.ScrollToCaret();
            TextBox.Refresh();
        }
        private void ControlInvokeRequired(Action action)
        {
            if (TextBox.Parent.InvokeRequired)
                TextBox.Parent.Invoke(action);
            else
                action.Invoke();
        }
        public void LogClear()
        {
            if (TextBox.Text.Length == 0)
                return;
#pragma warning disable
            if (1 == 2)
            {
                var dt = DateTime.Now;


                var dir = $@"{Application.StartupPath}\LogTextBox\{dt.Year}\{dt.Month}\{dt.Day}";
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                File.WriteAllText($@"{dir}\{dt.ToString("HH.mm.ss.txt")}", TextBox.Text);
            }
#pragma warning restore
            TextBox.Text = string.Empty;
        }
        public static LogTextBox Create(TextBox textBox) => new LogTextBox().SetTextBox(textBox);
    }
}
