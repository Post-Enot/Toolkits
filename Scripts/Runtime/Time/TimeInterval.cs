using System;
using System.Globalization;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Представляет временной интервал, определяемый точкой отсчёта и длительностью. Предоставляет удобные методы для
    /// высчитывания различных временных параметров, таких как прошедшее время, оставшееся время и пр.
    /// </summary>
    public readonly struct TimeInterval : IEquatable<TimeInterval>, IFormattable
    {
        internal TimeInterval(float startTime, float duration)
        {
            StartTime = startTime;
            Duration = duration;
        }

        /// <summary>
        /// Возвращает временной интервал с бесконечной (<see cref="float.PositiveInfinity"/>) продолжительностью и нулевой точкой отсчёта.
        /// </summary>
        public static TimeInterval Infinity => new(0.0f, float.PositiveInfinity);

        /// <summary>
        /// Начальное время интервала, она же точка отсчёта.
        /// </summary>
        public readonly float StartTime { get; }
        /// <summary>
        /// Длительность интервала.
        /// </summary>
        public readonly float Duration { get; }
        /// <summary>
        /// Время завершения интервала (<see cref="StartTime"/> + <see cref="Duration"/>).
        /// </summary>
        public readonly float CompleteTime => StartTime + Duration;

        private const string _defaultFormat = "F2";

        /// <summary>
        /// Определяет, будет ли являться интервал завершённым в указанный момент времени.
        /// </summary>
        /// <param name="currentTime">Момент времени, для которого выполняется проверка.</param>
        /// <returns><see langword="true"/>, если интервал будет завершённым в указанный момент времени;
        /// иначе <see langword="false"/>.</returns>
        public readonly bool IsCompleted(float currentTime) => CompleteTime <= currentTime;

        /// <summary>
        /// Определяет, будет ли являться интервал не завершённым в указанный момент времени.
        /// </summary>
        /// <param name="currentTime">Момент времени, для которого выполняется проверка.</param>
        /// <returns><see langword="true"/>, если интервал будет не завершённым в указанный момент времени;
        /// иначе <see langword="false"/>.</returns>
        public readonly bool IsNotCompleted(float currentTime) => CompleteTime > currentTime;

        /// <summary>
        /// Возвращает прошедшее время от начала интервала до указанного момента, ограниченное длительностью.
        /// Значение не превышает <see cref="Duration"/> и может быть отрицательным.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Прошедшее время в пределах [0, <see cref="Duration"/>].</returns>
        public readonly float Elapsed(float currentTime) => TimeUtility.Elapsed(StartTime, currentTime, Duration);

        /// <summary>
        /// Возвращает прошедшее время от начала интервала до указанного момента без ограничения длительностью.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Неограниченное прошедшее время (<paramref name="currentTime"/> - <see cref="StartTime"/>).</returns>
        public readonly float ElapsedUnclamped(float currentTime) => TimeUtility.ElapsedUnclamped(StartTime, currentTime);

        /// <summary>
        /// Возвращает отношение ограниченного прошедшего времени (<see cref="Elapsed(float)"/>) к длительности интервала.
        /// Результат всегда находится в диапазоне [0, 1] при ненулевой длительности.
        /// При нулевой длительности возвращает 0.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Нормализованное прошедшее время [0, 1] или 0, если <see cref="Duration"/> = 0.</returns>
        public readonly float ElapsedRatio(float currentTime) => TimeUtility.ElapsedRatio(StartTime, currentTime, Duration, 0.0f);

        /// <summary>
        /// Возвращает отношение ограниченного прошедшего времени (<see cref="Elapsed(float)"/>) к длительности интервала.
        /// Результат всегда находится в диапазоне [0, 1] при ненулевой длительности.
        /// При нулевой длительности возвращает <paramref name="zeroDurationReturn"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <param name="zeroDurationReturn">Значение, возвращаемое при нулевой длительности.</param>
        /// <returns>Нормализованное прошедшее время [0, 1] или <paramref name="zeroDurationReturn"/> при <see cref="Duration"/> = 0.</returns>
        public readonly float ElapsedRatio(float currentTime, float zeroDurationReturn)
            => TimeUtility.ElapsedRatio(StartTime, currentTime, Duration, zeroDurationReturn);

        /// <summary>
        /// Возвращает отношение неограниченного прошедшего времени (<see cref="ElapsedUnclamped(float)"/>) к длительности интервала.
        /// Может быть отрицательным, если <paramref name="currentTime"/> меньше <see cref="StartTime"/>,
        /// или больше 1, если <paramref name="currentTime"/> превышает <see cref="CompleteTime"/>.
        /// При нулевой длительности возвращает 0.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Отношение неограниченного прошедшего времени к длительности.</returns>
        public readonly float ElapsedRatioUnclamped(float currentTime)
            => TimeUtility.ElapsedRatioUnclamped(StartTime, currentTime, Duration, 0.0f);

        /// <summary>
        /// Возвращает отношение неограниченного прошедшего времени (<see cref="ElapsedUnclamped(float)"/>) к длительности интервала.
        /// Может выходить за пределы [0, 1] (см. <see cref="ElapsedRatioUnclamped(float)"/>).
        /// При нулевой длительности возвращает <paramref name="zeroDurationReturn"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <param name="zeroDurationReturn">Значение, возвращаемое при нулевой длительности.</param>
        /// <returns>Отношение неограниченного прошедшего времени к длительности.</returns>
        public readonly float ElapsedRatioUnclamped(float currentTime, float zeroDurationReturn)
            => TimeUtility.ElapsedRatioUnclamped(StartTime, currentTime, Duration, zeroDurationReturn);

        /// <summary>
        /// Возвращает оставшееся время до завершения интервала, ограниченное длительностью.
        /// Если интервал уже завершился, результат равен 0.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Оставшееся время в пределах [0, <see cref="Duration"/>].</returns>
        public readonly float Remaining(float currentTime) => TimeUtility.Remaining(StartTime, currentTime, Duration);

        /// <summary>
        /// Возвращает оставшееся время до завершения интервала без ограничения длительностью.
        /// Может быть отрицательным, если интервал уже завершился (т.е. <paramref name="currentTime"/> > <see cref="CompleteTime"/>).
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Неограниченное оставшееся время.</returns>
        public readonly float RemainingUnclamped(float currentTime) => TimeUtility.RemainingUnclamped(StartTime, currentTime, Duration);

        /// <summary>
        /// Возвращает отношение ограниченного оставшегося времени (<see cref="Remaining(float)"/>) к длительности интервала.
        /// Результат всегда находится в диапазоне [0, 1] при ненулевой длительности.
        /// При нулевой длительности возвращает 0.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Нормализованное оставшееся время [0, 1] или 0, если <see cref="Duration"/> = 0.</returns>
        public readonly float RemainingRatio(float currentTime) => TimeUtility.RemainingRatio(StartTime, currentTime, Duration, 0.0f);

        /// <summary>
        /// Возвращает отношение ограниченного оставшегося времени (<see cref="Remaining(float)"/>) к длительности интервала.
        /// Результат всегда находится в диапазоне [0, 1] при ненулевой длительности.
        /// При нулевой длительности возвращает <paramref name="zeroDurationReturn"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <param name="zeroDurationReturn">Значение, возвращаемое при нулевой длительности.</param>
        /// <returns>Нормализованное оставшееся время [0, 1] или <paramref name="zeroDurationReturn"/> при <see cref="Duration"/> = 0.</returns>
        public readonly float RemainingRatio(float currentTime, float zeroDurationReturn)
            => TimeUtility.RemainingRatio(StartTime, currentTime, Duration, zeroDurationReturn);

        /// <summary>
        /// Возвращает отношение неограниченного оставшегося времени (<see cref="RemainingUnclamped(float)"/>) к длительности интервала.
        /// Может быть отрицательным, если интервал уже завершился,
        /// или больше 1, если <paramref name="currentTime"/> значительно отстаёт от <see cref="StartTime"/>.
        /// При нулевой длительности возвращает 0.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Отношение неограниченного оставшегося времени к длительности.</returns>
        public readonly float RemainingRatioUnclamped(float currentTime)
            => TimeUtility.RemainingRatioUnclamped(StartTime, currentTime, Duration, 0.0f);

        /// <summary>
        /// Возвращает отношение неограниченного оставшегося времени (<see cref="RemainingUnclamped(float)"/>) к длительности интервала.
        /// Может выходить за пределы [0, 1] (см. <see cref="RemainingRatioUnclamped(float)"/>).
        /// При нулевой длительности возвращает <paramref name="zeroDurationReturn"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <param name="zeroDurationReturn">Значение, возвращаемое при нулевой длительности.</param>
        /// <returns>Отношение неограниченного оставшегося времени к длительности.</returns>
        public readonly float RemainingRatioUnclamped(float currentTime, float zeroDurationReturn)
            => TimeUtility.RemainingRatioUnclamped(StartTime, currentTime, Duration, zeroDurationReturn);

        /// <summary>
        /// Создаёт новый интервал с той же длительностью и новым начальным временем, равным указанному текущему моменту.
        /// Это эквивалентно перезапуску интервала с данным <paramref name="currentTime"/>.
        /// </summary>
        /// <param name="currentTime">Новый начальный момент времени.</param>
        /// <returns>Новый экземпляр <see cref="TimeInterval"/> с тем же <see cref="Duration"/> и <see cref="StartTime"/>
        /// = <paramref name="currentTime"/>.</returns>
        public readonly TimeInterval Restart(float currentTime) => new(currentTime, Duration);

        /// <summary>
        /// Возвращает новый интервал, который обращает оставшееся (ограниченное) время:
        /// новый интервал имеет ту же длительность, а его начальное время сдвигается так,
        /// чтобы оставшееся время исходного интервала стало равным прошедшему времени нового интервала.
        /// Ограничение длительностью учитывается.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени, относительно которого выполняется обращение.</param>
        /// <returns>Новый обращённый интервал.</returns>
        public readonly TimeInterval Reverse(float currentTime)
        {
            float startTime = TimeUtility.Reverse(StartTime, currentTime, Duration);
            return new TimeInterval(startTime, Duration);
        }

        /// <summary>
        /// Возвращает новый интервал, который обращает оставшееся (неограниченное) время
        /// без учёта ограничения длительностью. Может дать начальное время меньше исходного,
        /// если elapsed > duration.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <returns>Новый обращённый интервал без ограничения.</returns>
        public readonly TimeInterval ReverseUnclamped(float currentTime)
        {
            float startTime = TimeUtility.ReverseUnclamped(StartTime, currentTime, Duration);
            return new TimeInterval(startTime, Duration);
        }

        public readonly bool Equals(TimeInterval other) => (StartTime == other.StartTime) && (Duration == other.Duration);

        public readonly string ToString(string format, IFormatProvider formatProvider)
        {
            if (string.IsNullOrEmpty(format))
            {
                format = _defaultFormat;
            }
            formatProvider ??= CultureInfo.InvariantCulture.NumberFormat;
            return $"{nameof(TimeInterval)} {{ {nameof(StartTime)} = {StartTime.ToString(_defaultFormat, formatProvider)}, " +
                   $"{nameof(Duration)} = {Duration.ToString(_defaultFormat, formatProvider)} }}"; ;
        }

        public override string ToString()
            => $"{nameof(TimeInterval)} {{ {nameof(StartTime)} = {StartTime.ToString(_defaultFormat)}, " +
               $"{nameof(Duration)} = {Duration.ToString(_defaultFormat)} }}";

        public readonly override bool Equals(object obj) => obj is TimeInterval other && Equals(other);

        public readonly override int GetHashCode() => HashCode.Combine(StartTime, Duration);

        #region FabricMethods
        /// <summary>
        /// Создаёт новый интервал с начальным временем 0 и указанной длительностью.
        /// </summary>
        /// <param name="duration">Длительность интервала. Должна быть неотрицательным числом и не NaN.</param>
        /// <returns>Новый экземпляр <see cref="TimeInterval"/>.</returns>
        /// <exception cref="ArgumentException">Параметр <paramref name="duration"/> равен NaN.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Параметр <paramref name="duration"/> отрицателен.</exception>
        public static TimeInterval Create(float duration)
        {
            if (float.IsNaN(duration))
            {
                throw new ArgumentException($"{nameof(duration)} can not be NaN.", nameof(duration));
            }
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), $"{duration} must be positive number.");
            }
            return new TimeInterval(0.0f, duration);
        }

        /// <summary>
        /// Создаёт новый интервал с указанными начальным временем и длительностью.
        /// </summary>
        /// <param name="startTime">Начальное время. Не должно быть NaN.</param>
        /// <param name="duration">Длительность. Должна быть неотрицательной и не NaN.</param>
        /// <returns>Новый экземпляр <see cref="TimeInterval"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="startTime"/> равен NaN, либо <paramref name="duration"/> равен NaN.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> отрицательна.</exception>
        public static TimeInterval Create(float startTime, float duration)
        {
            if (float.IsNaN(startTime))
            {
                throw new ArgumentException($"{nameof(startTime)} can not be NaN.", nameof(startTime));
            }
            if (float.IsNaN(duration))
            {
                throw new ArgumentException($"{nameof(duration)} can not be NaN.", nameof(duration));
            }
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), $"{duration} must be positive number.");
            }
            return new TimeInterval(startTime, duration);
        }

        /// <summary>
        /// Создаёт новый интервал с начальным временем 0 и указанной длительностью без проверки аргументов.
        /// Используйте только в критичных к производительности участках, когда уверены в корректности значений.
        /// </summary>
        /// <param name="duration">Длительность интервала.</param>
        /// <returns>Новый экземпляр <see cref="TimeInterval"/>.</returns>
        public static TimeInterval CreateUnsafe(float duration) => new(0.0f, duration);

        /// <summary>
        /// Создаёт новый интервал с указанными начальным временем и длительностью без проверки аргументов.
        /// Используйте только в критичных к производительности участках, когда уверены в корректности значений.
        /// </summary>
        /// <param name="startTime">Начальное время.</param>
        /// <param name="duration">Длительность интервала.</param>
        /// <returns>Новый экземпляр <see cref="TimeInterval"/>.</returns>
        public static TimeInterval CreateUnsafe(float startTime, float duration) => new(startTime, duration);

        /// <summary>
        /// Создаёт интервал, который уже завершён к моменту времени 0 (начальное время равно -<paramref name="duration"/>).
        /// Удобен, когда нужно представить уже закончившееся действие с заданной длительностью.
        /// </summary>
        /// <param name="duration">Длительность интервала. Должна быть неотрицательной и не NaN.</param>
        /// <returns>Завершённый экземпляр <see cref="TimeInterval"/>.</returns>
        /// <exception cref="ArgumentException"><paramref name="duration"/> равен NaN.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> отрицательна.</exception>
        public static TimeInterval CreateCompleted(float duration)
        {
            if (float.IsNaN(duration))
            {
                throw new ArgumentException($"{nameof(duration)} can not be NaN.", nameof(duration));
            }
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), $"{duration} must be positive number.");
            }
            return new TimeInterval(-duration, duration);
        }

        /// <summary>
        /// Создаёт интервал, завершённый к указанному текущему времени.
        /// Начальное время вычисляется как <paramref name="currentTime"/> - <paramref name="duration"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени. Не должен быть NaN.</param>
        /// <param name="duration">Длительность. Должна быть неотрицательной и не NaN.</param>
        /// <returns>Завершённый экземпляр <see cref="TimeInterval"/>.</returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="currentTime"/> равен NaN, либо <paramref name="duration"/> равен NaN.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="duration"/> отрицательна.</exception>
        public static TimeInterval CreateCompleted(float currentTime, float duration)
        {
            if (float.IsNaN(currentTime))
            {
                throw new ArgumentException($"{nameof(currentTime)} can not be NaN.", nameof(currentTime));
            }
            if (float.IsNaN(duration))
            {
                throw new ArgumentException($"{nameof(duration)} can not be NaN.", nameof(duration));
            }
            if (duration < 0.0f)
            {
                throw new ArgumentOutOfRangeException(nameof(duration), $"{duration} must be positive number.");
            }
            float startTime = currentTime - duration;
            return new TimeInterval(startTime, duration);
        }

        /// <summary>
        /// Создаёт завершённый интервал к моменту 0 без проверки аргументов.
        /// Начальное время = -<paramref name="duration"/>.
        /// </summary>
        /// <param name="duration">Длительность интервала.</param>
        /// <returns>Завершённый экземпляр <see cref="TimeInterval"/>.</returns>
        public static TimeInterval CreateCompletedUnsafe(float duration) => new(-duration, duration);

        /// <summary>
        /// Создаёт интервал, завершённый к указанному текущему времени, без проверки аргументов.
        /// Начальное время = <paramref name="currentTime"/> - <paramref name="duration"/>.
        /// </summary>
        /// <param name="currentTime">Текущий момент времени.</param>
        /// <param name="duration">Длительность интервала.</param>
        /// <returns>Завершённый экземпляр <see cref="TimeInterval"/>.</returns>
        public static TimeInterval CreateCompletedUnsafe(float currentTime, float duration)
        {
            float startTime = currentTime - duration;
            return new TimeInterval(startTime, duration);
        }
        #endregion

        public static bool operator ==(TimeInterval left, TimeInterval right) => left.Equals(right);

        public static bool operator !=(TimeInterval left, TimeInterval right) => !left.Equals(right);
    }
}
