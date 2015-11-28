namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents an interpreter directory path provider.
    /// </summary>
    public interface IInterpreterDirectoryPathProvider
    {
        /// <summary>
        /// Gets the directory path of the interpreter.
        /// </summary>
        /// <returns></returns>
        string GetPath();
    }
}
