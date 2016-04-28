namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a service that returns the fully-qualified directory path of the directory in which compiled special
    /// form binaries are stored.
    /// </summary>
    public interface ISpecialFormDirectoryPathProvider
    {
        /// <summary>
        /// Gets the directory of the directory in which compiled special form binaries are stored.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
