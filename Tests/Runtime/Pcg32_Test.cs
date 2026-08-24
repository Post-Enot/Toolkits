using PostEnot.Toolkits.RandomEngines;

namespace PostEnot.Toolkits.Tests
{
    public sealed class Pcg32_Test : RandomEngineTestsBase
    {
        protected override IRandomEngine CreateEngine()
        {
            Pcg32 engine = new(20, 3);
            return engine;
        }
    }
}
