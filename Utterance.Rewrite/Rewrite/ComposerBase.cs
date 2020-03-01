namespace Utterance.Rewrite
{
    internal abstract class ComposerBase
    {
        public ExpressionRef Reference { get; }

        protected ComposerBase()
        {
            Reference = new ExpressionRef();
        }
    }
}