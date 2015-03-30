using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Linq
{
	public static class ExpressionEnumerable
	{
		// Expression<Func<IEnumerable<TSource>, Func<TSource, TResult>, IEnumerable<TResult>>>

		public static Expression<Func<IEnumerable<TSource>, IEnumerable<TResult>>> Select<TSource, TResult>(this Expression<Func<IEnumerable<TSource>>> source, Expression<Func<TSource, TResult>> converter)
		{
			//Utterance.Linq.Select.Cache<TSource, TResult>.SourceSelector
			return null;
		}
	}
}
