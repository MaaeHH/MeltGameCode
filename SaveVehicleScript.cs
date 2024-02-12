using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveVehicleScript
{
    private static List<VehicleData> vehicleDatas;
    private static List<VehicleInstance> vehicleInstances;
    private static int noSlots = 3;
    private static void LoadDatas()
    {
        if (vehicleDatas == null)
        {
            
                vehicleDatas = new List<VehicleData>();
                VehicleData[] vehicleDataObjects = Resources.LoadAll<VehicleData>("VehicleDatas");


                // You can now access and use the found Scriptable Objects
                foreach (VehicleData vehicleData in vehicleDataObjects)
                {
                    //Debug.Log(vehicleData.GetName());
                    vehicleDatas.Add(vehicleData);

                }
            //RefreshVehicleInstanceData();
        }
    }



    public static List<VehicleInstance> GetVehicles()
    {
        CheckData();
        //RefreshVehicleInstanceData();

        return vehicleInstances;
    }

    public static bool AddVehicle(VehicleData x)
    {
        CheckData();
        if(vehicleInstances.Count < noSlots)
        {
            VehicleInstance newInstance = new VehicleInstance(x);
            newInstance.SetData(x);
            vehicleInstances.Add(newInstance);
            SaveVehicleInstances();
            return true;
        }
        return false;
    }

    public static void SaveData()
    {
        CheckData();
        SaveVehicleInstances();
    }

    private static void CheckData()
    {
        //MakeNewList();
        //return;
        //Debug.Log("CheckingData");
        LoadDatas();
        if (vehicleInstances == null)
        {

            Debug.Log("Attempting to load records");

            if (File.Exists(Application.persistentDataPath + "/VehicleInstances.json"))
            {

                LoadVehicleInstances();
                //RefreshVehicleInstanceData();
                //RefreshVehicleData();
                Debug.Log("Vehicle Records loaded, " + vehicleInstances.Count + " found.");
            }
            else
            {
                MakeNewList();
                Debug.Log("No records found, making new");
            }
           
        }
        //RefreshVehicleInstanceData();
        //DeleteAllData();
    }



    // Method to save the list of GridObjectData to a file
    public static void SaveVehicleInstances()
    {
        string data = JsonUtility.ToJson(new VehiclesSave(vehicleInstances));
        
        string filePath = Application.persistentDataPath + "/VehicleInstances.json";
        File.WriteAllText(filePath, data);
    }

    // Method to load the list of GridObjectData from a file
    public static void LoadVehicleInstances()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/VehicleInstances.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            VehiclesSave serializableList = JsonUtility.FromJson<VehiclesSave>(data);
            if (serializableList == null) Debug.Log("null save");
            vehicleInstances = serializableList.vehicleInstances;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            vehicleInstances = new List<VehicleInstance>();
        }
        CheckVehicleMissions();
    }

    private static void CheckVehicleMissions()
    {
        foreach (VehicleInstance instance in vehicleInstances)
        {
            if(instance.GetMission() != null)
            {
                if(instance.GetMission().GetTimeRemaining() < TimeSpan.Zero)
                {
                    instance.SetMission(null);
                }
            }
        }
        SaveVehicleInstances();
    }

    private static void MakeNewList()
    {
        
        vehicleInstances = new List<VehicleInstance>();
        SaveVehicleInstances();
    }
    public static VehicleData GetDataFromID(int x)
    {
        if (vehicleDatas == null)
        {
            LoadDatas();
        }
        if (x == -1) return null;
        return vehicleDatas[x];
    }
    public static int GetIDFromData(VehicleData x)
    {
        if (vehicleDatas == null)
        {
            LoadDatas();
        }
        return vehicleDatas.IndexOf(x);
    }
}
