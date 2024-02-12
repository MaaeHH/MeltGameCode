using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class InProgressMission
{
    //private MissionData theData;
    //private List<MeltData> theMelts;
    [SerializeField] private int missionDataIndex;
    [SerializeField] private long completionTime;
    //[SerializeField] private long reSelectableTime;
    [SerializeField] private VehicleInstance vehicleOccupied;
    //[SerializeField] private List<int> meltsOccupied;
    [SerializeField] private List<string> meltsOccupied;
    [SerializeField] private List<bool> meltsOccupiedCustom;
    [SerializeField] private bool rewardsRecieved = false;

    public InProgressMission(MissionData x, VehicleInstance y,List<MeltData> z)
    {
        meltsOccupied = new List<string>();
        foreach (MeltData melt in z)
        {
            MeltAssigner.PutMeltOnMission(melt, DateTime.Now.AddMinutes(x.GetDuration()));
            meltsOccupied.Add(melt.name);

        }

        MissionAssigner.MakeMissionUnselectableUntil(x, DateTime.Now.AddHours(UnityEngine.Random.Range(24, 48)));

        

        //meltsOccupied = MeltAssigner.GetMeltIndexes(z);
        //meltsOccupied = MeltAssigner.GetMeltNames(z);

       

        //theMelts = z;
        completionTime = DateTime.Now.AddMinutes(x.GetDuration()).Ticks;

        missionDataIndex = MissionAssigner.GetIDFromData(x);
        vehicleOccupied = y;
        y.SetMission(this);

        SaveVehicleScript.SaveData();
    }


    public MissionData GetData()
    {
        return MissionAssigner.GetDataFromID(missionDataIndex);
    }

    public bool GetRewardsRecieved()
    {
        return rewardsRecieved;
    }
    public void SetRewardsRecieved(bool x)
    {
        rewardsRecieved = true;
    }

    public DateTime GetCompletionTime()
    {
        return new DateTime(completionTime);
    }

    public TimeSpan GetTimeRemaining()
    {
        return new DateTime(completionTime) - DateTime.Now;
    }

    public VehicleInstance GetVehicle()
    {
        return vehicleOccupied;
    }

    public void SetOccupiedMelts(List<MeltData> x)
    {
        meltsOccupied = new List<string>();
        foreach (MeltData melt in x)
        {
            meltsOccupied.Add(melt.name);
        }
            //meltsOccupied = MeltAssigner.GetMeltIndexes(x);
    }

    public List<MeltData> GetOccupiedMelts()
    {
        List<MeltData> theList = new List<MeltData>();

        for(int i = 0; i < meltsOccupied.Count; i++)
        {
            MeltAssigner.GetDataFromName(meltsOccupied[i], meltsOccupiedCustom[i]);
        }
        return theList;
        //return MeltAssigner.GetMeltsFromIndexes(meltsOccupied);
    }
}
