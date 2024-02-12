using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class CheerIncEnvironment : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public int clickableID = 0;
    public CheerInc cInc;
   

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        cInc.EnvironmentElementClicked(clickableID);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hovered");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("unhovered");
    }
}
