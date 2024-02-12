using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class MissionAssigner
{
    private static List<MissionInstance> missionRecords = null;
    private static List<InProgressMission> inProgressMissionRecords = null;
    private static List<UnselectableMission> unselectableMissionRecords = null;
    //[SerializeField] private SceneLoader sl;

    private static int playerLevel = 100;

    private static string saveFileName = "MissionInstanceSave.dat";
    public static float noMissionChance = 0.1f;

    private static List<MissionData> missionDatas = null; 

    private static void MakeNewList()
    {
        MissionData[] missionDataObjects = Resources.LoadAll<MissionData>("MissionDatas");
        missionRecords = new List<MissionInstance>();
        
        
        // You can now access and use the found Scriptable Objects
        foreach (MissionData missionData in missionDataObjects)
        {
            //Debug.Log(missionData.GetName());
            //missionRecords.Add(new MissionInstance(GetIDFromData(missionData), DateTime.Now.AddMinutes(missionData.GetExpiration())));
            MissionInstance toAdd = new MissionInstance(missionData);
            //toAdd.SetData(missionData);
            missionRecords.Add(toAdd);

        }
        SaveMissionInstances();
    }

    public static Color MissionToColour(MissionData.Rarity x)
    {
        switch (x)
        {
            case MissionData.Rarity.Common:
                return new Color(1f, 1f, 1f);
                break;

            case MissionData.Rarity.Uncommon:
                return new Color(0.4f, 1f, 0.4f);
                break;

            case MissionData.Rarity.Rare:
                return new Color(1f, 0.4f, 1f);
                break;

            case MissionData.Rarity.UltraRare:
                return new Color(1f, 0.6f, 0.2f);
                break;
        }
        //Debug.Log("asd");
        return new Color(0,0,0);
    }

    private static void LoadDatas()
    {
        missionDatas = new List<MissionData>();
        MissionData[] missionDataObjects = Resources.LoadAll<MissionData>("MissionDatas");
  

        // You can now access and use the found Scriptable Objects
        foreach (MissionData missionData in missionDataObjects)
        {
            //Debug.Log(missionData.GetName());
            missionDatas.Add(missionData);

        }
       
    }
    public static MissionData GetDataFromID(int x)
    {
        //Debug.Log("GettingData");
        if(missionDatas == null)
        {
            LoadDatas();
        }
        if (x == -1) return null;
        //if (missionDatas[x] == null) Debug.Log("Somehow null?");
        return missionDatas[x];
    }
    public static int GetIDFromData(MissionData x)
    {
        if (missionDatas == null)
        {
            LoadDatas();
        }
        return missionDatas.IndexOf(x);
    }

    public static List<InProgressMission> GetInProgressMissions()
    {
        CheckData();
        return inProgressMissionRecords;
    }

    public static void AddInProgressMission(InProgressMission x)
    {
        CheckData();
        inProgressMissionRecords.Add(x);
    }

    public static List<MissionInstance> GetCurrentMissions(int amount){
      
        //if(missionDatas == null)
        CheckData();
     
        List<MissionInstance> temp = new List<MissionInstance>();
        List<MissionInstance> toRemove = new List<MissionInstance>();
        foreach(MissionInstance record in missionRecords)
        {
            if (record.GetExpireTime() > DateTime.Now)
            {
               if(!IsMissionUnselectable(record.GetData()))
                temp.Add(record);
                
            }
            else
            {
               

                
                if (record.GetData() == null)
                {
                    toRemove.Add(record);
                    
                }
            }
        }


        foreach(MissionInstance record in toRemove)
        {
            missionRecords.Remove(record);
        }
        //Debug.Break();

        List<MissionInstance> newPickedMissions = new List<MissionInstance>();
        MissionInstance newPickedMission;
        for (int i = temp.Count; i < amount; i++)
        {
            newPickedMission = PickMission();
            temp.Add(newPickedMission);
            if(newPickedMission != null)newPickedMissions.Add(newPickedMission);
        }

        SaveMissionInstances();
       
        return temp;
    }

    public static void MakeMissionUnselectableUntil(MissionData x, DateTime y)
    {
        CheckData();
        if(unselectableMissionRecords == null)
        {
            Debug.Log("null unselectable");
        }

        unselectableMissionRecords.Add(new UnselectableMission(x, y));
    }

    private static bool IsMissionUnselectable(MissionData x)
    {
        List<UnselectableMission> toRemove = new List<UnselectableMission>();
        foreach(UnselectableMission record in unselectableMissionRecords)
        {

            if (record.IsTimeUp())
            {
                toRemove.Add(record);
            }
            else if(record.GetData() == x)
            {
                return true;
            }

            
        }

        foreach(UnselectableMission record in toRemove)
        {
            unselectableMissionRecords.Remove(record);
        }
        //SaveMissionInstances();
        return false;
    }

    private static MissionInstance PickMission()
    {
        //Debug.Log("ASEDASD");
        MissionInstance currentRecord = null;
        List<MissionInstance> tempList = new List<MissionInstance>();
        if (UnityEngine.Random.Range(0, 1.0f) > noMissionChance)
        {
            float total = 0;
            foreach (MissionInstance record in missionRecords)
            {

                if (!IsMissionUnselectable(record.GetData()))
                {

                    if (((record.GetExpireTime() < DateTime.Now) && (record.GetData().GetLevel() <= playerLevel))
                    || record.GetExpireTime() == new DateTime() && (record.GetData().GetLevel() <= playerLevel))
                    {
                        total += record.GetData().GetAppearanceTimes()[DateTime.Now.Hour];
                        tempList.Add(record);
                    }

                }
            }



            float randomNum = UnityEngine.Random.Range(0, total);
            Debug.Log("theRandomNum " + randomNum + "possible mission records: "  + tempList.Count);
            foreach (MissionInstance record in tempList)
            {
                if (currentRecord == null)
                {
                    randomNum = randomNum - record.GetData().GetAppearanceTimes()[DateTime.Now.Hour];

                    if (randomNum <= 0)
                    {

                        currentRecord = record;
                        //Debug.Log("CurrentRecordFound");

                    }

                }
            }
            if(currentRecord != null) 
            Debug.Log("current record" +  currentRecord.GetData().GetTitle());
        }
        else
        {
            currentRecord = new MissionInstance(null);
            missionRecords.Add(currentRecord);
        }

        if (currentRecord != null)
        {
            //currentRecord.SetLocation(x);
            //Debug.Log(currentRecord.GetDataIndex());
            if(currentRecord.GetData() != null)
            {
                currentRecord.SetExpireTime(DateTime.Now.AddMinutes(currentRecord.GetData().GetExpiration()));

            }
            else
            {
                currentRecord.SetExpireTime(DateTime.Now.AddHours(UnityEngine.Random.Range(1, 4)));
            }
            //currentRecord.SetExpireTime(DateTime.Now.AddMinutes(1));
            return currentRecord;
            //return GetDataFromID(currentRecord.GetDataIndex());
        }
        else
        {
            Debug.Log("Warning, null");
            return null;
        }
        
    }

    private static void CheckData()
    {
        //MakeNewList();
        //return;
        //Debug.Log("CheckingData");
        if (inProgressMissionRecords == null || 
            missionRecords == null|| 
            missionRecords.Count == 0)
        {

            Debug.Log("Attempting to load records");

            if (File.Exists(Application.persistentDataPath + "/MissionInstances.json"))
            {

                LoadMissionInstances();
              
                Debug.Log("Mission Records loaded, " + missionRecords.Count + " found.");
            }
            else
            {
                MakeNewList();
                inProgressMissionRecords = new List<InProgressMission>();
                unselectableMissionRecords = new List<UnselectableMission>();
                Debug.Log("No records found, making new");
            }
          
        }
        //DeleteAllData();
    }

    // Method to save the list of GridObjectData to a file
    public static void SaveMissionInstances()
    {
        string data = JsonUtility.ToJson(new MissionAssignerSave(missionRecords, inProgressMissionRecords, unselectableMissionRecords));
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/MissionInstances.json";
        File.WriteAllText(filePath, data);
    }

    // Method to load the list of GridObjectData from a file
    public static void LoadMissionInstances()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/MissionInstances.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            MissionAssignerSave serializableList = JsonUtility.FromJson<MissionAssignerSave>(data);
            if (serializableList == null) Debug.Log("null save");
            missionRecords = serializableList.missionRecords;
            inProgressMissionRecords = serializableList.inProgressMissionRecords;
            unselectableMissionRecords = serializableList.unselectableMissions;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            missionRecords =  new List<MissionInstance>();
            inProgressMissionRecords =  new List<InProgressMission>();
            unselectableMissionRecords = new List<UnselectableMission>();
        }
    }

}
