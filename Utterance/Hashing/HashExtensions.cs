namespace Utterance.Hashing
{
    using System.Collections.Generic;

    public static class HashExtensions
    {
        public static void Step(this FNV1aHash hash, params int[] steps)
        {
            hash.Step(HashHelpers.ToBytes(steps));
        }

        public static void Step(this FNV1aHash hash, IEnumerable<int> steps)
        {
            hash.Step(HashHelpers.ToBytes(steps));
        }
    }
}