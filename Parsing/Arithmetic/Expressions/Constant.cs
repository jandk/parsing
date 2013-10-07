using System.Globalization;

namespace Parsing.Arithmetic.Expressions
{
    public abstract class Literal<T> : Expression
    {
        protected Literal(T value)
        {
            Value = value;
        }

        protected T Value { get; private set; }

        public override string ToString()
        {
            // TODO: Why was this check here?
            //if (Equals(MathValue, default(T)))
            //    return string.Empty;

            return Value.ToString();
        }
    }

    public sealed class BoolLiteral : Literal<bool>
    {
        public BoolLiteral(bool value)
            : base(value)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            return new MathBoolean(Value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public sealed class NumberLiteral : Literal<double>
    {
        public NumberLiteral(double value)
            : base(value)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            return new MathNumber(Value);
        }

        public override string ToString()
        {
            return Value.ToString(CultureInfo.InvariantCulture);
        }
    }
}