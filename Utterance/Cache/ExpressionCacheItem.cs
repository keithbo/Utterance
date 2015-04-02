namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public class ExpressionCacheItem<TExpression> : ExpressionCacheItem<string, TExpression>
		where TExpression : Expression
	{
		public ExpressionCacheItem(string key, TExpression expression)
			: base(key, expression)
		{
		}
	}

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
