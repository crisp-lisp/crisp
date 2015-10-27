using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Parsing
{
    internal static class ParsingExtensions
    {
        public static IList<T> RemoveFirst<T>(this IList<T> collection)
        {
            return collection.Except(new[] { collection.First() }).ToList();
        }
    }
}
