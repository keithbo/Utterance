﻿namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public class ExpressionCache : ExpressionCache<string, ExpressionCacheItem>
	{
		public ExpressionCache()
			: base(new Cache<Expression, ExpressionCacheItem>.StringCacheKeyFactory(), EqualityComparer<string>.Default)
		{
		}

		protected override ExpressionCacheItem CreateCacheItem(string key, Expression value)
		{
			return new ExpressionCacheItem(key, value);
		}
	}

	public abstract class ExpressionCache<TKey, TCacheItem> : Cache<TKey, Expression, TCacheItem>
		where TKey : IEquatable<TKey>
		where TCacheItem : ExpressionCacheItem<TKey>
	{
		public ExpressionCache()
			: this(null, null)
		{
		}

		public ExpressionCache(ICacheKeyFactory core, IEqualityComparer<TKey> keyEqualityComparer)
			: base(core, keyEqualityComparer)
		{
		}
	}
}
