namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a service that returns the path of the source file.
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
