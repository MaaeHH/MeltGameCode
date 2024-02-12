using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/FriendshipReq", order = 1)]
public class FriendshipReqData : ScriptableObject
{
    
    [SerializeField] private string reqName;
    [SerializeField] private string description;
    [SerializeField] private int target;

    [SerializeField] InventoryObjectData data;
    [SerializeField] private bool readFromMelt;
    public InventoryObjectData GetData()
    {
        return data;
    }

    public string GetName()
    {
        return reqName;
    }
    public string GetDescription()
    {
        return description;
    }
    public int GetTarget()
    {
        return target;
    }
    public bool GetReadFromMelt()
    {
        return readFromMelt;
    }
 
}
