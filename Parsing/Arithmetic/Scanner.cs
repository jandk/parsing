using System;
using System.Collections.Generic;

namespace Parsing.Arithmetic
{
    public class Scanner : ScannerBase<Token>
    {
        #region Literals

        // TODO: They shouldn't be here, but be part of the global constants
        private static readonly Dictionary<string, Token> Literals
            = new Dictionary<string, Token>
            {
                { "true", Token.FromBool(true) },
                { "false", Token.FromBool(false) },
                { "nan", Token.FromNumber(double.NaN) },
                { "inf", Token.FromNumber(double.PositiveInfinity) },
            };

        #endregion

        #region ScannerBase<Token, Kind> Members

        protected override IEnumerator<Token> Scan()
        {
            while (Peek() != -1)
            {
                if (Maybe(' '))
                    continue;

                if (Peek().IsDec())
                {
                    double value = ScanNumber();
                    yield return Token.FromNumber(value);
                    continue;
                }

                if (Peek().IsAlpha())
                {
                    string value = ScanIdentifier();

                    if (Literals.ContainsKey(value))
                        yield return Literals[value];
                    else
                        yield return Token.FromIdentifier(value);

                    continue;
                }

                #region Operators

                int read;
                switch (read = Read())
                {
                    case '+':
                        yield return Token.FromKind(Kind.OpPlus);
                        break;

                    case '-':
                        yield return Token.FromKind(Kind.OpMinus);
                        break;

                    case '*':
                        yield return Token.FromKind(Kind.OpMultiply);
                        break;

                    case '/':
                        yield return Token.FromKind(Kind.OpDivide);
                        break;

                    case '%':
                        yield return Token.FromKind(Kind.OpModulo);
                        break;

                    case '~':
                        yield return Token.FromKind(Kind.OpBinaryNot);
                        break;

                    case '!':
                        yield return Token.FromKind(Kind.OpLogicalNot);
                        break;

                    case '(':
                        yield return Token.FromKind(Kind.ParenLeft);
                        break;

                    case ')':
                        yield return Token.FromKind(Kind.ParenRight);
                        break;

                    case ',':
                        yield return Token.FromKind(Kind.Comma);
                        break;

                    default:
                        Throw(string.Format("Illegal charcter '{0}'", (char)read));
                        break;
                }

                #endregion
            }

            yield return Token.FromKind(Kind.Eof);
        }

        #endregion

        #region Numbers

        private double ScanNumber()
        {
            if (!Peek().IsDec())
                Throw("Expected a digit");

            // Integer part
            double d = 0;
            if (!Maybe('0'))
                while (Peek().IsDec())
                    d = (d * 10) + Read().FromDec();

            // Fractional part
            if (Maybe('.'))
            {
                double f = 0;
                double w = 0.1;

                if (!Peek().IsDec())
                    Throw("At least one digit after '.'");

                while (Peek().IsDec())
                {
                    f += w * Read().FromDec();
                    w *= 0.1;
                }

                d += f;
            }

            // Exponent
            if (Maybe('e') || Maybe('E'))
            {
                bool negate = false;

                if (Peek() == '+' || Peek() == '-')
                {
                    negate = Peek() == '-';
                    Read();
                }

                if (!Peek().IsDec())
                    Throw("At least one digit after 'e' or 'E'.");

                double e = 0;
                while (Peek().IsDec())
                    e = (e * 10) + Read().FromDec();

                if (negate)
                    e = -e;

                d *= Math.Pow(10, e);
            }

            return d;
        }

        #endregion

        #region Identifiers

        private string ScanIdentifier()
        {
            if (!Peek().IsAlpha())
                Throw("Expected a letter");

            string identifier = string.Empty;
            while (Peek().IsAlphaDec())
                identifier += Read().FromAlphaDec();

            return identifier;
        }

        #endregion
    }
}