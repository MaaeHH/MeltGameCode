using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MeltRecord
{
    [SerializeField] public enum Location { Market, House, Mission, None}
    //] private DateTime timeUntil;
    [SerializeField] private long timeUntil;
    [SerializeField] private string dataName = "";
    [SerializeField] private Location theLocation = Location.None;
    [SerializeField] private bool locationLock = false;
    [SerializeField] private bool customMelt;

    public MeltRecord(MeltData x, bool custom)
    {
        SetData(x);
        customMelt = custom;
    }

    public Location GetLocation()
    {
        return theLocation;
    }

    public DateTime GetTime()
    {
        //return timeUntil;
        return new DateTime(timeUntil);
    }
    public MeltData GetData()
    {
        return MeltAssigner.GetDataFromName(dataName, customMelt);
    }
    public string GetDataName()
    {
        return dataName;
    }

    public void SetLocation(Location x)
    {
        theLocation = x;
    }

    public void SetTime(DateTime x)
    {
        timeUntil = x.Ticks;
    }
    public void SetData(MeltData x)
    {
        if (x == null)
        {
            dataName = "";
        }
        else
        {
            dataName = x.name;
        }
    }

    public bool GetLock()
    {
        return locationLock;
    }

    public void SetLock(bool x)
    {
        locationLock = x;
    }

    public bool GetCustom()
    {
        return customMelt;
    }
}
