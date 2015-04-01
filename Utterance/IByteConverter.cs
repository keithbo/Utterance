namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	public interface IByteConverter : IByteConverter<object>
	{
	}

	public interface IByteConverter<T>
	{
		byte[] Convert(T value);
	}
}
