using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Channels;
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

namespace VidoixLibrary
{
    public class VidoixServerManager
    {
        private readonly TcpServerChannel tsc;
        public TcpServerChannel TcpServerChannel => tsc;
        private const int port = 11000;
        public VidoixServerManager()
        {
            var tc = new TcpChannel(port);
            ChannelServices.RegisterChannel(tc, true);
            //RemotingConfiguration.RegisterWellKnownServiceType(typeof(BasicDialogue), nameof(BasicDialogue), WellKnownObjectMode.SingleCall);
        }
    }
}
