using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Packet
{
    public class MyHttpServer : HttpServer
    {
        public MyHttpServer(int port)
            : base(port)
        {
        }
        public override void HandleGetRequest(HttpProcessor p)
        {

            if (p.HttpUrl.Equals("/Test.png"))
            {
                Stream fs = File.Open("../../Test.png", FileMode.Open);

                p.WriteSuccess("image/png");
                fs.CopyTo(p.OutputStream.BaseStream);
                p.OutputStream.BaseStream.Flush();
            }

            Console.WriteLine("request: {0}", p.HttpUrl);
            p.WriteSuccess();
            p.OutputStream.WriteLine("<html><body><h1>test server</h1>");
            p.OutputStream.WriteLine("Current Time: " + DateTime.Now.ToString());
            p.OutputStream.WriteLine("url : {0}", p.HttpUrl);

            p.OutputStream.WriteLine("<form method=post action=/form>");
            p.OutputStream.WriteLine("<input type=text name=foo value=foovalue>");
            p.OutputStream.WriteLine("<input type=submit name=bar value=barvalue>");
            p.OutputStream.WriteLine("</form>");
        }

        public override void HandlePostRequest(HttpProcessor p, StreamReader inputData)
        {
            Console.WriteLine("POST request: {0}", p.HttpUrl);
            string data = inputData.ReadToEnd();

            p.WriteSuccess();
            p.OutputStream.WriteLine("<html><body><h1>test server</h1>");
            p.OutputStream.WriteLine("<a href=/test>return</a><p>");
            p.OutputStream.WriteLine("postbody: <pre>{0}</pre>", data);


        }
    }
}
