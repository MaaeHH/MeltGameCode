using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public static class TileObjectSaver
{
    private static List<InventoryObjectData> inventoryObjectDatas;
    private static List<FriendshipReqData> freqDatas;
    private static List<GridObjectInstance> gridObjectInstances;

    private static void LoadDatas()
    {
        if (inventoryObjectDatas == null)
        {

            inventoryObjectDatas = new List<InventoryObjectData>();
            InventoryObjectData[] inventoryObjectDataObjects = Resources.LoadAll<InventoryObjectData>("inventoryObjectDatas");


            // You can now access and use the found Scriptable Objects
            foreach (InventoryObjectData invObjectData in inventoryObjectDataObjects)
            {
                //Debug.Log(gridObjectData.GetName());
                inventoryObjectDatas.Add(invObjectData);

            }
            //RefreshGridObjectInstanceData();
        }


        if (freqDatas == null)
        {

            freqDatas = new List<FriendshipReqData>();
            FriendshipReqData[] freqs = Resources.LoadAll<FriendshipReqData>("FriendshipReqs");


            // You can now access and use the found Scriptable Objects
            foreach (FriendshipReqData freq in freqs)
            {
                //Debug.Log(gridObjectData.GetName());
                freqDatas.Add(freq);

            }
            //RefreshGridObjectInstanceData();
        }
    }



    public static List<GridObjectInstance> GetGridObjects()
    {
        CheckData();
        //RefreshGridObjectInstanceData();

        return gridObjectInstances;
    }

    public static void AddGridObjectInstance(GridObjectInstance x)
    {
        CheckData();
        if (!gridObjectInstances.Contains(x))
        {
            gridObjectInstances.Add(x);
            SaveGridObjectInstances();
        }
    }

    public static void RemoveGridObjectInstance(GridObjectInstance x)
    {
        CheckData();

        gridObjectInstances.Remove(x);
        SaveGridObjectInstances();
    }

    public static void SaveData()
    {
        CheckData();
        SaveGridObjectInstances();
    }

    private static void CheckData()
    {
        //MakeNewList();
        //return;
        //Debug.Log("CheckingData");
        LoadDatas();
        if (gridObjectInstances == null)
        {

            Debug.Log("Attempting to load records");

            if (File.Exists(Application.persistentDataPath + "/GridObjectInstances.json"))
            {

                LoadGridObjectInstances();
                //RefreshGridObjectInstanceData();
                //RefreshGridObjectInstance();
                //Debug.Log("GridObject Records loaded, " + gridObjectInstances.Count + " found.");
            }
            else
            {
                MakeNewList();
                //Debug.Log("No records found, making new");
            }

        }
        //RefreshGridObjectInstanceData();
        //DeleteAllData();
    }



    // Method to save the list of GridObjectData to a file
    public static void SaveGridObjectInstances()
    {
        string data = JsonUtility.ToJson(new GridObjectsSave(gridObjectInstances));

        string filePath = Application.persistentDataPath + "/GridObjectInstances.json";
        File.WriteAllText(filePath, data);
    }

    // Method to load the list of GridObjectData from a file
    public static void LoadGridObjectInstances()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/GridObjectInstances.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            GridObjectsSave serializableList = JsonUtility.FromJson<GridObjectsSave>(data);
            if (serializableList == null) Debug.Log("null save");
            gridObjectInstances = serializableList.gridObjects;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            gridObjectInstances = new List<GridObjectInstance>();
        }
        
    }

   
    private static void MakeNewList()
    {

        gridObjectInstances = new List<GridObjectInstance>();
        SaveGridObjectInstances();
    }
    public static InventoryObjectData GetDataFromID(int x)
    {
        if (inventoryObjectDatas == null)
        {
            LoadDatas();
        }
        if (x == -1) return null;
        return inventoryObjectDatas[x];
    }
    public static int GetIDFromData(InventoryObjectData x)
    {
        if (inventoryObjectDatas == null)
        {
            LoadDatas();
        }
        return inventoryObjectDatas.IndexOf(x);
    }


    public static FriendshipReqData GetFReqFromID(int x)
    {
        if (freqDatas == null)
        {
            LoadDatas();
        }
        if (x == -1) return null;
        return freqDatas[x];
    }
    public static int GetIDFromFReq(FriendshipReqData x)
    {
        if (freqDatas == null)
        {
            LoadDatas();
        }
        return freqDatas.IndexOf(x);
    }
}
