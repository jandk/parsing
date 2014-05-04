using System.Linq;
using System.Text;

namespace Parsing.Arithmetic.Expressions
{
    public sealed class FunctionExpression : Expression
    {
        private readonly Expression[] _args;
        private readonly string _name;

        public FunctionExpression(string name, Expression[] args)
        {
            _name = name;
            _args = args;
        }

        public override MathValue Evaluate(MathContext context)
        {
            var function = context.Get(_name) as MathFunction;
            if (function == null)
                throw new MathException("Invalid function name: " + _name);

            MathValue[] values = _args.Select(arg => arg.Evaluate(context)).ToArray();
            return function.Function(values);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append(_name).Append('(');

            foreach (Expression expr in _args)
                sb.Append(expr).Append(',');

            if (sb[sb.Length - 1] == ',')
                sb.Length -= 1;

            sb.Append(')');
            return sb.ToString();
        }
    }

    public sealed class VariableExpression : Expression
    {
        private readonly string _name;

        public VariableExpression(string name)
        {
            _name = name;
        }

        public override MathValue Evaluate(MathContext context)
        {
            return context.Get(_name);
        }

        public override string ToString()
        {
            return _name;
        }
    }
}