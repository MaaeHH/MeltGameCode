using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ColoursSave
{

    public List<string> unlockedColours;
    public List<string> unlockedTextures;
    public List<WindowInstance> windowInventory;
    public ColoursSave(List<string> colourNames, List<string> textureNames, List<WindowInstance> windowInstances)
    {
        this.windowInventory = windowInstances;
        this.unlockedColours = colourNames;
        this.unlockedTextures = textureNames;
    }
}
