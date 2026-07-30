using System;
using UnityEngine;

namespace PostEnot.Toolkits
{
    public static class Geometry
    {
        private const float PI2 = Mathf.PI * 2.0f;

        public static float WrapAngle180(float angle)
        {
            float num = Mathf.Repeat(angle, 360.0f);
            if (num > 180.0f)
            {
                num -= 360.0f;
            }
            return num;
        }

        public static Vector2 WrapAngle180(Vector2 angles) => new()
        {
            x = WrapAngle180(angles.x),
            y = WrapAngle180(angles.y)
        };

        public static Vector3 WrapAngle180(Vector3 angles) => new()
        {
            x = WrapAngle180(angles.x),
            y = WrapAngle180(angles.y),
            z = WrapAngle180(angles.z)
        };

        /// <summary>
        /// Проверяет, находится ли точка внутри сферы, используя сравнение квадратов расстояний.
        /// <br/>
        /// <br/>
        /// Примечание:<br/>
        /// Из-за особенностей реализации значение радиуса берётся по модулю; из этого следует,
        /// что <paramref name="radius"/> = -<paramref name="radius"/>.
        /// </summary>
        /// <param name="point">Проверяемая точка в мировых координатах.</param>
        /// <param name="center">Центр сферы в мировых координатах.</param>
        /// <param name="radius">Радиус сферы.</param>
        /// <returns>
        /// true, если точка находится внутри сферы или на её поверхности;<br/>
        /// false, если точка снаружи.
        /// </returns>
        public static bool IsPointInSphere(Vector3 point, Vector3 center, float radius)
        {
            float dx = point.x - center.x;
            float dy = point.y - center.y;
            float dz = point.z - center.z;
            return ((dx * dx) + (dy * dy) + (dz * dz)) <= (radius * radius);
        }

        /// <summary>
        /// Проверяет, находится ли точка внутри цилиндра с вертикальной осью (ось Y).
        /// </summary>
        /// <param name="point">Проверяемая точка в мировых координатах.</param>
        /// <param name="center">Центр цилиндра в мировых координатах.</param>
        /// <param name="height">
        /// Высота цилиндра вдоль оси Y (должна быть положительной).
        /// </param>
        /// <param name="radius">
        /// Радиус цилиндра в плоскости XZ (должен быть положительным).
        /// </param>
        /// <returns>
        /// true, если точка находится внутри цилиндра (включая граничные поверхности);<br/>
        /// false, если точка находится снаружи или высота или радиус цилиндра отрицательные.
        /// </returns>
        public static bool IsPointInCylinder(Vector3 point, Vector3 center, float height, float radius)
        {
            float dx = point.x - center.x;
            float dy = point.y - center.y;
            float dz = point.z - center.z;
            return (Mathf.Abs(dy) <= (height * 0.5f)) && (((dx * dx) + (dz * dz)) <= (radius * radius));
        }

        /// <summary>
        /// Вычисляет мировые координаты 8 углов <see cref="BoxCollider"/> с учётом текущей трансформации объекта.
        /// </summary>
        /// <param name="buffer">
        /// Буфер для записи углов размером минимум 8 элементов.<br/><br/>
        /// Порядок точек:<br/>
        /// 0 левый-верхний-задний  (-X, +Y, -Z)<br/>
        /// 1 правый-верхний-задний (+X, +Y, -Z)<br/>
        /// 2 правый-верхний-передний (+X, +Y, +Z)<br/>
        /// 3 левый-верхний-передний (-X, +Y, +Z)<br/>
        /// 4 левый-нижний-задний   (-X, -Y, -Z)<br/>
        /// 5 правый-нижний-задний  (+X, -Y, -Z)<br/>
        /// 6 правый-нижний-передний (+X, -Y, +Z)<br/>
        /// 7 левый-нижний-передний (-X, -Y, +Z)
        /// </param>
        public static void CalculateBoxColliderCorners(Vector3 center, Vector3 size, Span<Vector3> buffer)
        {
            Vector3 halfSize = size * 0.5f;
            buffer[0] = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
            buffer[1] = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
            buffer[2] = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);
            buffer[3] = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
            buffer[4] = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
            buffer[5] = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
            buffer[6] = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
            buffer[7] = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        }

