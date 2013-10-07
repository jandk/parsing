using System;

namespace Parsing.Arithmetic.Library
{
    public static class MathLib
    {
        private static readonly Random Random = new Random();

        public static void Register(MathContext module)
        {
            module.SetNameValue("e", new MathNumber(Math.E));
            module.SetNameValue("ln10", new MathNumber(Math.Log(10)));
            module.SetNameValue("ln2", new MathNumber(Math.Log(2)));
            module.SetNameValue("log2e", new MathNumber(1 / Math.Log(2)));
            module.SetNameValue("log10e", new MathNumber(1 / Math.Log(10)));
            module.SetNameValue("pi", new MathNumber(Math.PI));
            module.SetNameValue("sqrt1_2", new MathNumber(Math.Sqrt(.5)));
            module.SetNameValue("sqrt2", new MathNumber(Math.Sqrt(2)));

            module.Register("abs", CreateFunction(Math.Abs));
            module.Register("acos", CreateFunction(Math.Acos));
            module.Register("asin", CreateFunction(Math.Asin));
            module.Register("atan", CreateFunction(Math.Atan));
            module.Register("ceil", CreateFunction(Math.Ceiling));
            module.Register("cos", CreateFunction(Math.Cos));
            module.Register("exp", CreateFunction(Math.Exp));
            module.Register("floor", CreateFunction(Math.Floor));
            module.Register("log", CreateFunction(Math.Log));
            module.Register("random", args => new MathNumber(Random.NextDouble()));
            module.Register("round", CreateFunction(Math.Round));
            module.Register("sin", CreateFunction(Math.Sin));
            module.Register("sqrt", CreateFunction(Math.Sqrt));
            module.Register("tan", CreateFunction(Math.Tan));

            module.Register("atan2", CreateFunction2(Math.Atan2));
            module.Register("pow", CreateFunction2(Math.Pow));
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