namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	public class StringCacheKeyFactory : ICacheKeyFactory<string>
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
