using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowListing : MonoBehaviour
{
    [SerializeField] private Text theWindowText;
    [SerializeField] private Image theImage;
    [SerializeField] private Sprite inTex;
    [SerializeField] private Sprite outTex;
    [SerializeField] private Color inColour;
    [SerializeField] private Color outColour;
    private int thisInt;
    private EditWindowsMenu theMenu;
    private WindowInstance theInstance;
    public void SetData(int x, EditWindowsMenu y, WindowInstance z)
    {
        thisInt = x;
        theMenu = y;
        theInstance = z;
        theWindowText.text = x + ": "+ z.GetData().name;
    }

    public void ListingClicked()
    {
        theMenu.ListingClicked(thisInt, theInstance);
        theImage.sprite = inTex;
        theImage.color = inColour;
    }

    public void UnClick()
    {
        theImage.sprite = outTex;
        theImage.color = outColour;
    }
    public WindowInstance GetInstance()
    {
        return theInstance;
    }
}
