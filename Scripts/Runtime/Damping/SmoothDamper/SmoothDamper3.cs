using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Представляет методы для плавной интерполяции к целевому значению по алгоритму <see cref="Mathf.SmoothDamp"/>
    /// и сохранения значений между вызовами.
    /// </summary>
    public sealed class SmoothDamper3 : SmoothDamperBase<Vector3>
    {
        public override Vector3 Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private Vector3 _velocity;

        public override Vector3 Update(float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
        {
            Current = Vector3.SmoothDamp(Current, Target, ref _velocity, smoothTime, maxSpeed, deltaTime);
            return Current;
        }

        public override bool Approximately(float epsilon)
        {
            float deltaX = Target.x - Current.x;
            float deltaY = Target.y - Current.y;
            float deltaZ = Target.z - Current.z;
            float distanceSqr = (deltaX * deltaX) + (deltaY * deltaY) + (deltaZ * deltaZ);
            float epsilonSqr = epsilon * epsilon;
            return distanceSqr < epsilonSqr;
        }
    }
}
