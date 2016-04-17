using Crisp.Shared;

namespace Packet
{
    /// <summary>
    /// An implementation of a service that returns the options passed in via the command line.
    /// </summary>
    internal class OptionsProvider : Provider<Options>, IOptionsProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that returns the options passed in via the command line.
        /// </summary>
        /// <param name="options">The options passed in via the command line.</param>
        public OptionsProvider(Options options) : base(options)
        {
        }
    }
}
