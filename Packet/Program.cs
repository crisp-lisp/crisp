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
        /// Prints the help card for the application.
        /// </summary>
        /// <param name="options">The command-line options passed to the program.</param>
        private static void PrintHelp(Options options)
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
