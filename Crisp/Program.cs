using System;
using System.IO;
using System.Reflection;
using System.Text;
using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;
using Crisp.Visualization;
using SimpleInjector;

namespace Crisp
{
    class Program
    {
        private static void PrintHelp()
        {
            var version = Assembly.GetEntryAssembly().GetName().Version;
            Console.Write("      __                         \r\n"
                          + "    /    )         ,             \r\n"
                          + "   /        )__       __      __  \r\n"
                          + "  /        /   ) /   (_ `   /   )\r\n"
                          + " (____/___/_____/___(__)___/___/ \r\n"
                          + "                          /      \r\n"
                          + "                         /       \r\n"
                          + $" Intepreter v{version}             \r\n"
                          + " Usage: Crisp.exe <Filename> <Args>\r\n"
                          + " Notes: The source file should pass back a lambda\r\n"
                          + "        which may take arguments passed to the   \r\n"
                          + "        program from the command line in <Args>. \r\n");
        }
        
        static void Main(string[] args)
        {
            // Dependency injection.
            var container = new Container();
            container.Register<IDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IRequirePathTransformer, RequirePathTransformer>();
            container.Register<IRequirePathExtractor, RequirePathExtractor>();
            container.Register<IPreprocessor, Preprocessor>();
            container.Verify();

            // Not enough arguments given.
            if (args.Length < 1)
            {
                PrintHelp();
                return;
            }

            // File not found.
            if (!File.Exists(args[0]))
            {
                PrintHelp();
                Console.WriteLine("Error: Could not find input file.");
                return;
            }

            // Pre-process input. The pre-processor does the tokenizing.
            var preprocessor = container.GetInstance<IPreprocessor>();
            var tokens = preprocessor.Process(args[0]);

            // Create expression tree.
            var parser = new Parser();
            var parsed = parser.CreateExpressionTree(tokens);

            // Create evaluator.
            var evaluator = new Evaluator("native");
            preprocessor.BindExpressions(evaluator); // Load required bindings from preprocessor.

            // Evaluate program, which should give a function.
            var result = evaluator.Evaluate(parsed);
            if (result.Type != SymbolicExpressionType.Function)
            {
                PrintHelp();
                Console.WriteLine("Error: The program must return a lambda for execution.");
                return;
            }

            // Parse arguments and pass them into the function.
            var sb = new StringBuilder();
            for (var i = 1; i < args.Length; i++)
            {
                sb.Append(sb.Length != 0 ? " " : "");
                sb.Append(args[i]);
            }
            var argumentTokens = TokenizerFactory.GetCrispTokenizer(true).Tokenize($"({sb})");
            var parsedArgumentList = parser.CreateExpressionTree(argumentTokens);

            var output = result.AsFunction().Apply(parsedArgumentList, null); // Don't need an evaluator. This is a closure.

            // Write result to output.
            Console.Write(new LispSerializer().Serialize(output));
        }
    }
}




























































