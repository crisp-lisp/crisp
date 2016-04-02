using System;
using System.Reflection;
using System.Threading;

using CommandLine.Text;
using Packet.Server;

namespace Packet
{
    class Program
    {
        /// <summary>
        /// Prints the ASCII art title for the application.
        /// </summary>
        private static void PrintAsciiArt()
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
        }

        /// <summary>
        /// Prints version and copyright information for the application.
        /// </summary>
        private static void PrintVersionAndCopyright()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;

            // Automatically build help text based on options class.
            Console.WriteLine($"Server v{version}");
            Console.WriteLine("Copyright © Saul Johnson 2016");
        }
        
        /// <summary>
        /// Prints the help card for the application.
        /// </summary>
        /// <param name="options">The command-line options passed to the program.</param>
        private static void PrintHelp(Options options)
        {
            PrintAsciiArt(); // Print ASCII art.

            var version = Assembly.GetEntryAssembly().GetName().Version;

            // Automatically build help text based on options class.
            var helpText = HelpText.AutoBuild(options);
            helpText.AdditionalNewLineAfterOption = true;
            helpText.Heading = $"Server v{version}";
            helpText.Copyright = "Copyright © Saul Johnson 2016";

            Console.Write(helpText);
        }

        static void Main(string[] args)
        {
            // Parse command-line options.
            var options = new Options();
            if (!CommandLine.Parser.Default.ParseArguments(args, options))
            {
                PrintHelp(options);
                return;
            }

            // Arguments were valid, show title card.
            PrintAsciiArt();
            PrintVersionAndCopyright();
            Console.WriteLine();

            // Start server.
            var server = HttpServerFactory.GetPacketHttpServer(new ServerStartupSettings
            {
                Port = options.Port,
                WebRoot = options.WebRoot,
            });
            var thread = new Thread(server.Listen);
            thread.Start();
        }
    }
}
