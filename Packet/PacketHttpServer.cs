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
                var result = runtime.Run("0");

                if (result.Type != SymbolicExpressionType.String)
                {
                    // TODO: Error page.
                }
                else
                {
                    processor.WriteSuccess();
                    processor.OutputStream.Write(result.AsString().Value);
                }

//            if (processor.HttpUrl.Equals("/Test.png"))
//            {
//                Stream fs = File.Open("../../Test.png", FileMode.Open);
//
//                processor.WriteSuccess("image/png");
//                fs.CopyTo(processor.OutputStream.BaseStream);
//                processor.OutputStream.BaseStream.Flush();
//            }
//
//            Console.WriteLine("request: {0}", processor.HttpUrl);
//            processor.WriteSuccess();
//            processor.OutputStream.WriteLine("<html><body><h1>test server</h1>");
//            processor.OutputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
//            processor.OutputStream.WriteLine("url : {0}", processor.HttpUrl);
//
//            processor.OutputStream.WriteLine("<form method=post action=/form>");
//            processor.OutputStream.WriteLine("<input type=text name=foo value=foovalue>");
//            processor.OutputStream.WriteLine("<input type=submit name=bar value=barvalue>");
//            processor.OutputStream.WriteLine("</form>");
            }
        }

        public override void HandlePostRequest(HttpProcessor processor, StreamReader inputStream)
        {
            Console.WriteLine("POST request: {0}", processor.HttpUrl);
            string data = inputStream.ReadToEnd();

            processor.WriteSuccess();
            processor.OutputStream.WriteLine("<html><body><h1>test server</h1>");
            processor.OutputStream.WriteLine("<a href=/test>return</a><p>");
            processor.OutputStream.WriteLine("postbody: <pre>{0}</pre>", data);


        }
    }
}
