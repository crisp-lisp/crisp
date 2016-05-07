using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Interfaces.Server
{
    public interface IHttpRequestReader
    {
        byte[] GetData(Stream stream);
    }
}
