using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Rewrite
{
	public interface IExpressionComposer
	{
		IExpressionComposer Parameter(int index, Action<IParameterComposer> compose);
    }
}
