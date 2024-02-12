using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlacedObjRecord
{
    public int dataID = -1;
    public int count = 0;

    public PlacedObjRecord(InventoryObjectData x)
    {
        dataID = TileObjectSaver.GetIDFromData(x);
    }

    public InventoryObjectData GetData()
    {
        return TileObjectSaver.GetDataFromID(dataID);
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
