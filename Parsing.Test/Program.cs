using System;
using Parsing.Arithmetic;

namespace Parsing.Test
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            const string input = "sqrt(sin(abs(-5*3)))";
            MathInterpreter.DumpTokens(input);

            // Console.WriteLine("Result: " + MathInterpreter.Interpret(input));
        }
    }
}
