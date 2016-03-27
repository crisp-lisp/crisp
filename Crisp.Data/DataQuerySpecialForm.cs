using System.Collections.Generic;
using System.IO;
using System.Linq;

using Community.CsharpSqlite.SQLiteClient;

using Crisp.Core;
using Crisp.Core.Evaluation;
using Crisp.Core.Types;

namespace Crisp.Data
{
    /// <summary>
    /// An implementation of a function to execute SQLite queries against a database.
    /// </summary>
    public class DataQuerySpecialForm : SpecialForm
    {
        public override string Name => "data-query";

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
            var results = new List<SymbolicExpression>();
            using (var connection = new SqliteConnection($"Data Source={path};Version=3;"))
            {
                connection.Open(); // Open connection.
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            // Build up collection of name-value pairs.
                            var names = new List<string>();
                            for (var i = 0; i < reader.FieldCount; i++)
                            {
                                names.Add(reader.GetName(i));
                            }
                            var resultSet = names.Select(k =>
                                new Pair(new StringAtom(k), new StringAtom(reader[k].ToString())) as SymbolicExpression).ToList();
                            results.Add(resultSet.ToProperList());
                        }
                    }
                }
            }
            
            return results.ToProperList();
        }
    }
}
