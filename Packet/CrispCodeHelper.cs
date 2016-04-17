using SimpleInjector;

using Crisp.Evaluation;
using Crisp.Shared;
using Crisp.Parsing;
using Crisp.Runtime;
using Crisp.Tokenization;
using Packet.Configuration;

namespace Packet
{
    /// <summary>
    /// Provides a static factory for creating Crisp runtime instances.
    /// </summary>
    internal static class CrispCodeHelper
    {
        /// <summary>
        /// Creates a new <see cref="ICrispRuntime"/> instance to run the file at the specified path.
        /// </summary>
        /// <param name="filepath">The path of the input file to create the runtime for.</param>
        /// <returns></returns>
        internal static ICrispRuntime GetCrispRuntime(string filepath)
        {
            // Dependency injection.
            var container = new Container();
            container.Register<ISourceFilePathProvider>(() => new SourceFilePathProvider(filepath));
            container.Register(TokenizerConfigurationProviderFactory.GetCrispTokenizerConfigurationProvider);
            container.Register(TokenFilterFactory.GetCommentAndWhitespaceFilter);
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        internal static IExpressionTreeSource SourceToExpressionTree(string source)
        {
            // Dependency injection.
            var container = new Container();
            container.Register<ISourceCodeProvider>(() => new SourceCodeProvider(source));
            container.Register(TokenizerConfigurationProviderFactory.GetCrispTokenizerConfigurationProvider);
            container.Register(TokenFilterFactory.GetCommentAndWhitespaceFilter);
            container.Register<ITokenListSource, Tokenizer>();
            container.Register<IExpressionTreeSource, Parser>();
            container.Verify();
            
            return container.GetInstance<IExpressionTreeSource>(); // Return parsed source.
        }
    }
}
