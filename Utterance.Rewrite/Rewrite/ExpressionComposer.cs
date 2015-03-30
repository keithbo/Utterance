using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Utterance.Rewrite
{
	internal class ExpressionComposer : IExpressionComposer
	{
		private readonly Dictionary<int, ParameterComposer> _parameterComposition = new Dictionary<int,ParameterComposer>();

		private readonly LambdaExpression _template;
		private readonly ParameterExpression[] _templateParameters;

		public ExpressionComposer(LambdaExpression template)
		{
			_template = template;
			_templateParameters = _template.Parameters.ToArray();
		}

		public IExpressionComposer Parameter(int index, Action<IParameterComposer> compose)
		{
			ParameterComposer composer;
			if (!_parameterComposition.TryGetValue(index, out composer))
			{
				composer = new ParameterComposer(_templateParameters[index]);
				_parameterComposition.Add(index, composer);
			}
			compose(composer);

			return this;
		}
	}
}
