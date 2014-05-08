using System;
using System.Collections.Generic;
using System.IO;
using Parsing.Arithmetic.Expressions;
using Parsing.Arithmetic.Library;

namespace Parsing.Arithmetic
{
    public class MathInterpreter
    {
        private static readonly MathContext Context = CreateGlobalContext();

        public static MathValue InterpretSingle(string mathCode)
        {
            Expression expression;

            var scanner = new Scanner();
            var parser = new Parser();

            using (var reader = new StringReader(mathCode))
            {
				var tokens = scanner.Scan(reader);
				expression = (Expression)parser.Parse(tokens);
            }

            var context = CreateGlobalContext();
            var result = expression.Evaluate(context);

            return result;
        }

        public static void DumpTree(string mathCode)
        {
            Expression expression;

			var scanner = new Scanner();
			var parser = new Parser();

			using (var reader = new StringReader(mathCode))
            {
				var tokens = scanner.Scan(reader);
                expression = (Expression)parser.Parse(tokens);
            }

            Console.WriteLine(expression.ToString());
        }

        public static void DumpTokens(string mathCode)
        {
			var scanner = new Scanner();

			using (var reader = new StringReader(mathCode))
            {
                var tokens = scanner.Scan(reader);
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