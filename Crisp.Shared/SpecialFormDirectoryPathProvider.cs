using System.IO;
using Crisp.Interfaces;

namespace Crisp.Shared
{
    /// <summary>
    /// An implementation of a service that returns the fully-qualified directory path of the directory in which 
    /// compiled special form binaries are stored.
    /// </summary>
    public class SpecialFormDirectoryPathProvider : ISpecialFormDirectoryPathProvider
    {
        private readonly ICrispConfigurationProvider _configurationProvider;

        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        public string Get()
        {
            return Path.Combine(_interpreterDirectoryPathProvider.Get(),
                _configurationProvider.Get().SpecialFormDirectory);
        }

        /// <summary>
        /// Initializes a new instance of a special form directory provider.
        /// </summary>
        /// <param name="configurationProvider">The service to use to get the application configuration.</param>
        /// <param name="interpreterDirectoryPathProvider">The service to use to get the interpreter path.</param>
        public SpecialFormDirectoryPathProvider(ICrispConfigurationProvider configurationProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _configurationProvider = configurationProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
