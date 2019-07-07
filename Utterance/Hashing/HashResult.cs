namespace Utterance.Hashing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public sealed class HashResult : IEquatable<HashResult>
    {
        private readonly byte[] _bytes;

        public HashResult(byte[] byteValue)
        {
            _bytes = byteValue;
        }

        public HashResult(IEnumerable<byte> byteValue)
        {
            _bytes = byteValue.ToArray();
        }

        public HashResult(int intValue)
        {
            _bytes = BitConverter.GetBytes(intValue);
        }

        public HashResult(uint uintValue)
        {
            _bytes = BitConverter.GetBytes(uintValue);
        }

        public byte[] GetBytes()
        {
            return _bytes.ToArray();
        }

        public int GetInt()
        {
            if (_bytes.Length != 4)
            {
                throw new InvalidOperationException();
            }

            return BitConverter.ToInt32(_bytes, 0);
        }

        public uint GetUInt()
        {
            if (_bytes.Length != 4)
            {
                throw new InvalidOperationException();
            }

            return BitConverter.ToUInt32(_bytes, 0);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var b in _bytes)
            {
                sb.Append(b.ToString("X"));
            }

            return sb.ToString();
        }

        /// <inheritdoc />
        public bool Equals(HashResult other)
        {
            return ReferenceEquals(this, other) ||
                   !(other is null) &&
                   Equals(_bytes, other._bytes);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is HashResult other && Equals(other);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            if (_bytes.Length == 0)
            {
                return 0;
            }

            unchecked
            {
                var hashCode = _bytes[0].GetHashCode();
                for (var i = 1; i < _bytes.Length; i++)
                {
                    hashCode = (hashCode * 397) ^ _bytes[i].GetHashCode();
                }

                return hashCode;
            }
        }
    }
}