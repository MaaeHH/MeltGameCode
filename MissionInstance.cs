using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MissionInstance
{
    [SerializeField] private int dataIndex = -1;
    [SerializeField] private long expireTime;

    //private MissionData myData;
    /*public MissionInstance(int x)
    {
        dataIndex = x;
        //expireTime = y.Ticks;
        //expireTime = DateTime.Now.AddMinutes(x.GetExpiration()).Ticks; 
    }
    public MissionInstance(int x, DateTime y)
    {
        dataIndex = x;
        expireTime = y.Ticks;
        //expireTime = DateTime.Now.AddMinutes(x.GetExpiration()).Ticks; 
    }*/

    public MissionInstance(MissionData x, DateTime y)
    {
        SetData(x);
        expireTime = y.Ticks;
        //expireTime = DateTime.Now.AddMinutes(x.GetExpiration()).Ticks; 
    }
    public MissionInstance(MissionData x)
    {
       // dataIndex = x;

        SetData(x);
        //expireTime = y.Ticks;

        //expireTime = y.Ticks;
        if(x != null)
        {
            expireTime = DateTime.Now.AddMinutes(x.GetExpiration()).Ticks;
        }
        else
        {
            expireTime = DateTime.Now.AddHours(UnityEngine.Random.Range(1, 4)).Ticks;
        }
        
    }
    public DateTime GetExpireTime()
    {
        return new DateTime(expireTime);
    }
    public TimeSpan GetTimeUntilExpire()
    {
        return new DateTime(expireTime) - DateTime.Now;
    }
    public MissionData GetData()
    {
        return MissionAssigner.GetDataFromID(dataIndex);
        //return myData;
    }
    public void SetData(MissionData x)
    {
        dataIndex = MissionAssigner.GetIDFromData(x);
        //myData = x;
    }

    public void SetExpireTime(DateTime x)
    {
        expireTime = x.Ticks;
    }

    /*public int GetDataIndex()
    {
        return dataIndex;
    }

    
    public void SetDataIndex(int x)
    {
        dataIndex = x;
    }*/
}
