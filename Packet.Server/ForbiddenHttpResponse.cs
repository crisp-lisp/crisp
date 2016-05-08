using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Packet.Interfaces.Server;

namespace Packet.Server
{
    public class ForbiddenHttpResponse : HttpResponse
    {
        private readonly string _filename;

        public ForbiddenHttpResponse(IHttpVersion version, string filename) : base(version)
        {
            _filename = filename;
        }

        public override void WriteTo(Stream stream)
        {
            var writer = new StreamWriter(new BufferedStream(stream)) {AutoFlush = true};
            writer.WriteLine($"{Version} 403 Forbidden");
        }
    }
}
