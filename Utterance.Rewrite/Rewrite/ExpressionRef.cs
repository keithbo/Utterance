using System;

namespace Utterance.Rewrite
{
    public sealed class ExpressionRef : IEquatable<ExpressionRef>
    {
        private readonly Guid _value;

        public ExpressionRef()
            : this(Guid.NewGuid())
        {

        }

        internal ExpressionRef(Guid value)
        {
            _value = value;
        }

        public bool Equals(ExpressionRef other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _value.Equals(other._value);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ExpressionRef) obj);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}