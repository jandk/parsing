namespace Parsing.Arithmetic.Expressions
{
    public abstract class Node
    {
    }

    public abstract class Expression : Node
    {
        public abstract MathValue Evaluate(MathContext mathContext);
    }

    public abstract class Statement : Node
    {
        public abstract void Execute();
    }
}