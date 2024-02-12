using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class CustomMeltLoader 
{

  
    private static List<MeltData> meltDataList;

    private static bool needReload = true;

    //private static string GetPath(string )
    //Path.Combine(Application.persistentDataPath, name + "*.txt");
   /* public static void LoadMeltData()
    {
        Debug.Log(Application.persistentDataPath + "/CustomMelts/", "*.txt");
        meltDataList = new List<MeltData>();
        foreach (string filePath in Directory.GetFiles(Application.persistentDataPath + "/CustomMelts/", "*.txt"))
        {
            string json = File.ReadAllText(filePath);
            CustomMeltSave serializableMeltData = JsonUtility.FromJson<CustomMeltSave>(json);

            if (serializableMeltData != null)
            {
                MeltData meltData = serializableMeltData.ToMeltData();
                meltDataList.Add(meltData);
            }
        }
    }*/
   private static string GetCustomPath()
    {
        return Application.persistentDataPath + "/CustomMelts/";
    }

    public static void LoadMeltData()
    {
        string customMeltsPath = GetCustomPath();

        // Check if the directory exists, if not, create it
        if (!Directory.Exists(customMeltsPath))
        {
            Directory.CreateDirectory(customMeltsPath);
        }

        Debug.Log(customMeltsPath);
        meltDataList = new List<MeltData>();

        foreach (string filePath in Directory.GetFiles(customMeltsPath, "*.txt"))
        {
            string json = File.ReadAllText(filePath);
            CustomMeltSave serializableMeltData = JsonUtility.FromJson<CustomMeltSave>(json);

            if (serializableMeltData != null)
            {
                MeltData meltData = serializableMeltData.ToMeltData();
                meltDataList.Add(meltData);
            }
        }
    }

    public static void SaveMeltData(MeltData meltData)
    {
        needReload = true;
        CustomMeltSave serializableMeltData = CustomMeltSave.FromMeltData(meltData);
        string json = JsonUtility.ToJson(serializableMeltData);
        File.WriteAllText(Path.Combine(GetCustomPath(), $"{meltData.name}.txt"), json);
    }

    public static void SaveMeltSave(CustomMeltSave serializableMeltData)
    {
        needReload = true;
        //CustomMeltSave serializableMeltData = CustomMeltSave.FromMeltData(meltData);
        string json = JsonUtility.ToJson(serializableMeltData);
        File.WriteAllText(Path.Combine(GetCustomPath(), $"{serializableMeltData.meltName}.txt"), json);
    }

    public static List<MeltData> GetCustomMelts()
    {
        if (needReload)
            LoadMeltData();
        needReload = false;
        return meltDataList;
    }
    
    public static MeltData LoadMelt(string x)
    {

        string json = File.ReadAllText(GetCustomPath() + x + ".txt");
        CustomMeltSave myData = JsonUtility.FromJson<CustomMeltSave>(json);
        return myData.ToMeltData();
    }
}
