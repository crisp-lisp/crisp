namespace Packet.Interfaces.Server
{
    public interface IErrorPageContentRetriever
    {
        string GetErrorPageContent(int errorStatusCode);

        byte[] GetEncodedErrorPageContent(int errorStatusCode);
    }
}
