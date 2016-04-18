using Crisp.Shared;

namespace Crisp.Configuration
{
    /// <summary>
    /// Represents a set of application configuration settings.
    /// </summary>
    public class CrispConfiguration : ICrispConfiguration
    {
        /// <summary>
        /// Gets or sets the directory that special form libraries will be loaded from.
        /// </summary>
        public string SpecialFormDirectory { get; set; }
    }
}
