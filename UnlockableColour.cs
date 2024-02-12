using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Colour", menuName = "Meltagochi/Deco/UnlockableColour", order = 1)]
public class UnlockableColour : ScriptableObject
{
    [SerializeField] public Color colour = new Color(1,1,1,1);
    [SerializeField] public string description;
    [SerializeField] public int levelRequired;
    [SerializeField] public int price;
}
