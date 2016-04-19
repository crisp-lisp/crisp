using System;

namespace Crisp.Shared
{
    public interface ICrispConfigurationFileNameProvider
    {
        /// <summary>
        /// Gets the value from this provider.
        /// </summary>
        /// <returns></returns>
        string Get();
    }
}