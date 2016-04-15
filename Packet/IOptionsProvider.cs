namespace Packet
{
    /// <summary>
    /// Represents a service that returns the options passed in via the command line.
    /// </summary>
    internal interface IOptionsProvider
    {
        /// <summary>
        /// Gets the options passed in via the command line.
        /// </summary>
        /// <returns></returns>
        Options Get();
    }
}
