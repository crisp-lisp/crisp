namespace Crisp.Core
{   
    /// <summary>
    /// Implemented by classes that provide functions to the interpreter.
    /// </summary>
    public interface IFunction
    {
        /// <summary>
        /// Gets the name of the function .
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Contains a reference to the function host, usually the executing interpreter.
        /// </summary>
        IFunctionHost Host { get; set; }

        /// <summary>
        /// Applies the function to an expression.
        /// </summary>
        /// <param name="input">The expression to apply the function to.</param>
        /// <returns></returns>
        SymbolicExpression Apply(SymbolicExpression input, Context context);
    }
}
