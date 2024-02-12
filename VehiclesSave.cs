using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class VehiclesSave
{

    public List<VehicleInstance> vehicleInstances;
   
    public VehiclesSave(List<VehicleInstance> records)
    {
        this.vehicleInstances = records;
    }
}
