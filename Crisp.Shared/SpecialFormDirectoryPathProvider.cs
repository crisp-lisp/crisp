using System.IO;

using Crisp.Interfaces.Configuration;
using Crisp.Interfaces.Shared;

namespace Crisp.Shared
{
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
        /// Initializes a new instance of a special form directory path provider service.
        /// </summary>
        /// <param name="configurationProvider">The configuration provider service.</param>
        /// <param name="interpreterDirectoryPathProvider">The interpreter directory path provider service.</param>
        public SpecialFormDirectoryPathProvider(
            ICrispConfigurationProvider configurationProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _configurationProvider = configurationProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
