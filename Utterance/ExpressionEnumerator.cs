namespace Utterance
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    ///     Applies the IEnumerator interface to an Expression tree.
    ///     Provides in-order traversal of an expression tree, enumerating each
    ///     Expression node in the traversal.
    /// </summary>
    public class ExpressionEnumerator : IEnumerator<Expression>
    {
        private Expression _source;
        private Visitor _visitor;

        public ExpressionEnumerator(Expression source)
        {
            _visitor = new Visitor();
            _source = source;
            _visitor.Nodes.Push(_source);
        }

        public Expression Current { get; private set; }

        public void Dispose()
        {
            _visitor = null;
            _source = null;
            Current = null;
        }

        object IEnumerator.Current => Current;

        public bool MoveNext()
        {
            if (Current != null) _visitor.Step(Current);
            Current = _visitor.Nodes.Count > 0 ? _visitor.Nodes.Pop() : null;
            return Current != null;
        }

        public void Reset()
        {
            _visitor.Nodes.Clear();
            _visitor.Nodes.Push(_source);
            Current = null;
        }

        private class Visitor : ExpressionVisitor
        {
            public Stack<Expression> Nodes { get; }

            public Visitor()
            {
                Nodes = new Stack<Expression>();
            }

            public Expression Step(Expression node)
            {
                return base.Visit(node);
            }

            public override Expression Visit(Expression node)
            {
                if (node != null) Nodes.Push(node);
                return node;
            }
        }
    }
}