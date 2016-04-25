using Crisp.Interfaces;
using Crisp.Shared;

namespace Crisp.Configuration
{
    public class CrispConfigurationFileNameProvider : Provider<string>, ICrispConfigurationFileNameProvider
    {
        /// <summary>
        /// Initializes a new instance of a Crisp configuration file name provider service.
        /// </summary>
        /// <param name="value">The name of the configuration file on-disk.</param>
        public CrispConfigurationFileNameProvider(string value) : base(value)
        {
        }
    }
}
