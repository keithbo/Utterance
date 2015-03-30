namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public class ExpressionCache<TExpression> : ExpressionCache<string, TExpression, ExpressionCacheItem<TExpression>>
		where TExpression : Expression
	{
		public ExpressionCache()
			: base(new Cache<TExpression, ExpressionCacheItem<TExpression>>.StringCacheKeyFactory(), EqualityComparer<string>.Default)
		{
		}

		protected override ExpressionCacheItem<TExpression> CreateCacheItem(string key, TExpression value)
		{
			return new ExpressionCacheItem<TExpression>(key, value);
		}
	}

	public abstract class ExpressionCache<TKey, TExpression, TCacheItem> : Cache<TKey, TExpression, TCacheItem>
		where TKey : IEquatable<TKey>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<TKey, TExpression>
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
