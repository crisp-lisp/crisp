using System.Collections.Generic;

namespace Crisp.Core.Preprocessing
{
    /// <summary>
    /// Represents a dependency finder.
    /// </summary>
    public interface IDependencyFinder
    {
        /// <summary>
        /// Recursively searches for all dependencies required by the source file at the specified path.
        /// </summary>
        /// <param name="filepath">The path of the source file.</param>
        /// <returns></returns> 
        IList<string> FindDependencyFilepaths(string filepath);
    }
}
