using System;
using System.Collections.Generic;
using Parsing.Arithmetic.Expressions;

namespace Parsing.Arithmetic
{
    public class Parser : ParserBase<Token>
    {
        #region IParser<T> Members

        protected override object Parse()
        {
            _ts.MoveNext();

            Expression value = Expr();

            if (_ts.Current.Kind != Kind.Eof)
                throw new Exception("Expected EOF");

            return value;
        }

        #endregion

        private Expression Expr()
        {
            return ExprCont(Term());
        }

        private Expression ExprCont(Expression inval)
        {
            switch (_ts.Current.Kind)
            {
                case Kind.OpPlus:
                    _ts.MoveNext();
                    return ExprCont(new Addition(inval, Term()));

                case Kind.OpMinus:
                    _ts.MoveNext();
                    return ExprCont(new Subtraction(inval, Term()));

                default:
                    return inval;
            }
        }

        private Expression Term()
        {
            return TermCont(Unary());
        }

        private Expression TermCont(Expression inval)
        {
            switch (_ts.Current.Kind)
            {
                case Kind.OpMultiply:
                    _ts.MoveNext();
                    return TermCont(new Multiplication(inval, Unary()));

                case Kind.OpDivide:
                    _ts.MoveNext();
                    return TermCont(new Division(inval, Unary()));

                case Kind.OpModulo:
                    _ts.MoveNext();
                    return TermCont(new Modulo(inval, Unary()));

                default:
                    return inval;
            }
        }

        private Expression Unary()
        {
            switch (_ts.Current.Kind)
            {
                case Kind.OpPlus:
                    _ts.MoveNext();
                    return new Positive(Unary());

                case Kind.OpMinus:
                    _ts.MoveNext();
                    return new Negative(Unary());

                case Kind.OpBinaryNot:
                    _ts.MoveNext();
                    return new BinaryNot(Unary());

                case Kind.OpLogicalNot:
                    _ts.MoveNext();
                    return new LogicalNot(Unary());

                default:
                    return Factor();
            }
        }

        private Expression Factor()
        {
            Expression expr;
            switch (_ts.Current.Kind)
            {
                case Kind.Boolean:
                    bool boolValue = _ts.Current.BoolValue;
                    _ts.MoveNext();
                    return new BoolLiteral(boolValue);

                case Kind.Number:
                    double doubleValue = _ts.Current.DoubleValue;
                    _ts.MoveNext();
                    return new NumberLiteral(doubleValue);

                case Kind.Identifier:
                    string name = _ts.Current.StringValue;

                    _ts.MoveNext();
                    if (_ts.Current.Kind != Kind.Eof && _ts.Current.Kind == Kind.ParenLeft)
                        return FunctionCall(name);
                    return new VariableExpression(name);

                case Kind.ParenLeft:
                    _ts.MoveNext();
                    expr = Expr();
                    if (_ts.Current.Kind != Kind.ParenRight)
                        throw new Exception("Parse error: expected ')'");
                    _ts.MoveNext();
                    return expr;

                default:
                    throw new Exception("Parse error: expected number or '('");
            }
        }

        private Expression FunctionCall(string name)
        {
            // skip '('
            _ts.MoveNext();

            var arguments = new List<Expression>();
            if (_ts.Current.Kind != Kind.ParenRight)
            {
                arguments.Add(Expr());

                while (_ts.Current.Kind == Kind.Comma)
                {
                    _ts.MoveNext();
                    arguments.Add(Expr());
                }

                if (_ts.Current.Kind != Kind.ParenRight)
                    throw new Exception("Parse error: expected ')'");
            }

            _ts.MoveNext();

            return new FunctionExpression(name, arguments.ToArray());
        }
    }
}