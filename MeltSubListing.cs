using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeltSubListing : Listing
{
    MeltData thisData;

    [SerializeField] private Text meltName;
    //[SerializeField] private Text missionDuration;
    [SerializeField] private Text sillynessText;
    [SerializeField] private Text kindnessText;
    [SerializeField] private Text cuddlynessText;
    [SerializeField] private Text friendliness;
    //[SerializeField] private Toggle selectedBox;

    //[SerializeField] private List<Image> stuffToColour;

    private MissionSubMenu theMenu;

    private bool isSelected = false;


    public void SetData(MeltData x, MissionSubMenu z)
    {
        theMenu = z;
        thisData = x;
        thisData.InitSaveData();
        if (thisData != null)
        {
            Refresh();
        }

    }


    public void ToggleChanged()
    {
       
        if (!isSelected)
        {
            if (theMenu.MeltSelected(thisData))
            {
                isSelected = true;
            }
        }
        else
        {
            theMenu.MeltDeselected(thisData);
            isSelected = false;
        }
        Refresh();
    }


    public void Refresh()
    {

        sillynessText.text = thisData.GetSillyness() + "";
        kindnessText.text = thisData.GetKindness() + "";
        cuddlynessText.text = thisData.GetCuddliness() + "";
        friendliness.text = thisData.GetFriendliness()  + "";

        if (!isSelected)
        {
            meltName.text = thisData.GetName();
        }
        else
        {
            meltName.text = "\u2714" + thisData.GetName();
        }
        
        if(thisData.GetColour() != null)
       
        ColourStuff(thisData.GetColour());
    }
}
