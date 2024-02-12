using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/VehicleData", order = 1)]
public class VehicleData : ScriptableObject
{
    public enum VehicleTypes { Air, Land, Sea }

    [SerializeField] private List<VehicleTypes> thisTypes;
    [SerializeField] private List<int> capacityLevels;
    [SerializeField] private List<int> cheerFactorLevels;
    [SerializeField] private List<int> speedLevels;

    [SerializeField] private List<int> capacityCosts;
    [SerializeField] private List<int> cheerFactorCosts;
    [SerializeField] private List<int> speedCosts;

    [SerializeField] private string name;
    [SerializeField] private string desc;
    [SerializeField] private Color menuColour;

    [SerializeField] private string vehicleFileName = "";

    [SerializeField] private Vector3 theRotation;
    [SerializeField] private Vector3 theScale;

    public string GetName()
    {
        return name;
    }
    public string GetDescription()
    {
        return desc;
    }
    public Color GetColour()
    {
        return menuColour;
    }
    public string GetFileName()
    {
        return vehicleFileName;
    }
    public Vector3 GetObjectRotation()
    {
        return theRotation;
    }
    public Vector3 GetObjectScale()
    {
        return theScale;
    }

    public List<int> GetCapacityLevels()
    {
        return capacityLevels;
    }
    public List<int> GetCheerFactorLevels()
    {
        return cheerFactorLevels;
    }
    public List<int> GetSpeedLevels()
    {
        return speedLevels;

    }

    public List<int> GetCapacityCosts()
    {
        return capacityCosts;
    }
    public List<int> GetCheerFactorCosts()
    {
        return cheerFactorCosts;
    }
    public List<int> GetSpeedCosts()
    {
        return speedCosts;

    }

    public List<VehicleTypes> GetVehicleTypes()
    {
        return thisTypes;
    }
}
