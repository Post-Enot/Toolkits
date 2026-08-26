using System;
using System.Buffers.Binary;
using System.Runtime.CompilerServices;

namespace PostEnot.Toolkits.RandomEngines
{
    public sealed class Xoshiro256StarStar : IRandomEngine
    {
        public Xoshiro256StarStar(ulong state0, ulong state1, ulong state2, ulong state3)
        {
            State0 = state0;
            State1 = state1;
            State2 = state2;
            State3 = state3;
        }

        public Xoshiro256StarStar(ReadOnlySpan<byte> state) => SetState(state);

        public int StateSizeInBytes => sizeof(ulong) * 4;

        public ulong State0 { get; private set; }
        public ulong State1 { get; private set; }
        public ulong State2 { get; private set; }
        public ulong State3 { get; private set; }

        public byte[] GetState()
        {
            byte[] state = new byte[StateSizeInBytes];
            Span<byte> buffer = state.AsSpan();
            BinaryPrimitives.WriteUInt64LittleEndian(buffer[..8], State0);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(8, 8), State1);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(16, 8), State2);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(24, 8), State3);
            return state;
        }

        public void GetState(Span<byte> buffer)
        {
            if (buffer.Length < StateSizeInBytes)
            {
                throw RandomUtilities.ExceptionBufferTooSmall(nameof(buffer), StateSizeInBytes);
            }
            BinaryPrimitives.WriteUInt64LittleEndian(buffer[..8], State0);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(8, 8), State1);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(16, 8), State2);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer.Slice(24, 8), State3);
        }

        public void SetState(ReadOnlySpan<byte> state)
        {
            if (!TrySetState(state))
            {
                throw RandomUtilities.ExceptionInvalidState(nameof(state));
            }
        }

        public bool TrySetState(ReadOnlySpan<byte> state)
        {
            if (state.Length < StateSizeInBytes)
            {
                return false;
            }
            State0 = BinaryPrimitives.ReadUInt64LittleEndian(state[..8]);
            State1 = BinaryPrimitives.ReadUInt64LittleEndian(state.Slice(8, 8));
            State2 = BinaryPrimitives.ReadUInt64LittleEndian(state.Slice(16, 8));
            State3 = BinaryPrimitives.ReadUInt64LittleEndian(state.Slice(24, 8));
            return true;
        }

        public void Discard()
        {
            unchecked
            {
                ulong t = State1 << 17;
                State2 ^= State0;
                State3 ^= State1;
                State1 ^= State2;
                State0 ^= State3;
                State2 ^= t;
                State3 = RotateLeft(State3, 45);
            }
        }

        public void Discard(int number)
        {
            if (number < 0)
            {
                throw RandomUtilities.ExceptionDiscardNumberLessThanZero(nameof(number));
            }
            unchecked
            {
                for (int i = 0; i < number; i += 1)
                {
                    Discard();
                }
            }
        }

        public bool NextBoolean() => NextUInt64() >> 63 == 1;

        public float NextSingle() => (NextUInt64() >> 40) * RandomUtilities.Inverse2Pow24;

        public double NextDouble() => (NextUInt64() >> 11) * RandomUtilities.Inverse2Pow53;

        public void NextBytes(Span<byte> buffer) => RandomUtilities.NextBytesFromUInt64(this, buffer);

        public uint NextUInt32() => (uint)(NextUInt64() >> 32);

        public ulong NextUInt64()
        {
            unchecked
            {
                ulong result = RotateLeft(State1 * 5, 7) * 9;
                ulong t = State1 << 17;
                State2 ^= State0;
                State3 ^= State1;
                State1 ^= State2;
                State0 ^= State3;
                State2 ^= t;
                State3 = RotateLeft(State3, 45);
                return result;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ulong RotateLeft(ulong x, int k) => (x << k) | (x >> (64 - k));
    }
}
