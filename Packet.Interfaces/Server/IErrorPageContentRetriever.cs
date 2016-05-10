namespace Packet.Interfaces.Server
{
    public interface IErrorPageContentRetriever
    {
        string Get400ErrorPageContent();

        string Get403ErrorPageContent();

        string Get404ErrorPageContent();

        string Get500ErrorPageContent();
    }
}
