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
        private readonly string _path;

        public StaticFileHttpResponse(IHttpVersion version, string path) : base(version)
        {
            _path = path;
        }

        public override void WriteTo(Stream stream)
        {
            var data = File.ReadAllBytes(_path);
            stream.Write(data, 0, data.Length);
        }
    }
}
