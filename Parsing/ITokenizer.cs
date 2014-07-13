using System.Collections.Generic;
using System.IO;

namespace Parsing
{
    public interface ITokenizer<out TToken>
        where TToken : IToken
    {
        IEnumerator<TToken> Tokenize(TextReader reader);
    }
}