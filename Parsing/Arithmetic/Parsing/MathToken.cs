using System.Globalization;

namespace Parsing.Arithmetic.Parsing
{
    public class MathToken 
        : Token
    {
        private readonly Kind _kind;
        private readonly object _value;

        private MathToken(Kind kind, object value)
            : base(null)
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

        public static MathToken FromKind(Kind kind)
        {
            return new MathToken(kind, null);
        }

        public static MathToken FromBool(bool value)
        {
            return new MathToken(Kind.Boolean, value);
        }

        public static MathToken FromNumber(double value)
        {
            return new MathToken(Kind.Number, value);
        }

        public static MathToken FromIdentifier(string value)
        {
            return new MathToken(Kind.Identifier, value);
        }
    }
}