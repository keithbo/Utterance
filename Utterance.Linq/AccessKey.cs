namespace Utterance
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Utterance.Hashing;

    internal class AccessKey : IEquatable<AccessKey>
    {
        private readonly int _hashCode;

        public Access.MethodGroup Group { get; }

        public IReadOnlyCollection<Type> Types { get; }

        public AccessKey(Access.MethodGroup group, Type[] types)
        {
            var hash = new FNV1aHash();
            hash.Step(group.GetHashCode());
            hash.Step(types.Select(t => t.GetHashCode()));
            _hashCode = hash.Value.GetInt();

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