using System;
using System.Windows.Forms;

namespace Videoix.ManageClasses
{
    public class ManageEnvrionment
    {
        public const int _1MilliSecond = 1000;
        public readonly int m = _1MilliSecond;
        public WFCEF f;
        public ManageEnvrionment(WFCEF wFCEF) => f = wFCEF;
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
        public void InvokeRequired(Action action)
        {
            if (f.InvokeRequired)
                f.Invoke(action);
            else
                action.Invoke();
        }
    }
    public static class ManageToolExtension
    {
        public static void InvokeRequired(this Form form, Action action)
        {
            if (form.InvokeRequired)
                form.Invoke(action);
            else
                action.Invoke();
        }
    }
}
