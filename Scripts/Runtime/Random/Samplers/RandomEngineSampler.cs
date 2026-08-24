using System;
using UnityEngine;

namespace PostEnot.Toolkits
{
    public sealed class RandomEngineSampler : IRandom
    {
        public RandomEngineSampler(IRandomEngine engine)
            => Engine = engine ?? throw new ArgumentNullException(nameof(engine));

        public IRandomEngine Engine { get; }

        public bool NextBoolean() => Engine.NextBoolean();

        public byte[] NextBytes(int number)
        {
            if (number < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(number), $"{nameof(number)} must be non-negative.");
            }
            byte[] result = new byte[number];
            Engine.NextBytes(result);
            return result;
        }

        public void NextBytes(Span<byte> buffer) => Engine.NextBytes(buffer);

        public int NextInt32() => unchecked((int)Engine.NextUInt32());

        public int NextInt32(int maxExclusive)
        {
            if (maxExclusive < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxExclusive), $"{nameof(maxExclusive)} must be non-negative.");
            }
            return (int)NextUInt32((uint)maxExclusive);
        }

        public int NextInt32(int minInclusive, int maxExclusive)
        {
            if (minInclusive > maxExclusive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be greater than or equal to {nameof(minInclusive)}.");
            }
            long range = (long)maxExclusive - minInclusive;
            uint offset = NextUInt32((uint)range);
            return (int)(minInclusive + offset);
        }

        public uint NextUInt32() => Engine.NextUInt32();

        public uint NextUInt32(uint maxExclusive) => (uint)(((ulong)Engine.NextUInt32() * maxExclusive) >> 32);

        public uint NextUInt32(uint minInclusive, uint maxExclusive)
        {
            if (minInclusive > maxExclusive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be greater than or equal to {nameof(minInclusive)}.");
            }
            uint range = maxExclusive - minInclusive;
            return minInclusive + NextUInt32(range);
        }

        public long NextInt64() => unchecked((long)Engine.NextUInt64());

        public long NextInt64(long maxExclusive)
        {
            if (maxExclusive < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be non-negative.");
            }
            return (long)NextUInt64((ulong)maxExclusive);
        }

        public long NextInt64(long minInclusive, long maxExclusive)
        {
            if (minInclusive > maxExclusive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be greater than or equal to {nameof(minInclusive)}.");
            }
            unchecked
            {
                ulong range = (ulong)maxExclusive - (ulong)minInclusive;
                ulong offset = NextUInt64(range);
                return (long)((ulong)minInclusive + offset);
            }
        }

        public ulong NextUInt64() => Engine.NextUInt64();

        public ulong NextUInt64(ulong maxExclusive)
        {
            ulong a = Engine.NextUInt64();
            ulong b = maxExclusive;
            uint a0 = (uint)a;
            uint a1 = (uint)(a >> 32);
            uint b0 = (uint)b;
            uint b1 = (uint)(b >> 32);
            ulong p00 = (ulong)a0 * b0;
            ulong p01 = (ulong)a0 * b1;
            ulong p10 = (ulong)a1 * b0;
            ulong p11 = (ulong)a1 * b1;
            ulong middle = (p00 >> 32) + (uint)p01 + (uint)p10;
            return p11 + (p01 >> 32) + (p10 >> 32) + (middle >> 32);
        }

        public ulong NextUInt64(ulong minInclusive, ulong maxExclusive)
        {
            if (minInclusive > maxExclusive)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be greater than or equal to {nameof(minInclusive)}.");
            }
            ulong range = maxExclusive - minInclusive;
            return minInclusive + NextUInt64(range);
        }

        public float NextSingle() => Engine.NextSingle();

        public float NextSingle(float maxExclusive)
        {
            if ((maxExclusive < 0.0f) || float.IsNaN(maxExclusive) || float.IsInfinity(maxExclusive))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be finite and non-negative.");
            }
            return Engine.NextSingle() * maxExclusive;
        }

        public float NextSingle(float minInclusive, float maxExclusive)
        {
            if ((minInclusive > maxExclusive)
                || float.IsNaN(minInclusive)
                || float.IsNaN(maxExclusive)
                || float.IsInfinity(minInclusive)
                || float.IsInfinity(maxExclusive))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be finite and greater than or equal to {nameof(minInclusive)}.");
            }
            return minInclusive + Engine.NextSingle() * (maxExclusive - minInclusive);
        }

        public double NextDouble() => Engine.NextDouble();

        public double NextDouble(double maxExclusive)
        {
            if ((maxExclusive < 0.0) || double.IsNaN(maxExclusive) || double.IsInfinity(maxExclusive))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be finite and non-negative.");
            }
            return Engine.NextDouble() * maxExclusive;
        }

        public double NextDouble(double minInclusive, double maxExclusive)
        {
            if ((minInclusive > maxExclusive)
                || double.IsNaN(minInclusive)
                || double.IsNaN(maxExclusive)
                || double.IsInfinity(minInclusive)
                || double.IsInfinity(maxExclusive))
            {
                throw new ArgumentOutOfRangeException(
                    nameof(maxExclusive),
                    $"{nameof(maxExclusive)} must be finite and greater than or equal to {nameof(minInclusive)}.");
            }
            return Engine.NextDouble() * (maxExclusive - minInclusive) + minInclusive;
        }

        public Quaternion NextQuaternion()
        {
            float u = Engine.NextSingle();
            float v = Engine.NextSingle();
            float w = Engine.NextSingle();
            float sqrt1MinusU = Mathf.Sqrt(1.0f - u);
            float sqrtU = Mathf.Sqrt(u);
            float angle1 = 2.0f * Mathf.PI * v;
            float angle2 = 2.0f * Mathf.PI * w;
            return new Quaternion(
                sqrt1MinusU * Mathf.Sin(angle1),
                sqrt1MinusU * Mathf.Cos(angle1),
                sqrtU * Mathf.Sin(angle2),
                sqrtU * Mathf.Cos(angle2)
            );
        }

        public Vector2 NextDirection2D()
        {
            float u = NextSingle() * Mathf.PI * 2.0f;
            float sin = Mathf.Sin(u);
            float cos = Mathf.Cos(u);
            return new Vector2(cos, sin);
        }

        public Vector3 NextDirection3D()
        {
            float ux = NextSingle();
            float uy = NextSingle() * Mathf.PI * 2.0f;
            float num = (ux * 2.0f) - 1.0f;
            float num2 = Mathf.Sqrt(Mathf.Max(1.0f - num * num, 0.0f));
            float sin = Mathf.Sin(uy);
            float cos = Mathf.Cos(uy);
            return new Vector3(cos * num2, sin * num2, num);
        }

        public Vector2 NextPointInsideUnitCircle()
        {
            float radius = Mathf.Sqrt(NextSingle());
            return NextDirection2D() * radius;
        }

        public Vector3 NextPointInsideUnitSphere()
        {
            float radius = Mathf.Pow(NextSingle(), 1.0f / 3.0f);
            return NextDirection3D() * radius;
        }

        public void Shuffle<T>(Span<T> span)
        {
            for (int i = span.Length - 1; i > 0; i -= 1)
            {
                int j = NextInt32(i + 1);
                (span[i], span[j]) = (span[j], span[i]);
            }
        }

        public T NextElement<T>(ReadOnlySpan<T> span)
        {
            if (span.Length == 0)
            {
                throw new ArgumentException($"{nameof(span)} is empty.", nameof(span));
            }
            int index = NextInt32(span.Length);
            return span[index];
        }

        public T[] GetItems<T>(ReadOnlySpan<T> choices, int length)
        {
            if (choices.IsEmpty)
            {
                throw new ArgumentException($"{nameof(choices)} is empty.", nameof(choices));
            }
            if (length < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length), $"{nameof(length)} must be non-negative.");
            }

            T[] result = new T[length];
            for (int i = 0; i < result.Length; i += 1)
            {
                int index = NextInt32(choices.Length);
                result[i] = choices[index];
            }
            return result;
        }

        public void GetItems<T>(ReadOnlySpan<T> choices, Span<T> destination)
        {
            if (choices.IsEmpty)
            {
                throw new ArgumentException($"{nameof(choices)} span is empty.", nameof(choices));
            }
            for (int i = 0; i < destination.Length; i += 1)
            {
                int index = NextInt32(choices.Length);
                destination[i] = choices[index];
            }
        }
    }
}
