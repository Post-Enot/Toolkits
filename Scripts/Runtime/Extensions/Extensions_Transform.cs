using UnityEngine;

namespace PostEnot.Toolkits
{
    public static class Extensions_Transform
    {
        public static void Rotate(this Transform transform, Axis axis, float angle, Space relativeTo = Space.Self)
        {
            Vector3 vectorAxis = axis.ToVector3();
            transform.Rotate(vectorAxis, angle, relativeTo);
        }

        /// <summary>
        /// Отсоединяет Transform от родительского объекта, делая его дочерним к сцене.
        /// </summary>
        public static void Detach(this Transform self) => self.parent = null;

        /// <summary>
        /// Сбрасывает локальную позицию к <see cref="Vector3.zero"/>.
        /// </summary>
        public static void ResetLocalPosition(this Transform self) => self.localPosition = Vector3.zero;

        /// <summary>
        /// Сброс локального вращения к <see cref="Quaternion.identity"/>.
        /// </summary>
        public static void ResetLocalRotation(this Transform self) => self.localRotation = Quaternion.identity;

        /// <summary>
        /// Сбрасывает локальную позицию и вращение к <see cref="Vector3.zero"/> и <see cref="Quaternion.identity"/>;
        /// вызов данного метода эффективнее, чем поочерёдный вызов <see cref="ResetLocalPosition"/> и
        /// <see cref="ResetLocalRotation"/>.
        /// </summary>
        public static void ResetLocalPositionAndRotation(this Transform self)
            => self.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }
}
