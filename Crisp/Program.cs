using System;
using System.IO;
using System.Reflection;

using CommandLine.Text;

using Crisp.Visualization;

namespace Crisp
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
                "      __                         ",
                "    /    )         ,             ",
                "   /        )__       __      __ ",
                "  /        /   ) /   (_ `   /   )",
                " (____/___/_____/___(__)___/___/ ",
                "                          /      ",
                "                         /       ",
            };
            foreach (var line in titleCard)
            {
                Console.WriteLine(line);
            }

            var version = Assembly.GetEntryAssembly().GetName().Version;

            // Automatically build help text based on options class.
            var helpText = HelpText.AutoBuild(options);
            helpText.AdditionalNewLineAfterOption = true;
            helpText.Heading = $"Interpreter v{version}";
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

            // Check file exists.
            if (!File.Exists(options.InputFile))
            {
                PrintHelp(options);
                Console.WriteLine("Could not find input file.");
                return;
            }

            // Get runtime and run program.
            var runtime = CrispRuntimeFactory.GetCrispRuntime(options.InputFile);
            var output = runtime.Run(options.Args);
            
            // Write result to output.
            Console.Write(new LispSerializer().Serialize(output));
        }
    }
}




























































