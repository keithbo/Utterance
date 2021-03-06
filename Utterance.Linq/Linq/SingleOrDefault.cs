﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Linq
{
	public static class SingleOrDefault
	{
		internal class Access<TSource> : Utterance.Access.AccessBase
		{
			public LambdaExpression Source { get { return Cache<TSource>.Source; } }
			public LambdaExpression SourcePredicate { get { return Cache<TSource>.SourcePredicate; } }
		}
		public static class Cache<TSource>
		{
			public static readonly Expression<Func<IEnumerable<TSource>, TSource>> Source = (source) => source.SingleOrDefault();
			public static readonly Expression<Func<IEnumerable<TSource>, Func<TSource, bool>, TSource>> SourcePredicate = (source, predicate) => source.SingleOrDefault(predicate);
		}
	}
}
