using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utterance.Cache;

namespace Utterance
{
	internal class AccessCache : ExpressionCacheBase<AccessKey, Expression<ObjectFactory<Access.IAccess>>, AccessCacheItem>
	{
		public AccessCache()
			: this(null, null)
		{
		}

		public AccessCache(ICacheKeyFactory<AccessKey> keyFactory, IEqualityComparer<AccessKey> keyEqualityComparer)
			: base(keyFactory, keyEqualityComparer)
		{
		}

		protected override AccessCacheItem CreateCacheItem(AccessKey key, Expression<ObjectFactory<Access.IAccess>> value)
		{
			return new AccessCacheItem(key, value);
		}
	}
}
