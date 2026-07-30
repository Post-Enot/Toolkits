using UnityEngine;

namespace PostEnot.Toolkits
{
    public static class Extensions_Rect
    {
        public static void Encapsulate(ref this Rect self, Vector2 point)
        {
            float minX = Mathf.Min(self.xMin, point.x);
            float minY = Mathf.Min(self.yMin, point.y);
            float maxX = Mathf.Max(self.xMax, point.x);
            float maxY = Mathf.Max(self.yMax, point.y);
            self.x = minX;
            self.y = minY;
            self.width = maxX - minX;
            self.height = maxY - minY;
        }

        public static Vector2 ClampPoint(this Rect self, Vector2 point)
        {
            Vector2 min = self.min;
            Vector2 max = self.max;
            float x = Mathf.Clamp(point.x, min.x, max.x);
            float y = Mathf.Clamp(point.y, min.y, max.y);
            return new Vector2(x, y);
        }

        public static bool Contains(this Rect self, float x, float y)
        {
            if ((x >= self.xMin) && (x < (self.xMin + self.width)) && (y >= self.yMin))
            {
                return y < (self.yMin + self.height);
            }
            return false;
        }
    }
}
