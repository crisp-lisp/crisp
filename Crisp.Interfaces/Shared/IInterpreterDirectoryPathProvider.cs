namespace Crisp.Interfaces.Shared
{
    /// <summary>
    /// Represents an interpreter directory path provider service.
    /// </summary>
    public interface IInterpreterDirectoryPathProvider
    {
        /// <summary>
        /// Gets the directory path of the interpreter.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}
