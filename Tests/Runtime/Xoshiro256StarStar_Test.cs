using NUnit.Framework;
using PostEnot.Toolkits.RandomEngines;

namespace PostEnot.Toolkits.Tests
{
    public class Xoshiro256StarStar_Test
    {
        [Test]
        public void Xoshiro256StarStar_ZeroStates()
        {
            Xoshiro256StarStar engine = new(0, 0, 0, 0);
            ulong value = engine.NextUInt64();
            Assert.That(value, Is.EqualTo(0));
        }

        /// <summary>
        /// Эталонные значения взяты отсюда: https://github.com/Quuxplusone/Xoshiro256ss#1
        /// </summary>
        [Test]
        public void Xoshiro256StarStar_Test0()
        {
            ulong seed = 100;
            ulong state0 = SplitMix64.Next(ref seed);
            ulong state1 = SplitMix64.Next(ref seed);
            ulong state2 = SplitMix64.Next(ref seed);
            ulong state3 = SplitMix64.Next(ref seed);
            Xoshiro256StarStar engine = new(state0, state1, state2, state3);
            ulong value0 = engine.NextUInt64();
            ulong value1 = engine.NextUInt64();
            ulong value2 = engine.NextUInt64();
            ulong value3 = engine.NextUInt64();
            Assert.That(value0, Is.EqualTo(792317387143481937));
            Assert.That(value1, Is.EqualTo(1418856489092323125));
            Assert.That(value2, Is.EqualTo(6662743737787356053));
            Assert.That(value3, Is.EqualTo(9823178768685107703));
        }
    }
}
