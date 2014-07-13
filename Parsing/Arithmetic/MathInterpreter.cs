using System;
using System.IO;

using Parsing.Arithmetic.Expressions;
using Parsing.Arithmetic.Library;
using Parsing.Arithmetic.Parsing;

namespace Parsing.Arithmetic
{
    public class MathInterpreter
    {
        private static readonly MathContext Context = CreateGlobalContext();

        public static MathValue InterpretSingle(string mathCode)
        {
            Expression expression;

            var tokenizer = new MathTokenizer();
            var parser = new MathParser();

            using (var reader = new StringReader(mathCode))
            {
                var tokens = tokenizer.Tokenize(reader);
                expression = parser.Parse(tokens);
            }

            var context = CreateGlobalContext();
            var result = expression.Evaluate(context);

            return result;
        }

        public static void DumpTree(string mathCode)
        {
            Expression expression;

            var tokenizer = new MathTokenizer();
            var parser = new MathParser();

            using (var reader = new StringReader(mathCode))
            {
                var tokens = tokenizer.Tokenize(reader);
                expression = parser.Parse(tokens);
            }

            Console.WriteLine(expression.ToString());
        }

        public static void DumpTokens(string mathCode)
        {
            var tokenizer = new MathTokenizer();

            using (var reader = new StringReader(mathCode))
            {
                var tokens = tokenizer.Tokenize(reader);
                while (tokens.MoveNext())
                    Console.WriteLine(tokens.Current);
            }
        }

        private static MathContext CreateGlobalContext()
        {
            var modules = new ILibrary[]
                {
                    new MathLib()
                };

            var context = new MathContext();
            foreach (var module in modules)
                module.Register(context);
            return context;
        }
    }
}