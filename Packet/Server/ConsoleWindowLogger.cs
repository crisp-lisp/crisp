using System;

namespace Packet.Server
{
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
            Console.WriteLine("An error occurred:");
            Console.WriteLine(ex.Message);
            Console.WriteLine("Begin stack trace...");
            Console.WriteLine(ex.StackTrace);
            Console.WriteLine("Error string:");
            Console.WriteLine(ex.ToString());
        }
    }
}
