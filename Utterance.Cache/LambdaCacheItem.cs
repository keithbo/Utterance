namespace Utterance
{
    using System;
    using System.Linq.Expressions;

    public class LambdaCacheItem<TKey> : ExpressionCacheItem<TKey, LambdaExpression>
        where TKey : IEquatable<TKey>
    {
        private Delegate _compiled;

        public Delegate Compiled
        {
            get
            {
                if (_compiled == null) _compiled = Value.Compile();
                return _compiled;
            }
        }

        public LambdaCacheItem(TKey key, LambdaExpression lambdaExpression)
            : base(key, lambdaExpression)
        {
        }
    }

    public class LambdaCacheItem<TKey, TDelegate> : ExpressionCacheItem<TKey, Expression<TDelegate>>
        where TKey : IEquatable<TKey>
    {
        private TDelegate _compiled;

        public TDelegate Compiled
        {
            get
            {
                if (_compiled == null) _compiled = Value.Compile();
                return _compiled;
            }
        }

        public LambdaCacheItem(TKey key, Expression<TDelegate> lambdaExpression)
            : base(key, lambdaExpression)
        {
        }
    }
}