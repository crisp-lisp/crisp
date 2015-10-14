using Crisp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Visualization
{
    public class MermaidConverter
    {
        public string Serialize(SymbolicExpression expression, string nodeId = "")
        {
            var ex = "N" + nodeId;
            var l = "N" + nodeId + "L";
            var r = "N" + nodeId + "R";

            var s = new StringBuilder();
            s.Append(ex)
                .Append("-->")
                .Append(l)
                .AppendLine(";")
                .Append(ex)
                .Append("-->")
                .Append(r)
                .AppendLine(";")
                .Append(expression.LeftExpression == null ? "" : Serialize(expression.LeftExpression, nodeId + "L"))
                .Append(expression.RightExpression == null ? "" : (Serialize(expression.RightExpression, nodeId + "R")));
            return s.ToString();
        }
    }
}
