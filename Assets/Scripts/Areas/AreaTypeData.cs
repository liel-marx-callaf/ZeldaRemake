using System;
using System.Collections.Generic;
using Helper;
using UnityEngine;


[CreateAssetMenu(fileName = "AreaTypeData", menuName = "Game Data/Area Type Data")]
public class AreaTypeData : ScriptableObject
{
    [Serializable]
    public class AreaType
    {
        public int areaIndex;
        public AreaTypeEnum areaType;
    }
    
    public List<AreaType> areaTypes = new List<AreaType>();
    
    public bool TryGetAreaType(int areaIndex, out AreaTypeEnum areaType)
    {
        foreach (var area in areaTypes)
        {
            if (area.areaIndex == areaIndex)
            {
                areaType = area.areaType;
                return true;
            }
        }

        areaType = AreaTypeEnum.Error;
        return false;
    }   
}