using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static SaveData theSave;
    private static bool initialized = false;
    private static string saveDataName = "SaveData";
    private static int screamCoinLimit = 100000;
    private static string filePath;
    private static int currentLevel = -1;

    public static event IncreasedCount OnIncreasedCount;
    public delegate void IncreasedCount(InventoryObjectData x);

    public static event HouseColourChanged OnHouseColourChanged;
    public delegate void HouseColourChanged(int x);

    public static event HouseSizeChanged OnHouseSizeChanged;
    public delegate void HouseSizeChanged();

    public static string shaderName = "URP/OToonCustomLit";

    public static void IncreaseCount(InventoryObjectData x)
    {
        InitSaveSystem();
        GetCounter().IncreaseCount(x);
        OnIncreasedCount?.Invoke(x);

        Save(theSave);
    }
    public static void PreventCount(InventoryObjectData x)
    {
        InitSaveSystem();
        GetCounter().PreventCount(x);


        Save(theSave);
    }

    public static int GetCurrentDesktopWallpaper()
    {
        InitSaveSystem();
        return theSave.currentDesktopWallpaperIndex;
    }

    public static void SetCurrentDesktopWallpaper(int x)
    {
        InitSaveSystem();
        theSave.currentDesktopWallpaperIndex = x;
        Save(theSave);
    }

    public static void InitSaveSystem()
    {
        if (!initialized)
        {
            filePath = GetSavePath(saveDataName);
            theSave = Load();
            if (theSave == null)
            {
                Debug.Log("making new savesystem save");
                theSave = new SaveData();
                theSave.invObC.MakeNewRecords();
                //MakeNewRecords();
                /* testWindow = new WindowInstance();
                testWindow.windowName = "Default";
                testWindow.height = 0.5f;
                testWindow.xPos = 0.5f;
                theSave.houseWindowsY1.Add(testWindow);
                theSave.houseWindowsX1.Add(testWindow);
                theSave.houseWindowsX2.Add(testWindow);


                WindowInstance testDoor = new WindowInstance();
                testDoor.windowName = "Doorway";
                testDoor.height = 0f;
                testDoor.xPos = 0.5f;
                theSave.houseWindowsY2.Add(testDoor);*/
                for(int num = 0; num <4; num++)
                {
                    theSave.houseMaterials[num] = "DEFAULT";
                    theSave.houseColours[num]= "DEFAULT";
                }


                Save(theSave);
            }
            
            initialized = true;
         
        }
    }

    public static InventoryObjectCounter GetCounter()
    {
        InitSaveSystem();
        return theSave.invObC;
    }

    public static int GetScreamcoin()
    {
        InitSaveSystem();
        return theSave.screamCoinAmount;
    }

    public static bool SubtractScreamcoin(int x)
    {
        InitSaveSystem();
        bool temp = false;
        theSave.screamCoinAmount = theSave.screamCoinAmount - x;

        if(x < 0)
        {
            x = 0;
        }
        else
        {
            temp = true;
        }
        Save(theSave);
        return temp;
    }

    public static bool Save()
    {
        return Save(theSave);
    }

    public static bool AddScreamcoin(int x)
    {
        InitSaveSystem();
        bool temp = false;
        theSave.screamCoinAmount = theSave.screamCoinAmount + x;

        if (x > screamCoinLimit)
        {
            x = screamCoinLimit;

        }
        else
        {
            temp = true;
        }
        Save(theSave);
        return temp;
    }

    public static bool Save(SaveData data)
    {
        try
        {
            string jsonData = JsonUtility.ToJson(data);
            File.WriteAllText(filePath, jsonData);
            Debug.Log("Data saved successfully.");
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to save data: " + e.Message);
            return false;
        }
    }

    public static SaveData Load()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string jsonData = File.ReadAllText(filePath);
                SaveData data = JsonUtility.FromJson<SaveData>(jsonData);
                //Debug.Log("Data loaded successfully.");
                return data;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to load data: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found: " + filePath);
        }

        return null;
    }

    public static bool DeleteSave()
    {
        if (File.Exists(filePath))
        {
            try
            {
                File.Delete(filePath);
                Debug.Log("Save data deleted successfully.");
                return true;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to delete save data: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Save file not found: " + filePath);
        }

        return false;
    }

    public static bool SaveExists()
    {
        return File.Exists(filePath);
    }

    public static string[] GetSavesInDirectory(string directoryPath)
    {
        if (Directory.Exists(directoryPath))
        {
            try
            {
                string[] saveFiles = Directory.GetFiles(directoryPath, "*.json");
                return saveFiles;
            }
            catch (System.Exception e)
            {
                Debug.LogError("Failed to get saves in directory: " + e.Message);
            }
        }
        else
        {
            Debug.LogWarning("Directory not found: " + directoryPath);
        }

        return new string[0];
    }
    private static string GetSavePath(string name)
    {
        //Debug.Log(Path.Combine(Application.persistentDataPath, name + ".sav"));
        return Path.Combine(Application.persistentDataPath, name);
    }

    public static List<int> levelBoundaries = new List<int> { 10, 30, 60, 90, 120, 150, 200,230,260,300 };

    public static List<int> maxWallSizeX = new List<int> { 10,11, 12,13, 14,15, 16,17,18, 20 };
    public static List<int> maxWallSizeY = new List<int> { 10, 11, 12, 13, 14, 15, 16, 17, 18, 20 };

    public static List<int> maxVehicleSlots = new List<int> { 3, 3, 4, 4, 5, 5, 6,6,7,7 };

    public static List<WindowInstance> GetWindows(int x)
    {
        InitSaveSystem();

        switch (x)
        {
            case 0:
                return theSave.houseWindowsX1;
                break;
            case 1:
                return theSave.houseWindowsY1;
                break;
            case 2:
                return theSave.houseWindowsX2;
                break;
            case 3:
                return theSave.houseWindowsY2;
                break;
            default:
                break;
        }

        return null;
    }

    public static void AddWindow(int wall, WindowInstance window)
    {
        switch (wall)
        {
            case 0:
                theSave.houseWindowsX1.Add(window);
              
                break;
            case 1:
                theSave.houseWindowsY1.Add(window);
            
                break;
            case 2:
                theSave.houseWindowsX2.Add(window);
                
                break;
            case 3:
                theSave.houseWindowsY2.Add(window);
                
                break;
            default:
                break;
        }
        Save(theSave);
    }
    public static void RemoveWindow(int wall, WindowInstance window)
    {
        switch (wall)
        {
            case 0:
                theSave.houseWindowsX1.Remove(window);
                
                break;
            case 1:
                theSave.houseWindowsY1.Remove(window);
           
                
                break;
            case 2:
                theSave.houseWindowsX2.Remove(window);
              
                
                break;
            case 3:
                theSave.houseWindowsY2.Remove(window);
           
                
                break;
            default:
                break;
        }
        Save(theSave);
        
    }

    public static void SetHouseColour(UnlockableColour x, int y)
    {
        InitSaveSystem();
        theSave.houseColours[y] = x.name;
        
        Save(theSave);
        OnHouseColourChanged?.Invoke(y);
    }

    public static void SetHouseMaterial(UnlockableMaterial x, int y)
    {
        InitSaveSystem();
        theSave.houseMaterials[y] = x.name;

        Save(theSave);
        OnHouseColourChanged?.Invoke(y);
    }

    public static UnlockableColour GetHouseColour(int y)
    {
        InitSaveSystem();

        //Debug.Log(theSave.houseColours.Count);

        //if(Resources.Load<UnlockableColour>("UnlockableColours/" + theSave.houseColours[y]) != null)
        return Resources.Load<UnlockableColour>("UnlockableColours/" + theSave.houseColours[y]);
       
        //return new Color(theSave.houseColoursR[y], theSave.houseColoursG[y], theSave.houseColoursB[y]);
    }//0 = wall colour, 1 = floor colour, 2 = door colour, 3 = skirting colour

    public static UnlockableMaterial GetHouseMaterial(int y)
    {
        InitSaveSystem();

        //if (theSave.houseMaterials[y]) == null) Debug.Log("sheet");
        return Resources.Load<UnlockableMaterial>("UnlockableMaterials/" + theSave.houseMaterials[y]);
        //return new Color(theSave.houseColoursR[y], theSave.houseColoursG[y], theSave.houseColoursB[y]);
    }

    public static int GetNumberOfVehicleSlots()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return theSave.currentVehicleSlots;
    }
    public static int GetVehicleSlotLimit()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return maxVehicleSlots[currentLevel];
    }
    public static int GetWallSizeLimitX()
    {
        InitSaveSystem();
        SetCurrentLevel();
        //Debug.Log(currentLevel);
        //return maxWallSizeX[currentLevel];

        return theSave.currentMaxWallSizeX;
    }
    public static int GetWallSizeLimitY()
    {
        InitSaveSystem();
        SetCurrentLevel();
        //return maxWallSizeY[currentLevel];
        return theSave.currentMaxWallSizeY;
    }

    public static bool IncreaseMaxWallSizeX()
    {
        if(GetWallSizeLimitX() >= maxWallSizeX[currentLevel]) return false;
        
        theSave.currentMaxWallSizeX++;
      
        Save(theSave);
        return true;
    }

    public static bool GetIsLevelLimitedX()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return (GetWallSizeLimitX() >= maxWallSizeX[currentLevel]);
    }
    public static bool GetIsLevelLimitedY()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return (GetWallSizeLimitY() >= maxWallSizeY[currentLevel]);
    }

    public static bool IncreaseMaxWallSizeY()
    {
        InitSaveSystem();
        SetCurrentLevel();
        if (GetWallSizeLimitY() >= maxWallSizeY[currentLevel]) return false;

        theSave.currentMaxWallSizeY++;
       
        Save(theSave);
        return true;
    }

    public static int GetWallSizeX()
    {
        InitSaveSystem();
        return theSave.currentWallSizeX;
    }
    public static int GetWallSizeY()
    {
        InitSaveSystem();
        return theSave.currentWallSizeY;
    }
    public static bool SetWallSizeX(int x)
    {
        InitSaveSystem();
        SetCurrentLevel();
        if (x <= 1 || x > GetWallSizeLimitX()) return false;

        theSave.currentWallSizeX = x;
        OnHouseSizeChanged?.Invoke();
        return true;
    }
    public static bool SetWallSizeY(int y)
    {
        InitSaveSystem();
        SetCurrentLevel();
        if (y <= 1 || y > GetWallSizeLimitY()) return false;

        theSave.currentWallSizeY = y;
        OnHouseSizeChanged?.Invoke();
        return true;
    }

    public static int GetHouseUpgradeCost()
    {
        return 10;
    }
    public static void SetCurrentLevel()
    {
        currentLevel = 0;
        for (int x = 0; x < levelBoundaries.Count; x++)
        {
            if (theSave.exp >= levelBoundaries[x])
            {
                currentLevel = x;
            }
            else
            {
                return;
            }
        }
    }

    public static float GetExpOnLevel()
    {
        InitSaveSystem();
        //if (saveData == null) InitSaveData();
        SetCurrentLevel();
        if (currentLevel == 0)
        {
            return theSave.exp;
        }
        else
        {
            return levelBoundaries[currentLevel - 1] - theSave.exp;
        }

    }

    public static float GetExpReqForNextLevel()
    {
        InitSaveSystem();
        //if (saveData == null) InitSaveData();
        SetCurrentLevel();

        if (!GetIsMaxLevel())
            return levelBoundaries[currentLevel + 1] - levelBoundaries[currentLevel];

        return -1.0f;
    }

    public static bool GetIsMaxLevel()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return (currentLevel == levelBoundaries.Count - 1);

    }

    public static int GetCurrentLevel()
    {
        InitSaveSystem();
        SetCurrentLevel();
        return currentLevel;
    }

    public static void AddExp(float x)
    {
        InitSaveSystem();
        int prevLevel = currentLevel;
        theSave.exp = theSave.exp + x;
        SetCurrentLevel();
        if (currentLevel > prevLevel)
        {
            //WE LEVELLED UP WOO
        }
        Save(theSave);
    }

    
    public static int GetMeltHouseSlots()
    {
        float avgWallSize = (GetWallSizeX() + GetWallSizeY()) / 2;

        return (Mathf.FloorToInt((avgWallSize - 10) / 3) + 2);
        //10 = 2, 13 = 3, 16 = 4, 
    }
    public static int GetFriendMeltHouseSlots()
    {
        float avgWallSize = (GetWallSizeX() + GetWallSizeY()) / 2;

        return (2 + (Mathf.FloorToInt((avgWallSize - 10) / 3) + 2));
        //10 = 2, 13 = 3, 16 = 4, 
    }
}
