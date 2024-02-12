using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseColourTemplate : MonoBehaviour
{
    [SerializeField] private DecorateHouseMenu theMenu;
    private UnlockableColour myColour;
    [SerializeField] private Image theImage;
    private int ID;
    public void SetData(int x, DecorateHouseMenu y, UnlockableColour z)
    {
        if (x == null) Debug.Log("null x");
        if (y == null) Debug.Log("null y");
        if (z == null) Debug.Log("null z");
        ID = x;
        theMenu = y;
        myColour = z;
        theImage.color = z.colour;
    }

    public void ThisClicked()
    {
        theMenu.ButtonClicked(ID, myColour);
    }
}
