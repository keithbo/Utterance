namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// CacheItemExpression is an Expression implementation that uses the CanReduce behavior of Expression
	/// tree visitation to substitute a cached Expression tree in its stead. This allows for scenarios where
	/// a pre-existing Expression tree can be substituted for potential performance gains.
	/// This implementation uses string as its key lookup type.
	/// </summary>
	/// <typeparam name="TExpression">Type derived from Expression</typeparam>
	/// <typeparam name="TCacheItem">Type derived from ExpressionCacheItem</typeparam>
	public class CacheItemExpression<TExpression, TCacheItem> : CacheItemExpression<string, TExpression, TCacheItem>
		where TExpression : Expression
		where TCacheItem : ExpressionCacheItem<string, TExpression>
	{
		public CacheItemExpression(TCacheItem cacheItem)
			: base(cacheItem)
		{
		}
	}

	/// <summary>
	/// CacheItemExpression is an Expression implementation that uses the CanReduce behavior of Expression
	/// tree visitation to substitute a cached Expression tree in its stead. This allows for scenarios where
	/// a pre-existing Expression tree can be substituted for potential performance gains.
	/// </summary>
	/// <typeparam name="TKey">Type that implements IEquatable</typeparam>
	/// <typeparam name="TExpression">Type derived from Expression</typeparam>
	/// <typeparam name="TCacheItem">Type derived from ExpressionCacheItem</typeparam>
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
