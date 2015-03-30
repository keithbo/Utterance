using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Rewrite
{
	internal class ParameterComposer : IParameterComposer
	{
		private readonly ParameterExpression _parameter;

		public ParameterComposer(ParameterExpression parameter)
		{
			_parameter = parameter;
		}
	}
}
