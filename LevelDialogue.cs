using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelDialogue
{
    public int theLevel;
    public List<string> theDialogue;
    public LevelDialogue(List<string> x, int y)
    {
        theDialogue = x;
        theLevel = y;
    }
}
