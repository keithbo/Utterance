namespace Utterance
{
    public interface ICacheKeyFactory<TKey>
    {
        TKey NewKey();
    }
}