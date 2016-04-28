namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a source file path provider that returns the path of the source file to be interpreted.
    /// </summary>
    public interface ISourceFilePathProvider
    {
        /// <summary>
        /// Gets the path of the source file to be interpreted.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
