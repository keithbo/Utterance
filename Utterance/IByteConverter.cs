namespace Utterance
{
	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Threading.Tasks;

	/// <summary>
	/// Basic converter interface that takes an arbitrary object type and returns a byte array
	/// </summary>
	public interface IByteConverter : IByteConverter<object>
	{
	}

	/// <summary>
	/// Generic converter interface that takes a strong typed instance and converts it to a byte array
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public interface IByteConverter<T>
	{
		/// <summary>
		/// Converts an object instance to a byte array
		/// </summary>
		/// <param name="value">Strong typed instance to be converted</param>
		/// <returns>byte array representation of the instance value</returns>
		byte[] Convert(T value);
	}
}
