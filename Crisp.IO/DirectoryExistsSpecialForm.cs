using System.Collections.Generic;
using System.IO;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.IO
{
    /// <summary>
    /// Returns a true boolean atom if a directory exists at the specified path. Otherwise returns a false boolean 
    /// atom.
    /// </summary>
    public class DirectoryExistsSpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"directory-exists"};

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
            var path = rawPath; //Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);

            // Return whether or not file exists.
            return new BooleanAtom(Directory.Exists(path));
        }
    }
}
