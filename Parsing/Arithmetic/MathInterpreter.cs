using System;
using System.Collections.Generic;
using System.IO;
using Parsing.Arithmetic.Expressions;
using Parsing.Arithmetic.Library;

namespace Parsing.Arithmetic
{
    public class MathInterpreter
    {
        private static readonly Scanner Scanner = new Scanner();
        private static readonly Parser Parser = new Parser();

        public static MathValue Interpret(string mathCode)
        {
            Scanner.Reset();
            var tokens = Scanner.Scan(new StringReader(mathCode));
            var expression = (Expression)Parser.Parse(tokens);

            Console.WriteLine("Input:  " + mathCode);
            Console.WriteLine("Interp: " + expression);

            return expression.Evaluate(CreateGlobalContext());
        }

        public static MathContext CreateGlobalContext()
        {
            var context = new MathContext();
            MathLib.Register(context);
            return context;
        }
    }
}