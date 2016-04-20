using System.Collections.Generic;
using System.IO;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.IO
{
    /// <summary>
    /// Reads all text from a file and returns it as a string atom.
    /// </summary>
    public class FileGetTextSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"file-get-text"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Evaluate argument.
            var evaluated = evaluator.Evaluate(arguments[0]);
            if (evaluated.Type != SymbolicExpressionType.String)
            {
                throw new FunctionApplicationException(
                    $"The argument to the function '{Name}' must evaluate to the string type.");
            }

            // Compute filepath.
            var rawPath = evaluated.AsString().Value;
            string path;
            if (rawPath.StartsWith("~"))
            {
                path = rawPath.Replace("~", evaluator.InterpreterDirectory);
            }
            else
            {
                path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.WorkingDirectory, rawPath);
            }

            // Check file exists
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} could not be found for reading.", path);
            }

            // Return file content.
            var content = File.ReadAllText(path);
            return new StringAtom(content);
        }
    }
}
