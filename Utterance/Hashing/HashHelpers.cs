namespace Utterance.Hashing
{
    using System;
    using System.Collections.Generic;

    public static class HashHelpers
    {
        private const int StepSize = sizeof(int);
        public static IEnumerable<int> ToIntegerArray(byte[] bytes)
        {
            var bytesOffsetGap = bytes.Length % StepSize;
            var bytesWholeLength = bytes.Length - bytesOffsetGap;
            for (int b = 0; b < bytesWholeLength; b += StepSize)
            {
                yield return BitConverter.ToInt32(bytes, b);
            }
            if (bytesOffsetGap > 0)
            {
                var lastBytes = new byte[StepSize];
                Array.Copy(bytes, bytesWholeLength, lastBytes, 0, bytesOffsetGap);
                yield return BitConverter.ToInt32(lastBytes, 0);
            }
        }

        public static IEnumerable<byte> ToBytes(IEnumerable<int> ints)
        {
            foreach (var i in ints)
            {
                var bytes = BitConverter.GetBytes(i);
                foreach (var b in bytes)
                {
                    yield return b;
                }
            }
        }

        /// <summary>
        /// Partition a source enumerable into smaller segments of <paramref name="chunkSize"/> elements each.
        /// </summary>
        /// <typeparam name="T">The source type</typeparam>
        /// <param name="source">The source.</param>
        /// <param name="chunkSize">number of elements per partition</param>
        /// <returns></returns>
        public static IEnumerable<T[]> Buffer<T>(this IEnumerable<T> source, int chunkSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (chunkSize <= 0)
            {
                throw new ArgumentException("chunkSize must be greater than 0", nameof(chunkSize));
            }

            var n = 0;
            List<T> chunk = null;
            foreach (var item in source)
            {
                if (chunk == null)
                {
                    chunk = new List<T>();
                }

                chunk.Add(item);

                n = (n + 1) % chunkSize;
                if (n != 0) continue;

                yield return chunk.ToArray();
                chunk = null;
            }
            if (chunk != null)
            {
                yield return chunk.ToArray();
            }
        }

    }
}