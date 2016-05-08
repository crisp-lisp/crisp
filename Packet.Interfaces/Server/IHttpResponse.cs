using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Interfaces.Server
{
    public interface IHttpResponse
    {
        IHttpVersion Version { get; }

        byte[] Content { get; set; }

        void WriteTo(TcpClient socket);
    }
}
