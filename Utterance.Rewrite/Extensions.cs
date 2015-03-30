using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance
{
	public static class Extensions
	{


		//public static Expression<Func<TFirstParam, TResult>> Combine<TFirstParam, TIntermediate, TResult>(this Expression<Func<TFirstParam, TIntermediate>> first, Expression<Func<TFirstParam, TIntermediate, TResult>> second)
		//{
		//	var param = Expression.Parameter(typeof(TFirstParam), "param");

		//	var newFirst = first.Body.Replace(first.Parameters[0], param);
		//	var newSecond = second.Body.Replace(second.Parameters[0], param)
		//		.Replace(second.Parameters[1], newFirst);

		//	return Expression.Lambda<Func<TFirstParam, TResult>>(newSecond, param);
		//}

		//public static LambdaExpression Combine(this LambdaExpression template, LambdaExpression[] arguments, ParameterExpression[] parameters)
		//{
		//	if (template.Parameters.Count != arguments.Length)
		//	{
		//		throw new ArgumentException("number of arguments provided don't match the number of arguments on the template LambdaExpression provided", "arguments");
		//	}

		//	var uniqueArgumentParameters = arguments.SelectMany(a => a.Parameters).Distinct(ExpressionEqualityComparer.Default).ToArray();

		//	if (parameters.Length != uniqueArgumentParameters.Length)
		//	{
		//		throw new ArgumentException("number of parameters provided don't match those found in provided arguments", "parameters");
		//	}

		//	Queue<ParameterExpression> paramQueue = new Queue<ParameterExpression>(parameters);
		//	var replaceMap = uniqueArgumentParameters.ToDictionary(p => (Expression)p, p => (Expression)paramQueue.Dequeue());

		//	var newArgumentsQueue = new Queue<Expression>(arguments.Select(a => a.Body.ReplaceAll(replaceMap)));
		//	replaceMap = template.Parameters.ToDictionary(p => (Expression)p, p => newArgumentsQueue.Dequeue());
		//	var newTemplate = template.Body.ReplaceAll(replaceMap);

		//	return Expression.Lambda(newTemplate, parameters);
		//}
	}
}
