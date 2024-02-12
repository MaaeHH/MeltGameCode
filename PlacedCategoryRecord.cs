using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacedCategoryRecord
{
    public GridObjectData.ObjectType type = GridObjectData.ObjectType.Nothing;
    public int count = 0;

    public PlacedCategoryRecord(GridObjectData.ObjectType x)
    {
        type = x;
    }

    public GridObjectData.ObjectType GetType()
    {
        return type;
    }

    public int GetCount()
    {
        return count;
    }
    public void SetCount(int x)
    {
        count = x;
    }
    public void IncCount()
    {
        count++;
    }
    public void DecCount()
    {
        count--;
    }
   
}
