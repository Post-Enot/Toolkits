using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Базовый класс для плавной интерполяции к целевому значению по алгоритму <see cref="Mathf.SmoothDamp"/>
    /// и сохранения значений между вызовами.
    /// <br/>Конкретная математическая реализация зависит от типа <typeparamref name="TValue"/> и определяется в наследниках.
    /// </summary>
    /// <typeparam name="TValue">Тип интерполируемого значения.</typeparam>
    public abstract class SmoothDamperBase<TValue>
    {
        /// <summary>
        /// Текущее интерполируемое значение.
        /// </summary>
        public virtual TValue Current { get; set; }

        /// <summary>
        /// Целевое значение интерполяции.
        /// </summary>
        public virtual TValue Target { get; set; }

        /// <summary>
        /// Текущая скорость изменения значения. В большинстве сценариев не рекомендуется напрямую изменить значение;
        /// для сброса значения для мгновенного перемещения следует использовать <see cref="Reset"/> и <see cref="ResetTo"/>.
        /// </summary>
        public virtual TValue Velocity { get; set; }

        /// <summary>
        /// Выполняет шаг интерполяции, используя <see cref="Time.deltaTime"/> в качестве дельты времени.
        /// <br/>
        /// <br/>
        /// Примечания:<br/>
        /// - Оригинальная реализация метода <see cref="Mathf.SmoothDamp"/> избегает превышения целевого значения.<br/>
        /// - Граничный случай: если <see cref="Current"/> == <see cref="Target"/> и <see cref="Time.deltaTime"/> == 0,
        /// <see cref="Velocity"/> примет значение NaN.
        /// </summary>
        /// <param name="smoothTime">Приблизительное время в секундах, за которое значение достигнет цели.
        /// Алгоритм <see cref="Mathf.SmoothDamp"/> не допускает значение <paramref name="smoothTime"/>,
        /// меньшее <see cref="DampUtility.MinAllowedSmoothTime"/>;
        /// если переданное значение <paramref name="smoothTime"/> меньше <see cref="DampUtility.MinAllowedSmoothTime"/>, алгоритм установит
        /// <paramref name="smoothTime"/> в <see cref="DampUtility.MinAllowedSmoothTime"/>.</param>
        /// <returns>
        /// Новое текущее значение <see cref="Current"/> после выполнения шага интерполяции.
        /// </returns>
        public virtual TValue Update(float smoothTime) => Update(smoothTime, Time.deltaTime);

        /// <summary>
        /// Выполняет шаг интерполяции.
        /// <br/>
        /// <br/>
        /// Примечания:<br/>
        /// - Оригинальная реализация метода <see cref="Mathf.SmoothDamp"/> избегает превышения целевого значения.<br/>
        /// - Граничный случай: если <see cref="Current"/> == <see cref="Target"/> и <paramref name="deltaTime"/> == 0,
        /// <see cref="Velocity"/> примет значение NaN.
        /// </summary>
        /// <param name="smoothTime">Приблизительное время в секундах, за которое значение достигнет цели.
        /// Алгоритм <see cref="Mathf.SmoothDamp"/> не допускает значение <paramref name="smoothTime"/>,
        /// меньшее <see cref="DampUtility.MinAllowedSmoothTime"/>;
        /// если переданное значение <paramref name="smoothTime"/> меньше <see cref="DampUtility.MinAllowedSmoothTime"/>, алгоритм установит
        /// <paramref name="smoothTime"/> в <see cref="DampUtility.MinAllowedSmoothTime"/>.</param>
        /// <param name="deltaTime">Время, прошедшее с предыдущего шага.</param>
        /// <param name="maxSpeed">Максимально допустимая скорость изменения значения.</param>
        /// <returns>
        /// Новое текущее значение <see cref="Current"/> после выполнения шага интерполяции.
        /// </returns>
        public abstract TValue Update(float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity);

        /// <summary>
        /// Сбрасывает состояние интерполятора, устанавливая значения <see cref="Current"/> и <see cref="Target"/>
        /// в <paramref name="value"/> и сбрасывая значение <see cref="Velocity"/> в <see langword="default"/>.
        /// Метод равносилен мгновенному переходу к заданному значению.
        /// </summary>
        /// <param name="value">Значение, которое будет присвоено <see cref="Current"/> и <see cref="Target"/>.</param>
        public virtual void ResetTo(TValue value)
        {
            Current = value;
            Target = value;
            Velocity = default;
        }

        public virtual bool Approximately() => Approximately(Mathf.Epsilon);

        public abstract bool Approximately(float epsilon);

        /// <summary>
        /// Сбрасывает состояние интерполятора, устанавливая все значения в <see langword="default"/>.
        /// </summary>
        public virtual void Reset() => ResetTo(default);

        internal static bool ApproximatelyAngle(float currentX, float targetX, float epsilon)
        {
            float delta = Mathf.DeltaAngle(currentX, targetX);
            float deltaModule = Mathf.Abs(delta);
            float epsilonModule = Mathf.Abs(epsilon);
            return deltaModule < epsilonModule;
        }
    }
}
