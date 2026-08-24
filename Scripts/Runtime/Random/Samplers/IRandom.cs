using System;
using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Предоставляет методы для генерации случайных значений различных типов.
    /// </summary>
    public interface IRandom
    {
        /// <summary>
        /// Возвращает случайное значение <see langword="bool"/>.
        /// </summary>
        /// <returns>Случайное значение <see langword="bool"/>.</returns>
        public bool NextBoolean();

        /// <summary>
        /// Создаёт массив байтов заданной длины и заполняет его случайными значениями.
        /// </summary>
        /// <param name="number">Количество байтов в массиве. Должно быть неотрицательным.</param>
        /// <returns>Массив байтов, заполненный случайными значениями.</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="number"/> меньше нуля.
        /// </exception>
        public byte[] NextBytes(int number);

        /// <summary>
        /// Заполняет указанный буфер случайными байтами.
        /// </summary>
        /// <param name="buffer">Буфер, который будет заполнен случайными байтами.</param>
        public void NextBytes(Span<byte> buffer);

        /// <summary>
        /// Возвращает случайное значение <see langword="int"/> в полном диапазоне
        /// [<see cref="int.MinValue"/>, <see cref="int.MaxValue"/>].
        /// </summary>
        /// <returns>Случайное значение <see langword="int"/> в диапазоне [<see cref="int.MinValue"/>, <see cref="int.MaxValue"/>].</returns>
        public int NextInt32();

        /// <summary>
        /// Возвращает случайное значение <see langword="int"/> из диапазона [0, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="maxExclusive"/> равен нулю, возвращается 0.
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть неотрицательной.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> больше нуля, случайное значение <see langword="int"/> в диапазоне
        /// [0, <paramref name="maxExclusive"/> - 1]; иначе 0.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="maxExclusive"/> меньше нуля.
        /// </exception>
        public int NextInt32(int maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="int"/> из диапазона
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="minInclusive"/> равен <paramref name="maxExclusive"/>, возвращается <paramref name="minInclusive"/>.
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > <paramref name="minInclusive"/>, случайное значение <see langword="int"/>
        /// в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1];
        /// иначе <paramref name="minInclusive"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>.
        /// </exception>
        public int NextInt32(int minInclusive, int maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="uint"/> в полном диапазоне
        /// [<see cref="uint.MinValue"/>, <see cref="uint.MaxValue"/>].
        /// </summary>
        /// <returns>Случайное значение <see langword="uint"/>.</returns>
        public uint NextUInt32();

        /// <summary>
        /// Возвращает случайное значение <see langword="uint"/> из диапазона [0, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="maxExclusive"/> равен нулю, возвращается 0.
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > 0, случайное значение <see langword="uint"/> в диапазоне
        /// [0, <paramref name="maxExclusive"/> - 1]; иначе 0.
        /// </returns>
        public uint NextUInt32(uint maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="uint"/> из диапазона
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="minInclusive"/> равен <paramref name="maxExclusive"/>, возвращается <paramref name="minInclusive"/>.
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > <paramref name="minInclusive"/>, случайное значение <see langword="uint"/>
        /// в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1];
        /// иначе <paramref name="minInclusive"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>.
        /// </exception>
        public uint NextUInt32(uint minInclusive, uint maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="long"/> в полном диапазоне
        /// [<see cref="long.MinValue"/>, <see cref="long.MaxValue"/>].
        /// </summary>
        /// <returns>Случайное значение <see langword="long"/>.</returns>
        public long NextInt64();

        /// <summary>
        /// Возвращает случайное значение <see langword="long"/> из диапазона [0, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="maxExclusive"/> равен нулю, возвращается 0.
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть неотрицательной.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > 0, случайное значение <see langword="long"/> в диапазоне
        /// [0, <paramref name="maxExclusive"/> - 1]; иначе 0.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="maxExclusive"/> меньше нуля.
        /// </exception>
        public long NextInt64(long maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="long"/> из диапазона
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="minInclusive"/> равен <paramref name="maxExclusive"/>, возвращается <paramref name="minInclusive"/>.
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > <paramref name="minInclusive"/>, случайное значение <see langword="long"/>
        /// в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1];
        /// иначе <paramref name="minInclusive"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>.
        /// </exception>
        public long NextInt64(long minInclusive, long maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="ulong"/> в полном диапазоне
        /// [<see cref="ulong.MinValue"/>, <see cref="ulong.MaxValue"/>].
        /// </summary>
        /// <returns>Случайное значение <see langword="ulong"/>.</returns>
        public ulong NextUInt64();

        /// <summary>
        /// Возвращает случайное значение <see langword="ulong"/> из диапазона [0, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="maxExclusive"/> равен нулю, возвращается 0.
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > 0, случайное значение <see langword="ulong"/> в диапазоне
        /// [0, <paramref name="maxExclusive"/> - 1]; иначе 0.
        /// </returns>
        public ulong NextUInt64(ulong maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see langword="ulong"/> из диапазона
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1].
        /// Если <paramref name="minInclusive"/> равен <paramref name="maxExclusive"/>, возвращается <paramref name="minInclusive"/>.
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>
        /// Если <paramref name="maxExclusive"/> > <paramref name="minInclusive"/>, случайное значение <see langword="ulong"/>
        /// в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/> - 1];
        /// иначе <paramref name="minInclusive"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>.
        /// </exception>
        public ulong NextUInt64(ulong minInclusive, ulong maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see cref="float"/> в диапазоне [0, 1).
        /// </summary>
        /// <returns>Случайное значение <see cref="float"/> в диапазоне [0, 1).</returns>
        public float NextSingle();

        /// <summary>
        /// Возвращает случайное значение <see cref="float"/> в диапазоне [0, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть конечной и неотрицательной.</param>
        /// <returns>Случайное значение <see cref="float"/> в диапазоне [0, <paramref name="maxExclusive"/>).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="maxExclusive"/> отрицательна, равна <see cref="float.NaN"/> или <see cref="float.PositiveInfinity"/>.
        /// </exception>
        public float NextSingle(float maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see cref="float"/> в диапазоне
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>Случайное значение <see cref="float"/> в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>,
        /// или любой из параметров равен <see cref="float.NaN"/> или бесконечности.
        /// </exception>
        public float NextSingle(float minInclusive, float maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see cref="double"/> в диапазоне [0, 1).
        /// </summary>
        /// <returns>Случайное значение <see cref="double"/> в диапазоне [0, 1).</returns>
        public double NextDouble();

        /// <summary>
        /// Возвращает случайное значение <see cref="double"/> в диапазоне [0, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть конечной и неотрицательной.</param>
        /// <returns>Случайное значение <see cref="double"/> в диапазоне [0, <paramref name="maxExclusive"/>).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="maxExclusive"/> отрицательна, равна <see cref="double.NaN"/> или <see cref="double.PositiveInfinity"/>.
        /// </exception>
        public double NextDouble(double maxExclusive);

        /// <summary>
        /// Возвращает случайное значение <see cref="double"/> в диапазоне
        /// [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).
        /// </summary>
        /// <param name="minInclusive">Нижняя граница (включается) диапазона.</param>
        /// <param name="maxExclusive">Верхняя граница (исключается) диапазона. Должна быть не меньше <paramref name="minInclusive"/>.</param>
        /// <returns>Случайное значение <see cref="double"/> в диапазоне [<paramref name="minInclusive"/>, <paramref name="maxExclusive"/>).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Выбрасывается, если <paramref name="minInclusive"/> больше <paramref name="maxExclusive"/>,
        /// или любой из параметров равен <see cref="double.NaN"/> или бесконечности.
        /// </exception>
        public double NextDouble(double minInclusive, double maxExclusive);

        /// <summary>
        /// Возвращает случайный единичный вектор в двумерном пространстве (равномерно распределённое направление).
        /// </summary>
        /// <returns>Случайный единичный вектор <see cref="Vector2"/>.</returns>
        public Vector2 NextDirection2D();

        /// <summary>
        /// Возвращает случайный единичный вектор в трёхмерном пространстве (равномерно распределённое направление).
        /// </summary>
        /// <returns>Случайный единичный вектор <see cref="Vector3"/>.</returns>
        public Vector3 NextDirection3D();

        /// <summary>
        /// Возвращает случайный единичный кватернион (равномерно распределённое вращение).
        /// </summary>
        /// <returns>Случайный единичный кватернион <see cref="Quaternion"/>.</returns>
        public Quaternion NextQuaternion();

        /// <summary>
        /// Возвращает случайную точку внутри единичного круга (равномерно по площади).
        /// </summary>
        /// <returns>Случайная точка внутри единичного круга.</returns>
        public Vector2 NextPointInsideUnitCircle();

        /// <summary>
        /// Возвращает случайную точку внутри единичной сферы (равномерно по объёму).
        /// </summary>
        /// <returns>Случайная точка внутри единичной сферы.</returns>
        public Vector3 NextPointInsideUnitSphere();

        /// <summary>
        /// Перемешивает элементы в указанном диапазоне.
        /// </summary>
        /// <typeparam name="T">Тип элементов диапазона.</typeparam>
        /// <param name="span">Перемешиваемый диапазон.</param>
        public void Shuffle<T>(Span<T> span);

        /// <summary>
        /// Возвращает случайный элемент из диапазона.
        /// </summary>
        /// <typeparam name="T">Тип элементов диапазона.</typeparam>
        /// <param name="span">Диапазон, из которого выбирается элемент.</param>
        /// <returns>Случайный элемент диапазона.</returns>
        /// <exception cref="ArgumentException">Если диапазон пуст.</exception>
        public T NextElement<T>(ReadOnlySpan<T> span);

        /// <summary>
        /// Создает массив заданной длины и заполняет его случайными элементами из указанного набора.
        /// </summary>
        /// <typeparam name="T">Тип элементов.</typeparam>
        /// <param name="choices">Набор элементов, из которого производится выбор.</param>
        /// <param name="length">Длина создаваемого массива.</param>
        /// <returns>Массив случайных элементов из <paramref name="choices"/>.</returns>
        /// <exception cref="ArgumentException">Бросается, если <paramref name="choices"/> пуст.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Бросается, если <paramref name="length"/> меньше нуля.</exception>
        public T[] GetItems<T>(ReadOnlySpan<T> choices, int length);

        /// <summary>
        /// Заполняет указанный диапазон случайными элементами из заданного набора.
        /// </summary>
        /// <typeparam name="T">Тип элементов.</typeparam>
        /// <param name="choices">Набор элементов, из которого производится выбор.</param>
        /// <param name="destination">Диапазон, который нужно заполнить.</param>
        /// <exception cref="ArgumentException">Бросается, если <paramref name="choices"/> пуст.</exception>
        public void GetItems<T>(ReadOnlySpan<T> choices, Span<T> destination);
    }
}
