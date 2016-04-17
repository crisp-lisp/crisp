using System.IO;

using Crisp.Configuration;
using Crisp.Shared;

namespace Crisp
{
    /// <summary>
    /// An implementation of a service that returns the fully-qualified directory path of the directory in which 
    /// compiled special form binaries are stored.
    /// </summary>
    internal class SpecialFormDirectoryPathProvider : ISpecialFormDirectoryPathProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        public string Get()
        {
            return Path.Combine(_interpreterDirectoryPathProvider.Get(),
                _configurationProvider.GetConfiguration().SpecialFormDirectory);
        }

        /// <summary>
        /// Initializes a new instance of a special form directory provider.
        /// </summary>
        /// <param name="configurationProvider">The service to use to get the application configuration.</param>
        /// <param name="interpreterDirectoryPathProvider">The service to use to get the interpreter path.</param>
        public SpecialFormDirectoryPathProvider(IConfigurationProvider configurationProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _configurationProvider = configurationProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
