﻿namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public static class LambdaExpressionExtensions
	{
		/// <summary>
		/// Constructs a new strongly typed LambdaExpression by substituting the body of the <paramref name="first"/>
		/// expression for the parameter usages in the <paramref name="second"/> expression.
		/// This is basically like creating an expression that calls the first and uses the result as the input for the second
		/// but as one complete expression.
		/// </summary>
		/// <typeparam name="TFirstParam"></typeparam>
		/// <typeparam name="TIntermediate"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Expression<Func<TResult>> Compose<TIntermediate, TResult>(this Expression<Func<TIntermediate>> first, Expression<Func<TIntermediate, TResult>> second)
		{
			return (Expression<Func<TResult>>)Compose(first, second, new ParameterExpression[0]);
		}

		/// <summary>
		/// Constructs a new strongly typed LambdaExpression by substituting the body of the <paramref name="first"/>
		/// expression for the parameter usages in the <paramref name="second"/> expression.
		/// This is basically like creating an expression that calls the first and uses the result as the input for the second
		/// but as one complete expression.
		/// </summary>
		/// <typeparam name="TFirstParam"></typeparam>
		/// <typeparam name="TIntermediate"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Expression<Func<T1, TResult>> Compose<T1, TIntermediate, TResult>(this Expression<Func<T1, TIntermediate>> first, Expression<Func<TIntermediate, TResult>> second)
		{
			return (Expression<Func<T1, TResult>>)Compose(first, second, Expression.Parameter(typeof(T1)));
		}

		/// <summary>
		/// Constructs a new strongly typed LambdaExpression by substituting the body of the <paramref name="first"/>
		/// expression for the parameter usages in the <paramref name="second"/> expression.
		/// This is basically like creating an expression that calls the first and uses the result as the input for the second
		/// but as one complete expression.
		/// </summary>
		/// <typeparam name="TFirstParam"></typeparam>
		/// <typeparam name="TIntermediate"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Expression<Func<T1, T2, TResult>> Compose<T1, T2, TIntermediate, TResult>(this Expression<Func<T1, T2, TIntermediate>> first, Expression<Func<TIntermediate, TResult>> second)
		{
			return (Expression<Func<T1, T2, TResult>>)Compose(first, second,
				Expression.Parameter(typeof(T1)),
				Expression.Parameter(typeof(T2)));
		}

		/// <summary>
		/// Constructs a new strongly typed LambdaExpression by substituting the body of the <paramref name="first"/>
		/// expression for the parameter usages in the <paramref name="second"/> expression.
		/// This is basically like creating an expression that calls the first and uses the result as the input for the second
		/// but as one complete expression.
		/// </summary>
		/// <typeparam name="TFirstParam"></typeparam>
		/// <typeparam name="TIntermediate"></typeparam>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="first"></param>
		/// <param name="second"></param>
		/// <returns></returns>
		public static Expression<Func<T1, T2, T3, TResult>> Compose<T1, T2, T3, TIntermediate, TResult>(this Expression<Func<T1, T2, T3, TIntermediate>> first, Expression<Func<TIntermediate, TResult>> second)
		{
			return (Expression<Func<T1, T2, T3, TResult>>)Compose(first, second,
				Expression.Parameter(typeof(T1)),
				Expression.Parameter(typeof(T2)),
				Expression.Parameter(typeof(T3)));
		}

		public static LambdaExpression Compose(this LambdaExpression first, LambdaExpression second, params ParameterExpression[] parameters)
		{
			Stack<Expression> paramStack = new Stack<Expression>(parameters.Reverse().Cast<Expression>());

			if (first.Parameters.Count > paramStack.Count)
			{
				throw new InvalidOperationException();
			}

			var pairs = first.Parameters.ToDictionary(p => (Expression)p, p => paramStack.Pop());
			var newFirst = first.Body.ReplaceAll(pairs);
			paramStack.Push(newFirst);

			if (second.Parameters.Count != paramStack.Count)
			{
				throw new InvalidOperationException();
			}

			pairs = second.Parameters.ToDictionary(p => (Expression)p, p => paramStack.Pop());

			var newSecond = second.Body.ReplaceAll(pairs);

			return Expression.Lambda(newSecond, parameters);
		}
	}
}