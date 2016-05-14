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

        /// <summary>
        /// Returns true if this HTTP version matches the version numbers given, otherwise returns false.
        /// </summary>
        /// <param name="major">The major version number.</param>
        /// <param name="minor">The minor version number.</param>
        /// <returns></returns>
        bool IsVersion(int major, int minor);
    }
}
