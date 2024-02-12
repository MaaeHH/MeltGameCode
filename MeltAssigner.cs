using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;

[System.Serializable]
public static class MeltAssigner
{
    [SerializeField] private static List<MeltRecord> meltRecords = null;
    [SerializeField] private static List<MeltRecord> meltRecordsCustom = null;
    //[SerializeField] private SceneLoader sl;


    private static string saveFileName = "MeltRecordSave.dat";
    public static float noMeltChance = 0.1f;

    //private static List<MeltData> meltDatas = null;

    private static void MakeNewList()
    {
        MeltData[] meltDataObjects = Resources.LoadAll<MeltData>("MeltDatas");
        meltRecords = new List<MeltRecord>();
        meltRecordsCustom = new List<MeltRecord>();

        // You can now access and use the found Scriptable Objects
        foreach (MeltData meltData in meltDataObjects)
        {
            //Debug.Log(meltData.GetName());
            meltRecords.Add(new MeltRecord(meltData, false));

        }
        foreach (MeltData customMeltData in CustomMeltLoader.GetCustomMelts())
        {
            meltRecordsCustom.Add(new MeltRecord(customMeltData, true));
        }
        SaveMeltRecords();
    }
    
    public static bool MakeCustomMeltRecord(MeltData theData)
    {
        CheckData();
        foreach (MeltRecord mr in meltRecordsCustom)
        {

            if (mr.GetDataName() == theData.name)
            {
                return false;
            }


        }
        meltRecordsCustom.Add(new MeltRecord(theData, true));
        SaveMeltRecords();
        return true;
    }

    public static bool RemoveRecord(MeltData theData)
    {
        CheckData();
        MeltRecord toRemove = null;
        foreach (MeltRecord mr in meltRecordsCustom)
        {

            if (mr.GetDataName() == theData.name)
            {
               toRemove = mr;
                break;
            }


        }
        if(toRemove != null) {
            meltRecordsCustom.Remove(toRemove);
            return true;
        }
        else
        {
            return false;
        }
       
    }

    /*private static void LoadDatas(bool custom)
    {
        meltDatas = new List<MeltData>();
        MeltData[] meltDataObjects = Resources.LoadAll<MeltData>("MeltDatas");
        foreach (MeltData customMeltData in CustomMeltLoader.GetCustomMelts())
        {
            meltDatas.Add(customMeltData);
        }

        // You can now access and use the found Scriptable Objects
        foreach (MeltData meltData in meltDataObjects)
        {
            //Debug.Log(meltData.GetName());
            meltDatas.Add(meltData);

        }

    }*/
    
    private static MeltData LoadData(string x, bool custom)
    {
        if (custom)
        {
            return CustomMeltLoader.LoadMelt(x);
        }
        else
        {
            return Resources.Load<MeltData>("MeltDatas/" +x);
        }
     }


    public static MeltData GetDataFromName(string x, bool custom)
    {
        /*if (meltDatas == null)
        {
            LoadDatas(custom);
        }*/
        if (x == "") return null;

        return LoadData(x, custom);

        //return meltDatas[x];
    }
    /*public static int GetNameFromData(MeltData x)
    {
        if (meltDatas == null)
        {
            LoadDatas();
        }
        if (x == null) return -1;
        return meltDatas.IndexOf(x);
    }*/

    /*public static List<int> GetMeltIndexes(List<MeltData> x)
    {
        List<int> intsOut = new List<int>();
        foreach (MeltData data in x)
        {
            intsOut.Add(GetIDFromData(data));
        }
        return intsOut;
    }

    public static List<MeltData> GetMeltsFromIndexes(List<int> x)
    {
        List<MeltData> dataOut = new List<MeltData>();
        foreach (int data in x)
        {
            dataOut.Add(GetDataFromID(data));
        }
        return dataOut;
    }*/

    public static List<MeltData> GetMissionSelectableMelts()
    {
        CheckData();
        List<MeltData> outMelts = new List<MeltData>();
        foreach (MeltRecord record in meltRecords)
        {
            if (((record.GetTime() < DateTime.Now) && (record.GetLocation() == MeltRecord.Location.Mission))
                || record.GetTime() == new DateTime() || record.GetLocation() != MeltRecord.Location.Mission)
            {
                if (record.GetData() != null)
                if (record.GetData().IsFriend())
                outMelts.Add(record.GetData());
            }

        }

        foreach (MeltRecord record in meltRecordsCustom)
        {
            if (((record.GetTime() < DateTime.Now) && (record.GetLocation() == MeltRecord.Location.Mission))
                || record.GetTime() == new DateTime() || record.GetLocation() != MeltRecord.Location.Mission)
            {

                if (record.GetData() != null)
                    outMelts.Add(record.GetData());
            }

        }

        return outMelts;
    }

