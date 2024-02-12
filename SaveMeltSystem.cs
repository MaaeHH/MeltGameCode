using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveMeltSystem
{
    public static bool SaveMelt(string meltID, MeltSave saveGame)
    {
        string name = meltID.ToString();
        string savePath = GetSavePath(name);

        try
        {
            string json = JsonUtility.ToJson(saveGame);
            File.WriteAllText(savePath, json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save Melt: " + e.Message);
            return false;
        }
    }

    public static MeltSave LoadMelt(string meltID)
    {
        string name = meltID.ToString();
        string savePath = GetSavePath(name);

        if (!DoesSaveExist(savePath))
        {
            //Debug.Log("File not found");
            return null;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            MeltSave myData = JsonUtility.FromJson<MeltSave>(json);
            //Debug.Log("Data found");
            return myData;
        }
        catch (Exception e)
        {
            Debug.LogError("Exception/Data not found: " + e.Message);
            return null;
        }
    }




    public static bool SaveFriendshipReq(string meltID, FriendshipRequirement saveGame)
    {
        string name = meltID.ToString();
        string savePath = GetSavePath(name);

        try
        {
            string json = JsonUtility.ToJson(saveGame);
            File.WriteAllText(savePath, json);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save FriendshipRequirement: " + e.Message);
            return false;
        }
    }

    public static FriendshipRequirement LoadFriendshipReq(string meltID)
    {
        string name = meltID.ToString();
        string savePath = GetSavePath(name);

        if (!DoesSaveExist(savePath))
        {
            return null;
        }

        try
        {
            string json = File.ReadAllText(savePath);
            FriendshipRequirement myData = JsonUtility.FromJson<FriendshipRequirement>(json);
            return myData;
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static bool DeleteMeltSave(int meltID)
    {
        string name = meltID.ToString();
        string savePath = GetSavePath(name);

        try
        {
            File.Delete(savePath);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    private static string GetSavePath(string name)
    {
        return Path.Combine(Application.persistentDataPath, name + ".json");
    }

    private static bool DoesSaveExist(string path)
    {
        return File.Exists(path);
    }

    public static string GetVehicleSaveDir()
    {
        string dir = Application.persistentDataPath;
        Directory.CreateDirectory(dir);
        return dir;
    }

    public static string[] GetAllSavesInFile()
    {
        DirectoryInfo di = new DirectoryInfo(GetVehicleSaveDir());
        FileInfo[] files = di.GetFiles("*.json");
        string[] filestrings = new string[files.Length];

        for (int x = 0; x < files.Length; x++)
        {
            filestrings[x] = Path.GetFileNameWithoutExtension(files[x].Name);
        }

        return filestrings;
    }
}
