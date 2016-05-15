namespace Crisp.Interfaces.Evaluation
{
    /// <summary>
    /// Represents a factory for producing <see cref="IEvaluator"/> instances.
    /// </summary>
    public interface IEvaluatorFactory
    {
        /// <summary>
        /// Creates and returns an evaluator.
        /// </summary>
        /// <returns></returns>
        IEvaluator Get();
    }
}