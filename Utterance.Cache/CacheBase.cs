﻿namespace Utterance
{
    using System;
    using System.Collections.Generic;

    /// <summary>
	/// Base caching class provides for easy extension and thread safe storage and retrieval of
	/// keyed data. This class provides the basic mechanism used by any downstream cache implementations
	/// without hard-coding the storage type of the cache. As such it is up to the implementing class to
	/// determine what internal storage unit is used. CacheItem may be sufficient, but can be extended
	/// for use in subsequent implementations.
	/// The only real constraint this class has is that key types must be IEquatable in order to determine
	/// storage bucket and lookup.
	/// </summary>
	/// <typeparam name="TKey">Type that implements IEquatable</typeparam>
	/// <typeparam name="TValue">Type to be stored</typeparam>
	/// <typeparam name="TCacheItem">Type derived from CacheItem</typeparam>
	/// <see cref="CacheItem"/>
	public abstract class CacheBase<TKey, TValue, TCacheItem>
		where TKey : IEquatable<TKey>
		where TCacheItem : CacheItem<TKey, TValue>
	{
		private readonly IEqualityComparer<TKey> _keyEqualityComparer;
		private readonly Dictionary<TKey, TCacheItem> _cache;
		private readonly ICacheKeyFactory<TKey> _keyFactory;
		private readonly object _synchronizationContext = new object();
		protected object SynchronizationContext
		{
			get { return _synchronizationContext; }
		}

		protected CacheBase(ICacheKeyFactory<TKey> keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
		{
			_keyFactory = keyFactory ?? new DefaultCacheKeyFactory<TKey>();
			_keyEqualityComparer = keyEqualityComparer ?? EqualityComparer<TKey>.Default;
			_cache = new Dictionary<TKey, TCacheItem>(_keyEqualityComparer);
		}

		#region AddAll

		public void AddAll(IDictionary<TKey, TValue> map)
		{
			if (map == null)
			{
				throw new ArgumentNullException("map");
			}
			AddAllInternal(map);
		}

		protected void AddAllInternal(IDictionary<TKey, TValue> map)
		{
			lock (SynchronizationContext)
			{
				foreach (var pair in map)
				{
					_cache.Add(pair.Key, CreateCacheItem(pair.Key, pair.Value));
				}
			}
		}

		#endregion AddAll

		#region AddOrUpdate

		public void AddOrUpdate(TKey key, TValue value, Func<TKey, TValue, TValue> update)
		{
			if (update == null)
			{
				throw new ArgumentNullException("update");
			}
			AddOrUpdateInternal(key, _ => value, update);
		}

		public void AddOrUpdate(TKey key, Func<TKey, TValue> add, Func<TKey, TValue, TValue> update)
		{
			if (add == null)
			{
				throw new ArgumentNullException("add");
			}
			if (update == null)
			{
				throw new ArgumentNullException("update");
			}
			AddOrUpdateInternal(key, add, update);
		}

		protected TCacheItem AddOrUpdateInternal(TKey key, Func<TKey, TValue> add, Func<TKey, TValue, TValue> update)
		{
			lock (SynchronizationContext)
			{
				TCacheItem item;
				if (_cache.TryGetValue(key, out item))
				{
					item = CreateCacheItem(key, update(key, item.Value));
				}
				else
				{
					item = CreateCacheItem(key, add(key));
				}

				_cache[key] = item;
				return item;
			}
		}

		#endregion AddOrUpdate

		#region GetOrAdd

		public TCacheItem GetOrAdd(TKey key, TValue value)
		{
			return GetOrAddInternal(key, _ => value);
		}

		public TCacheItem GetOrAdd(TKey key, Func<TKey, TValue> add)
		{
			if (add == null)
			{
				throw new ArgumentNullException("add");
			}
			return GetOrAddInternal(key, add);
		}

		protected TCacheItem GetOrAddInternal(TKey key, Func<TKey, TValue> add)
		{
			lock (SynchronizationContext)
			{
				TCacheItem item;
				if (!_cache.TryGetValue(key, out item))
				{
					item = CreateCacheItem(key, add(key));
					_cache.Add(key, item);
				}
				return item;
			}
		}

		#endregion GetOrAdd

		#region Get

		public TCacheItem Get(TKey key)
		{
			return GetInternal(key);
		}

		protected TCacheItem GetInternal(TKey key)
		{
			lock (SynchronizationContext)
			{
				TCacheItem item;
				_cache.TryGetValue(key, out item);
				return item;
			}
		}

		#endregion Get

		public void Clear()
		{
			lock (SynchronizationContext)
			{
				_cache.Clear();
			}
		}

		protected abstract TCacheItem CreateCacheItem(TKey key, TValue value);
	}
}
