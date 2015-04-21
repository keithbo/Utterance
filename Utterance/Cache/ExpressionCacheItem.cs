namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// CacheItem implementation that uses <see cref="string"/> as its key type.
	/// </summary>
	/// <typeparam name="TExpression">Expression derived type for storage</typeparam>
	public class ExpressionCacheItem<TExpression> : ExpressionCacheItem<string, TExpression>
		where TExpression : Expression
	{
		public ExpressionCacheItem(string key, TExpression expression)
			: base(key, expression)
		{
		}
	}

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
