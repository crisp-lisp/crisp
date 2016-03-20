using Community.CsharpSqlite.SQLiteClient;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Data
{
    public class DataNonQuerySpecialForm : SpecialForm
    {
        public override string Name => "data-non-query";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two argument.

            // Get arguments.
            var connectionString = evaluator.Evaluate(arguments[0]).AsString().Value;
            var query = evaluator.Evaluate(arguments[1]).AsString().Value;

            // Execute SQLite command.
            int result;
            using (var connection = new SqliteConnection($"Data Source={connectionString};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    result = command.ExecuteNonQuery();
                }
            }
            
            return new NumericAtom(result);
        }
    }
}
