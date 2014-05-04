using System;
using System.Collections.Generic;
using System.IO;
using Parsing.Arithmetic.Expressions;
using Parsing.Arithmetic.Library;

namespace Parsing.Arithmetic
{
    public static class MathInterpreter
    {
        private static readonly Scanner Scanner = new Scanner();
        private static readonly Parser Parser = new Parser();

        public static MathValue Interpret(string mathCode)
        {
            Expression expression;

            using (var reader = new StringReader(mathCode))
            {
                IEnumerator<Token> tokens = Scanner.Scan(reader);
                expression = (Expression)Parser.Parse(tokens);
            }

            MathContext context = CreateGlobalContext();
            MathValue result = expression.Evaluate(context);

            Scanner.Reset();
            return result;
        }

        public static void DumpTree(string mathCode)
        {
            Expression expression;

            using (var reader = new StringReader(mathCode))
            {
                IEnumerator<Token> tokens = Scanner.Scan(reader);
                expression = (Expression)Parser.Parse(tokens);
            }

            Console.WriteLine(expression.ToString());
            Scanner.Reset();
        }

        public static void DumpTokens(string mathCode)
        {
            using (var reader = new StringReader(mathCode))
            {
                IEnumerator<Token> tokens = Scanner.Scan(reader);
                while (tokens.MoveNext())
                    Console.WriteLine(tokens.Current);
            }

            Scanner.Reset();
        }

        private static MathContext CreateGlobalContext()
        {
            var modules = new ILibrary[]
                {
                    new MathLib()
                };

            var context = new MathContext();
            foreach (ILibrary module in modules)
                module.Register(context);
            return context;
        }
    }
}