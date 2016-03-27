using System.IO;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.IO
{
    /// <summary>
    /// An implementation of a function that will write the given text to the file at the specified path.
    /// </summary>
    public class FileSetTextSpecialForm : SpecialForm
    {
        public override string Name => "file-set-text";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Check arguments evaluate to the correct type.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new RuntimeException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }

            // Compute filepath.
            var rawPath = evaluated[0].AsString().Value;
            var path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);
            var text = evaluated[1].AsString().Value;

            // Try to Write file.
            try
            {
                File.WriteAllText(path, text);
                return new BooleanAtom(true); // Success.
            }
            catch
            {
                return new BooleanAtom(false); // Failure.
            }
        }
    }
}
