using System.IO;

using Crisp.Configuration;
using Crisp.Core.Preprocessing;

namespace Crisp
{
    internal class SpecialFormDirectoryPathProvider : ISpecialFormDirectoryPathProvider
    {
        private readonly IConfigurationProvider _configurationProvider;

        private readonly IInterpreterDirectoryPathProvider _interpreterDirectoryPathProvider;

        public string GetPath()
        {
            return Path.Combine(_interpreterDirectoryPathProvider.GetPath(),
                _configurationProvider.GetConfiguration().SpecialFormDirectory);
        }

        public SpecialFormDirectoryPathProvider(IConfigurationProvider configurationProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            _configurationProvider = configurationProvider;
            _interpreterDirectoryPathProvider = interpreterDirectoryPathProvider;
        }
    }
}
