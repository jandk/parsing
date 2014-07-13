using System;
using System.Collections.Generic;
using System.IO;

namespace Parsing
{
    public abstract class Parser<TToken, TResult>
        : IParser<TToken, TResult> where TToken : IToken
    {
        protected IEnumerator<TToken> _ts;

        public TResult Parse(IEnumerator<TToken> tokenStream)
        {
            if (tokenStream == null)
                throw new ArgumentNullException("tokenStream");

            _ts = tokenStream;

            return Parse();
        }

        protected abstract TResult Parse();
    }
}