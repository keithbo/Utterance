namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Simple implementation of the IByteConverter and IByteConverter[object] interfaces that
	/// uses a provided converter function as its Convert call.
	/// </summary>
	public class DelegatingByteConverter : DelegatingByteConverter<object>, IByteConverter
	{
		public DelegatingByteConverter(Func<object, byte[]> converter)
			: base(converter)
		{
		}
	}

	/// <summary>
	/// Simple implementation of the IByteConverter[T] interfaces that
	/// uses a provided converter function as its Convert call.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public class DelegatingByteConverter<T> : IByteConverter<T>
	{
		private readonly Func<T, byte[]> _converter;

		public DelegatingByteConverter(Func<T, byte[]> converter)
		{
			if (converter == null)
			{
				throw new ArgumentNullException("converter");
			}

			_converter = converter;
		}

		public byte[] Convert(T value)
		{
			return _converter(value);
		}
	}
}
