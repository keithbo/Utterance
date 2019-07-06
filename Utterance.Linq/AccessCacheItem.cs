using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance
{
	internal class AccessCacheItem : ExpressionCacheItem<AccessKey, Expression<ObjectFactory<Access.IAccess>>>
	{
		private ObjectFactory<Access.IAccess> _compiled;

		public ObjectFactory<Access.IAccess> Compiled
		{
			get
			{
				if (_compiled == null)
				{
					_compiled = Value.Compile();
				}
				return _compiled;
			}
		}

		public AccessCacheItem(AccessKey key, Expression<ObjectFactory<Access.IAccess>> expression)
			: base(key, expression)
		{
		}
	}
}