        public static void CalculateBoundsCorners(Bounds worldBounds, Span<Vector3> buffer)
        {
            buffer[0] = worldBounds.min;
            buffer[1] = worldBounds.max;
            buffer[2] = new Vector3(worldBounds.min.x, worldBounds.min.y, worldBounds.max.z);
            buffer[3] = new Vector3(worldBounds.min.x, worldBounds.max.y, worldBounds.min.z);
            buffer[4] = new Vector3(worldBounds.max.x, worldBounds.max.y, worldBounds.min.z);
            buffer[5] = new Vector3(worldBounds.min.x, worldBounds.max.y, worldBounds.max.z);
            buffer[6] = new Vector3(worldBounds.max.x, worldBounds.min.y, worldBounds.max.z);
            buffer[7] = new Vector3(worldBounds.max.x, worldBounds.max.y, worldBounds.min.z);
        }

        public static void CalculateOrthogonalCirclesPoints(
            Span<Vector3> pointsBuffer,
            Vector3 center,
            float radius,
            int pointsPerQuarter)
        {
            int pointsPerCircleCount = 4 * pointsPerQuarter;

            float angleStep = PI2 / pointsPerCircleCount;

            for (int plane = 0; plane < 3; plane += 1)
            {
                int offset = plane * pointsPerCircleCount;
                for (int i = 0; i < pointsPerCircleCount; i += 1)
                {
                    float angle = i * angleStep;
                    float cos = Mathf.Cos(angle);
                    float sin = Mathf.Sin(angle);
                    int idx = offset + i;
                    if (plane == 0)
                    {
                        pointsBuffer[idx] = new Vector3(
                            center.x + radius * cos,
                            center.y + radius * sin,
                            center.z
                        );
                    }
                    else if (plane == 1)
                    {
                        pointsBuffer[idx] = new Vector3(
                            center.x + radius * cos,
                            center.y,
                            center.z + radius * sin
                        );
                    }
                    else
                    {
                        pointsBuffer[idx] = new Vector3(
                            center.x,
                            center.y + radius * cos,
                            center.z + radius * sin
                        );
                    }
                }
            }
        }

        /// <summary>
        /// Вычисляет 6 плоскостей усечённой пирамиды видимости камеры.<br/><br/>
        /// Порядок плоскостей:<br/>
        /// 0 левая (Left)<br/>
        /// 1 правая (Right)<br/>
        /// 2 нижняя (Down)<br/>
        /// 3 верхняя (Up)<br/>
        /// 4 ближняя (Near)<br/>
        /// 5 дальняя (Far)<br/>
        /// </summary>
        /// <param name="camera">Целевая камера для расчёта плоскостей.</param>
        /// <param name="planes">
        /// Span для записи плоскостей (должен иметь ёмкость не менее 6 элементов).<br/>
        /// Результат сохраняется в порядке: Left, Right, Down, Up, Near, Far.
        /// </param>
        public static void CalculateFrustumPlanes(Matrix4x4 projectionMatrix, Matrix4x4 worldToCameraMatrix, Span<Plane> planes)
        {
            Matrix4x4 m = projectionMatrix * worldToCameraMatrix;
            planes[0] = CalculatePlane(m.m30 + m.m00, m.m31 + m.m01, m.m32 + m.m02, m.m33 + m.m03); // Left
            planes[1] = CalculatePlane(m.m30 - m.m00, m.m31 - m.m01, m.m32 - m.m02, m.m33 - m.m03); // Right
            planes[2] = CalculatePlane(m.m30 + m.m10, m.m31 + m.m11, m.m32 + m.m12, m.m33 + m.m13); // Down
            planes[3] = CalculatePlane(m.m30 - m.m10, m.m31 - m.m11, m.m32 - m.m12, m.m33 - m.m13); // Up
            planes[4] = CalculatePlane(m.m30 + m.m20, m.m31 + m.m21, m.m32 + m.m22, m.m33 + m.m23); // Near
            planes[5] = CalculatePlane(m.m30 - m.m20, m.m31 - m.m21, m.m32 - m.m22, m.m33 - m.m23); // Far
        }

