using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Data
{
    public class DataQuerySpecialForm : SpecialForm
    {
        public override string Name => "data-query";

        public override SymbolicExpression Apply(SymbolicExpression expression, IEvaluator evaluator)
        {
            expression.ThrowIfNotList(Name); // Takes a list of arguments.

            var arguments = expression.AsPair().Expand();
            arguments.ThrowIfWrongLength(Name, 2); // Must have two argument.

            // Get arguments.
            var connectionString = evaluator.Evaluate(arguments[0]).AsString().Value;
            var query = evaluator.Evaluate(arguments[1]).AsString().Value;

            // Execute SQLite command.
            var results = new List<SymbolicExpression>();
            using (var connection = new SQLiteConnection($"Data Source={connectionString};Version=3;"))
            {
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var values = reader.GetValues();
                            var resultSet = values.AllKeys.Select(k =>
                                new Pair(new StringAtom(k), new StringAtom(values[k])) as SymbolicExpression).ToList();
                            results.Add(resultSet.ToProperList());
                        }
                    }
                }
            }
            
            return results.ToProperList();
        }
    }
}
