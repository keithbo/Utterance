namespace Utterance.Cache
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface ICacheKeyFactory<TKey>
	{
		TKey NewKey();
	}
}
