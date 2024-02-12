using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehicleInstance
{
    //private VehicleData GetData();
    [SerializeField] private int cheerFactorLevel = 0;
    [SerializeField] private int capacityLevel = 0;
    [SerializeField] private int speedLevel = 0;
    [SerializeField] private int vehicleDataIndex = -1;
    [SerializeField] private InProgressMission mission = null;

  

    /*public VehicleInstance(int x)
    {
        vehicleDataIndex = x;
    }*/

    public VehicleInstance(VehicleData x)
    {
        vehicleDataIndex = SaveVehicleScript.GetIDFromData(x);
    }

    /*public int GetDataIndex()
    {
        return vehicleDataIndex;
    }*/

    public int GetCheerFactorLevel()
    {
        return cheerFactorLevel;
    }
    public int GetCapacityLevel()
    {
        return capacityLevel;
    }
    public int GetSpeedLevel()
    {
        return speedLevel;
    }


    public int GetCheerFactor()
    {
        return GetData().GetCheerFactorLevels()[cheerFactorLevel];
    }
    public int GetCapacity()
    {
        return GetData().GetCapacityLevels()[capacityLevel];
    }
    public int GetSpeed()
    {
        return GetData().GetSpeedLevels()[speedLevel];
    }


    public int GetCheerFactorCost()
    {
        return GetData().GetCheerFactorCosts()[cheerFactorLevel];
    }
    public int GetCapacityCost()
    {
        return GetData().GetCapacityCosts()[capacityLevel];
    }
    public int GetSpeedCost()
    {
        return GetData().GetSpeedCosts()[speedLevel];
    }

 

    public VehicleData GetData()
    {
        //return GetData();

        return SaveVehicleScript.GetDataFromID(vehicleDataIndex);
    }

    public void SetData(VehicleData x)
    {
        //GetData() = x;

        vehicleDataIndex = SaveVehicleScript.GetIDFromData(x);
    }

    public InProgressMission GetMission()
    {
        return mission;
    }

    public void SetMission(InProgressMission x)
    {
        mission = x;
    }

    public bool IncreaseCheerFactorLevel()
    {
        if(cheerFactorLevel + 1 != GetData().GetCheerFactorLevels().Count)
        {
            cheerFactorLevel++;
            return true;
        }
        
        return false;
    }

    public bool IncreaseCapacityLevel()
    {
        if (capacityLevel + 1 != GetData().GetCapacityLevels().Count)
        {
            capacityLevel++;
            return true;
        }

        return false;
    }

    public bool IncreaseSpeedLevel()
    {
        if (speedLevel + 1 != GetData().GetSpeedLevels().Count)
        {
            speedLevel++;
            return true;
        }

        return false;
    }

}
