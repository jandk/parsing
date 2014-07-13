using System;
using System.Globalization;

namespace Parsing.Arithmetic
{
    public abstract class MathValue
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
        private readonly bool _value;

        public MathBoolean(bool value)
        {
            _value = value;
        }

        public override bool ToBoolean()
        {
            return _value;
        }

        public override double ToNumber()
        {
            return _value ? 1 : 0;
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture).ToLowerInvariant();
        }
    }

    public sealed class MathNumber : MathValue
    {
        private readonly double _value;

        public MathNumber(double value)
        {
            _value = value;
        }

        public override bool ToBoolean()
        {
            return !(_value == 0 || double.IsNaN(_value));
        }

        public override double ToNumber()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
        }
    }

    public sealed class MathInteger : MathValue
    {
        private readonly double _value;

        public MathInteger(int value)
        {
            _value = value;
        }

        public override bool ToBoolean()
        {
            return _value != 0;
        }

        public override double ToNumber()
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString(CultureInfo.InvariantCulture);
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