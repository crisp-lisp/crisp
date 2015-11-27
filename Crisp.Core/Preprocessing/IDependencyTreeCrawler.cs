using System.Collections.Generic;

namespace Crisp.Core.Preprocessing
{
    public interface IDependencyTreeCrawler
    {
        IList<string> Crawl(string filepath, IList<string> loaded = null);
    }
}
