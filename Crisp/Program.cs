using System;
using System.IO;
using System.Reflection;

using Crisp.Configuration;
using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Parsing;
using Crisp.Core.Preprocessing;
using Crisp.Core.Tokenizing;
using Crisp.Core.Types;
using Crisp.Visualization;

using SimpleInjector;

using CommandLine.Text;

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
            helpText.Copyright = "Copyright © Saul Johnson 2015";

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
                Console.WriteLine("Could not file input file.");
                return;
            }

            // Dependency injection.
            var container = new Container();
            container.Register<IInterpreterDirectoryPathProvider, InterpreterDirectoryPathProvider>();
            container.Register<IConfigurationProvider, ConfigurationProvider>();
            container.Register<ISpecialFormDirectoryPathProvider, SpecialFormDirectoryPathProvider>();
            container.Register<IRequirePathTransformer, RequirePathTransformer>();
            container.Register<IRequirePathExtractor, RequirePathExtractor>();
            container.Register(() => TokenizerFactory.GetCrispTokenizer());
            container.Register<IParser, Parser>();
            container.Register<IDependencyFinder, DependencyFinder>();
            container.Register<IDependencyLoader, DependencyLoader>();
            container.Register<ISpecialFormLoader, SpecialFormLoader>();
            container.Verify();

            // Get special form bindings.
            var specialFormLoader = container.GetInstance<ISpecialFormLoader>();
            var specialFormBindings = specialFormLoader.GetBindings();

            // Get dependencies for input source file.
            var dependencyLoader = container.GetInstance<IDependencyLoader>();
            var dependencyBindings = dependencyLoader.GetBindings(options.InputFile);

            // Prepare evaluator with special forms and dependencies.
            var evaluator = new Evaluator(); 
            evaluator.MutableBind(specialFormBindings);
            evaluator.MutableBind(dependencyBindings);

            // Read and tokenize input source file.
            var source = File.ReadAllText(options.InputFile);
            var filter = TokenFilterFactory.GetCommentWhitespaceAndDirectiveFilter();
            var tokens = filter.Filter(container.GetInstance<ITokenizer>().Tokenize(source));
            
            // Create expression tree.
            var parser = container.GetInstance<IParser>();
            var parsed = parser.CreateExpressionTree(tokens);
            
            // Evaluate program, which should give a function.
            var result = evaluator.Evaluate(parsed);
            if (result.Type != SymbolicExpressionType.Function)
            {
                PrintHelp(options);
                Console.WriteLine("Error: The program must return a lambda for execution.");
                return;
            }
            
            // Parse arguments passed in.
            var argumentTokens = TokenizerFactory.GetCrispTokenizer(true).Tokenize($"({options.Args})");
            var parsedArgumentList = parser.CreateExpressionTree(argumentTokens);

            // Don't need an evaluator. This is a closure.
            var output = result.AsFunction().Apply(parsedArgumentList, null); 
            
            // Write result to output.
            Console.Write(new LispSerializer().Serialize(output));
        }
    }
}




























































