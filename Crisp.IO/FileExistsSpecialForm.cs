using System.IO;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.IO
{
    /// <summary>
    /// An implementation of a function that will return true if a file exists at the specified path or false 
    /// otherwise.
    /// </summary>
    public class FileExistsSpecialForm : SpecialForm
    {
        public override string Name => "file-exists";

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

            // Return whether or not file exists.
            return new BooleanAtom(File.Exists(path));
        }
    }
}
