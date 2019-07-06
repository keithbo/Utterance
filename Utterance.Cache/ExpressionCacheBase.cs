namespace Utterance
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
	/// A base Cache implementation that stores Expression trees using an equatable key type for lookup
	/// and retrieval.
	/// </summary>
	/// <typeparam name="TKey">Type that is IEquatable</typeparam>
	/// <typeparam name="TExpression">Type derived from Expression</typeparam>
	/// <typeparam name="TCacheItem">Type derived from ExpressionCacheItem</typeparam>
	public abstract class ExpressionCacheBase<TKey, TExpression, TCacheItem> : CacheBase<TKey, TExpression, TCacheItem>
		where TKey : IEquatable<TKey>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<TKey, TExpression>
	{
		protected ExpressionCacheBase(ICacheKeyFactory<TKey> keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}
	}
}
