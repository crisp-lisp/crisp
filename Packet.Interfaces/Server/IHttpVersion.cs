namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a HTTP version.
    /// </summary>
    public interface IHttpVersion
    {
        /// <summary>
        /// Gets or sets the major version number.
        /// </summary>
        int Major { get; set; }

        /// <summary>
        /// Gets or sets the minor version number.
        /// </summary>
        int Minor { get; set; }
    }
}
