﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Linq
{
	public static class Take
	{
		internal class Access<TSource> : Utterance.Access.AccessBase
		{
			public LambdaExpression SourceCount { get { return Cache<TSource>.SourceCount; } }
		}
		public static class Cache<TSource>
		{
			public static readonly Expression<Func<IEnumerable<TSource>, int, IEnumerable<TSource>>> SourceCount = (source, count) => source.Take(count);
		}
	}
}
