using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utterance
{
	public class DelegatingByteConverter : DelegatingByteConverter<object>, IByteConverter
	{
		public DelegatingByteConverter(Func<object, byte[]> converter)
			: base(converter)
		{
		}
	}

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
