namespace Utterance.Hashing
{
    using System;
    using System.Linq;
    using Xunit;

    public class Fnv1aHashTests
    {
        [Fact]
        public void SimpleTest()
        {
            var bytes = new byte[]
            {
                1, 2, 3, 4, 5
            };

            int prime = 16777619;
            uint uprime = 16777619;

            var h1 = bytes.Aggregate(unchecked((int)2166136261), (r, o) => (r ^ o) * prime);

            var h2 = bytes.Aggregate(2166136261, (r, o) => (r ^ o) * uprime);

            Assert.Equal(BitConverter.GetBytes(h1), BitConverter.GetBytes(h2));
        }
    }
}