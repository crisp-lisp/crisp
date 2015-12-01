namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a require path transformer, capable of converting raw filepaths extracted from require statements
    /// into fully qualified paths for loading.
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
