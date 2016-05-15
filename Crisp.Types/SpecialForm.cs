using System.Collections.Generic;
using System.Linq;

namespace Crisp.Types
{
    /// <summary>
    /// Represents a special form.
    /// </summary>
    public abstract class SpecialForm : Function
    {
        public override bool SkipArgumentEvaluation => true; // Don't evaluate arguments to special forms.

        /// <summary>
        /// Gets the names of the special form.
        /// </summary>
        public abstract IEnumerable<string> Names { get; }

        /// <summary>
        /// Gets the first assigned name of the special form.
        /// </summary>
        public string Name => Names.First();
    }
}
