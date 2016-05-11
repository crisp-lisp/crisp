using System.Text;

namespace Packet.Interfaces.Server
{
    public interface IEncodingProvider
    {
        Encoding Get();
    }
}
