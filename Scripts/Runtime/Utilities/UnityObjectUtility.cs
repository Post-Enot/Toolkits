#nullable enable

using System.Diagnostics.CodeAnalysis;

namespace PostEnot.Toolkits
{
    public static class UnityObjectUtility
    {
        public static bool IsNull<T>([NotNullWhen(false)] T value)
        {
            if (value == null)
            {
                return true;
            }
            if (value is UnityEngine.Object unityObject)
            {
                return unityObject == null;
            }
            return false;
        }

        public static bool IsNotNull<T>([NotNullWhen(true)] T value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is UnityEngine.Object unityObject)
            {
                return unityObject != null;
            }
            return true;
        }
    }
}
