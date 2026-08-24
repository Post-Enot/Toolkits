using System;
using System.Buffers.Binary;

namespace PostEnot.Toolkits.RandomEngines
{
    public sealed class Pcg32 : IRandomEngine
    {
        private const ulong Multiplier = 6364136223846793005UL;

        public ulong State { get; private set; }
        public ulong Increment { get; private set; }

        public int StateSizeInBytes => 16;

        public Pcg32(ulong seed, ulong sequence)
        {
            Increment = (sequence << 1) | 1UL;
            State = 0;
            NextUInt32();
            State += seed;
            NextUInt32();
        }

        public byte[] GetState()
        {
            byte[] buffer = new byte[StateSizeInBytes];
            GetState(buffer);
            return buffer;
        }

        public void GetState(Span<byte> buffer)
        {
            if (buffer.Length < StateSizeInBytes)
            {
                throw new ArgumentException(
                    $"Buffer is too small. Expected at least {StateSizeInBytes} bytes.",
                    nameof(buffer));
            }
            BinaryPrimitives.WriteUInt64LittleEndian(buffer, State);
            BinaryPrimitives.WriteUInt64LittleEndian(buffer[8..], Increment);
        }

        public void SetState(ReadOnlySpan<byte> state)
        {
            if (!TrySetState(state))
            {
                throw new ArgumentException("Invalid state. Expected at least 16 bytes.", nameof(state));
            }
        }

        public bool TrySetState(ReadOnlySpan<byte> state)
        {
            if (state.Length < StateSizeInBytes)
            {
                return false;
            }
            ulong newState = BinaryPrimitives.ReadUInt64LittleEndian(state);
            ulong newIncrement = BinaryPrimitives.ReadUInt64LittleEndian(state[8..]);
            State = newState;
            Increment = newIncrement;
            return true;
        }

        public bool NextBoolean() => (NextUInt32() >> 31) == 1;

        public uint NextUInt32()
        {
            unchecked
            {
                ulong oldState = State;
                State = oldState * Multiplier + Increment;
                uint xorShifted = (uint)(((oldState >> 18) ^ oldState) >> 27);
                int rot = (int)(oldState >> 59);
                return (xorShifted >> rot) | (xorShifted << ((-rot) & 31));
            }
        }

        public ulong NextUInt64()
        {
            uint high = NextUInt32();
            uint low = NextUInt32();
            return ((ulong)high << 32) | low;
        }

        public float NextSingle() => (NextUInt32() >> 8) * RandomUtilities.Inverse2Pow24;

        public double NextDouble() => (NextUInt64() >> 11) * RandomUtilities.Inverse2Pow53;

        public void NextBytes(Span<byte> buffer)
        {
            while (buffer.Length >= 4)
            {
                BinaryPrimitives.WriteUInt32LittleEndian(buffer, NextUInt32());
                buffer = buffer[4..];
            }

            if (!buffer.IsEmpty)
            {
                Span<byte> temp = stackalloc byte[4];
                BinaryPrimitives.WriteUInt32LittleEndian(temp, NextUInt32());
                temp[..buffer.Length].CopyTo(buffer);
            }
        }
    }
}
