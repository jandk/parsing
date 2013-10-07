using System.Diagnostics;

namespace Parsing.Arithmetic.Expressions
{
    public abstract class Unary : Expression
    {
        protected Unary(Expression child)
        {
            Debug.Assert(child != null);

            Child = child;
        }

        protected Expression Child { get; private set; }
    }

    public sealed class Positive : Unary
    {
        public Positive(Expression child)
            : base(child)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            double value = Child.Evaluate(mathContext).ToNumber();

            return new MathNumber(value);
        }

        public override string ToString()
        {
            return string.Format("+({0})", Child);
        }
    }

    public sealed class Negative : Unary
    {
        public Negative(Expression child)
            : base(child)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            double value = Child.Evaluate(mathContext).ToNumber();

            if (double.IsNaN(value))
                return new MathNumber(double.NaN);

            return new MathNumber(-value);
        }

        public override string ToString()
        {
            return string.Format("-({0})", Child);
        }
    }

    public sealed class BinaryNot : Unary
    {
        public BinaryNot(Expression child)
            : base(child)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            int value = Child.Evaluate(mathContext).ToInt32();
            return new MathInteger(~value);
        }

        public override string ToString()
        {
            return string.Format("~({0})", Child);
        }
    }

    public sealed class LogicalNot : Unary
    {
        public LogicalNot(Expression child)
            : base(child)
        {
        }

        public override MathValue Evaluate(MathContext mathContext)
        {
            bool value = Child.Evaluate(mathContext).ToBoolean();
            return new MathBoolean(!value);
        }

        public override string ToString()
        {
            return string.Format("!({0})", Child);
        }
    }
}