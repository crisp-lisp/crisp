using SimpleInjector;

using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Runtime;
using Crisp.Core.Tokenizing;

using Packet.Configuration;

namespace Packet
{
    /// <summary>
    /// An implementation of a factory for creating <see cref="ICrispRuntime"/> instances.
    /// </summary>
    internal class CrispRuntimeFactory : ICrispRuntimeFactory
    {
        public ICrispRuntime GetCrispRuntime(string inputFile)
        {
            // Dependency injection.
            var container = new Container();
            container.Register<IRequirePathExtractor, RequirePathExtractor>();
            container.Register<ISourceFilePathProvider>(() => new SourceFilePathProvider(inputFile));
            container.Register(() => TokenizerFactory.GetCrispTokenizer());
            container.Register<IParser, Parser>();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<ISpecialFormDirectoryPathProvider, SpecialFormDirectoryPathProvider>();
            container.Register<IRequirePathTransformer, RequirePathTransformer>();
            container.Register<IDependencyFinder, DependencyFinder>();
            container.Register<IDependencyLoader, DependencyLoader>();
            container.Register<ISpecialFormLoader, SpecialFormLoader>();
            container.Register<IEvaluator>(() =>
            {
                var evaluator = new Evaluator(
                    container.GetInstance<ISourceFilePathProvider>(),
                    container.GetInstance<ISpecialFormLoader>(), 
                    container.GetInstance<IDependencyLoader>());
                return evaluator;
            });
            container.Register<ICrispRuntime, CrispRuntime>();
            container.Verify();

            return container.GetInstance<ICrispRuntime>(); // Return runtime.
        }
    }
}
