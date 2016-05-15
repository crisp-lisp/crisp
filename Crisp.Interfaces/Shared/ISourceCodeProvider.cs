namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a source code provider service.
    /// </summary>
    public interface ISourceCodeProvider
    {
        /// <summary>
        /// Gets the source code from this provider.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
