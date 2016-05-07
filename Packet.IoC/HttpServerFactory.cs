using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Interfaces.Configuration;
using Crisp.Interfaces.Shared;
using Crisp.Shared;
using Packet.Configuration;
using Packet.Interfaces.Configuration;
using Packet.Interfaces.Logging;
using Packet.Interfaces.Server;
using Packet.Logging;
using Packet.Server;
using SimpleInjector;

namespace Packet.IoC
{
    public static class HttpServerFactory
    {
        public static IHttpServer GetPacketHttpServer()
        {
            // Dependency injection.
            var container = new Container();
            container.Register<IInterpreterFilePathProvider, InterpreterFilePathProvider>();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IPacketConfigurationFileNameProvider>(() => new PacketConfigurationFileNameProvider("packet.json"));
            container.Register<IRawPacketConfigurationProvider, RawPacketConfigurationProvider>();
            container.Register<IPacketConfigurationProvider, PacketConfigurationProvider>();
            container.Register<IHttpRequestParser>(() => new FullHttpRequestParser(new SimpleRequestParser(null)));
            container.Register<ILogger, ConsoleWindowLogger>();
            container.Register<IHttpServer, PacketHttpServer>();
            container.Verify();

            return container.GetInstance<IHttpServer>();
        }
    }
}
