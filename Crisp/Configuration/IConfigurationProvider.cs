namespace Crisp.Configuration
{
    /// <summary>
    /// Represents an application configuration settings provider.
    /// </summary>
    internal interface IConfigurationProvider
    {
        Configuration GetConfiguration();
    }
}
