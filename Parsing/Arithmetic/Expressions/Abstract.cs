namespace Parsing.Arithmetic.Expressions
{
    public abstract class Node
    {
    }

    public abstract class Expression : Node
    {
        public abstract MathValue Evaluate(MathContext context);
    }

    public abstract class Statement : Node
    {
        public abstract void Execute(MathContext context);
    }
}