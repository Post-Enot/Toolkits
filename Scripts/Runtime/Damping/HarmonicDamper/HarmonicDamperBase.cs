using UnityEngine;

namespace PostEnot.Toolkits
{
    public abstract class HarmonicDamperBase<TValue>
    {
        public virtual TValue Current { get; set; }
        public virtual TValue Target { get; set; }
        public virtual TValue Velocity { get; set; }

        public virtual TValue Update(float frequency, float damping) => Update(frequency, damping, Time.deltaTime);

        public abstract TValue Update(float frequency, float daming, float deltaTime);

        public void Reset() => ResetTo(default);

        public void ResetTo(TValue value)
        {
            Current = value;
            Target = value;
            Velocity = default;
        }
    }
}
