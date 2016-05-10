using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;

using Packet.IoC;

namespace Packet
{
    class Program
    {
        /// <summary>
        /// Prints the title card for the application.
        /// </summary>
        private static void PrintTitleCard()
        {
            // Write ASCII art.
            var titleCard = new[]
            {
                "    ____                                ",
                "    /    )               /              ",
                "   /____/    __    __   / __    __  _/_ ",
                "  /        /   ) /   ' /(     /___) /   ",
                " /________(___(_(___ _/___\\__(___ _(_   "
            };
            foreach (var line in titleCard)
            {
                Console.WriteLine(line);
            }

            // Print version and copyright.
            var assembly = Assembly.GetEntryAssembly();
            var version = assembly.GetName().Version;
            var copyright = FileVersionInfo.GetVersionInfo(assembly.Location).LegalCopyright;
            Console.WriteLine($"Server v{version}");
            Console.WriteLine(copyright);
        }
        
        static void Main(string[] args)
        {
            // Print title card.
            PrintTitleCard();

            // Start server.
            var server = HttpServerFactory.GetPacketHttpServer();
            var thread = new Thread(server.Listen);
            thread.Start();
        }
    }
}
