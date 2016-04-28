using Crisp.Interfaces.Shared;
using Crisp.Shared;
using Crisp.Visualization;

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
        public static IHttpServer GetPacketHttpServer(Options options)
        {
            var container = new Container();
            container.Register<ISymbolicExpressionSerializer, LispSerializer>();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<IOptionsProvider>(() => new OptionsProvider(options));
            container.Register<IServerSettingsProvider, CommandLineOverrideServerSettingsProvider>();
            container.Register<ILogger, ConsoleWindowLogger>();
            container.Register<IHttpServer, PacketHttpServer>();

            return container.GetInstance<IHttpServer>();
        }
    }
}
