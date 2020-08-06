using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels.Ipc;
using System.Runtime.Remoting.Channels.Tcp;
using System.Runtime.Remoting.Contexts;
using System.Runtime.Remoting.Lifetime;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Metadata;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Runtime.Remoting.MetadataServices;
using System.Runtime.Remoting.Proxies;
using System.Runtime.Remoting.Services;

namespace VidoixServer
{
    public class Program
    {
        public static void Main()
        {
            var vsm = new VidoixLibrary.VidoixServerManager();
            var uri = vsm.TcpServerChannel.GetChannelUri();
            var vcm = new VidoixLibrary.VidoixClientManager();
            var asd123 = vcm.TcpClientChannel.Parse(uri, out string objectURI1);
            vcm.TcpClientChannel.CreateMessageSink(uri,"Hello my friend", out string objectURI2);
            vsm.TcpServerChannel.StartListening("Hello mada faka");
        }
    }
}