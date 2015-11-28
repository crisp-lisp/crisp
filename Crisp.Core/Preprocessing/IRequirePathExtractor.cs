namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a require path extractor.
    /// </summary>
    public interface IRequirePathExtractor
    {
        /// <summary>
        /// Extracts the raw filepath of the dependency from the require statement sequence given.
        /// </summary>
        /// <param name="sequence">The sequence of the require statement token.</param>
        /// <returns></returns>
        string Extract(string sequence);
    }
}
