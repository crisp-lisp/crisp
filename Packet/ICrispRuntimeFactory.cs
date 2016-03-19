using Crisp.Core.Runtime;

namespace Packet
{
    /// <summary>
    /// Represents a factory for creating Crisp runtime instances.
    /// </summary>
    internal interface ICrispRuntimeFactory
    {
        /// <summary>
        /// Creates a new Crisp runtime to run the file at the specified path.
        /// </summary>
        /// <param name="inputFile">The path of the input file to create the runtime for.</param>
        /// <returns></returns>
        ICrispRuntime GetCrispRuntime(string inputFile);
    }
}
