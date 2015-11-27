using System.Collections.Generic;

using Crisp.Core.Evaluation;
using Crisp.Core.Tokenizing;

namespace Crisp.Core.Preprocessing
{
    public interface IPreprocessor
    {
        void BindExpressions(string filepath, IEvaluator evaluator);
    }
}
