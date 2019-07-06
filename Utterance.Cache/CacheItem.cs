namespace Utterance
{
    using System;

    /// <summary>
	/// Base cache storage unit. This class can be extended for downstream <see cref="Cache"/> implementations
	/// to allow additional data to be stored alongside the base values.
	/// </summary>
	/// <typeparam name="TKey">Type that implements IEquatable</typeparam>
	/// <typeparam name="TValue">Type to be stored</typeparam>
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
