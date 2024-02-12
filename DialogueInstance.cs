using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueInstance
{
    public DialogueInstance(MeltScript x, List<string> y)
    {
        theMelt = x;
        theDialogue = y;
    }
    public MeltScript theMelt;
    public List<string> theDialogue;
}
