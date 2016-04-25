using System.IO;

using Crisp.Interfaces.Configuration;
using Crisp.Shared;

namespace Crisp.Configuration
{
    public class RawCrispConfigurationProvider : Provider<string>, IRawCrispConfigurationProvider
    {
        /// <summary>
        /// Initializes a new instance of a service to load the raw text of a Crisp configuration file.
        /// </summary>
        /// <param name="interpreterDirectoryPathProvider">The interpreter directory path provider service.</param>
        /// <param name="crispConfigurationFileNameProvider">The configuration file name provider service.</param>
        public RawCrispConfigurationProvider(
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider,
            ICrispConfigurationFileNameProvider crispConfigurationFileNameProvider)
        {
            // Calculate path and check file exists.
            var path = Path.Combine(interpreterDirectoryPathProvider.Get(),
                crispConfigurationFileNameProvider.Get());
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Crisp configuration file at '{path}' not found.");
            }

            // Return file text.
            Value = File.ReadAllText(path);
        }
    }
}
