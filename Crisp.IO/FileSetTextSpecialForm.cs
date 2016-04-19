using System.Collections.Generic;
using System.IO;
using System.Linq;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.IO
{
    /// <summary>
    /// Writes the given text to the file at the specified path and returns a true boolean atom on success. On failure
    /// returns a false boolean atom.
    /// </summary>
    public class FileSetTextSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"file-set-text"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Check arguments evaluate to the correct type.
            var evaluated = arguments.Select(evaluator.Evaluate).ToArray();
            if (evaluated.Any(e => e.Type != SymbolicExpressionType.String))
            {
                throw new FunctionApplicationException(
                    $"The arguments to the function '{Name}' must all evaluate to the string type.");
            }
            
            // Compute filepath.
            var rawPath = evaluated[0].AsString().Value;
            string path;
            if (rawPath.StartsWith("~"))
            {
                path = rawPath.Replace("~", evaluator.InterpreterDirectory);
            }
            else
            {
                path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFileDirectory, rawPath);
            }
            
            // Try to Write file.
            try
            {
                var text = evaluated[1].AsString().Value;
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
