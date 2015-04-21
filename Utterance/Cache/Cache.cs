namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Implementation of Cache that uses string as its key type and CacheItem as its storage unit.
	/// </summary>
	/// <typeparam name="TValue">Type to be stored</typeparam>
	public class Cache<TValue> : CacheBase<TValue, CacheItem<TValue>>
	{
		public Cache()
			: this(new CacheBase<TValue, CacheItem<TValue>>.StringCacheKeyFactory(), EqualityComparer<string>.Default)
		{
		}

		public Cache(ICacheKeyFactory keyFactory, IEqualityComparer<string> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		protected override CacheItem<TValue> CreateCacheItem(string key, TValue value)
		{
			return new CacheItem<TValue>(key, value);
		}
	}

	/// <summary>
	/// Implementation of Cache that uses the base CacheItem as its storage unit.
	/// Must provide the key type and data type.
	/// </summary>
	/// <typeparam name="TKey">Type that implements IEquatable</typeparam>
	/// <typeparam name="TValue">Type to be stored</typeparam>
	public class Cache<TKey, TValue> : CacheBase<TKey, TValue, CacheItem<TKey, TValue>>
		where TKey : IEquatable<TKey>
	{
		public Cache()
		{
		}

		public Cache(ICacheKeyFactory keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		protected override CacheItem<TKey, TValue> CreateCacheItem(TKey key, TValue value)
		{
			return new CacheItem<TKey, TValue>(key, value);
		}
	}
}
