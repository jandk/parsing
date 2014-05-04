using System;
using System.Diagnostics;

namespace Parsing.Arithmetic.Expressions
{
    public abstract class BinaryExpression : Expression
    {
        protected BinaryExpression(Expression left, Expression right)
        {
            Debug.Assert(left != null);
            Debug.Assert(right != null);

            Left = left;
            Right = right;
        }

        protected Expression Left { get; private set; }
        protected Expression Right { get; private set; }
    }

    public sealed class Addition : BinaryExpression
    {
        public Addition(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(lval + rval);
        }

        public override string ToString()
        {
            return string.Format("({0}+{1})", Left, Right);
        }
    }

    public sealed class Subtraction : BinaryExpression
    {
        public Subtraction(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(lval - rval);
        }

        public override string ToString()
        {
            return string.Format("({0}-{1})", Left, Right);
        }
    }

    public sealed class Multiplication : BinaryExpression
    {
        public Multiplication(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(lval * rval);
        }

        public override string ToString()
        {
            return string.Format("({0}*{1})", Left, Right);
        }
    }

    public sealed class Division : BinaryExpression
    {
        public Division(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(lval / rval);
        }

        public override string ToString()
        {
            return string.Format("({0}/{1})", Left, Right);
        }
    }

    public sealed class Modulo : BinaryExpression
    {
        public Modulo(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            // TODO: Correct rules
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(lval % rval);
        }

        public override string ToString()
        {
            return string.Format("({0}%{1})", Left, Right);
        }
    }


    public sealed class Power : BinaryExpression
    {
        public Power(Expression left, Expression right)
            : base(left, right)
        {
        }

        public override MathValue Evaluate(MathContext context)
        {
            // TODO: Correct rules
            double lval = Left.Evaluate(context).ToNumber();
            double rval = Right.Evaluate(context).ToNumber();
            return new MathNumber(Math.Pow(lval, rval));
        }

        public override string ToString()
        {
            return string.Format("({0}**{1})", Left, Right);
        }
    }
}