    public static bool PutMeltOnMission(MeltData theMelt, DateTime missionEndingTime)
    {
        CheckData();
        foreach (MeltRecord record in meltRecords)
        {
            if (((record.GetTime() < DateTime.Now) && (record.GetLocation() == MeltRecord.Location.Mission))
                || record.GetTime() == new DateTime() || record.GetLocation() != MeltRecord.Location.Mission)
            {

                if (record.GetData() == theMelt)
                {
                    record.SetTime(missionEndingTime);
                    record.SetLocation(MeltRecord.Location.Mission);
                    return true;
                }
            }

        }

        foreach (MeltRecord record in meltRecordsCustom)
        {
            if (((record.GetTime() < DateTime.Now) && (record.GetLocation() == MeltRecord.Location.Mission))
                || record.GetTime() == new DateTime() || record.GetLocation() != MeltRecord.Location.Mission)
            {

                if (record.GetData() == theMelt)
                {
                    record.SetTime(missionEndingTime);
                    record.SetLocation(MeltRecord.Location.Mission);
                    return true;
                }
            }

        }


        return false;
    }

    public static List<MeltData> GetCurrentMelts(MeltRecord.Location x, int amount)
    {
       
        CheckData();

        List<MeltData> temp = new List<MeltData>();
        List<MeltRecord> toRemove = new List<MeltRecord>();
        foreach (MeltRecord record in meltRecords)
        {
            if (!record.GetLock())
            {
                if (record.GetTime() > DateTime.Now)
                {
                    if (record.GetLocation() == x)
                    {
                        temp.Add(record.GetData());
                    }
                }
                else
                {
                    record.SetLocation(MeltRecord.Location.None);


                    if (record.GetData() == null)
                    {
                        toRemove.Add(record);

                    }
                }
            }
        }

        foreach (MeltRecord record in toRemove)
        {
            meltRecords.Remove(record);
        }
        //Debug.Break();

        List<MeltData> newPickedMelts = new List<MeltData>();
        MeltData newPickedMelt;
        for (int i = temp.Count; i < amount; i++)
        {
            newPickedMelt = PickMelt(x);
            temp.Add(newPickedMelt);
            if (newPickedMelt != null) newPickedMelts.Add(newPickedMelt);
        }

        SaveMeltRecords();

        if (x == MeltRecord.Location.House && newPickedMelts.Count != 0)
        {
            DoorScenePassover.newMelts = newPickedMelts;
            SceneManager.LoadScene("DoorwayScene");
        }
        return temp;
    }


