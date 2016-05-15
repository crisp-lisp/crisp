using System;

using Packet.Interfaces.Logging;

namespace Packet.Logging
{
    /// <summary>
    /// An implementation of a logger that prints messages to the console window.
    /// </summary>
    public class ConsoleWindowLogger : ILogger
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
