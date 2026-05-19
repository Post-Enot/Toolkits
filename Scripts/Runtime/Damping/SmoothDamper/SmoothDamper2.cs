using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Представляет методы для плавной интерполяции к целевому значению по алгоритму <see cref="Mathf.SmoothDamp"/>
    /// и сохранения значений между вызовами.
    /// </summary>
    public sealed class SmoothDamper2 : SmoothDamperBase<Vector2>
    {
        public override Vector2 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private Vector2 _velocity;

        public override Vector2 Update(float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
        {
            Current = Vector2.SmoothDamp(Current, Target, ref _velocity, smoothTime, maxSpeed, deltaTime);
            return Current;
        }

        public override bool Approximately(float epsilon)
        {
            float deltaX = Target.x - Current.x;
            float deltaY = Target.y - Current.y;
            float distanceSqr = (deltaX * deltaX) + (deltaY * deltaY);
            float epsilonSqr = epsilon * epsilon;
            return distanceSqr < epsilonSqr;
        }
    }
}
