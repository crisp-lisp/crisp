using System.Collections.Generic;
using System.IO;
using Community.CsharpSqlite.SQLiteClient;
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
            var rawPath = evaluator.Evaluate(arguments[0]).AsString().Value;
            var path = Path.IsPathRooted(rawPath) ? rawPath : Path.Combine(evaluator.SourceFolderPath, rawPath);
            var query = evaluator.Evaluate(arguments[1]).AsString().Value;

            // Execute SQLite command.
            var results = new List<SymbolicExpression>();
            using (var connection = new SqliteConnection($"Data Source={path};Version=3;"))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = query;
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var names = new List<string>();
                            for (int i = 0; i < reader.FieldCount; i++)
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
