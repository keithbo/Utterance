﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Linq
{
	public static class OrderBy
	{
		internal class Access<TSource, TKey> : Utterance.Access.AccessBase
		{
			public LambdaExpression SourceKeySelector { get { return Cache<TSource, TKey>.SourceKeySelector; } }
			public LambdaExpression SourceKeySelectorComparer { get { return Cache<TSource, TKey>.SourceKeySelectorComparer; } }
		}
		public static class Cache<TSource, TKey>
		{
			public static readonly Expression<Func<IEnumerable<TSource>, Func<TSource, TKey>, IOrderedEnumerable<TSource>>> SourceKeySelector = (source, keySelector) => source.OrderBy(keySelector);
			public static readonly Expression<Func<IEnumerable<TSource>, Func<TSource, TKey>, IComparer<TKey>, IOrderedEnumerable<TSource>>> SourceKeySelectorComparer = (source, keySelector, comparer) => source.OrderBy(keySelector, comparer);
		}
	}
}
