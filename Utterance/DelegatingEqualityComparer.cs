namespace Utterance
{
	using System;
	using System.Collections.Generic;

	/// <summary>
	/// Simple IEqualityComparer implementation that passes comparison to provided
	/// delegate functions.
	/// </summary>
	/// <typeparam name="T">Type to be compared</typeparam>
	public class DelegatingEqualityComparer<T> : IEqualityComparer<T>
	{
		private static IEqualityComparer<T> Default { get { return EqualityComparer<T>.Default; } }

		private readonly Func<T, T, bool> _equals;
		private readonly Func<T, int> _getHashCode;

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
