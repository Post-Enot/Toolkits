using System;

namespace PostEnot.Toolkits
{
    public interface IRandomEngine
    {
        public int StateSizeInBytes { get; }

        // Состояние:
        public byte[] GetState();
        public void GetState(Span<byte> buffer);
        public void SetState(ReadOnlySpan<byte> state);
        public bool TrySetState(ReadOnlySpan<byte> state);
        
        // Гарантируется [uint.MinValue, uint.MaxValue]
        public uint NextUInt32();
        // Гарантируется [ulong.MinValue, ulong.MaxValue]
        public ulong NextUInt64();
        // Гарантируется [0, 1)
        public float NextSingle();
        // Гарантируется [0, 1)
        public double NextDouble();
        public bool NextBoolean();
        public void NextBytes(Span<byte> buffer);
    }
}
