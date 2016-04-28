namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a special form directory path provider service.
    /// </summary>
    public interface ISpecialFormDirectoryPathProvider
    {
        /// <summary>
        /// Gets the path of the directory in which compiled special form binaries are stored.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
