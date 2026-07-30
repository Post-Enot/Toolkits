using UnityEngine;

namespace PostEnot.Toolkits
{
    public static class Extensions_LayerMask
    {
        public static bool Contains(this LayerMask self, int layer) => (self.value & (1 << layer)) != 0;
    }
}
