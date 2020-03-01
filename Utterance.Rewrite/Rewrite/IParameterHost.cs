namespace Utterance.Rewrite
{
    public interface IParameterHost
    {
        IParameterComposer Sibling(ExpressionRef reference, Direction direction);
    }
}