using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Cache
{
	public class CacheItem<TValue> : CacheItem<string, TValue>
	{
		public CacheItem(string key, TValue value)
			: base(key, value)
		{
		}
	}

	public class CacheItem<TKey, TValue>
		where TKey : IEquatable<TKey>
	{
		public CacheItem(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public TKey Key { get; protected set; }
		public TValue Value { get; protected set; }
	}
}
