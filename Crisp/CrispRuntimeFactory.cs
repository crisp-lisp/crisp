using SimpleInjector;

using Crisp.Configuration;
using Crisp.Evaluation;
using Crisp.Shared;
using Crisp.Parsing;
using Crisp.Runtime;
using Crisp.Tokenization;

namespace Crisp
{
    /// <summary>
    /// Provides a static factory for creating Crisp runtime instances.
    /// </summary>
    internal static class CrispRuntimeFactory
    {
        /// <summary>
        /// Creates a new Crisp runtime to run the file at the specified path.
        /// </summary>
        /// <param name="inputFile">The path of the input file to create the runtime for.</param>
        /// <returns></returns>
        public static ICrispRuntime GetCrispRuntime(string inputFile)
        {
            // Dependency injection.
            var container = new Container();
            container.Register<ISourceFilePathProvider>(() => new SourceFilePathProvider(inputFile));
            container.Register(TokenizerConfigurationProviderFactory.GetCrispTokenizerConfigurationProvider);
            container.Register<ITokenListSource, Tokenizer>();
            container.Register<IExpressionTreeSource, Parser>();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<ISpecialFormDirectoryPathProvider, SpecialFormDirectoryPathProvider>();
            container.Register<ISpecialFormLoader, SpecialFormLoader>();
            container.Register<IEvaluatorFactory, EvaluatorFactory>();
            container.Register<ICrispRuntime, CrispRuntime>();
            container.Verify();

            return container.GetInstance<ICrispRuntime>(); // Return runtime.
        }
    }
}
