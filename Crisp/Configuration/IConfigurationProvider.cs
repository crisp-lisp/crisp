using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Configuration
{
    /// <summary>
    /// Represents an application configuration settings provider.
    /// </summary>
    internal interface IConfigurationProvider
    {
        Configuration GetConfiguration();
    }
}
