using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core
{
    /// <summary>
    /// Represents a symbolic expression in an expression tree.
    /// </summary>
    public class SymbolicExpression
    {
        private string symbolValue;

        private string stringValue;

        private int? integerValue;

        private double? realValue;

        public SymbolicExpression LeftExpression { get; private set; }

        public SymbolicExpression RightExpression { get; private set; }

        /// <summary>
        /// Gets whether or not this expression is atomic.
        /// </summary>
        public bool IsAtomic
        {
            get
            {
                return symbolValue != null
                    || stringValue != null
                    || integerValue != null
                    || realValue != null;
            }
        }

        public SymbolicExpression(SymbolicExpression leftExpression, SymbolicExpression rightExpression)
        {
            LeftExpression = leftExpression;
            RightExpression = rightExpression;
        }
        
        public SymbolicExpression(string value, bool isString = false)
        {
            if (isString)
                stringValue = value;
            else
                symbolValue = value;
        }

        public SymbolicExpression(int value)
        {
            integerValue = value;
        }

        public SymbolicExpression(double value)
        {
            realValue = value;
        }

        public string AsSymbol()
        {
            if (symbolValue == null)
                throw new Exception("Expected symbol but didn't get a symbol.");

            return symbolValue;
        }

        public string AsString()
        {
            if (stringValue == null)
                throw new Exception("Expected string but didn't get a string.");

            return stringValue;
        }

        public int AsInteger()
        {
            if (integerValue == null)
                throw new Exception("Expected integer but didn't get an integer");

            return integerValue.Value;
        }

        public double AsReal()
        {
            if (realValue == null)
                throw new Exception("Expected real but didn't get a real.");

            return realValue.Value;
        }
    }
}
