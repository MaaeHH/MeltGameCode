using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New material", menuName = "Meltagochi/Deco/UnlockableMaterial", order = 1)]
public class UnlockableMaterial : ScriptableObject
{
    [SerializeField] public Material material;
    [SerializeField] public string name;
    [SerializeField] public string description;
    [SerializeField] public int levelRequired;
    [SerializeField] public int price;
}

