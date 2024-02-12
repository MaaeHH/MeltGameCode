using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MultiDialogueHolder
{
    [SerializeField] private List<ListWrapper<string>> myDialogues;
    private int index = 0;
    private bool hasLooped = false;
    public MultiDialogueHolder(List<ListWrapper<string>> x)
    {
        myDialogues = x;
    }

    public List<string> GetNextDialogue()
    {
        List<string> temp = myDialogues[index].list;
        
        index++;
        //Debug.Log("there are dialogues: " + myDialogues.Count);
        //Debug.Log("haslooped: " + hasLooped);
        if (index >= myDialogues.Count)
        {
            index = 0;
            hasLooped = true;
        }
        return temp;
    }

    public bool GetHasLooped()
    {
        return hasLooped;
    }
    public void ResetHasLooped()
    {
        Debug.Log("resettingHasLooped");
        index = 0;
        hasLooped = false;
    }
}
