using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Colour", menuName = "Meltagochi/Deco/Window", order = 1)]
public class WindowData : ScriptableObject
{
    [SerializeField] public GameObject winObj;
    [SerializeField] public string description;
    [SerializeField] public int levelRequired;
    [SerializeField] public int price;
    
}
