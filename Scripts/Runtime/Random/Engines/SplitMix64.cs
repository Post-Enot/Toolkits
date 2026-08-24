using System;
using System.Buffers.Binary;

namespace PostEnot.Toolkits.RandomEngines
{
    public sealed class SplitMix64 : IRandomEngine
    {
        public const ulong GoldenGamma = 0x9E3779B97F4A7C15UL;
        public const ulong Mix1 = 0xBF58476D1CE4E5B9UL;
        public const ulong Mix2 = 0x94D049BB133111EBUL;

        public int StateSizeInBytes => sizeof(ulong);

        public ulong State { get; private set; }

        public byte[] GetState()
        {
            byte[] state = new byte[StateSizeInBytes];
            BinaryPrimitives.WriteUInt64LittleEndian(state, State);
            return state;
        }

        public void GetState(Span<byte> buffer)
        {
            if (buffer.Length < StateSizeInBytes)
            {
                throw new ArgumentException("Buffer is too small.", nameof(buffer));
            }
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, State);
        }

        public void SetState(ReadOnlySpan<byte> state)
        {
            if (!TrySetState(state))
            {
                throw new ArgumentException("Invalid state.", nameof(state));
            }
        }

        public bool TrySetState(ReadOnlySpan<byte> state)
        {
            if (state.Length < StateSizeInBytes)
            {
                return false;
            }
            State = BinaryPrimitives.ReadUInt64LittleEndian(state[..8]);
            return true;
        }

        public bool NextBoolean() => NextUInt64() >> 63 == 1;

        public void NextBytes(Span<byte> buffer) => RandomUtilities.NextBytesFromUInt64(this, buffer);

        public float NextSingle() => (NextUInt64() >> 40) * RandomUtilities.Inverse2Pow24;

        public double NextDouble() => (NextUInt64() >> 11) * RandomUtilities.Inverse2Pow53;

        public uint NextUInt32() => (uint)(NextUInt64() >> 32);

        public ulong NextUInt64()
        {
            unchecked
            {
                State += GoldenGamma;
                ulong z = State;
                z = (z ^ (z >> 30)) * Mix1;
                z = (z ^ (z >> 27)) * Mix2;
                return z ^ (z >> 31);
            }
        }

        public static ulong Next(ref ulong x)
        {
            unchecked
            {
                x += GoldenGamma;
                ulong z = x;
                z = (z ^ (z >> 30)) * Mix1;
                z = (z ^ (z >> 27)) * Mix2;
                return z ^ (z >> 31);
            }
        }
    }
}
