namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Threading;

	public abstract class CacheBase<TValue, TCacheItem> : CacheBase<string, TValue, TCacheItem>
		where TCacheItem : CacheItem<string, TValue>
	{

		protected CacheBase()
		{
		}

		protected CacheBase(ICacheKeyFactory keyFactory, IEqualityComparer<string> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		public class StringCacheKeyFactory : CacheBase<string, TValue, TCacheItem>.ICacheKeyFactory
		{
			private readonly string _root;
			private int _index;

			public StringCacheKeyFactory()
			{
				_root = Guid.NewGuid().ToString();
				_index = 0;
			}

			public string NewKey()
			{
				var next = Interlocked.Increment(ref _index);
				return _root + next;
			}
		}
	}

	public abstract class CacheBase<TKey, TValue, TCacheItem>
		where TKey : IEquatable<TKey>
		where TCacheItem : CacheItem<TKey, TValue>
	{
		private readonly IEqualityComparer<TKey> _keyEqualityComparer;
		private readonly Dictionary<TKey, TCacheItem> _cache;
		private readonly ICacheKeyFactory _keyFactory;
		private readonly object _synchronizationContext = new object();
		protected object SynchronizationContext
		{
			get { return _synchronizationContext; }
		}

		protected CacheBase()
			: this(null, null)
		{
		}

		protected CacheBase(ICacheKeyFactory keyFactory, IEqualityComparer<TKey> keyEqualityComparer)
		{
			_keyFactory = keyFactory ?? new DefaultCacheKeyFactory();
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

		protected abstract TCacheItem CreateCacheItem(TKey key, TValue value);

		public interface ICacheKeyFactory
		{
			TKey NewKey();
		}

		public class DefaultCacheKeyFactory : ICacheKeyFactory
		{
			public TKey NewKey()
			{
				throw new NotSupportedException("Automatic key generation not supported");
			}
		}
	}
}
