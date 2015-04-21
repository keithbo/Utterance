namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// General use helper extensions for operating on Expression trees.
	/// </summary>
	public static class ExpressionExtensions
	{
		/// <summary>
		/// Parses an Expression tree and evaluates all sub-expression trees that don't have a ParameterExpression
		/// Returns a copy of the tree after evaluations have been processed.
		/// 
		/// This method was pulled from the excellent blog series:
		/// http://blogs.msdn.com/b/mattwar/archive/2007/08/01/linq-building-an-iqueryable-provider-part-iii.aspx
		/// </summary>
		/// <param name="source">Expression to parse</param>
		/// <returns>Expression tree after all evaluations have been completed</returns>
		public static Expression PartialEval(this Expression source)
		{
			return PartialEval(source, null);
		}

		/// <summary>
		/// Parses an Expression tree and evaluates all sub-expression trees flagged as evaluatable
		/// by the provided predicate <paramref name="canEvaluate"/>
		/// Returns a copy of the tree after evaluations have been processed.
		/// 
		/// This method was pulled from the excellent blog series:
		/// http://blogs.msdn.com/b/mattwar/archive/2007/08/01/linq-building-an-iqueryable-provider-part-iii.aspx
		/// </summary>
		/// <param name="source">Expression to parse</param>
		/// <param name="canEvaluate">Predicate function that determines what Expressions are evaluatable</param>
		/// <returns>Expression tree after all evaluations have been completed</returns>
		public static Expression PartialEval(this Expression source, Func<Expression, bool> canEvaluate)
		{
			return new SubtreeEvaluator(new Nominator(canEvaluate ?? DefaultPartialEvalPredicate).Nominate(source)).Eval(source);
		}

		private static bool DefaultPartialEvalPredicate(Expression expression)
		{
			return expression.NodeType != ExpressionType.Parameter;
		}

		/// <summary>
		/// Retrieves an IEnumerator of all Expression nodes in the passed in Expression tree
		/// </summary>
		/// <param name="source">Expression tree to enumerate</param>
		/// <returns>IEnumerator[Expression]</returns>
		public static IEnumerator<Expression> GetEnumerator(this Expression source)
		{
			return new ExpressionEnumerator(source);
		}

		/// <summary>
		/// Constructs a new Expression by replacing usages of <paramref name="searchEx"/> with <paramref name="replaceEx"/>
		/// in the expresion tree of <paramref name="expression"/>
		/// </summary>
		/// <param name="expression">Expression tree to replace in</param>
		/// <param name="searchEx">Expression to be replaced</param>
		/// <param name="replaceEx">Expression to replace <paramref name="searchEx"/> with</param>
		/// <returns>A new Expression with <paramref name="replaceEx"/> substituted for <paramref name="searchEx"/></returns>
		public static Expression Replace(this Expression expression, Expression searchEx, Expression replaceEx)
		{
			return new ReplaceExpressionVisitor(searchEx, replaceEx).Visit(expression);
		}

		/// <summary>
		/// Constructs a new Expression by replacing usages of all expressions in <paramref name="pairs"/>
		/// with their counterparts in the expression tree of <paramref name="expression"/>.
		/// </summary>
		/// <param name="expression">Expression tree to replace in</param>
		/// <param name="pairs">Expression pairs stored in Tuple form, first value is the search expression,
		/// second value is the replace expression</param>
		/// <returns>A new Expression with all found pairs substituted</returns>
		public static Expression ReplaceAll(this Expression expression, params Tuple<Expression, Expression>[] pairs)
		{
			return new ReplaceExpressionVisitor(pairs).Visit(expression);
		}

		/// <summary>
		/// Constructs a new Expression by replacing usages of all expressions in <paramref name="pairs"/>
		/// with their counterparts in the expression tree of <paramref name="expression"/>.
		/// </summary>
		/// <param name="expression">Expression tree to replace in</param>
		/// <param name="pairs">An IDictionary map of Expression pairs to be replaced</param>
		/// <returns>A new Expression with all found pairs substituted</returns>
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
