using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Предоставляет методы демпфирования значений (иммитации движения пружины).
    /// </summary>
    public static class DampUtility
    {
        /// <summary>
        /// Минимально допустимое значение SmoothTime.
        /// </summary>
        public const float MinAllowedSmoothTime = 0.0001f;

        /// <summary>
        /// Плавно изменяет значение от текущего к целевому, имитируя движение затухающей пружины. Предоставляет более гибкий
        /// контроль, нежели чем <see cref="Mathf.SmoothDamp"/>, позволяя реализовать упругое демпфирование.
        /// <br/>
        /// <br/>
        /// Примечание:<br/>
        /// Реализация использует полушаговое интегрирование для устойчивости расчётов.
        /// </summary>
        /// <param name="current">Текущее значение.</param>
        /// <param name="target">Целевое значение.</param>
        /// <param name="velocity">Текущая скорость.</param>
        /// <param name="frequency">Частота колебаний. Чем выше, тем "жестче пружина" и быстрее достижение цели.</param>
        /// <param name="damping">Коэффициент затухания. Чем выше, тем меньше колебаний.</param>
        /// <param name="deltaTime">Временной шаг.</param>
        /// <returns>Новое интерполированное значение на текущем шаге.</returns>
        public static float HarmonicDamp(
            float current,
            float target,
            ref float velocity,
            float frequency,
            float damping,
            float deltaTime)
        {
            float delta = current - target;
            float a0 = (-damping * velocity) - (frequency * delta);
            float v_mid = velocity + (a0 * (deltaTime * 0.5f));
            float x_mid = current + (v_mid * (deltaTime * 0.5f));
            float delta_mid = x_mid - target;
            float a1 = (-damping * v_mid) - (frequency * delta_mid);
            velocity += a1 * deltaTime;
            return x_mid + (v_mid * (deltaTime * 0.5f));
        }
    }
}
