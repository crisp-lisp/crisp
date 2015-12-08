using CommandLine;

namespace Crisp
{
    /// <summary>
    /// Represents a container object to hold arguments passed via the command line.
    /// </summary>
    internal class Options
    {
        /// <summary>
        /// Gets or sets the input file to be interpreted.
        /// </summary>
        [Option('f', "file", Required = true, HelpText = "Input file to be interpreted.")]
        public string InputFile { get; set; }

        /// <summary>
        /// Gets or sets he arguments to the program contained in the input file.
        /// </summary>
        [Option('a', "args", Required = false, HelpText = "The arguments to the program as a list.")]
        public string Args { get; set; }
    }
}
