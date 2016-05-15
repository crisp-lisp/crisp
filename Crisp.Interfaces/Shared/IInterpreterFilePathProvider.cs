namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents a file path provider that returns the path of the interpreter executable.
    /// </summary>
    public interface IInterpreterFilePathProvider
    {
        /// <summary>
        /// Gets the path of the interpreter executable.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
