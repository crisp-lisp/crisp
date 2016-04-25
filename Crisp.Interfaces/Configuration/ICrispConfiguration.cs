namespace Crisp.Interfaces.Configuration
{
    /// <summary>
    /// Represents a set of application configuration settings.
    /// </summary>
    public interface ICrispConfiguration
    {
        /// <summary>
        /// Gets or sets the directory that special form libraries will be loaded from.
        /// </summary>
        string SpecialFormDirectory { get; set; }
    }
}
