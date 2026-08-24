using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace PostEnot.Toolkits.Tests
{
    /// <summary>
    /// Базовый набор тестов для любой реализации <see cref="IRandomEngine"/>.
    /// Для использования создайте класс-наследник и реализуйте фабрику <see cref="CreateEngine"/>.
    /// </summary>
    [TestFixture]
    public abstract class RandomEngineTestsBase
    {
        /// <summary>
        /// Должен возвращать новый экземпляр тестируемой реализации ГПСЧ.
        /// </summary>
        protected abstract IRandomEngine CreateEngine();

        #region NextUInt32 / NextUInt64
        [Test]
        public void NextUInt32_ReturnsDifferentValues_AndDoesNotThrow()
        {
            IRandomEngine engine = CreateEngine();
            HashSet<uint> seen = new();
            for (int i = 0; i < 10_000; i += 1)
            {
                uint value = engine.NextUInt32();
                seen.Add(value);
            }
            Assert.That(seen.Count, Is.GreaterThan(1), "NextUInt32 должен выдавать разнообразные значения.");
        }

        [Test]
        public void NextUInt64_ReturnsDifferentValues_AndDoesNotThrow()
        {
            var engine = CreateEngine();
            var seen = new HashSet<ulong>();
            for (int i = 0; i < 10_000; i += 1)
            {
                ulong value = engine.NextUInt64();
                seen.Add(value);
            }
            Assert.That(seen.Count, Is.GreaterThan(1), "NextUInt64 должен выдавать разнообразные значения.");
        }

        #endregion

        #region NextSingle
        [Test]
        public void NextSingle_AlwaysInUnitInterval()
        {
            IRandomEngine engine = CreateEngine();
            for (int i = 0; i < 10_000; i += 1)
            {
                float value = engine.NextSingle();
                Assert.That(value, Is.GreaterThanOrEqualTo(0.0f), $"NextSingle вернул отрицательное значение: {value}");
                Assert.That(value, Is.LessThan(1.0f), $"NextSingle вернул значение >= 1.0: {value}");
                Assert.That(float.IsNaN(value), Is.False, "NextSingle вернул NaN");
                Assert.That(float.IsInfinity(value), Is.False, "NextSingle вернул бесконечность");
            }
        }

        [Test]
        public void NextSingle_ProducesMoreThanOneUniqueValue()
        {
            IRandomEngine engine = CreateEngine();
            HashSet<float> seen = new();
            for (int i = 0; i < 1_000; i += 1)
            {
                seen.Add(engine.NextSingle());
            }
            Assert.That(seen.Count, Is.GreaterThan(1), "NextSingle должен выдавать разнообразные значения.");
        }
        #endregion

        #region NextDouble
        [Test]
        public void NextDouble_AlwaysInUnitInterval()
        {
            IRandomEngine engine = CreateEngine();
            for (int i = 0; i < 10_000; i += 1)
            {
                double value = engine.NextDouble();
                Assert.That(value, Is.GreaterThanOrEqualTo(0.0), $"NextDouble вернул отрицательное значение: {value}");
                Assert.That(value, Is.LessThan(1.0), $"NextDouble вернул значение >= 1.0: {value}");
                Assert.That(double.IsNaN(value), Is.False, "NextDouble вернул NaN");
                Assert.That(double.IsInfinity(value), Is.False, "NextDouble вернул бесконечность");
            }
        }

        [Test]
        public void NextDouble_ProducesMoreThanOneUniqueValue()
        {
            var engine = CreateEngine();
            var seen = new HashSet<double>();
            for (int i = 0; i < 1_000; i += 1)
            {
                seen.Add(engine.NextDouble());
            }

            Assert.That(seen.Count, Is.GreaterThan(1), "NextDouble должен выдавать разнообразные значения.");
        }
        #endregion

        #region NextBoolean
        [Test]
        public void NextBoolean_ReturnsBothTrueAndFalse()
        {
            var engine = CreateEngine();
            bool sawTrue = false;
            bool sawFalse = false;
            for (int i = 0; i < 1_000; i += 1)
            {
                bool value = engine.NextBoolean();
                if (value)
                {
                    sawTrue = true;
                }
                else
                {
                    sawFalse = true;
                }

                if (sawTrue && sawFalse)
                {
                    break;
                }
            }
            Assert.That(sawTrue, Is.True, "NextBoolean должен хотя бы раз вернуть true.");
            Assert.That(sawFalse, Is.True, "NextBoolean должен хотя бы раз вернуть false.");
        }
        #endregion

        #region NextBytes

        [Test]
        public void NextBytes_EmptyBuffer_DoesNotThrow()
        {
            var engine = CreateEngine();
            Assert.DoesNotThrow(() => engine.NextBytes(Span<byte>.Empty));
        }

        [Test]
        public void NextBytes_HandlesVariousBufferSizes()
        {
            var engine = CreateEngine();
            int[] sizes = { 0, 1, 2, 3, 4, 7, 8, 15, 16, 31, 32, 64, 100, 1024 };

            foreach (int size in sizes)
            {
                byte[] buffer = new byte[size];
                Assert.DoesNotThrow(() => engine.NextBytes(buffer), $"NextBytes выбросил исключение для буфера размером {size}.");
            }
        }

        [Test]
        public void NextBytes_ModifiesBufferContent()
        {
            var engine = CreateEngine();
            byte[] buffer = new byte[1024];
            // Заполняем известным паттерном
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0xAB;

            byte[] original = (byte[])buffer.Clone();
            engine.NextBytes(buffer);

            bool changed = false;
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != original[i])
                {
                    changed = true;
                    break;
                }
            }

            Assert.That(changed, Is.True, "NextBytes должен изменить содержимое буфера.");
        }

        [Test]
        public void NextBytes_ProducesMoreThanOneUniqueByteValue()
        {
            var engine = CreateEngine();
            byte[] buffer = new byte[10_000];
            engine.NextBytes(buffer);

            var unique = new HashSet<byte>();
            foreach (byte b in buffer)
                unique.Add(b);

            Assert.That(unique.Count, Is.GreaterThan(1), "NextBytes должен генерировать более одного уникального значения байта.");
        }

        [Test]
        public void NextBytes_WithSubarray_DoesNotWriteOutsideRange()
        {
            var engine = CreateEngine();
            const int totalLength = 10;
            const int segmentStart = 2;
            const int segmentLength = 6;

            byte[] buffer = new byte[totalLength];
            // Заполняем канареечным значением
            for (int i = 0; i < buffer.Length; i++)
                buffer[i] = 0xAA;

            Span<byte> segment = buffer.AsSpan(segmentStart, segmentLength);
            engine.NextBytes(segment);

            // Проверяем, что байты до и после сегмента не изменились
            for (int i = 0; i < segmentStart; i += 1)
            {
                Assert.That(buffer[i], Is.EqualTo(0xAA), $"Байт {i} был изменён вне переданного диапазона.");
            }

            for (int i = segmentStart + segmentLength; i < buffer.Length; i += 1)
            {
                Assert.That(buffer[i], Is.EqualTo(0xAA), $"Байт {i} был изменён вне переданного диапазона.");
            }

            // Проверяем, что внутри сегмента хотя бы один байт изменился (вероятностно, но практически гарантировано)
            bool segmentChanged = false;
            for (int i = segmentStart; i < segmentStart + segmentLength; i += 1)
            {
                if (buffer[i] != 0xAA)
                {
                    segmentChanged = true;
                    break;
                }
            }
            Assert.That(segmentChanged, Is.True, "NextBytes не изменил байты внутри переданного сегмента.");
        }

        [Test]
        public void NextBytes_WithStackAllocatedBuffer_Works()
        {
            var engine = CreateEngine();
            Span<byte> stackBuffer = stackalloc byte[64];
            // Инициализируем нулями
            stackBuffer.Clear();

            engine.NextBytes(stackBuffer);

            bool changed = false;
            for (int i = 0; i < stackBuffer.Length; i += 1)
            {
                if (stackBuffer[i] != 0)
                {
                    changed = true;
                    break;
                }
            }

            Assert.That(changed, Is.True, "NextBytes должен заполнить стековый буфер ненулевыми данными.");
        }
        #endregion
    }
}
