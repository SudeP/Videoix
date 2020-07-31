using System;
using System.Threading;

namespace Videoix.ManageClasses
{
    public class ManageThread
    {
        public Thread Thread { get; set; }
        public ThreadStart ThreadStart { get; set; }
        private ManageThread()
        {

        }
        public ManageThread NewThread()
        {
            if (!(Thread is null))
                try { Thread.Abort(); } catch { }

            if (ThreadStart is null)
                throw new Exception("ThreadStart is null. Not Allow! Required");

            Thread = new Thread(ThreadStart);
            Thread.SetApartmentState(ApartmentState.STA);

            return this;
        }
        public ManageThread SetThreadStart(Action action)
        {

            ThreadStart = new ThreadStart(action);

            return this;
        }
        public ManageThread Start()
        {
            if (!(Thread is null))
            {
                try { Thread.Abort(); } catch { }
                try { NewThread(); } catch { }
            }

            Thread.Start();

            return this;
        }
        public ManageThread Abort()
        {
            if (Thread is null)
                return this;

            if (
                Thread.ThreadState == ThreadState.Aborted
                ||
                Thread.ThreadState == ThreadState.Aborted
                ||
                Thread.ThreadState == ThreadState.AbortRequested
                ||
                Thread.ThreadState == ThreadState.Stopped
                ||
                Thread.ThreadState == ThreadState.StopRequested
                ||
                Thread.ThreadState == ThreadState.Suspended
                ||
                Thread.ThreadState == ThreadState.SuspendRequested
                ||
                Thread.ThreadState == ThreadState.Unstarted
                )
                return this;


            try { Thread.Abort(); } catch { }

            return this;
        }
        public static ManageThread Create(Action action) => new ManageThread().SetThreadStart(action).NewThread();
    }
}
