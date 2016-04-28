using Crisp.Interfaces.Evaluation;
using Crisp.Interfaces.Shared;

namespace Crisp.Evaluation
{
    public class EvaluatorFactory : IEvaluatorFactory
    {
        private readonly ISourceFileDirectoryPathProvider _sourceFileDirectoryPathProvider;

        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        private readonly ISpecialFormLoader _specialFormLoader;

        /// <summary>
        /// Initializes a new instance of a factory for producing <see cref="IEvaluator"/> instances.
        /// </summary>
        /// <param name="sourceFileDirectoryPathProvider">The source file diretory path provider service.</param>
        /// <param name="interpreterDirectoryPathProvider">The interpreter directory path provider service.</param>
        /// <param name="specialFormLoader">The service to use to load special forms.</param>
        public EvaluatorFactory(
            ISourceFileDirectoryPathProvider sourceFileDirectoryPathProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider, 
            ISpecialFormLoader specialFormLoader)
        {
            _sourceFileDirectoryPathProvider = sourceFileDirectoryPathProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
            _specialFormLoader = specialFormLoader;
        }

        public IEvaluator Get()
        {
            // Create evaluator, specifying working directory and loading special forms.
            var evaluator = new Evaluator
            {
                SourceFileDirectory = _sourceFileDirectoryPathProvider.Get(),
                InterpreterDirectory = _interpreterDirectoryPathProvider.Get(),
                WorkingDirectory = _sourceFileDirectoryPathProvider.Get()
            };
            evaluator.Mutate(_specialFormLoader.GetBindings());

            return evaluator;
        }
    }
}
