using System;

namespace Parsing.Arithmetic.Library
{
    public class MathLib : ILibrary
    {
        private readonly Random _random = new Random();

        public void Register(MathContext context)
        {
            context.SetNameValue("e", new MathNumber(Math.E));
            context.SetNameValue("ln10", new MathNumber(Math.Log(10)));
            context.SetNameValue("ln2", new MathNumber(Math.Log(2)));
            context.SetNameValue("log2e", new MathNumber(1 / Math.Log(2)));
            context.SetNameValue("log10e", new MathNumber(1 / Math.Log(10)));
            context.SetNameValue("pi", new MathNumber(Math.PI));
            context.SetNameValue("sqrt1_2", new MathNumber(Math.Sqrt(.5)));
            context.SetNameValue("sqrt2", new MathNumber(Math.Sqrt(2)));

            context.Register("abs", CreateFunction(Math.Abs));
            context.Register("acos", CreateFunction(Math.Acos));
            context.Register("asin", CreateFunction(Math.Asin));
            context.Register("atan", CreateFunction(Math.Atan));
            context.Register("ceil", CreateFunction(Math.Ceiling));
            context.Register("cos", CreateFunction(Math.Cos));
            context.Register("exp", CreateFunction(Math.Exp));
            context.Register("floor", CreateFunction(Math.Floor));
            context.Register("log", CreateFunction(Math.Log));
            context.Register("random", args => new MathNumber(_random.NextDouble()));
            context.Register("round", CreateFunction(Math.Round));
            context.Register("sin", CreateFunction(Math.Sin));
            context.Register("sqrt", CreateFunction(Math.Sqrt));
            context.Register("tan", CreateFunction(Math.Tan));

            context.Register("atan2", CreateFunction2(Math.Atan2));
            context.Register("pow", CreateFunction2(Math.Pow));
        }

        private static MathFunc CreateFunction(Func<double, double> func)
        {
            return args => new MathNumber(func(CheckArgs(args)));
        }

        private static MathFunc CreateFunction2(Func<double, double, double> func)
        {
            return args =>
            {
                Tuple<double, double> arg = CheckArgs2(args);
                return new MathNumber(func(arg.Item1, arg.Item2));
            };
        }

        private static double CheckArgs(MathValue[] values)
        {
            if (values.Length < 1)
                throw new MathException("Need 1 argument");

            var arg = values[0] as MathNumber;
            if (arg == null)
                throw new MathException("Argument is not a number");

            return arg.ToNumber();
        }

        private static Tuple<double, double> CheckArgs2(MathValue[] values)
        {
            if (values.Length < 1)
                throw new MathException("Need 1 argument");

            var arg1 = values[0] as MathNumber;
            if (arg1 == null)
                throw new MathException("Argument 1 is not a number");

            var arg2 = values[1] as MathNumber;
            if (arg2 == null)
                throw new MathException("Argument 2 is not a number");

            return Tuple.Create(arg1.ToNumber(), arg2.ToNumber());
        }
    }
}