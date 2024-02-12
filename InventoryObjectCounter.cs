using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] 
public class InventoryObjectCounter
{
    [SerializeField] public List<PlacedObjRecord> placedObjRecords = new List<PlacedObjRecord>();
    [SerializeField] public List<PlacedCategoryRecord> placedCategoryRecords = new List<PlacedCategoryRecord>();

    [SerializeField] public List<PlacedObjRecord> dontCountRecords = new List<PlacedObjRecord>();
    [SerializeField] public List<PlacedCategoryRecord> dontCountCategoryRecords = new List<PlacedCategoryRecord>();

    public InventoryObjectCounter()
    {
        
    }

    void OnEnable()
    {
        MakeNewRecords();
    }

    public bool IncreaseCount(InventoryObjectData x)
    {

        bool countIncreased = false;
        PlacedObjRecord dontCountObj = GetDontCountRecordFromObj(x);
        PlacedCategoryRecord dontCountCat = GetDontCountCategoryRecordFromCat(((GridObjectData)x).GetGridObjectType());
        if (dontCountObj != null && dontCountCat != null)
        {
            if (dontCountObj.GetCount() > 0)
            {
                dontCountObj.DecCount();
            }
            else
            {
                GetPlacedObjRecordFromObj(x).IncCount();
            }

            if (dontCountCat.GetCount() > 0)
            {
                dontCountCat.DecCount();
            }
            else
            {
                GetCategoryRecordFromCat(((GridObjectData)x).GetGridObjectType()).IncCount();
                countIncreased = true;
            }
        }
        else
        {
            Debug.Log("Can't find record");
        }

        return countIncreased;
    }

    public void PreventCount(InventoryObjectData x)
    {
        PlacedObjRecord dontCountObj = GetDontCountRecordFromObj(x);
        PlacedCategoryRecord dontCountCat = GetDontCountCategoryRecordFromCat(((GridObjectData)x).GetGridObjectType());

        if (dontCountObj != null && dontCountCat != null)
        {
            GetDontCountRecordFromObj(x).IncCount();
            GetDontCountCategoryRecordFromCat(((GridObjectData)x).GetGridObjectType()).IncCount();
        }
        else
        {
            Debug.Log("Can't find record");
        }
    }

    public int GetCountOf(InventoryObjectData x)
    {
        PlacedObjRecord dontCountObj = GetPlacedObjRecordFromObj(x);

        if (dontCountObj != null)
        {
            return dontCountObj.GetCount();
        }
        else
        {
            return -1;
        }

    }
    public int GetCountOf(GridObjectData.ObjectType x)
    {
        
        PlacedCategoryRecord dontCountCat = GetCategoryRecordFromCat(x);

        if (dontCountCat != null)
        {
            return dontCountCat.GetCount();
        }
        else
        {
            return -1;
        }
    }

 

    public PlacedObjRecord GetPlacedObjRecordFromObj(InventoryObjectData x)
    {
        int myID = TileObjectSaver.GetIDFromData(x);
        //Debug.Log(myID);
        foreach (PlacedObjRecord record in placedObjRecords)
        {
            if (record.dataID == myID)
            {
                return record;
            }
        }
        return null;
    }

    public PlacedObjRecord GetDontCountRecordFromObj(InventoryObjectData x)
    {
        int myID = TileObjectSaver.GetIDFromData(x);
        //Debug.Log(myID);
        foreach (PlacedObjRecord record in dontCountRecords)
        {
            if (record.dataID == myID)
            {
                return record;
            }
        }
        return null;
    }

    public PlacedCategoryRecord GetCategoryRecordFromCat(GridObjectData.ObjectType x)
    {
        //int ID = TileObjectSaver.GetIDFromData(x);
        foreach (PlacedCategoryRecord record in placedCategoryRecords)
        {
            if (record.GetType() == x)
            {
                return record;
            }
        }
        return null;
    }
    public PlacedCategoryRecord GetDontCountCategoryRecordFromCat(GridObjectData.ObjectType x)
    {
        //int ID = TileObjectSaver.GetIDFromData(x);
        foreach (PlacedCategoryRecord record in dontCountCategoryRecords)
        {
            if (record.GetType() == x)
            {
                return record;
            }
        }
        return null;
    }


    public void MakeNewRecords()
    {
        InventoryObjectData[] gridObjectDataObjects = Resources.LoadAll<InventoryObjectData>("InventoryObjectDatas");

        List<PlacedObjRecord> placedObjRecords = new List<PlacedObjRecord>();
        List<PlacedCategoryRecord> placedCategoryRecords = new List<PlacedCategoryRecord>();

        List<PlacedObjRecord> dontCountRecords = new List<PlacedObjRecord>();
        List<PlacedCategoryRecord> dontCountCategoryRecords = new List<PlacedCategoryRecord>();

        foreach (GridObjectData gridObjectData in gridObjectDataObjects)
        {
            placedObjRecords.Add(new PlacedObjRecord(gridObjectData));
            dontCountRecords.Add(new PlacedObjRecord(gridObjectData));
        }

        // Get all values of the ObjectType enum
        GridObjectData.ObjectType[] objectTypes = (GridObjectData.ObjectType[])System.Enum.GetValues(typeof(GridObjectData.ObjectType));

        // Iterate through each ObjectType
        foreach (GridObjectData.ObjectType objectType in objectTypes)
        {
            placedCategoryRecords.Add(new PlacedCategoryRecord(objectType));
            dontCountCategoryRecords.Add(new PlacedCategoryRecord(objectType));

        }

        this.placedObjRecords = placedObjRecords;
        this.placedCategoryRecords = placedCategoryRecords;

        this.dontCountRecords = dontCountRecords;
        this.dontCountCategoryRecords = dontCountCategoryRecords;
    }
}
