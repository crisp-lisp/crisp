namespace Crisp.Core.Types
{
    /// <summary>
    /// Represents a special form.
    /// </summary>
    public abstract class SpecialForm : Function
    {
        public override bool SkipArgumentEvaluation => true; // Don't evaluate arguments to special forms.

        /// <summary>
        /// Gets the name of the special form.
        /// </summary>
        public abstract string Name { get; }
    }
}
