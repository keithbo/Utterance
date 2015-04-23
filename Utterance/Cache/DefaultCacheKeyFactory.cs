namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public class DefaultCacheKeyFactory<TKey> : ICacheKeyFactory<TKey>
	{
		public TKey NewKey()
		{
			throw new NotSupportedException("Automatic key generation not supported");
		}
	}
}
