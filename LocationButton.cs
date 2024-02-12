using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class LocationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private PointOfInterestController poic;
    private PointOfInterest poi;
    [SerializeField] private Text theText;
    public void SetPoic(PointOfInterestController x)
    {
        poic = x;
    }
    public void SetPoi(PointOfInterest x)
    {
        poi = x;
        theText.text = x.GetTitle();
    }

    public void Clicked()
    {
        poic.ButtonPressed(poi);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        poic.ButtonHovered(poi);
        //Debug.Log("hovered");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        poic.ButtonExit(poi);
        //Debug.Log("hovered");
    }
}
