namespace Utterance
{
	using System.Collections.Generic;
	using System.Linq.Expressions;

	public interface IExpressionArchive
	{
		void Add(Expression expression);
	}
}
