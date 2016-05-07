using System.Text;

namespace Packet.Server.Tests
{
    /// <summary>
    /// A static helper class to retrieve sample byte arrays corresponding to raw HTTP request data.
    /// </summary>
    public static class SampleRawHttpRequestFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHttpRequest_0_9()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_0_9_1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHttpRequestWithExtraData_0_9()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_0_9_2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetMultilineHttpRequestWithExtraData_0_9()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_0_9_3);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetGetHttpRequest_1_0()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_1_0_1);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static byte[] GetHeadHttpRequest_1_0()
        {
            return new ASCIIEncoding().GetBytes(Properties.Resources.SampleHttpRequest_1_0_2);
        }
    }
}
