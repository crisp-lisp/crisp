using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Interfaces.Server
{
    public interface IHttpVersion
    {
        int Major { get; set; }

        int Minor { get; set; }
    }
}
