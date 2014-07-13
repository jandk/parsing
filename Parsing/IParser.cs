using System.Collections.Generic;

namespace Parsing
{
    public interface IParser<in TToken, out TResult>
        where TToken : IToken
    {
        TResult Parse(IEnumerator<TToken> tokenStream);
    }
}