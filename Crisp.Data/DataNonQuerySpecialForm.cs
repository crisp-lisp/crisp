using System.IO;

using Community.CsharpSqlite.SQLiteClient;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Data
{
    /// <summary>
    /// An implementation of a function to execute SQLite non-queries against a database.
    /// </summary>
    public class DataNonQuerySpecialForm : SpecialForm
    {
        public override string Name => "data-non-query";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Get arguments.
            var rawPath = evaluator.Evaluate(arguments[0]).AsString().Value;
            var path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);
            var query = evaluator.Evaluate(arguments[1]).AsString().Value;

            // Execute SQLite command.
            int result;
            using (var connection = new SqliteConnection($"Data Source={path};Version=3;"))
            {
                connection.Open(); // Open connection.
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteNonQuery(); // Return number of affected rows.
                }
            }
            
            return new NumericAtom(result);
        }
    }
}
