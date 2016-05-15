namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a source file directory path provider service.
    /// </summary>
    public interface ISourceFileDirectoryPathProvider
    {
        /// <summary>
        /// Gets the directory of the source file.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
