using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Crisp.Core;

namespace Crisp.Basic.Tests
{
    internal static class SpecialFormTestExtensions
    {
        private static SymbolicExpression ConvertToProperList(IList<SymbolicExpression> members, int index = 0)
        {
            return new Pair(members[index],
                index == members.Count - 1 ? SymbolAtom.Nil : ConvertToProperList(members, index + 1));
        }

        public static SymbolicExpression ToProperList(this IList<SymbolicExpression> members)
        {
            return ConvertToProperList(members);
        }
    }
}
