using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet.Server.Tests
{
    public static class SampleRawHttpRequestFactory
    {
        public static byte[] GetSimpleHttpRequest()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleSimpleHttpRequest_1);
        }
        public static byte[] GetSimpleHttpRequestWithExtraData()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleSimpleHttpRequest_2);
        }
    }
}
