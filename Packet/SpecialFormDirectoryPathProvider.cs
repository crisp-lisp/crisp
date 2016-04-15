using System.IO;

using Crisp.Common;
using Crisp.Core.Preprocessing;

using Packet.Configuration;

namespace Packet
{
    /// <summary>
    /// An implementation of a service that returns the absolute path of the directory in which compiled special form 
    /// binaries are located.
    /// </summary>
    internal class SpecialFormDirectoryPathProvider : Provider<string>, ISpecialFormDirectoryPathProvider
    {
        /// <summary>
        /// Initializes a new instance of a service that returns the absolute path of the directory in which compiled 
        /// special form binaries are located.
        /// </summary>
        /// <param name="configurationProvider">The service to use to get the application configuration.</param>
        /// <param name="interpreterDirectoryPathProvider">The service to use to get the interpreter path.</param>
        public SpecialFormDirectoryPathProvider(
            IConfigurationProvider configurationProvider,
            IInterpreterDirectoryPathProvider interpreterDirectoryPathProvider)
        {
            // Path of special form directory is relative to the interpreter directory.
            Obj = Path.Combine(interpreterDirectoryPathProvider.Get(),
                configurationProvider.Get().SpecialFormDirectory);
        }
    }
}
