using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Представляет методы для плавной интерполяции к целевому значению по алгоритму <see cref="Mathf.SmoothDamp"/>
    /// и сохранения значений между вызовами.
    /// </summary>
    public sealed class SmoothDamper : SmoothDamperBase<float>
    {
        public override float Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private float _velocity;

        public override float Update(float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
        {
            Current = Mathf.SmoothDamp(Current, Target, ref _velocity, smoothTime, maxSpeed, deltaTime);
            return Current;
        }

        public override bool Approximately(float epsilon)
        {
            float deltaModule = Mathf.Abs(Target - Current);
            float epsilonModule = Mathf.Abs(epsilon);
            return deltaModule < epsilonModule;
        }
    }
}
