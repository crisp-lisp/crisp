using Newtonsoft.Json;

using Crisp.Shared;

namespace Crisp.Configuration
{
    /// <summary>
    /// An implementation of an application configuration settings provider service.
    /// </summary>
    public class CrispConfigurationProvider : Provider<ICrispConfiguration>, ICrispConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of an application configuration settings provider service.
        /// </summary>
        /// <param name="rawCrispConfigurationProvider">The raw configuration provider service.</param>
        public CrispConfigurationProvider(IRawCrispConfigurationProvider rawCrispConfigurationProvider)
        {
            var rawConfigurationFileText = rawCrispConfigurationProvider.Get();
            Value = JsonConvert.DeserializeObject<CrispConfiguration>(rawConfigurationFileText);
        }
    }
}
