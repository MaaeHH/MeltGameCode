using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnselectableMission 
{
    
    [SerializeField] private int missionDataIndex;
    [SerializeField] private long timeUntil;
  
    public UnselectableMission(MissionData x,DateTime y)
    {
  


        missionDataIndex = MissionAssigner.GetIDFromData(x);
        
        timeUntil = y.Ticks;
    }


    public MissionData GetData()
    {
        return MissionAssigner.GetDataFromID(missionDataIndex);
    }

    

    public bool IsTimeUp()
    {
        return (new DateTime(timeUntil) < DateTime.Now);
    }

}
