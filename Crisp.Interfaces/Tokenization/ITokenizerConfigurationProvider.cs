using System.Collections.Generic;

using Crisp.Enums;

namespace Crisp.Interfaces.Tokenization
{
    public interface ITokenizerConfigurationProvider
    {
        Dictionary<string, TokenType> Get();
    }
}