        /// <summary>
        /// Вычисляет 5 плоскостей усечённой пирамиды видимости камеры без дальней плоскости.<br/><br/>
        /// Порядок плоскостей:<br/>
        /// 0 левая (Left)<br/>
        /// 1 правая (Right)<br/>
        /// 2 нижняя (Down)<br/>
        /// 3 верхняя (Up)<br/>
        /// 4 ближняя (Near)<br/>
        /// </summary>
        /// <param name="camera">Целевая камера для расчёта плоскостей.</param>
        /// <param name="planes">
        /// Span для записи плоскостей (должен иметь ёмкость не менее 5 элементов).<br/>
        /// Результат сохраняется в порядке: Left, Right, Down, Up, Near.
        /// </param>
        public static void CalculateFrustumPlanesWithoutFar(Matrix4x4 projectionMatrix, Matrix4x4 worldToCameraMatrix, Span<Plane> planes)
        {
            Matrix4x4 m = projectionMatrix * worldToCameraMatrix;
            planes[0] = CalculatePlane(m.m30 + m.m00, m.m31 + m.m01, m.m32 + m.m02, m.m33 + m.m03); // Left
            planes[1] = CalculatePlane(m.m30 - m.m00, m.m31 - m.m01, m.m32 - m.m02, m.m33 - m.m03); // Right
            planes[2] = CalculatePlane(m.m30 + m.m10, m.m31 + m.m11, m.m32 + m.m12, m.m33 + m.m13); // Down
            planes[3] = CalculatePlane(m.m30 - m.m10, m.m31 - m.m11, m.m32 - m.m12, m.m33 - m.m13); // Up
            planes[4] = CalculatePlane(m.m30 + m.m20, m.m31 + m.m21, m.m32 + m.m22, m.m33 + m.m23); // Near
        }

        /// <summary>
        /// Проверяет, находится ли точка в усечённом конусе камеры в порядке: ближняя (Near),
        /// боковые (Left, Right), вертикальные (Up, Down), дальняя (Far).
        /// </summary>
        /// <param name="frustumPlanes">Набор плоскостей фрустума (минимум 5 элементов).<br/><br/>
        /// Порядок плоскостей:<br/>
        /// 0 левая (Left)<br/>
        /// 1 правая (Right)<br/>
        /// 2 нижняя (Down)<br/>
        /// 3 верхняя (Up)<br/>
        /// 4 ближняя (Near)<br/>
        /// 5 дальняя (Far)<br/>
        /// </param>
        /// <param name="point">Позиция точки в мировых координатах.</param>
        /// <returns>
        /// True, если точка находится в усечённом конусе камеры без учёта дальней плоскости;
        /// иначе false.
        /// </returns>
        public static bool IsPointInFrustum(ReadOnlySpan<Plane> frustumPlanes, Vector3 point) =>
                (frustumPlanes[4].GetDistanceToPoint(point) >= 0.0f) && // Near.
                (frustumPlanes[0].GetDistanceToPoint(point) >= 0.0f) && // Left.
                (frustumPlanes[1].GetDistanceToPoint(point) >= 0.0f) && // Right.
                (frustumPlanes[3].GetDistanceToPoint(point) >= 0.0f) && // Up.
                (frustumPlanes[2].GetDistanceToPoint(point) >= 0.0f) && // Down.
                (frustumPlanes[5].GetDistanceToPoint(point) >= 0.0f); // Far.

