using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Listing : MonoBehaviour
{
    [SerializeField] private List<Image> stuffToColour;

    public void ColourStuff(Color x)
    {
        //Debug.Log("Colouring stuff");
        if (stuffToColour != null)
        {
            foreach (Image thing in stuffToColour)
            {
                thing.color = x;
            }
        }
    }
    
}
