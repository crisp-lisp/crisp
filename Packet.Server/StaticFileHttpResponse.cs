using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class StaticFileHttpResponse : HttpResponse
    {
        public StaticFileHttpResponse(IHttpVersion version) : base(version)
        {
        }

        public override void WriteTo(Stream stream)
        {
            
        }
    }
}