        /// <summary>
        /// Проверяет, находится ли точка в усечённом конусе камеры без учёта дальней плоскости в
        /// порядке: ближняя (Near), боковые (Left, Right), вертикальные (Up, Down).
        /// </summary>
        /// <param name="frustumPlanes">Набор плоскостей фрустума (минимум 5 элементов).<br/><br/>
        /// Порядок плоскостей:<br/>
        /// 0 левая (Left)<br/>
        /// 1 правая (Right)<br/>
        /// 2 нижняя (Down)<br/>
        /// 3 верхняя (Up)<br/>
        /// 4 ближняя (Near)<br/>
        /// 5 дальняя (Far) - не используется<br/>
        /// </param>
        /// <param name="point">Позиция точки в мировых координатах.</param>
        /// <returns>
        /// True, если точка находится в усечённом конусе камеры без учёта дальней плоскости;
        /// иначе false.
        /// </returns>
        public static bool IsPointInFrustumWithoutFar(
            ReadOnlySpan<Plane> frustumPlanes,
            Vector3 point) =>
                (frustumPlanes[4].GetDistanceToPoint(point) >= 0.0f) && // Near.
                (frustumPlanes[0].GetDistanceToPoint(point) >= 0.0f) && // Left.
                (frustumPlanes[1].GetDistanceToPoint(point) >= 0.0f) && // Right.
                (frustumPlanes[3].GetDistanceToPoint(point) >= 0.0f) && // Up.
                (frustumPlanes[2].GetDistanceToPoint(point) >= 0.0f); // Down.

        public static float CalculateBoundingSphereRadius(float height, float radius)
        {
            float halfHeight = height * 0.5f;
            return Mathf.Sqrt((halfHeight * halfHeight) + (radius * radius));
        }

        /// <summary>
        /// Вычисляет точку на поверхности цилиндра в заданном направлении от центра.
        /// </summary>
        /// <param name="height">Высота цилиндра (по оси Y).</param>
        /// <param name="radius">Радиус цилиндра.</param>
        /// <param name="direction">Направление от центра цилиндра к точке на поверхности.</param>
        /// <returns>Координаты точки на поверхности цилиндра в мировом пространстве; иначе <see cref="Vector3.zero"/>.</returns>
        public static Vector3 CalculateCylinderSurfacePoint(float height, float radius, Vector3 direction)
        {
            if (CheckCylinderSideIntersection(height, radius, direction, out Vector3 point))
            {
                return point;
            }
            _ = CheckCylinderBaseIntersection(height, radius, direction, out point);
            return point;
        }

        private static Plane CalculatePlane(float a, float b, float c, float d)
        {
            float magnitude = Mathf.Sqrt((a * a) + (b * b) + (c * c));
            float invLength = 1.0f / magnitude;
            Plane plane = new()
            {
                distance = d * invLength,
                normal = new Vector3()
                {
                    x = a * invLength,
                    y = b * invLength,
                    z = c * invLength
                }
            };
            return plane;
        }

        private static bool CheckCylinderSideIntersection(float height, float radius, Vector3 direction, out Vector3 point)
        {
            float a = (direction.x * direction.x) + (direction.z * direction.z);
            if (a <= Mathf.Epsilon)
            {
                point = Vector3.zero;
                return false;
            }

            point = radius / Mathf.Sqrt(a) * direction;
            float halfHeight = height / 2;
            if (Mathf.Abs(point.y) <= halfHeight)
            {
                return true;
            }
            point = Vector3.zero;
            return false;
        }

        private static bool CheckCylinderBaseIntersection(float height, float radius, Vector3 direction, out Vector3 point)
        {
            if (Mathf.Abs(direction.y) <= Mathf.Epsilon)
            {
                point = Vector3.zero;
                return false;
            }
            float halfHeight = height / 2;
            point = halfHeight / Mathf.Abs(direction.y) * direction;
            if ((point.x * point.x) + (point.z * point.z) <= (radius * radius))
            {
                point.y = Mathf.Sign(direction.y) * halfHeight;
                return true;
            }

            point = Vector3.zero;
            return false;
        }
    }
}
