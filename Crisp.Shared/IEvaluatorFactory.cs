using Crisp.Shared;

namespace Crisp.Evaluation
{
    public interface IEvaluatorFactory
    {
        IEvaluator Get();
    }
}