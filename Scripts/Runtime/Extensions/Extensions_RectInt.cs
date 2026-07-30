using UnityEngine;

namespace PostEnot.Toolkits
{
    public static class Extensions_RectInt
    {
        public static void Encapsulate(ref this RectInt self, Vector2Int point)
        {
            int minX = Mathf.Min(self.xMin, point.x);
            int minY = Mathf.Min(self.yMin, point.y);
            int maxX = Mathf.Max(self.xMax, point.x);
            int maxY = Mathf.Max(self.yMax, point.y);
            self.x = minX;
            self.y = minY;
            self.width = maxX - minX;
            self.height = maxY - minY;
        }

        public static Vector2Int ClampPoint(this RectInt self, Vector2Int point)
        {
            Vector2Int min = self.min;
            Vector2Int max = self.max;
            int x = Mathf.Clamp(point.x, min.x, max.x);
            int y = Mathf.Clamp(point.y, min.y, max.y);
            return new Vector2Int(x, y);
        }

        public static bool Contains(this RectInt self, int x, int y)
        {
            if (x >= self.xMin && y >= self.yMin && x < self.xMax)
            {
                return y < self.yMax;
            }
            return false;
        }
    }
}
