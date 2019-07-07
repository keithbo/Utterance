namespace Utterance
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class LambdaCache<TKey>
        : ExpressionCacheBase<TKey, LambdaExpression, LambdaCacheItem<TKey>>
        where TKey : IEquatable<TKey>
    {
        public LambdaCache()
            : this(null, null)
        {
        }

        public LambdaCache(ICacheKeyFactory<TKey> keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
            : base(keyFactory, keyEqualityComparer)
        {
        }

        protected override LambdaCacheItem<TKey> CreateCacheItem(TKey key, LambdaExpression value)
        {
            return new LambdaCacheItem<TKey>(key, value);
        }
    }

    //public class LambdaCache<TDelegate> : LambdaCache<string, TDelegate>
    //{
    //	public LambdaCache()
    //		: this(null, null)
    //	{
    //	}

    //	public LambdaCache(ICacheKeyFactory<string> keyFactory, IEqualityComparer<string> keyEqualityComparer)
    //		: base(keyFactory ?? new StringCacheKeyFactory(), keyEqualityComparer)
    //	{
    //	}
    //}

    public class LambdaCache<TKey, TDelegate>
        : ExpressionCacheBase<TKey, Expression<TDelegate>, LambdaCacheItem<TKey, TDelegate>>
        where TKey : IEquatable<TKey>
    {
        public LambdaCache()
            : this(null, null)
        {
        }

        public LambdaCache(ICacheKeyFactory<TKey> keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
            : base(keyFactory, keyEqualityComparer)
        {
        }

        protected override LambdaCacheItem<TKey, TDelegate> CreateCacheItem(TKey key, Expression<TDelegate> value)
        {
            return new LambdaCacheItem<TKey, TDelegate>(key, value);
        }
    }
}