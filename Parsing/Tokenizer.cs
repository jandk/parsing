using System;
using System.Collections.Generic;
using System.IO;

namespace Parsing
{
    public abstract class Tokenizer<TToken>
        : ITokenizer<TToken>
        where TToken : IToken
    {
        protected TextReader _reader;

        public void Reset()
        {
            _reader = null;
        }

        public IEnumerator<TToken> Tokenize(TextReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException("reader");

            _reader = reader;

            return Tokenize();
        }

        protected abstract IEnumerator<TToken> Tokenize();

        #region Reading

        private int? _peek;
        private int _line = 1;
        private int _lineRef = 1;
        private int _col;
        private int _colPrev;

        protected int Read()
        {
            int c;
            if (_peek.HasValue)
            {
                c = _peek.Value;
                _peek = null;
            }
            else c = _reader.Read();

            switch (c)
            {
                case '\r':
                    if (Peek() == '\n')
                        _peek = null;
                    c = '\n';
                    NextLine();
                    break;
                case '\n':
                    NextLine();
                    break;
                default:
                    _col++;
                    break;
            }

            return c;
        }

        protected int Peek()
        {
            if (!_peek.HasValue)
                _peek = _reader.Read();
            return _peek.Value;
        }

        protected int Peek2()
        {
            if (_peek.HasValue)
                return _peek.Value;
            return Peek();
        }

        private void NextLine()
        {
            _line++;
            _lineRef++;
            _colPrev = _col;
            _col = 0;
        }

        #endregion

        #region Characters

        /// <summary>
        ///  Checks if the next character is alpha.
        /// </summary>
        protected bool IsAlpha()
        {
            int c = Peek();
            return
                (c >= 'A' && c <= 'Z') ||
                (c >= 'a' && c <= 'z');
        }

        /// <summary>
        ///  Checks if the next character is decimal.
        /// </summary>
        protected bool IsDec()
        {
            int c = Peek();
            return c >= '0' && c <= '9';
        }

        /// <summary>
        ///  Reads the next character number as an integer.
        /// </summary>
        protected int GetDec()
        {
            int c = Read();
            if (c >= '0' && c <= '9')
                return (c - '0');

            throw NewTokenizerException("Invalid decimal character");
        }

        /// <summary>
        ///  Checks if the next character is hexadecimal.
        /// </summary>
        protected bool IsHex()
        {
            int c = Peek();
            return
                (c >= '0' && c <= '9') ||
                (c >= 'A' && c <= 'F') ||
                (c >= 'a' && c <= 'f');
        }

        /// <summary>
        ///  Converts the next character from hexadecimal to integer representation.
        /// </summary>
        protected int GetHex()
        {
            int c = Read();
            if (c >= '0' && c <= '9') return (c - '0');
            if (c >= 'a' && c <= 'f') return (c - 'a') + 10;
            if (c >= 'A' && c <= 'F') return (c - 'A') + 10;

            throw NewTokenizerException("Invalid hexadecimal character");
        }

        protected bool IsOctal()
        {
            int c = Peek();
            return c >= '0' && c <= '7';
        }

        protected int GetOctal()
        {
            int c = Read();
            if (c >= '0' && c <= '7') return (c - '0');

            throw NewTokenizerException("Invalid hexadecimal character");
        }

        #endregion

        #region Helpers

        /// <summary>
        ///  Check if the next character is in a list of characters,
        ///  if so, return the one that was found.
        /// </summary>
        /// <param name="chars">
        ///  The characters to check for.
        /// </param>
        /// <returns>
        ///  The character that was found, or null if it wasn't.
        /// </returns>
        protected int? Maybe(params int[] chars)
        {
            int ch = Peek();
            if (Array.IndexOf(chars, ch) < 0)
                return null;

            return ch;
        }

        /// <summary>
        ///  Check if the next character is the character we want.
        ///  If it is, we discard it.
        /// </summary>
        /// <param name="c">
        ///  The character we want to check for.
        /// </param>
        /// <returns>
        ///  True if it was the correct character, false it if wasn't.
        /// </returns>
        protected bool Maybe(int c)
        {
            if (Peek() != c)
                return false;

            Read();
            return true;
        }

        /// <summary>
        ///  Check if the next character is the character we expect.
        ///  If it is not, we throw an exception.
        /// </summary>
        /// <param name="c">
        ///  The character we want to check for.
        /// </param>
        protected void Expect(int c)
        {
            int ch;
            if ((ch = Read()) != c)
                throw NewTokenizerException(String.Format("Expected '{0}', got '{1}'", (char)c, (char)ch));
        }

        /// <summary>
        ///  Check if the next characters form the string we expect them to.
        /// </summary>
        /// <param name="s">
        ///  The string we want to check for.
        /// </param>
        protected void Expect(string s)
        {
            for (int i = 0; i < s.Length; i++)
                if (Read() != s[i])
                    throw NewTokenizerException(String.Format("Error in expected string '{0}' on place '{1}'", s, i));
        }

        /// <summary>
        ///  Returns a new exception, containing the current position.
        /// </summary>
        /// <param name="message">
        ///  The message explaining what went wrong.
        /// </param>
        protected TokenizerException NewTokenizerException(string message)
        {
            string file = "<null>";
            var reader = _reader as StreamReader;
            if (reader != null)
            {
                var stream = reader.BaseStream as FileStream;
                file = stream.Name;
            }

            var location = new Location
            {
                File = file,
                Line = _line,
                Column = _col,
            };
            return new TokenizerException(message, location);
        }

        #endregion

    }
}