using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Interfaces.Server
{
    public interface IUrlResolver
    {
        /// <summary>
        /// Creates a physical path from the given URL relative to the web root.
        /// </summary>
        /// <param name="url">The URL of the requested resource.</param>
        /// <returns></returns>
        string GetUrlPath(string url);
    }
}
