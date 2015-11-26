using System.Collections.Generic;

using Crisp.Core.Evaluation;
using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Implemented by classes that expose language preprocessing functionality.
    /// </summary>
    public interface IPreprocessor
    {
        /// <summary>
        /// Crawls the dependency tree for a program source file and loads required libraries.
        /// </summary>
        /// <param name="filename">The path of the file to start crawling at.</param>
        /// <returns>The sanitized token list without requires, comments or whitespace.</returns>
        IList<Token> Process(string filename);

        /// <summary>
        /// Sets up an evaluator with the bindings contained in the currently loaded libraries.
        /// </summary>
        /// <param name="evaluator"></param>
        void BindExpressions(IEvaluator evaluator);
    }
}
