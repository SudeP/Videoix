using CefSharp;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Videoix.ManageClasses
{
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
}