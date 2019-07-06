namespace Utterance
{
    using System;

    public class DefaultCacheKeyFactory<TKey> : ICacheKeyFactory<TKey>
	{
		public TKey NewKey()
		{
			throw new NotSupportedException("Automatic key generation not supported");
		}
	}
}
