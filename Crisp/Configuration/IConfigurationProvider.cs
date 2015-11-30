using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Configuration
{
    internal interface IConfigurationProvider
    {
        Configuration GetConfiguration();
    }
}
