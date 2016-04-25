using Crisp.Interfaces;

namespace Crisp.Shared
{
    public interface IEvaluatorFactory
    {
        IEvaluator Get();
    }
}