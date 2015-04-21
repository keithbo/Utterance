namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading;

	/// <summary>
	/// ExpressionVisitor implementation based on IdentityExpressionVisitor. This class will
	/// construct an FNV1a hash representation of an Expression tree for use as a hashcode impementation.
	/// HashCode output is repeatable given identical input.
	/// This impementation includes parameter names in uniqueness calculations by default.
	/// </summary>
	public class HashCodeExpressionVisitor : IdentityExpressionVisitor
	{
		private bool _valueExists;
		private bool _needsHash;
		private readonly FNV1aHash _hash;

		/// <summary>
		/// Gets the calculated HashCode value for the most recent Expression tree visited.
		/// </summary>
		public int HashCode
		{
			get
			{
				if (!_valueExists)
				{
					return 0;
				}
				if (_needsHash)
				{
					_needsHash = false;
					_hash.Step(ToIntegerArray(this.Bytes));
				}
				return _hash.Value; 
			}
		}

		private const int StepSize = sizeof(int);
		private static IEnumerable<int> ToIntegerArray(byte[] bytes)
		{
			var bytesOffsetGap = bytes.Length % StepSize;
			var bytesWholeLength = bytes.Length - bytesOffsetGap;
			for (int b = 0; b < bytesWholeLength; b += StepSize)
			{
				yield return BitConverter.ToInt32(bytes, b);
			}
			if (bytesOffsetGap > 0)
			{
				var lastBytes = new byte[StepSize];
				Array.Copy(bytes, bytesWholeLength, lastBytes, 0, bytesOffsetGap);
				yield return BitConverter.ToInt32(lastBytes, 0);
			}
		}

		public HashCodeExpressionVisitor()
		{
			_hash = new FNV1aHash();
			this.IncludePropertyNames = true;
		}

		protected override void Reset()
		{
			base.Reset();
			_hash.Reset();
			_valueExists = true;
			_needsHash = true;
		}
	}
}
