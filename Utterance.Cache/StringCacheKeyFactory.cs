namespace Utterance
{
    using System;
    using System.Threading;

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
