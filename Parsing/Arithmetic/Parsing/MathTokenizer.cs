using System;
using System.Collections.Generic;

namespace Parsing.Arithmetic.Parsing
{
    public class MathTokenizer
        : Tokenizer<MathToken>
    {
        #region Literals

        // TODO: They shouldn't be here, but be part of the global constants
        private static readonly Dictionary<string, MathToken> Literals
            = new Dictionary<string, MathToken>
            {
                { "true", MathToken.FromBool(true) },
                { "false", MathToken.FromBool(false) },
                { "nan", MathToken.FromNumber(double.NaN) },
                { "inf", MathToken.FromNumber(double.PositiveInfinity) },
            };

        #endregion

        #region Tokenizer<Token> Members

        protected override IEnumerator<MathToken> Tokenize()
        {
            while (Peek() != -1)
            {
                if (Maybe(' '))
                    continue;

                if (IsDec())
                {
                    double value = ScanNumber();
                    yield return MathToken.FromNumber(value);
                    continue;
                }

                if (IsAlpha())
                {
                    string value = ScanIdentifier();

                    if (Literals.ContainsKey(value))
                        yield return Literals[value];
                    else
                        yield return MathToken.FromIdentifier(value);

                    continue;
                }

                #region Operators

                int read;
                switch (read = Read())
                {
                    case '+':
                        yield return MathToken.FromKind(Kind.OpPlus);
                        break;

                    case '-':
                        yield return MathToken.FromKind(Kind.OpMinus);
                        break;

                    case '*':
                        if (Maybe('*'))
                            yield return MathToken.FromKind(Kind.OpPower);
                        else
                            yield return MathToken.FromKind(Kind.OpMultiply);
                        break;

                    case '/':
                        yield return MathToken.FromKind(Kind.OpDivide);
                        break;

                    case '%':
                        yield return MathToken.FromKind(Kind.OpModulo);
                        break;

                    case '~':
                        yield return MathToken.FromKind(Kind.OpBinaryNot);
                        break;

                    case '!':
                        yield return MathToken.FromKind(Kind.OpLogicalNot);
                        break;

                    case '(':
                        yield return MathToken.FromKind(Kind.ParenLeft);
                        break;

                    case ')':
                        yield return MathToken.FromKind(Kind.ParenRight);
                        break;

                    case ',':
                        yield return MathToken.FromKind(Kind.Comma);
                        break;

                    default:
                        throw NewTokenizerException(string.Format("Illegal charcter '{0}'", (char)read));
                }

                #endregion
            }

            yield return MathToken.FromKind(Kind.Eof);
        }

        #endregion

        #region Numbers

        private double ScanNumber()
        {
            if (!IsDec())
                throw NewTokenizerException("Expected a digit");

            // Integer part
            double d = 0;
            if (!Maybe('0'))
                while (IsDec())
                    d = (d * 10) + GetDec();

            // Fractional part
            if (Maybe('.'))
            {
                double f = 0;
                double w = 0.1;

                if (!IsDec())
                    throw NewTokenizerException("At least one digit after '.'");

                while (IsDec())
                {
                    f += w * GetDec();
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

                if (!IsDec())
                    throw NewTokenizerException("At least one digit after 'e' or 'E'.");

                double e = 0;
                while (IsDec())
                    e = (e * 10) + GetDec();

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
            if (!IsAlpha())
                throw NewTokenizerException("Expected a letter");

            string identifier = string.Empty;
            while (IsAlpha() || IsDec())
                identifier += (char)Read();

            return identifier;
        }

        #endregion
    }
}