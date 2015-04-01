namespace Utterance.Cache
{
	using System;

	public class Cache<TValue> : CacheBase<TValue, CacheItem<TValue>>
	{
		public Cache()
		{
		}

		protected override CacheItem<TValue> CreateCacheItem(string key, TValue value)
		{
			return new CacheItem<TValue>(key, value);
		}
	}

	public class Cache<TKey, TValue> : CacheBase<TKey, TValue, CacheItem<TKey, TValue>>
		where TKey : IEquatable<TKey>
	{
		public Cache()
		{
		}

		protected override CacheItem<TKey, TValue> CreateCacheItem(TKey key, TValue value)
		{
			return new CacheItem<TKey, TValue>(key, value);
		}
	}
}
