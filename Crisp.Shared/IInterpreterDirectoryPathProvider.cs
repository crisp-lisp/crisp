namespace Crisp.Shared
{
    /// <summary>
    /// Represents an interpreter directory path provider, capable of returning the directory that contains the 
    /// currently executing interpreter.
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
