using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;

namespace Packet
{
    public class PacketHttpServer : HttpServer
    {
        public PacketHttpServer(int port)
            : base(port)
        {
        }

        public override void HandleGetRequest(HttpProcessor processor)
        {
            if (processor.HttpUrl.EndsWith(".csp"))
            {
                var runtime = CrispRuntimeFactory.GetCrispRuntime("public-www/" + processor.HttpUrl);
                var result = runtime.Run($"\"{processor.HttpUrl}\" \"GET\" nil");

                if (result.Type != SymbolicExpressionType.String)
                {
                    // TODO: Error page.
                }
                else
                {
                    processor.WriteSuccess();
                    processor.OutputStream.Write(result.AsString().Value);
                }
            }
            else
            {
                // TODO: Serve static files or whatever.
            }
        }

        public override void HandlePostRequest(HttpProcessor processor, StreamReader inputStream)
        {
            // Get post data.
            var data = inputStream.ReadToEnd();

            if (processor.HttpUrl.EndsWith(".csp"))
            {
                var runtime = CrispRuntimeFactory.GetCrispRuntime("public-www/" + processor.HttpUrl);
                var result = runtime.Run($"\"{processor.HttpUrl}\" \"POST\" \"{data}\"");

                if (result.Type != SymbolicExpressionType.String)
                {
                    // TODO: Error page.
                }
                else
                {
                    processor.WriteSuccess();
                    processor.OutputStream.Write(result.AsString().Value);
                }
            }
            else
            {
                // TODO: Serve static files or whatever.
            }
        }
    }
}
