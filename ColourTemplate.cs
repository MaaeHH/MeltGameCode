using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColourTemplate : MonoBehaviour
{
    private ColoursMenu cm;
    private UnlockableColour uc;
    [SerializeField] private Image img;
    public void SetColoursMenu(ColoursMenu x)
    {
        cm = x;
    }
    public void SetUnlockedColour(UnlockableColour x)
    {
        uc = x;
        img.color = uc.colour;
    }

    public void ButtonPressed()
    {
        cm.ButtonPressed(uc);
    }
    
}
