using Crisp.Interfaces.Runtime;
using Crisp.Interfaces.Shared;
using Crisp.Shared;

namespace Packet
{
    /// <summary>
    /// Represents a factory for creating <see cref="ICrispRuntime"/> instances.
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
