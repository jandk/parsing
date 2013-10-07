using System;
using Parsing.Arithmetic;

namespace Parsing.Test
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const string input = "sqrt(sin(abs(-5*3)))";

            Console.WriteLine("Result: " + MathInterpreter.Interpret(input).ToNumber());
        }
    }
}