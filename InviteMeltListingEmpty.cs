using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteMeltListingEmpty : Listing
{
    
    private InviteMeltsMenu theMenu;

    public void SetData( InviteMeltsMenu z)
    {
        theMenu = z;
    }

    public void Clicked()
    {
        theMenu.EmptyClicked();
        //thisMS.emptyClicked();
    }
}
