using System.IO;
using Crisp.Shared;

namespace Crisp.Evaluation
{
    /// <summary>
    /// A factory to produce <see cref="IEvaluator"/> instances.
    /// </summary>
    public class EvaluatorFactory : IEvaluatorFactory
    {
        private readonly ISourceFilePathProvider _sourceFilePathProvider;

        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        private readonly ISpecialFormLoader _specialFormLoader;

        /// <summary>
        /// Initializes a new instance of a factory to produce <see cref="IEvaluator"/> instances.
        /// </summary>
        /// <param name="sourceFilePathProvider"></param>
        /// <param name="interpreterDirectoryPathProvider">The service to use to retrieve the interpreter directory path.</param>
        /// <param name="specialFormLoader">The service to use to load special forms.</param>
        public EvaluatorFactory(
            ISourceFilePathProvider sourceFilePathProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider, 
            ISpecialFormLoader specialFormLoader)
        {
            _sourceFilePathProvider = sourceFilePathProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
            _specialFormLoader = specialFormLoader;
        }

        public IEvaluator Get()
        {
            // Create evaluator, specifying working directory and loading special forms.
            var sourceFileDirectory = Path.GetDirectoryName(_sourceFilePathProvider.Get()); // TODO: Separate out.
            var evaluator = new Evaluator
            {
                SourceFileDirectory = sourceFileDirectory,
                InterpreterDirectory = _interpreterDirectoryPathProvider.Get(),
                WorkingDirectory = sourceFileDirectory
            };
            evaluator.Mutate(_specialFormLoader.GetBindings());

            return evaluator;
        }
    }
}
