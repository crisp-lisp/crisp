using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Packet
{
    public abstract class HttpServer
    {
        protected int _port;
        private TcpListener _listener;
        private bool isActive = true;

        protected HttpServer(int port)
        {
            _port = port;
        }


        public void listen()
        {
            _listener = new TcpListener(_port);
            _listener.Start();
            while (isActive)
            {
                TcpClient s = _listener.AcceptTcpClient();
                HttpProcessor processot = new HttpProcessor(s, this);
                Thread thread = new Thread(new ThreadStart(processot.Process));
                thread.Start();
                Thread.Sleep(1);
            }
        }

        public abstract void HandleGetRequest(HttpProcessor p);
        public abstract void HandlePostRequest(HttpProcessor p, StreamReader inputData);
    }
}
