using Crisp.Core.Preprocessing;
using Packet.Configuration;

using SimpleInjector;

namespace Packet.Server
{
    /// <summary>
    /// Provides a static factory for creating <see cref="IHttpServer"/> instances.
    /// </summary>
    internal static class HttpServerFactory
    {
        /// <summary>
        /// Gets a <see cref="PacketHttpServer"/> instance.
        /// </summary>
        /// <returns></returns>
        public static IHttpServer GetPacketHttpServer(ServerStartupSettings serverStartupSettings)
        {
            var container = new Container();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<ICrispRuntimeFactory, CrispRuntimeFactory>();
            container.Register<IServerStartupSettingsProvider>(() =>
                new ServerStartupSettingsProvider(serverStartupSettings));
            container.Register<IHttpServer, PacketHttpServer>();

            return container.GetInstance<IHttpServer>();
        }
    }
}
