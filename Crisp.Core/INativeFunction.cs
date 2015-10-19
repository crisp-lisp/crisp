namespace Crisp.Core
{   
    /// <summary>
    /// Implemented by classes that provide native functions to the interpreter.
    /// </summary>
    public interface INativeFunction
    {
        /// <summary>
        /// Gets the name of the native function provided.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Contains a reference to the native function host, usually the executing interpreter.
        /// </summary>
        INativeFunctionHost Host { get; set; }

        /// <summary>
        /// Applies the native function to an expression.
        /// </summary>
        /// <param name="input">The expression to apply the function to.</param>
        /// <returns></returns>
        SymbolicExpression Apply(SymbolicExpression input);
    }
}
