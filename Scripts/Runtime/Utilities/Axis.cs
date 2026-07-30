using System;
using UnityEngine;

namespace PostEnot.Toolkits
{
    public enum Axis
    {
        None = 0,
        X = 1 << 0,
        Y = 1 << 2,
        Z = 1 << 3,
        XY = X | Y,
        XZ = X | Z,
        YZ = Y | Z,
        XYZ = X | Y | Z
    }

    public static class AxisMethods
    {
        public static Vector3 ToVector3(this Axis axis) => axis switch
        {
            Axis.None => Vector3.zero,
            Axis.X => Vector3.right,
            Axis.Y => Vector3.up,
            Axis.Z => Vector3.forward,
            Axis.XY => new Vector3(1.0f, 1.0f, 0.0f),
            Axis.XZ => new Vector3(1.0f, 0.0f, 1.0f),
            Axis.YZ => new Vector3(0.0f, 1.0f, 1.0f),
            Axis.XYZ => new Vector3(1.0f, 1.0f, 1.0f),
            _ => throw new ArgumentOutOfRangeException(nameof(axis))
        };

        public static Vector3Int ToVector3Int(this Axis axis) => axis switch
        {
            Axis.None => Vector3Int.zero,
            Axis.X => Vector3Int.right,
            Axis.Y => Vector3Int.up,
            Axis.Z => Vector3Int.forward,
            Axis.XY => new Vector3Int(1, 1, 0),
            Axis.XZ => new Vector3Int(1, 0, 1),
            Axis.YZ => new Vector3Int(0, 1, 1),
            Axis.XYZ => new Vector3Int(1, 1, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(axis))
        };
    }
}
