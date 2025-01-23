using System;
using UnityEngine;

[Serializable]
public enum DirectionsEnum
{
    Up = 1,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}

public static class DirectionVector
{
    public static Vector2 GetDirectionVector(DirectionsEnum directionEnum)
    {
        switch (directionEnum)
        {
            case DirectionsEnum.Up:
                return Vector2.up;
            case DirectionsEnum.Down:
                return Vector2.down;
            case DirectionsEnum.Left:
                return Vector2.left;
            case DirectionsEnum.Right:
                return Vector2.right;
            case DirectionsEnum.UpLeft:
                return new Vector2(-1, 1);
            case DirectionsEnum.UpRight:
                return new Vector2(1, 1);
            case DirectionsEnum.DownLeft:
                return new Vector2(-1, -1);
            case DirectionsEnum.DownRight:
                return new Vector2(1, -1);
        }

        return Vector2.zero;
    }
}