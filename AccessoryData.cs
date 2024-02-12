using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "NewAccessory", menuName = "Meltagochi/Accessory", order = 1)]
public class AccessoryData : ScriptableObject
{
    [SerializeField] public GameObject gameObj;
    [SerializeField] public string accessoryName;
    [SerializeField] public string description;

}
