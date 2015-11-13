namespace Crisp.Core
{
    /// <summary>
    /// Represents a special form.
    /// </summary>
    public abstract class SpecialForm : Function
    {
        public override bool SkipArgumentEvaluation => true;

        /// <summary>
        /// Gets the name of the special form.
        /// </summary>
        public abstract string Name { get; }
    }
}
