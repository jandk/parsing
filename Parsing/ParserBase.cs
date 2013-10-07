using System;
using System.Collections.Generic;
using System.IO;

namespace Parsing
{

    #region Extensions

    internal static class Extensions
    {
        public static bool IsDec(this int c)
        {
            return c >= '0' && c <= '9';
        }

        public static int FromDec(this int c)
        {
            if (c >= '0' && c <= '9')
                return c - '0';

            throw new Exception("Invalid decimal character");
        }

        public static bool IsHex(this int c)
        {
            return
                (c >= '0' && c <= '9') ||
                (c >= 'a' && c <= 'f') ||
                (c >= 'A' && c <= 'F');
        }

        public static int FromHex(this int c)
        {
            if (c >= '0' && c <= '9') return (c - '0');
            if (c >= 'a' && c <= 'f') return (c - 'a') + 10;
            if (c >= 'A' && c <= 'F') return (c - 'A') + 10;

            throw new Exception("Invalid hexadecimal character");
        }

        public static bool IsAlpha(this int c)
        {
            return
                (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z');
        }

        public static char FromAlpha(this int c)
        {
            return (char)c;
        }

        public static bool IsAlphaDec(this int c)
        {
            return c.IsAlpha() || c.IsDec();
        }

        public static char FromAlphaDec(this int c)
        {
            return (char)c;
        }
    }

    #endregion

    #region IToken

    public interface IToken
    {
    }

    #endregion

    #region IScanner

    public interface IScanner<out TToken>
        where TToken : IToken
    {
        IEnumerator<TToken> Scan(TextReader reader);
    }

    #endregion

    #region ScannerBase

    public abstract class ScannerBase<TToken>
        : IScanner<TToken>
        where TToken : IToken
    {
        protected TextReader _reader;

        public IEnumerator<TToken> Scan(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _reader = reader;

            return Scan();
        }

        protected abstract IEnumerator<TToken> Scan();

        #region Helpers

        private int? _peek;

        protected int Peek()
        {
            if (!_peek.HasValue)
                _peek = _reader.Read();

            return _peek.Value;
        }

        protected int Read()
        {
            int c = _peek.HasValue ? _peek.Value : _reader.Read();
            _peek = null;
            return c;
        }

        protected bool Maybe(char c)
        {
            if (Peek() == c)
            {
                Read();
                return true;
            }

            return false;
        }

        protected void Expect(char e)
        {
            int c;
            if ((c = Read()) != e)
                Throw(String.Format("Expected '{0}', got '{1}'", e, c));
        }

        protected void Expect(string e)
        {
            for (int i = 0; i < e.Length; i++)
                if (Read() != e[i])
                    Throw(String.Format("Error in expected string '{0}' on place '{1}'", e, i));
        }

        protected void Throw(string e)
        {
            // throw new Exception(String.Format("Scanner error: {0} on Line {1}, Column {2}", e, _line, _column));
            throw new Exception(String.Format("Scanner error: {0}", e));
        }

        #endregion
    }

    #endregion

    #region IParser<T>

    public interface IParser<in TToken>
        where TToken : IToken
    {
        object Parse(IEnumerator<TToken> tokenStream);
    }

    #endregion

    #region Parser

    public abstract class ParserBase<TToken>
        : IParser<TToken> where TToken : IToken
    {
        protected IEnumerator<TToken> _ts;

        public object Parse(IEnumerator<TToken> tokenStream)
        {
            if (tokenStream == null)
                throw new ArgumentNullException("tokenStream");

            _ts = tokenStream;

            return Parse();
        }

        protected abstract object Parse();
    }

    #endregion
}