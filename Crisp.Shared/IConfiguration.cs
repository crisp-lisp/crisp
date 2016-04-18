namespace Crisp.Shared
{
    public interface ICrispConfiguration
    {
        /// <summary>
        /// Gets or sets the directory that special form libraries will be loaded from.
        /// </summary>
        string SpecialFormDirectory { get; set; }
    }
}
