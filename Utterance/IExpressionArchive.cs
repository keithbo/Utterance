namespace Utterance
{
	using System.Collections.Generic;
	using System.Linq.Expressions;

	/// <summary>
	/// Basic interface that can be used to aggregate Expressions
	/// </summary>
	public interface IExpressionArchive
	{
		/// <summary>
		/// Add the given expression to this archive.
		/// </summary>
		/// <param name="expression">Expression</param>
		void Add(Expression expression);
	}
}
