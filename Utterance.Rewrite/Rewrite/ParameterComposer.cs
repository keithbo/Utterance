using System;
using System.Linq.Expressions;

namespace Utterance.Rewrite
{
	internal class ParameterComposer : ComposerBase, IParameterComposer
    {
        private readonly IParameterHost _host;
		private ParameterExpression _original;
        private string _name;
        private Type _type;
        private bool _byRef;

		public ParameterComposer(IParameterHost host, ParameterExpression original)
        {
            _host = host ?? throw new ArgumentNullException(nameof(host));
			_original = original;
		}

        public string Name
        {
            get => _original?.Name ?? _name;
            set
            {
                Extract();

                _name = value;
            }
        }

        public Type Type
        {
            get => _original?.Type ?? _type;
            set
            {
                Extract();

                _type = value;
            }
        }

        public bool IsByRef
        {
            get => _original?.IsByRef ?? _byRef;
            set
            {
                Extract();

                _byRef = value;
            }
        }

        public ParameterExpression Build()
        {
            if (_original != null) return _original;

            return string.IsNullOrEmpty(_name)
                ? Expression.Parameter(_type)
                : Expression.Parameter(_type, _name);
        }

        private void Extract()
        {
            if (_original == null) return;

            _byRef = _original.IsByRef;
            _name = _original.Name;
            _type = _original.Type;
            _original = null;
        }
	}
}
