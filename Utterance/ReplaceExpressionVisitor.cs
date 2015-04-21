namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;

	/// <summary>
	/// ExpressionVisitor implementation that takes as input one or more pairs of expressions
	/// and parses a given Expression tree, replacing any instances of the first half of each
	/// pair with the second half.
	/// </summary>
	public class ReplaceExpressionVisitor : ExpressionVisitor
	{
		private IDictionary<Expression, Expression> _pairs;

		public ReplaceExpressionVisitor(Expression from, Expression to)
			: this(new Dictionary<Expression, Expression> { {from, to} })
		{
		}

		public ReplaceExpressionVisitor(params Tuple<Expression, Expression>[] pairs)
			: this(pairs.ToDictionary(p => p.Item1, p => p.Item2))
		{
		}

		public ReplaceExpressionVisitor(IDictionary<Expression, Expression> pairs)
		{
			_pairs = pairs.ToDictionary(p => p.Key, p => p.Value, ExpressionEqualityComparer.Default);
		}

		public override Expression Visit(Expression node)
		{
			Expression to;
			return (node != null && _pairs.TryGetValue(node, out to)) ? to : base.Visit(node);
		}
	}
}
