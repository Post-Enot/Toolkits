namespace PostEnot.Toolkits
{
    public sealed class HarmonicDamper : HarmonicDamperBase<float>
    {
        public override float Velocity
        {
            get => _velocity;
            set => _velocity = value;
        }

        private float _velocity;

        public override float Update(float frequency, float daming, float deltaTime)
        {
            Current = DampUtility.HarmonicDamp(Current, Target, ref _velocity, frequency, daming, deltaTime);
            return Current;
        }
    }
}
