using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinimapData", menuName = "Game Data/Minimap Data")]
public class MinimapData : ScriptableObject
{
    [Serializable]
    public class AreaPosition
    {
        public int areaIndex;    // Unique index for the area
        public Vector2 position; // Local position on the minimap
    }

    // List of all area positions
    public List<AreaPosition> areaPositions = new List<AreaPosition>();

    /// <summary>
    /// Get the position on the minimap for a given areaIndex.
    /// Returns true if the area exists, false otherwise.
    /// </summary>
    public bool TryGetPosition(int areaIndex, out Vector2 position)
    {
        foreach (var area in areaPositions)
        {
            if (area.areaIndex == areaIndex)
            {
                position = area.position;
                return true;
            }
        }

        position = Vector2.zero;
        return false;
    }
}