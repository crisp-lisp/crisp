using System;
using System.Threading;

using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Tokenizing;

using Packet.Configuration;

using SimpleInjector;

namespace Packet
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new PacketHttpServer(Convert.ToInt16(args[0]));
            }
            else
            {
                httpServer = new PacketHttpServer(8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.Listen));
            thread.Start();
        }
    }
}
