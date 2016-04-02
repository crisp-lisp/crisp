using System;

namespace Packet.Server
{
    /// <summary>
    /// An implementation of a logger that prints messages to the console window.
    /// </summary>
    internal class ConsoleWindowLogger : ILogger
    {
        public void WriteLine(string msg)
        {
            Console.WriteLine(msg);
        }

        public void Write(string msg)
        {
            Console.Write(msg);
        }

        public void WriteError(Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}
