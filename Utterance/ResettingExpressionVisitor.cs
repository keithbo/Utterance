namespace Utterance
{
    using System.Linq.Expressions;
    using System.Threading;

    /// <summary>
    ///     ResettingExpressionVisitor is a basic implementation of ExpressionVisitor that provides a simple
    ///     abstract implementation which can reset its state upon subsequent calls to a new Expression tree.
    ///     The instance will "reset" on first call of Visit, nested calls will not trigger the reset behavior.
    /// </summary>
    public abstract class ResettingExpressionVisitor : ExpressionVisitor
    {
        private int _visitCount;

        public override Expression Visit(Expression node)
        {
            try
            {
                // first visit on this visitor will reset before continuing
                if (Interlocked.Increment(ref _visitCount) == 1) Reset();

                return base.Visit(node);
            }
            finally
            {
                Interlocked.Decrement(ref _visitCount);
            }
        }

        /// <summary>
        ///     Perform implementation specific resetting of this visitor
        /// </summary>
        protected virtual void Reset()
        {
        }
    }
}