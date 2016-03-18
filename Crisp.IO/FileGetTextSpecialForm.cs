using System.IO;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.IO
{
    public class FileGetTextSpecialForm : SpecialForm
    {
        public override string Name => "file-get-text";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 1); // Must have one argument.

            // Check file exists.
            var path = evaluator.Evaluate(arguments[0]).AsString().Value;
            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"File {path} could not be found for reading.", path);
            }

            // Read file.
            var content = File.ReadAllText(path);

            return new StringAtom(content);
        }
    }
}
