namespace Utterance
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
	/// CacheItem implementation that explicitly stores Expression derivatives for a Cache implementation.
	/// </summary>
	/// <typeparam name="TKey">IEquatable type used as a key for lookup</typeparam>
	/// <typeparam name="TExpression">Expression derived type for storage</typeparam>
	public class ExpressionCacheItem<TKey, TExpression> : CacheItem<TKey, TExpression>
		where TKey : IEquatable<TKey>
		where TExpression : Expression
	{
		public ExpressionCacheItem(TKey key, TExpression expression)
			: base(key, expression)
		{
		}
	}
}
