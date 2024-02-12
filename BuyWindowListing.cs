using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindowListing : MonoBehaviour
{
    [SerializeField] private Text priceText;
    [SerializeField] private Text titleText;
    
    BuyWindowsMenu theMenu;
    WindowData theData;
    

    public void SetData(BuyWindowsMenu x, WindowData y)
    {
        theMenu = x;
        theData = y;
        //if(theData.listingSprite != null)
        //icon.sprite = theData.listingSprite;
        priceText.text = theData.price + "";
        titleText.text = theData.name;
    }

    public void Clicked()
    {
        theMenu.ButtonPressed(theData);
    }
}
