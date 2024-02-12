using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public static class SaveColours
{
    //[SerializeField] private static ColoursMenu cm;
    private static List<string> unlockedColours;
    private static List<string> unlockedTextures;
    private static List<WindowInstance> windowInventory;

    
    // Start is called before the first frame update
    private static void MakeNewList()
    {
        unlockedColours = new List<string>();
        unlockedTextures = new List<string>();
        windowInventory = new List<WindowInstance>();


        Save();
    }

    private static void CheckData()
    {
        //MakeNewList();
        //return;
        //Debug.Log("CheckingData");
        if (unlockedColours == null || unlockedColours.Count == 0)
        {

            //Debug.Log("Attempting to load records");

            if (File.Exists(Application.persistentDataPath + "/UnlockedColours.json"))
            {

                Load();
                //Debug.Log("Melt Records loaded, " + meltRecords.Count + " found.");
            }
            else
            {
                MakeNewList();
                //Debug.Log("No records found, making new");
            }

        }
        //DeleteAllData();
    }


    // Method to save the list of GridObjectData to a file
    public static void Save()
    {
        string data = JsonUtility.ToJson(new ColoursSave(unlockedColours, unlockedTextures, windowInventory));
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/UnlockedColours.json";
        File.WriteAllText(filePath, data);
    }

    // Method to load the list of GridObjectData from a file
    public static void Load()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/UnlockedColours.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            ColoursSave serializableList = JsonUtility.FromJson<ColoursSave>(data);
            if (serializableList == null) Debug.Log("null save");
            unlockedColours = serializableList.unlockedColours;
            unlockedTextures = serializableList.unlockedTextures;
            windowInventory = serializableList.windowInventory;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            unlockedColours = new List<string>();
            unlockedTextures = new List<string>();
            windowInventory = new List<WindowInstance>();
        }
    }

    public static bool UnlockColour(UnlockableColour newCol)
    {
        CheckData();

        foreach(string str in unlockedColours)
        {
            if (str == newCol.name) return false;
        }
        unlockedColours.Add(newCol.name);
        Save();
        return true;
    }
    public static bool IsUnlocked(UnlockableColour newCol)
    {
        CheckData();
        return unlockedColours.Contains(newCol.name);
    }
    public static bool IsUnlocked(UnlockableMaterial newMat)
    {
        CheckData();
        return unlockedTextures.Contains(newMat.name);
    }
    public static bool UnlockMaterial(UnlockableMaterial newMat)
    {
        CheckData();

        foreach (string str in unlockedTextures)
        {
            if (str == newMat.name) return false;
        }
        unlockedTextures.Add(newMat.name);
        Save();
        return true;
    }


    public static List<UnlockableColour> GetUnlockedColours()
    {
        CheckData();
        List<UnlockableColour> outList = new List<UnlockableColour>();
        foreach (string str in unlockedColours)
        {
            outList.Add(Resources.Load<UnlockableColour>("UnlockableColours/" + str));
        }
        return outList;
    }

    public static List<UnlockableMaterial> GetUnlockedMaterials()
    {
        CheckData();
        List<UnlockableMaterial> outList = new List<UnlockableMaterial>();
        foreach(string str in unlockedTextures)
        {
            outList.Add(Resources.Load<UnlockableMaterial>("UnlockableMaterials/" + str));
        }
        return outList;
    }

    public static void AddWindow(WindowInstance x)
    {
        CheckData();
        windowInventory.Add(x);
        Save();
    }

    public static int GetCountOf(WindowData x)
    {
        int count = 0;
        CheckData();
        foreach (WindowInstance ins in windowInventory)
        {
            if(ins.GetData().winObj == x.winObj)
            {
                count++;
            }
        }
        return count;
    }

    public static List<WindowInstance> GetInventory()
    {
        CheckData();
        return windowInventory;
    }

    public static bool RemoveWindow(WindowInstance x)
    {
        CheckData();
        WindowInstance toRemove = null;
        foreach(WindowInstance winIns in windowInventory)
        {
            if(winIns.GetData().name == x.GetData().name)
            {
                if(winIns.height == x.height)
                {
                    if(winIns.xPos == x.xPos)
                    {
                        toRemove = winIns;
                        
                    }
                }
            }
        }
        if(toRemove == null)
        {
            return false;
        }
        windowInventory.Remove(toRemove);
        //windowInventory.Remove(x);
        Save();
        return true;
    }
}