    private static MeltData PickMelt(MeltRecord.Location x)
    {
        CheckData();
        MeltRecord currentRecord = null;
        List<MeltRecord> tempList = new List<MeltRecord>();
        if (UnityEngine.Random.Range(0, 1.0f) > noMeltChance)
        {
            float total = 0;
            foreach (MeltRecord record in meltRecords)
            {
                Debug.Log("check A");
                if(record.GetData() != null)
                {
                    Debug.Log("check B");
                    if (x == MeltRecord.Location.House && !record.GetData().IsFriend() || x != MeltRecord.Location.House)
                    {// A melt that visits home cannot be an already friended melt
                        Debug.Log("check C");
                        if (!record.GetLock())
                        {//If they aren't locked to a location
                            if (((record.GetTime() < DateTime.Now) && (record.GetLocation() == MeltRecord.Location.None))
                                || record.GetTime() == new DateTime())//If they aren't busy somewhere else
                            {
                                total += record.GetData().GetAppearanceTimes()[DateTime.Now.Hour];
                                tempList.Add(record);
                            }

                        }
                    }
                }
                
            }//Here we decide which melts to put in the pool to pick from.



            float randomNum = UnityEngine.Random.Range(0, total);

            foreach (MeltRecord record in tempList)
            {
                if (currentRecord == null)
                {
                    randomNum = randomNum - record.GetData().GetAppearanceTimes()[DateTime.Now.Hour];

                    if (randomNum <= 0)
                    {

                        currentRecord = record;


                    }

                }
            }


        }
        else
        {
            currentRecord = new MeltRecord(null, false);
            meltRecords.Add(currentRecord);
        }

        if (currentRecord != null)
        {
            currentRecord.SetLocation(x);
            currentRecord.SetTime(DateTime.Now.AddHours(UnityEngine.Random.Range(1, 4)));
            //currentRecord.SetTime(DateTime.Now.AddMinutes(1));
            return currentRecord.GetData();
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
        if (meltRecords == null || meltRecords.Count == 0)
        {

            //Debug.Log("Attempting to load records");

            if (File.Exists(Application.persistentDataPath + "/MeltRecords.json"))
            {

                LoadMeltRecords();
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
    public static void SaveMeltRecords()
    {
        string data = JsonUtility.ToJson(new MeltAssignerSave(meltRecords, meltRecordsCustom));
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/MeltRecords.json";
        File.WriteAllText(filePath, data);
    }

    // Method to load the list of GridObjectData from a file
    public static void LoadMeltRecords()
    {
        //string filePath = Path.Combine(Application.persistentDataPath, fileName);
        string filePath = Application.persistentDataPath + "/MeltRecords.json";
        if (File.Exists(filePath))
        {
            string data = File.ReadAllText(filePath);
            MeltAssignerSave serializableList = JsonUtility.FromJson<MeltAssignerSave>(data);
            if (serializableList == null) Debug.Log("null save");
            meltRecords = serializableList.meltRecords;
            meltRecordsCustom = serializableList.meltRecordsCustom;
        }
        else
        {
            Debug.LogWarning("File not found: " + filePath);
            meltRecords = new List<MeltRecord>();
        }
    }


    public static List<MeltData> GetAllFriendMeltNoMission()
    {
        CheckData();
        List<MeltData> tempList = new List<MeltData>();
        foreach (MeltRecord record in meltRecords)
        {
            if (record.GetData() != null)
            {
                if (record.GetData().IsFriend() && record.GetLocation() != MeltRecord.Location.Mission)
                {
                    tempList.Add(record.GetData());
                }
            }
        }
        return tempList;
    }

    public static List<MeltData> GetLockedMelts(MeltRecord.Location x)
    {
        CheckData();
        List<MeltData> tempList = new List<MeltData>();
        foreach (MeltRecord record in meltRecords)
        {
            if (record.GetLock() && record.GetLocation() == x)
            {

                tempList.Add(record.GetData());
                
            }
        }
        foreach (MeltRecord record in meltRecordsCustom)
        {
            if (record.GetLock() && record.GetLocation() == x)
            {

                tempList.Add(record.GetData());

            }
        }
        return tempList;
    }

    public static void LockMeltToLocation(MeltData x, MeltRecord.Location y)
    {
        CheckData();
        x.SetRestoreLastUpdate(true);
        foreach (MeltRecord record in meltRecords)
        {
            if (record.GetData() == x)
            {
                record.SetLocation(y);
                record.SetLock(true);
                record.SetTime(new DateTime());
                SaveMeltRecords();
                return;
            }
        }
        foreach (MeltRecord record in meltRecordsCustom)
        {
            if (record.GetData() == x)
            {
                record.SetLocation(y);
                record.SetLock(true);
                record.SetTime(new DateTime());
                SaveMeltRecords();
                return;
            }
        }

    }

    public static void UnlockMelt(MeltData x)
    {
        CheckData();
        x.SetRestoreLastUpdate(false);
        foreach (MeltRecord record in meltRecords)
        {
            if (record.GetData() == x)
            {
                
                record.SetLock(false);
                record.SetTime(new DateTime());
                SaveMeltRecords();
                return;
            }
        }
        foreach (MeltRecord record in meltRecordsCustom)
        {
            if (record.GetData() == x)
            {

                record.SetLock(false);
                record.SetTime(new DateTime());
                SaveMeltRecords();
                return;
            }
        }
    }

    public static void UnFriendMelt(MeltData x)
    {
        CheckData();
        UnlockMelt(x);
        MeltRecord asd = new MeltRecord(null, false);
        meltRecords.Add(asd);
        asd.SetLocation(MeltRecord.Location.House);
        asd.SetTime(DateTime.Now.AddHours(UnityEngine.Random.Range(1, 4)));
        foreach (MeltRecord record in meltRecords)
        {
            if (record.GetData() == x)
            {
                record.SetTime(DateTime.Now);
                break;
            }
        }
        SaveMeltRecords();
    }
}
