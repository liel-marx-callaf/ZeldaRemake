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
    DownRight,
    Error
}

public static class DirectionVector
{
    public static Vector2 GetDirectionToVector(DirectionsEnum directionEnum)
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
    public static DirectionsEnum GetVectorToDirection(Vector2 direction)
    {
        if (direction == Vector2.up)
        {
            return DirectionsEnum.Up;
        }
        if (direction == Vector2.down)
        {
            return DirectionsEnum.Down;
        }
        if (direction == Vector2.left)
        {
            return DirectionsEnum.Left;
        }
        if (direction == Vector2.right)
        {
            return DirectionsEnum.Right;
        }
        if (direction == new Vector2(-1, 1))
        {
            return DirectionsEnum.UpLeft;
        }
        if (direction == new Vector2(1, 1))
        {
            return DirectionsEnum.UpRight;
        }
        if (direction == new Vector2(-1, -1))
        {
            return DirectionsEnum.DownLeft;
        }
        if (direction == new Vector2(1, -1))
        {
            return DirectionsEnum.DownRight;
        }
        return DirectionsEnum.Error;
    }
}