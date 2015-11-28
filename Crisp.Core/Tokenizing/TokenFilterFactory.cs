using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Crisp.Core.Tokenizing
{
    public class TokenFilterFactory
    {
        public static TokenFilter GetCommentAndWhitespaceFilter()
        {
            return new TokenFilter(new[]
            {
                TokenType.Whitespace, 
                TokenType.Comment, 
            });
        }

        public static TokenFilter GetCommentWhitespaceAndDirectiveFilter()
        {
            return new TokenFilter(new[]
            {
                TokenType.Whitespace, 
                TokenType.Comment, 
                TokenType.RequireStatement,
            });
        }
    }
}
