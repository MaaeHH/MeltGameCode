using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DialogueBox : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Text theText;
    [SerializeField] private AudioController _audioCont;
    private bool isDisplaying = false;
    private bool skip = false;
    private bool isTalking = false;
    private List<string> theStrings;
    private MeltScript theTalkingMelt;
    private int stringNum = 0;
    [SerializeField] private CameraControl cc;
    [SerializeField] private MeltSpawner ms;

    private Queue<DialogueInstance> queuedDialogue = new Queue<DialogueInstance>();

    public void MakeDialogue(List<string> theDialogue, MeltScript theMelt)
    {
        queuedDialogue.Enqueue(new DialogueInstance(theMelt, theDialogue));
        NextQueuedDialogue();
    }

    public void MakeDialogue(List<string> theDialogue)
    {
        queuedDialogue.Enqueue(new DialogueInstance(null, theDialogue));
        NextQueuedDialogue();
    }

    public void NextQueuedDialogue()
    {
        if (!isTalking)
        {
            this.gameObject.SetActive(true);
            if (queuedDialogue.Count != 0)
            {
                ShowFirstString(queuedDialogue.Dequeue());
            }
        }
    }
   
    private void ShowFirstString(DialogueInstance currentDialogue)
    {
        isTalking = true;
        stringNum = 0;
        _audioCont.PlayDialogueSound();
        theStrings = currentDialogue.theDialogue;
        theTalkingMelt = currentDialogue.theMelt;
        if(theTalkingMelt != null)
        theTalkingMelt.SetMode(5);
        cc.MakeLookAt(theTalkingMelt.transform);
        //Do something witht he melt: currentDialogue.theMelt;

        ShowString(theStrings[stringNum]);
    }

    private void ShowNextString()
    {
        stringNum++;
        if(stringNum >= theStrings.Count)
        {
            StringsDone();
            return;
        }
        ShowString(theStrings[stringNum]);
    }

    private void StringsDone()
    {
        isTalking = false;
        
        //End something witht he melt: currentDialogue.theMelt;
        theTalkingMelt.SetMode(0);
        theStrings = new List<string>();
        theTalkingMelt = null;
        if (queuedDialogue.Count != 0)
        {
            ShowFirstString(queuedDialogue.Dequeue());
        }
        else
        {
            //Else we're done
            _audioCont.PlayDialogueEndSound();
            this.gameObject.SetActive(false);
            ms.ChangeMelts(null);
        }

    }

    private void ShowString(string x)
    {

        StartCoroutine(ShowTextOneLetterAtATime(x));
        
    }

    private IEnumerator ShowTextOneLetterAtATime(string x)
    {
        isDisplaying = true;
        
        for (int i = 0; i < x.Length; i++)
        {
            theText.text = x.Substring(0, i + 1);
            yield return new WaitForSeconds(0.2f);
            if (skip)
            {
                isDisplaying = false;
                skip = false;
                theText.text = x;
                break;
            }
        }

        isDisplaying = false;
    }

    public void SkipPressed()
    {
        if (isDisplaying)
        {
            if (!skip) skip = true;
        }
        else
        {
            ShowNextString();
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        SkipPressed();
    }
}
