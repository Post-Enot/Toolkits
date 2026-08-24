using NUnit.Framework;
using System;
using UnityEngine;

namespace PostEnot.Toolkits.Tests
{
    [TestFixture]
    public class RandomEngineSampler_Tests
    {
        private RandomEngineSampler CreateSampler(MockRandomEngine engine) => new(engine);

        #region Constructor

        [Test]
        public void Constructor_WithNullEngine_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new RandomEngineSampler(null));
        }

        [Test]
        public void Constructor_WithEngine_StoresEngine()
        {
            var engine = new MockRandomEngine();
            var sampler = new RandomEngineSampler(engine);
            Assert.AreSame(engine, sampler.Engine);
        }

        #endregion

        #region NextBoolean

        [TestCase(true)]
        [TestCase(false)]
        public void NextBoolean_ReturnsEngineValue(bool engineValue)
        {
            var engine = new MockRandomEngine();
            engine.Enqueue(engineValue);
            var sampler = CreateSampler(engine);

            bool result = sampler.NextBoolean();

            Assert.AreEqual(engineValue, result);
        }

        #endregion

        #region NextBytes(int)

        [Test]
        public void NextBytes_WithNegativeNumber_ThrowsArgumentOutOfRangeException()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextBytes(-1));
        }

        [Test]
        public void NextBytes_WithZero_ReturnsEmptyArray()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            byte[] result = sampler.NextBytes(0);

            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Length);
        }

        [Test]
        public void NextBytes_WithPositiveNumber_ReturnsArrayFilledByEngine()
        {
            var engine = new MockRandomEngine();
            engine.BytePattern = new byte[] { 0xAB, 0xCD, 0xEF };
            var sampler = CreateSampler(engine);
            const int length = 7;

            byte[] result = sampler.NextBytes(length);

            Assert.AreEqual(length, result.Length);
            for (int i = 0; i < length; i++)
            {
                Assert.AreEqual(engine.BytePattern[i % engine.BytePattern.Length], result[i]);
            }
        }

        #endregion

        #region NextBytes(Span<byte>)

        [Test]
        public void NextBytes_Span_CallsEngine()
        {
            var engine = new MockRandomEngine();
            engine.BytePattern = new byte[] { 0x11, 0x22 };
            var sampler = CreateSampler(engine);
            byte[] buffer = new byte[5];

            sampler.NextBytes(buffer);

            for (int i = 0; i < buffer.Length; i++)
            {
                Assert.AreEqual(engine.BytePattern[i % 2], buffer[i]);
            }
        }

        [Test]
        public void NextBytes_EmptySpan_DoesNothing()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);
            Assert.DoesNotThrow(() => sampler.NextBytes(Span<byte>.Empty));
        }

        #endregion

        #region NextInt32()

        [Test]
        public void NextInt32_ReturnsUncheckedIntFromUInt32()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(uint.MaxValue); // 0xFFFFFFFF -> -1
            var sampler = CreateSampler(engine);

            int result = sampler.NextInt32();

            Assert.AreEqual(-1, result);
        }

        [Test]
        public void NextInt32_WithZero_ReturnsZero()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(0);
            var sampler = CreateSampler(engine);

            int result = sampler.NextInt32();

            Assert.AreEqual(0, result);
        }

        #endregion

        #region NextInt32(int maxExclusive)

        [Test]
        public void NextInt32_MaxExclusiveNegative_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextInt32(-1));
        }

        [TestCase(0, 0u, 0)]
        [TestCase(1, 0u, 0)]
        [TestCase(10, 0u, 0)]
        [TestCase(100, uint.MaxValue, 99)]
        public void NextInt32_MaxExclusiveValid_ReturnsExpected(int maxExclusive, uint engineValue, int expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(engineValue);
            var sampler = CreateSampler(engine);

            int result = sampler.NextInt32(maxExclusive);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void NextInt32_MaxExclusiveIntMax_ReturnsValueInRange()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(uint.MaxValue);
            var sampler = CreateSampler(engine);

            int result = sampler.NextInt32(int.MaxValue);

            Assert.IsTrue(result >= 0 && result < int.MaxValue);
        }

        #endregion

        #region NextInt32(int minInclusive, int maxExclusive)

        [Test]
        public void NextInt32_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextInt32(5, 4));
        }

        [TestCase(10, 10, 0u, 10)]
        [TestCase(5, 15, 0u, 5)]
        [TestCase(10, 20, uint.MaxValue, 19)]
        [TestCase(-10, -5, 0u, -10)]
        [TestCase(-10, -5, uint.MaxValue, -6)]
        public void NextInt32_ValidRange_ReturnsExpected(int min, int max, uint engineValue, int expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(engineValue);
            var sampler = CreateSampler(engine);

            int result = sampler.NextInt32(min, max);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void NextInt32_FullRange_NoOverflow()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(uint.MaxValue);
            var sampler = CreateSampler(engine);

            Assert.DoesNotThrow(() => sampler.NextInt32(int.MinValue, int.MaxValue));
        }

        #endregion

        #region NextUInt32()

        [Test]
        public void NextUInt32_ReturnsEngineValue()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(123456789);
            var sampler = CreateSampler(engine);

            uint result = sampler.NextUInt32();

            Assert.AreEqual(123456789u, result);
        }

        #endregion

        #region NextUInt32(uint maxExclusive)

        [TestCase(0u, 0u, 0u)]
        [TestCase(1u, 0u, 0u)]
        [TestCase(10u, 0u, 0u)]
        [TestCase(100u, uint.MaxValue, 99u)]
        public void NextUInt32_MaxExclusiveValid_ReturnsExpected(uint maxExclusive, uint engineValue, uint expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(engineValue);
            var sampler = CreateSampler(engine);

            uint result = sampler.NextUInt32(maxExclusive);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region NextUInt32(uint minInclusive, uint maxExclusive)

        [Test]
        public void NextUInt32_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextUInt32(10, 9));
        }

        [TestCase(5u, 5u, 0u, 5u)]
        [TestCase(10u, 20u, 0u, 10u)]
        [TestCase(5u, 100u, uint.MaxValue, 99u)]
        public void NextUInt32_ValidRange_ReturnsExpected(uint min, uint max, uint engineValue, uint expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt32(engineValue);
            var sampler = CreateSampler(engine);

            uint result = sampler.NextUInt32(min, max);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region NextInt64()

        [Test]
        public void NextInt64_ReturnsUncheckedLongFromUInt64()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(ulong.MaxValue); // -1
            var sampler = CreateSampler(engine);

            long result = sampler.NextInt64();

            Assert.AreEqual(-1L, result);
        }

        [Test]
        public void NextInt64_WithZero_ReturnsZero()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(0);
            var sampler = CreateSampler(engine);

            long result = sampler.NextInt64();

            Assert.AreEqual(0L, result);
        }

        #endregion

        #region NextInt64(long maxExclusive)

        [Test]
        public void NextInt64_MaxExclusiveNegative_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextInt64(-1));
        }

        [TestCase(0L, 0UL, 0L)]
        [TestCase(1L, 0UL, 0L)]
        [TestCase(10L, 0UL, 0L)]
        [TestCase(100L, ulong.MaxValue, 99L)]
        public void NextInt64_MaxExclusiveValid_ReturnsExpected(long maxExclusive, ulong engineValue, long expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(engineValue);
            var sampler = CreateSampler(engine);

            long result = sampler.NextInt64(maxExclusive);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void NextInt64_MaxExclusiveLongMax_ReturnsValueInRange()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(ulong.MaxValue);
            var sampler = CreateSampler(engine);

            long result = sampler.NextInt64(long.MaxValue);

            Assert.IsTrue(result >= 0 && result < long.MaxValue);
        }

        #endregion

        #region NextInt64(long minInclusive, long maxExclusive)

        [Test]
        public void NextInt64_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextInt64(5, 4));
        }

        [TestCase(10L, 10L, 0UL, 10L)]
        [TestCase(5L, 15L, 0UL, 5L)]
        [TestCase(10L, 20L, ulong.MaxValue, 19L)]
        [TestCase(-10L, -5L, 0UL, -10L)]
        [TestCase(-10L, -5L, ulong.MaxValue, -6L)]
        public void NextInt64_ValidRange_ReturnsExpected(long min, long max, ulong engineValue, long expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(engineValue);
            var sampler = CreateSampler(engine);

            long result = sampler.NextInt64(min, max);

            Assert.AreEqual(expected, result);
        }

        [Test]
        public void NextInt64_FullRange_NoOverflow()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(ulong.MaxValue);
            var sampler = CreateSampler(engine);

            Assert.DoesNotThrow(() => sampler.NextInt64(long.MinValue, long.MaxValue));
        }

        #endregion

        #region NextUInt64()

        [Test]
        public void NextUInt64_ReturnsEngineValue()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(12345678901234567890UL);
            var sampler = CreateSampler(engine);

            ulong result = sampler.NextUInt64();

            Assert.AreEqual(12345678901234567890UL, result);
        }

        #endregion

        #region NextUInt64(ulong maxExclusive)

        [TestCase(0UL, 0UL, 0UL)]
        [TestCase(1UL, 0UL, 0UL)]
        [TestCase(10UL, 0UL, 0UL)]
        [TestCase(100UL, ulong.MaxValue, 99UL)]
        public void NextUInt64_MaxExclusiveValid_ReturnsExpected(ulong maxExclusive, ulong engineValue, ulong expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(engineValue);
            var sampler = CreateSampler(engine);

            ulong result = sampler.NextUInt64(maxExclusive);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region NextUInt64(ulong minInclusive, ulong maxExclusive)

        [Test]
        public void NextUInt64_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextUInt64(10, 9));
        }

        [TestCase(5UL, 5UL, 0UL, 5UL)]
        [TestCase(10UL, 20UL, 0UL, 10UL)]
        [TestCase(5UL, 100UL, ulong.MaxValue, 99UL)]
        public void NextUInt64_ValidRange_ReturnsExpected(ulong min, ulong max, ulong engineValue, ulong expected)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueUInt64(engineValue);
            var sampler = CreateSampler(engine);

            ulong result = sampler.NextUInt64(min, max);

            Assert.AreEqual(expected, result);
        }

        #endregion

        #region NextSingle()

        [Test]
        public void NextSingle_ReturnsEngineValue()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.12345f);
            var sampler = CreateSampler(engine);

            float result = sampler.NextSingle();

            Assert.AreEqual(0.12345f, result, 1e-6f);
        }

        #endregion

        #region NextSingle(float maxExclusive)

        [TestCase(-0.1f)]
        [TestCase(float.NaN)]
        [TestCase(float.PositiveInfinity)]
        public void NextSingle_InvalidMaxExclusive_Throws(float maxExclusive)
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextSingle(maxExclusive));
        }

        [Test]
        public void NextSingle_MaxExclusiveZero_ReturnsZero()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            float result = sampler.NextSingle(0f);

            Assert.AreEqual(0f, result);
        }

        [Test]
        public void NextSingle_MaxExclusiveValid_ReturnsEngineTimesMax()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.5f);
            var sampler = CreateSampler(engine);

            float result = sampler.NextSingle(10.0f);

            Assert.AreEqual(5.0f, result, 1e-5f);
        }

        #endregion

        #region NextSingle(float minInclusive, float maxExclusive)

        [Test]
        public void NextSingle_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextSingle(5.0f, 4.0f));
        }

        [TestCase(float.NaN, 1.0f)]
        [TestCase(1.0f, float.NaN)]
        [TestCase(float.PositiveInfinity, 1.0f)]
        [TestCase(1.0f, float.PositiveInfinity)]
        [TestCase(float.NegativeInfinity, 1.0f)]
        [TestCase(1.0f, float.NegativeInfinity)]
        public void NextSingle_InvalidArgs_Throws(float min, float max)
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextSingle(min, max));
        }

        [Test]
        public void NextSingle_ValidRange_ReturnsMinPlusEngineTimesRange()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.25f);
            var sampler = CreateSampler(engine);

            float result = sampler.NextSingle(10.0f, 20.0f);

            Assert.AreEqual(12.5f, result, 1e-5f);
        }

        #endregion

        #region NextDouble()

        [Test]
        public void NextDouble_ReturnsEngineValue()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueDouble(0.123456789);
            var sampler = CreateSampler(engine);

            double result = sampler.NextDouble();

            Assert.AreEqual(0.123456789, result, 1e-9);
        }

        #endregion

        #region NextDouble(double maxExclusive)

        [TestCase(-0.1)]
        [TestCase(double.NaN)]
        [TestCase(double.PositiveInfinity)]
        public void NextDouble_InvalidMaxExclusive_Throws(double maxExclusive)
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextDouble(maxExclusive));
        }

        [Test]
        public void NextDouble_MaxExclusiveZero_ReturnsZero()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            double result = sampler.NextDouble(0d);

            Assert.AreEqual(0d, result);
        }

        [Test]
        public void NextDouble_MaxExclusiveValid_ReturnsEngineTimesMax()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueDouble(0.5);
            var sampler = CreateSampler(engine);

            double result = sampler.NextDouble(10.0);

            Assert.AreEqual(5.0, result, 1e-9);
        }

        #endregion

        #region NextDouble(double minInclusive, double maxExclusive)

        [Test]
        public void NextDouble_MinGreaterThanMax_Throws()
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextDouble(5.0, 4.0));
        }

        [TestCase(double.NaN, 1.0)]
        [TestCase(1.0, double.NaN)]
        [TestCase(double.PositiveInfinity, 1.0)]
        [TestCase(1.0, double.PositiveInfinity)]
        [TestCase(double.NegativeInfinity, 1.0)]
        [TestCase(1.0, double.NegativeInfinity)]
        public void NextDouble_InvalidArgs_Throws(double min, double max)
        {
            var engine = new MockRandomEngine();
            var sampler = CreateSampler(engine);

            Assert.Throws<ArgumentOutOfRangeException>(() => sampler.NextDouble(min, max));
        }

        [Test]
        public void NextDouble_ValidRange_ReturnsMinPlusEngineTimesRange()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueDouble(0.25);
            var sampler = CreateSampler(engine);

            double result = sampler.NextDouble(10.0, 20.0);

            Assert.AreEqual(12.5, result, 1e-9);
        }

        #endregion

        #region NextDirection2D

        [Test]
        public void NextDirection2D_ReturnsUnitVector()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.3f);
            var sampler = CreateSampler(engine);

            Vector2 vec = sampler.NextDirection2D();

            float magnitude = vec.magnitude;
            Assert.AreEqual(1.0f, magnitude, 1e-6f);
        }

        [TestCase(0.0f, 1.0f, 0.0f)]
        [TestCase(0.5f, -1.0f, 0.0f)]
        [TestCase(0.25f, 0.0f, 1.0f)]
        public void NextDirection2D_WithSpecificEngineValue_ReturnsExpectedVector(
            float engineValue, float expectedX, float expectedY)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(engineValue);
            var sampler = CreateSampler(engine);

            Vector2 vec = sampler.NextDirection2D();

            Assert.AreEqual(expectedX, vec.x, 1e-6f);
            Assert.AreEqual(expectedY, vec.y, 1e-6f);
        }

        #endregion

        #region NextDirection3D

        [Test]
        public void NextDirection3D_ReturnsUnitVector()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.2f);
            engine.EnqueueSingle(0.7f);
            var sampler = CreateSampler(engine);

            Vector3 vec = sampler.NextDirection3D();

            Assert.AreEqual(1.0f, vec.magnitude, 1e-6f);
        }

        [TestCase(0.5f, 0.0f, 1.0f, 0.0f, 0.0f)]
        [TestCase(1.0f, 0.25f, 0.0f, 0.0f, 1.0f)]
        public void NextDirection3D_WithSpecificValues_ReturnsExpectedVector(
            float ux, float uy, float expectedX, float expectedY, float expectedZ)
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(ux);
            engine.EnqueueSingle(uy);
            var sampler = CreateSampler(engine);

            Vector3 vec = sampler.NextDirection3D();

            Assert.AreEqual(expectedX, vec.x, 1e-6f);
            Assert.AreEqual(expectedY, vec.y, 1e-6f);
            Assert.AreEqual(expectedZ, vec.z, 1e-6f);
        }

        #endregion

        #region NextQuaternion

        [Test]
        public void NextQuaternion_ReturnsUnitQuaternion()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.1f);
            engine.EnqueueSingle(0.2f);
            engine.EnqueueSingle(0.3f);
            var sampler = CreateSampler(engine);

            Quaternion q = sampler.NextQuaternion();

            float magnitudeSqr = q.x * q.x + q.y * q.y + q.z * q.z + q.w * q.w;
            Assert.AreEqual(1.0f, magnitudeSqr, 1e-5f);
        }

        [Test]
        public void NextQuaternion_WithSpecificValues_ReturnsExpectedQuaternion()
        {
            var engine = new MockRandomEngine();
            engine.EnqueueSingle(0.0f);
            engine.EnqueueSingle(0.0f);
            engine.EnqueueSingle(0.0f);
            var sampler = CreateSampler(engine);

            Quaternion q = sampler.NextQuaternion();

            Assert.AreEqual(0.0f, q.x, 1e-6f);
            Assert.AreEqual(1.0f, q.y, 1e-6f);
            Assert.AreEqual(0.0f, q.z, 1e-6f);
            Assert.AreEqual(0.0f, q.w, 1e-6f);
        }

        #endregion
    }
}
