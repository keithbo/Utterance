namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	public class CacheItemExpression<TExpression, TCacheItem> : CacheItemExpression<string, TExpression, TCacheItem>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<TExpression>
	{
		public CacheItemExpression(TCacheItem cacheItem)
			: base(cacheItem)
		{
		}
	}

	public abstract class CacheItemExpression<TKey, TExpression, TCacheItem> : Expression
		where TKey : IEquatable<TKey>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<TKey, TExpression>
	{
		public TCacheItem CacheItem
		{
			get;
			private set;
		}

		public override bool CanReduce
		{
			get
			{
				return true;
			}
		}

		protected CacheItemExpression(TCacheItem cacheItem)
		{
			if (cacheItem == null)
			{
				throw new ArgumentNullException();
			}
			CacheItem = cacheItem;
		}

		public override Expression Reduce()
		{
			return CacheItem.Value;
		}
	}
}
