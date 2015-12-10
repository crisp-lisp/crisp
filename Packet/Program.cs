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
            // Dependency injection.
            var container = new Container();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<ISpecialFormDirectoryPathProvider, SpecialFormDirectoryPathProvider>();
            container.Register<IRequirePathTransformer, RequirePathTransformer>();
            container.Register<IRequirePathExtractor, RequirePathExtractor>();
            container.Register(() => TokenizerFactory.GetCrispTokenizer());
            container.Register<IParser, Parser>();
            container.Register<IDependencyFinder, DependencyFinder>();
            container.Register<IDependencyLoader, DependencyLoader>();
            container.Register<ISpecialFormLoader, SpecialFormLoader>();
            container.Verify();

            HttpServer httpServer;
            if (args.GetLength(0) > 0)
            {
                httpServer = new MyHttpServer(Convert.ToInt16(args[0]));
            }
            else
            {
                httpServer = new MyHttpServer(8080);
            }
            Thread thread = new Thread(new ThreadStart(httpServer.listen));
            thread.Start();
        }
    }
}
