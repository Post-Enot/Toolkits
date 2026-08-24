using System;
using System.Buffers.Binary;

namespace PostEnot.Toolkits.RandomEngines
{
    public static class RandomUtilities
    {
        internal const float Inverse2Pow24 = 1.0f / (1U << 24);
        internal const double Inverse2Pow53 = 1.0 / (1UL << 53);

        internal static void NextBytesFromUInt64(IRandomEngine engine, Span<byte> buffer)
        {
            int offset = 0;
            while (buffer.Length - offset >= 8)
            {
                BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(offset, 8), engine.NextUInt64());
                offset += 8;
            }
            if (offset < buffer.Length)
            {
                ulong value = engine.NextUInt64();
                for (int i = offset; i < buffer.Length; i += 1)
                {
                    buffer[i] = (byte)value;
                    value >>= 8;
                }
            }
        }
    }
}
