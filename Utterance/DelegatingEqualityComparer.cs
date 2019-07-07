namespace Utterance
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    ///     Simple IEqualityComparer implementation that passes comparison to provided
    ///     delegate functions.
    /// </summary>
    /// <typeparam name="T">Type to be compared</typeparam>
    public class DelegatingEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _equals;
        private readonly Func<T, int> _getHashCode;
        private static IEqualityComparer<T> Default => EqualityComparer<T>.Default;

        public DelegatingEqualityComparer(Func<T, T, bool> equals)
            : this(equals, Default.GetHashCode)
        {
            _equals = equals;
        }

        public DelegatingEqualityComparer(Func<T, T, bool> equals, Func<T, int> getHashCode)
        {
            _equals = equals;
            _getHashCode = getHashCode;
        }

        public bool Equals(T x, T y)
        {
            return _equals(x, y);
        }

        public int GetHashCode(T obj)
        {
            return _getHashCode(obj);
        }
    }
}