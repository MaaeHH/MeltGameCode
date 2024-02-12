using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/Object", order = 1)]
public class GridObjectData : InventoryObjectData
{
    
    [SerializeField] private int maxOccupants = 1;
    [SerializeField] private int level = 0;
    [SerializeField] private int upgradeCost = 3;
    [SerializeField] private string assocMenu = "";
    [SerializeField] private int assocMenuTransition;
    [SerializeField] private GridObjectData upgradesInto;
    [SerializeField] private Vector3 theRotation;
    [SerializeField] private Vector3 theScale;
    [SerializeField] private ObjectType thisType;
    [SerializeField] private bool CheckMeltStats;
    public enum ObjectType { Bed, Kitchen, Cosmetic, SprayBooth, Nothing }
    public ObjectType GetGridObjectType()
    {
        return thisType;
    }
    public Vector3 GetObjectRotation()
    {
        return theRotation;
    }
    public Vector3 GetObjectScale()
    {
        return theScale;
    }
  
    public int GetLevel()
    {
        return level;
    }

    public string GetAssocMenuName()
    {
        return assocMenu;
    }

    public int GetTransitionNumber()
    {
        return assocMenuTransition;
    }

    public int GetMaxOccupants()
    {
        return maxOccupants;
    }
    public GridObjectData GetUpgradeData()
    {
        return upgradesInto;
    }

    public int GetUpgradeCost()
    {
        return upgradeCost;
    }


    
}
