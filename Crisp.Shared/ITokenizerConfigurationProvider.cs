using System.Collections.Generic;

namespace Crisp.Shared
{
    public interface ITokenizerConfigurationProvider
    {
        Dictionary<string, TokenType> Get();
    }
}
