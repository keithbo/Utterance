namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	public abstract class ResettingExpressionVisitor : ExpressionVisitor
	{
		private int _visitCount = 0;

		protected ResettingExpressionVisitor()
		{
		}

		public override Expression Visit(Expression node)
		{
			try
			{
				// first visit on this visitor will reset before continuing
				if (Interlocked.Increment(ref _visitCount) == 1)
				{
					Reset();
				}

				return base.Visit(node);
			}
			finally
			{
				Interlocked.Decrement(ref _visitCount);
			}
		}

		protected virtual void Reset()
		{
		}
	}
}
