using System.Collections.Generic;

using Community.CsharpSqlite.SQLiteClient;

using Crisp.Shared;
using Crisp.Types;

namespace Crisp.Data
{
    /// <summary>
    /// Executes an SQLite non-query against a database file, returning the number of affected rows.
    /// </summary>
    public class DataNonQuerySpecialForm : SpecialForm
    {
        public override IEnumerable<string> Names => new List<string> {"data-non-query"};

        public override ISymbolicExpression Apply(ISymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two arguments.

            // Get arguments.
            var rawPath = evaluator.Evaluate(arguments[0]).AsString().Value;
            var path = rawPath; //Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);
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
