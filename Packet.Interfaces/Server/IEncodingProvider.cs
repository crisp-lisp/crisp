using System.Text;

namespace Packet.Interfaces.Server
{
    /// <summary>
    /// Represents a text encoding provider service.
    /// </summary>
    public interface IEncodingProvider
    {
        /// <summary>
        /// Gets the text encoding from this provider.
        /// </summary>
        /// <returns></returns>
        Encoding Get();
    }
}
