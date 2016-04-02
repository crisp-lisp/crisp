using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Server
{
    interface ILogger
    {
        void WriteLine(string msg);

        void Write(string msg);

        void WriteError(Exception ex);
    }
}
