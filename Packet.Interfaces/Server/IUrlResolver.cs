namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a URL resolution service to map request URLs to physical filepaths.
    /// </summary>
    public interface IUrlResolver
    {
        /// <summary>
        /// Resolves a physical path from the given URL relative to the web root.
        /// </summary>
        /// <param name="url">The URL of the requested resource.</param>
        /// <returns></returns>
        string Resolve(string url);
    }
}
