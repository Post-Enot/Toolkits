using UnityEngine;

namespace PostEnot.Toolkits
{
    /// <summary>
    /// Представляет методы для плавной интерполяции углов эйлера к целевому значению по алгоритму <see cref="Mathf.SmoothDampAngle"/>
    /// и сохранения значений между вызовами.
    /// Алгоритм <see cref="Mathf.SmoothDampAngle"/> использует <see cref="Mathf.DeltaAngle"/>,
    /// гарантируя перемещение по кратчайшему пути.
    /// </summary>
    public sealed class SmoothDamperAngle : SmoothDamperBase<float>
    {
        public override float Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private float _velocity;

        public override float Update(float smoothTime, float deltaTime, float maxSpeed = float.PositiveInfinity)
        {
            Current = Mathf.SmoothDampAngle(Current, Target, ref _velocity, smoothTime, maxSpeed, deltaTime);
            return Current;
        }

        public override bool Approximately(float epsilon)
        {
            float delta = Mathf.DeltaAngle(Current, Target);
            float deltaModule = Mathf.Abs(delta);
            float epsilonModule = Mathf.Abs(epsilon);
            return deltaModule < epsilonModule;
        }
    }
}
