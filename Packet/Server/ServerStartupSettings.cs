using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Server
{
    internal class ServerStartupSettings
    {
        public int Port { get; set; }
        public string WebRoot { get; set; }
    }
}
