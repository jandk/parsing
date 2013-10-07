using System.Globalization;

namespace Parsing.Arithmetic
{
    public class Token : IToken
    {
        private readonly Kind _kind;
        private readonly object _value;

        private Token(Kind kind, object value)
        {
            _kind = kind;
            _value = value;
        }

        public Kind Kind
        {
            get { return _kind; }
        }

        public bool BoolValue
        {
            get { return (bool)_value; }
        }

        public double DoubleValue
        {
            get { return (double)_value; }
        }

        public string StringValue
        {
            get { return (string)_value; }
        }

        public override string ToString()
        {
            switch (_kind)
            {
                case Kind.Boolean:
                    return "Boolean: " + BoolValue.ToString(CultureInfo.InvariantCulture);

                case Kind.Number:
                    return "Number: " + DoubleValue.ToString(CultureInfo.InvariantCulture);

                case Kind.Identifier:
                    return "Identifier: " + StringValue;

                default:
                    return _kind.ToString();
            }
        }

        public static Token FromKind(Kind kind)
        {
            return new Token(kind, null);
        }

        public static Token FromBool(bool value)
        {
            return new Token(Kind.Boolean, value);
        }

        public static Token FromNumber(double value)
        {
            return new Token(Kind.Number, value);
        }

        public static Token FromIdentifier(string value)
        {
            return new Token(Kind.Identifier, value);
        }
    }
}