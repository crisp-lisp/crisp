using System.IO;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.IO
{
    /// <summary>
    /// Reads all text from a file and returns it as a string atom.
    /// </summary>
    public class FileGetTextSpecialForm : SpecialForm
    {
        public override string Name => "file-get-text";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Evaluate argument.
            var evaluated = evaluator.Evaluate(arguments[0]);
            if (evaluated.Type != SymbolicExpressionType.String)
            {
                throw new RuntimeException(
                    $"The argument to the function '{Name}' must evaluate to the string type.");
            }

            // Compute filepath.
            var rawPath = evaluated.AsString().Value;
            var path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);

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
