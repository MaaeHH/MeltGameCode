using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowLocationMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [SerializeField] private PointOfInterestController poic;
   
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        poic.ButtonHovered();
        //Debug.Log("hovered");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        poic.ButtonExit();
        //Debug.Log("hovered");
    }
}
