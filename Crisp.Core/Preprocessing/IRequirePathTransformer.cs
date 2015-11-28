namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a require path transformer.
    /// </summary>
    public interface IRequirePathTransformer
    {
        /// <summary>
        /// Transforms the given raw path into an absolute path.
        /// </summary>
        /// <param name="path">The raw path to transform.</param>
        /// <returns></returns>
        string Transform(string path);
    }
}
