namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public static class ExpressionExtensions
	{
		public static Expression PartialEval(this Expression source)
		{
			return PartialEval(source, null);
		}

		/// <summary>
		/// This method was pulled from the excellent blog series:
		/// http://blogs.msdn.com/b/mattwar/archive/2007/08/01/linq-building-an-iqueryable-provider-part-iii.aspx
		/// </summary>
		/// <param name="source"></param>
		/// <param name="canEvaluate"></param>
		/// <returns></returns>
		public static Expression PartialEval(this Expression source, Func<Expression, bool> canEvaluate)
		{
			return new SubtreeEvaluator(new Nominator(canEvaluate ?? DefaultPartialEvalPredicate).Nominate(source)).Eval(source);
		}

		private static bool DefaultPartialEvalPredicate(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter;
		}

		public static IEnumerator<Expression> GetEnumerator(this Expression source)
		{
			return new ExpressionEnumerator(source);
		}

		/// <summary>
		/// Constructs a new Expression by replacing usages of <paramref name="searchEx"/> with <paramref name="replaceEx"/>
		/// in the expresion tree of <paramref name="expression"/>
		/// </summary>
		/// <param name="expression"></param>
		/// <param name="searchEx"></param>
		/// <param name="replaceEx"></param>
		/// <returns></returns>
		public static Expression Replace(this Expression expression, Expression searchEx, Expression replaceEx)
		{
			return new ReplaceExpressionVisitor(searchEx, replaceEx).Visit(expression);
		}

		public static Expression ReplaceAll(this Expression expression, params Tuple<Expression, Expression>[] pairs)
		{
			return new ReplaceExpressionVisitor(pairs).Visit(expression);
		}

		public static Expression ReplaceAll(this Expression expression, IDictionary<Expression, Expression> pairs)
		{
			return new ReplaceExpressionVisitor(pairs).Visit(expression);
		}
	}

	/// <summary>
	/// Evaluates & replaces sub-trees when first candidate is reached (top-down)
	/// </summary>
	class SubtreeEvaluator : ExpressionVisitor
	{
		HashSet<Expression> candidates;

		internal SubtreeEvaluator(HashSet<Expression> candidates)
		{
			this.candidates = candidates;
		}

		internal Expression Eval(Expression exp)
		{
			return this.Visit(exp);
		}

		public override Expression Visit(Expression exp)
		{
			if (exp == null)
			{
				return null;
			}
			if (this.candidates.Contains(exp))
			{
				return this.Evaluate(exp);
			}
			return base.Visit(exp);
		}

		private Expression Evaluate(Expression e)
		{
			if (e.NodeType == ExpressionType.Constant)
			{
				return e;
			}
			Expression<Func<object>> lambda = Expression.Lambda<Func<object>>(e);
			return Expression.Constant(lambda.Compile()(), e.Type);
		}
	}

	/// <summary>
	/// Performs bottom-up analysis to determine which nodes can possibly
	/// be part of an evaluated sub-tree.
	/// </summary>
	class Nominator : ExpressionVisitor
	{
		Func<Expression, bool> fnCanBeEvaluated;
		HashSet<Expression> candidates;
		bool cannotBeEvaluated;

		internal Nominator(Func<Expression, bool> fnCanBeEvaluated)
		{
			this.fnCanBeEvaluated = fnCanBeEvaluated;
		}

		internal HashSet<Expression> Nominate(Expression expression)
		{
			this.candidates = new HashSet<Expression>();
			this.Visit(expression);
			return this.candidates;
		}

		public override Expression Visit(Expression expression)
		{
			if (expression != null)
			{
				bool saveCannotBeEvaluated = this.cannotBeEvaluated;
				this.cannotBeEvaluated = false;
				base.Visit(expression);
				if (!this.cannotBeEvaluated)
				{
					if (this.fnCanBeEvaluated(expression))
					{
						this.candidates.Add(expression);
					}
					else
					{
						this.cannotBeEvaluated = true;
					}
				}
				this.cannotBeEvaluated |= saveCannotBeEvaluated;
			}
			return expression;
		}
	}
}
