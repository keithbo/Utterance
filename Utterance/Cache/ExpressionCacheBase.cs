namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public abstract class ExpressionCacheBase<TKey, TExpression, TCacheItem> : CacheBase<TKey, TExpression, TCacheItem>
		where TKey : IEquatable<TKey>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<TKey, TExpression>
	{
		protected ExpressionCacheBase()
		{
		}

		protected ExpressionCacheBase(ICacheKeyFactory keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}
	}
}
