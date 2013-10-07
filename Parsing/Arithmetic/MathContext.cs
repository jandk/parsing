using System;
using System.Collections.Generic;

namespace Parsing.Arithmetic
{
    public class MathContext
    {
        private readonly Dictionary<string, MathValue> _values
            = new Dictionary<string, MathValue>(StringComparer.Ordinal);

        public MathFunction Register(string name, MathFunc function)
        {
            var mathFunction = new MathFunction(function);
            SetNameValue(name, mathFunction);
            return mathFunction;
        }

        public MathValue Get(string name)
        {
            return _values[name];
        }

        public void SetNameValue(string name, MathValue value)
        {
            _values[name] = value;
        }
    }
}