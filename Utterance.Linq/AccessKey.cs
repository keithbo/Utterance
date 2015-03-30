using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utterance
{
	internal class AccessKey : IEquatable<AccessKey>
	{
		private readonly int _hashCode;

		public Access.MethodGroup Group
		{
			get;
			private set;
		}

		public IReadOnlyCollection<Type> Types
		{
			get;
			private set;
		}

		public AccessKey(Access.MethodGroup group, Type[] types)
		{
			var hash = new FNV1aHash();
			hash.Step(group.GetHashCode());
			hash.Step(types.Select(t => t.GetHashCode()).ToArray());
			_hashCode = hash.Value;

			Group = group;
			Types = new ReadOnlyCollection<Type>(types);
		}

		public bool Equals(AccessKey other)
		{
			return other != null
				&& Group == other.Group &&
				Types.SequenceEqual(other.Types);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as AccessKey);
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}
	}
}
