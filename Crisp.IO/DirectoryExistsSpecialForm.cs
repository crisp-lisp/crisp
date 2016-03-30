using System.IO;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.IO
{
    /// <summary>
    /// Returns a true boolean atom if a directory exists at the specified path. Otherwise returns a false boolean 
    /// atom.
    /// </summary>
    public class DirectoryExistsSpecialForm : SpecialForm
    {
        public override string Name => "directory-exists";

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
            return new BooleanAtom(Directory.Exists(path));
        }
    }
}
