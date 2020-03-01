using System;

namespace Utterance.Rewrite
{
	public interface IParameterComposer
    {
        string Name { get; set; }

        Type Type { get; set; }

        bool IsByRef { get; set; }
    }
}
