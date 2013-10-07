using System;
using System.Globalization;

namespace Parsing.Arithmetic
{
    public interface IValue
    {
        bool ToBoolean();
        double ToNumber();
    }

    public abstract class MathValue : IValue
    {
        public abstract bool ToBoolean();
        public abstract double ToNumber();

        public int ToInt32()
        {
            const long power31 = 1L << 31;
            const long power32 = 1L << 32;

            double number = ToNumber();
            if (number == 0 || double.IsInfinity(number) || double.IsNaN(number))
                return 0;

            double posInt = Math.Sign(number) * Math.Floor(Math.Abs(number));
            double int32Bit = posInt % power32;

            if (int32Bit > power31)
                return (int)(int32Bit - power32);

            return (int)int32Bit;
        }
    }

    public sealed class MathBoolean : MathValue
    {
        private readonly bool value;

        public MathBoolean(bool value)
        {
            this.value = value;
        }

        public override bool ToBoolean()
        {
            return value;
        }

        public override double ToNumber()
        {
            return value ? 1 : 0;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }
    }

    public sealed class MathNumber : MathValue
    {
        private readonly double value;

        public MathNumber(double value)
        {
            this.value = value;
        }

        public override bool ToBoolean()
        {
            return !(value == 0 || double.IsNaN(value));
        }

        public override double ToNumber()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public sealed class MathInteger : MathValue
    {
        private readonly double value;

        public MathInteger(int value)
        {
            this.value = value;
        }

        public override bool ToBoolean()
        {
            return value != 0;
        }

        public override double ToNumber()
        {
            return value;
        }

        public override string ToString()
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public delegate MathValue MathFunc(MathValue[] args);

    public sealed class MathFunction : MathValue
    {
        private readonly MathFunc _func;

        public MathFunction(MathFunc func)
        {
            _func = func;
        }

        public MathFunc Function
        {
            get { return _func; }
        }

        public override bool ToBoolean()
        {
            throw new MathException("ToBoolean not valid for MathFunction");
        }

        public override double ToNumber()
        {
            throw new MathException("ToNumber not valid for MathFunction");
        }
    }
}