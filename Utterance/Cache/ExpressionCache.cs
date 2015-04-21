namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Cache implementation that stores and retrieves Expression trees for performance expression interaction.
	/// This implementation uses string as its key type and ExpressionCacheItem as its internal storage unit.
	/// </summary>
	/// <typeparam name="TExpression">Type derived from Expression</typeparam>
	public class ExpressionCache<TExpression> : ExpressionCacheBase<string, TExpression, ExpressionCacheItem<TExpression>>
		where TExpression : Expression
	{
		public ExpressionCache()
			: this(new CacheBase<TExpression, ExpressionCacheItem<TExpression>>.StringCacheKeyFactory(), EqualityComparer<string>.Default)
		{
		}

		public ExpressionCache(ICacheKeyFactory keyFactory, IEqualityComparer<string> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		protected override ExpressionCacheItem<TExpression> CreateCacheItem(string key, TExpression value)
		{
			return new ExpressionCacheItem<TExpression>(key, value);
		}
	}

	/// <summary>
	/// Cache implementation that stores and retrieves Expression trees for performance expression interaction.
	/// This implementation uses ExpressionCacheItem as its internal storage unit.
	/// </summary>
	/// <typeparam name="TKey">Type that implements IEquatable</typeparam>
	/// <typeparam name="TExpression">Type derived from Expression</typeparam>
	public class ExpressionCache<TKey, TExpression> : ExpressionCacheBase<TKey, TExpression, ExpressionCacheItem<TKey, TExpression>>
		where TKey : IEquatable<TKey>
		where TExpression : Expression
	{
		public ExpressionCache()
		{
		}

		public ExpressionCache(ICacheKeyFactory keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		protected override ExpressionCacheItem<TKey, TExpression> CreateCacheItem(TKey key, TExpression value)
		{
			return new ExpressionCacheItem<TKey, TExpression>(key, value);
		}
	}
}
