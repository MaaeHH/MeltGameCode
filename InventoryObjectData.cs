using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public abstract class InventoryObjectData : ScriptableObject
{
    [SerializeField] private Sprite icon;
    [SerializeField] private string objName;
    [SerializeField] private string objectFileName;
    [SerializeField] private int itemCost = 3;
    [SerializeField] private string description;
    public Sprite GetIcon()
    {
        //return null;
        return icon;
    }
    public string GetName()
    {
        return objName;
    }
    public string GetDescription()
    {
        return description;
    }

    public void SetName(string x)
    {
        objName = x;
    }

    public int GetCost()
    {
        return itemCost;
    }

    public string GetFileName()
    {
        return objectFileName;
    }


}
