using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteMeltListingNotFriend : Listing
{
    

    [SerializeField] private Text meltName;
   
    private MeltData thisData;
    private InviteMeltsMenu theMenu;

    public void SetData(MeltData x, InviteMeltsMenu z)
    {
        thisData = x;
        theMenu = z;
        
        thisData.InitSaveData();
        if (thisData != null)
        {
            Refresh();
        }

    }

    public void Refresh()
    {     
        meltName.text = thisData.GetName();
 
        if (thisData.GetColour() != null)
            ColourStuff(thisData.GetColour());
    }

    public void Clicked()
    {
        theMenu.Clicked(thisData);
   
    }
}
