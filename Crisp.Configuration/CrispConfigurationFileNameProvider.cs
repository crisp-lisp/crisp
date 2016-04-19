using Crisp.Shared;

namespace Crisp.Configuration
{
    /// <summary>
    /// An implementation of a Crisp configuration file name provider service.
    /// </summary>
    public class CrispConfigurationFileNameProvider : Provider<string>, ICrispConfigurationFileNameProvider
    {
        /// <summary>
        /// Initializes a new instance of a Crisp configuration file name provider service.
        /// </summary>
        /// <param name="fileName">The name of the configuration file on-disk.</param>
        public CrispConfigurationFileNameProvider(string fileName) : base(fileName)
        {
        }
    }
}
