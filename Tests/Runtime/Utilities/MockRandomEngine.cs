#nullable enable

using System;
using System.Collections.Generic;

namespace PostEnot.Toolkits.Tests
{
    /// <summary>
    /// Детерминированная реализация <see cref="IRandomEngine"/> для тестирования.
    /// Позволяет задавать последовательности возвращаемых значений для каждого метода генерации.
    /// Если очередь значений исчерпана, бросается исключение (поведение можно изменить).
    /// </summary>
    public sealed class MockRandomEngine : IRandomEngine
    {
        private readonly Queue<uint> _uints = new();
        private readonly Queue<ulong> _ulongs = new();
        private readonly Queue<float> _floats = new();
        private readonly Queue<double> _doubles = new();
        private readonly Queue<bool> _bools = new();
        private byte[]? _bytePattern;

        /// <summary>
        /// Если <see langword="true"/>, методы генерации бросают <see cref="InvalidOperationException"/>,
        /// когда очередь значений пуста. Если <see langword="false"/>, возвращаются значения по умолчанию.
        /// По умолчанию <see langword="true"/>.
        /// </summary>
        public bool ThrowOnEmptyQueue { get; set; } = true;

        /// <summary>
        /// Задаёт паттерн байт, который используется при заполнении буфера в <see cref="NextBytes(Span{byte})"/>.
        /// Если <see langword="null"/>, буфер заполняется нулями.
        /// </summary>
        public byte[]? BytePattern
        {
            get => _bytePattern;
            set => _bytePattern = value;
        }

        // Методы для добавления ожидаемых значений в очередь

        public void Enqueue(uint value) => _uints.Enqueue(value);
        public void Enqueue(ulong value) => _ulongs.Enqueue(value);
        public void Enqueue(float value) => _floats.Enqueue(value);
        public void Enqueue(double value) => _doubles.Enqueue(value);
        public void Enqueue(bool value) => _bools.Enqueue(value);

        /// <summary>
        /// Добавляет несколько значений типа <see cref="uint"/> в очередь.
        /// </summary>
        public void EnqueueUInt32(params uint[] values)
        {
            foreach (uint value in values)
            {
                _uints.Enqueue(value);
            }
        }

        /// <summary>
        /// Добавляет несколько значений типа <see cref="ulong"/> в очередь.
        /// </summary>
        public void EnqueueUInt64(params ulong[] values)
        {
            foreach (ulong value in values)
            {
                _ulongs.Enqueue(value);
            }
        }

        /// <summary>
        /// Добавляет несколько значений типа <see cref="float"/> в очередь.
        /// </summary>
        public void EnqueueSingle(params float[] values)
        {
            foreach (float value in values)
            {
                _floats.Enqueue(value);
            }
        }

        /// <summary>
        /// Добавляет несколько значений типа <see cref="double"/> в очередь.
        /// </summary>
        public void EnqueueDouble(params double[] values)
        {
            foreach (double value in values)
            {
                _doubles.Enqueue(value);
            }
        }

        /// <summary>
        /// Добавляет несколько значений типа <see cref="bool"/> в очередь.
        /// </summary>
        public void EnqueueBoolean(params bool[] values)
        {
            foreach (bool value in values)
            {
                _bools.Enqueue(value);
            }
        }

        public int StateSizeInBytes => 0;

        public byte[] GetState() => Array.Empty<byte>();

        public void GetState(Span<byte> buffer) { }

        public void SetState(ReadOnlySpan<byte> state) { }

        public bool TrySetState(ReadOnlySpan<byte> state) => true;

        public uint NextUInt32() => _uints.Count > 0 ? _uints.Dequeue() : 0;

        public ulong NextUInt64() => _ulongs.Count > 0 ? _ulongs.Dequeue() : 0;

        public float NextSingle() => _floats.Count > 0 ? _floats.Dequeue() : 0.0f;

        public double NextDouble() => _doubles.Count > 0 ? _doubles.Dequeue() : 0.0;

        public bool NextBoolean() => _bools.Count > 0 && _bools.Dequeue();

        public void NextBytes(Span<byte> buffer)
        {
            if (_bytePattern == null || _bytePattern.Length == 0)
            {
                buffer.Clear(); // заполняем нулями
                return;
            }
            for (int i = 0; i < buffer.Length; i += 1)
            {
                buffer[i] = _bytePattern[i % _bytePattern.Length];
            }
        }
    }
}
