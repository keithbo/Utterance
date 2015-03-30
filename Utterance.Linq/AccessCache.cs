using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Utterance.Cache;

namespace Utterance
{
	internal class AccessCache : ExpressionCache<AccessKey, Expression<ObjectFactory<Access.IAccess>>, AccessCacheItem>
	{
		protected override AccessCacheItem CreateCacheItem(AccessKey key, Expression<ObjectFactory<Access.IAccess>> value)
		{
			return new AccessCacheItem(key, value);
		}
	}
}
