using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSubListing : Listing
{
    VehicleInstance thisData;

    [SerializeField] private Text vehicleName;

    [SerializeField] private Text speedText;
    [SerializeField] private Text capacityText;
    [SerializeField] private Text cheerFactorText;
 
    //[SerializeField] private Toggle selectedBox;

    //[SerializeField] private List<Image> stuffToColour;

    private MissionSubMenu theMenu;


    public bool selected = false;

    public void SetData(VehicleInstance x, MissionSubMenu z)
    {
        theMenu = z;
        thisData = x;
        //thisData.InitSaveData();
        //if (thisData != null)
        //{
        Refresh();
        //}

    }

    public void Clicked()
    {
        selected = true;
        theMenu.VehicleSelected(this);
        Refresh();
    }

    public void Deselected()
    {
        selected = false;
        Refresh();
    }

    public VehicleInstance GetData()
    {
        return thisData;
    }

    public void Refresh()
    {

       
        ColourStuff(thisData.GetData().GetColour());

        speedText.text = thisData.GetSpeed() + "";
        capacityText.text = thisData.GetCapacity() + "";
        cheerFactorText.text = thisData.GetCheerFactor() + "";


        if (!selected)
        {
            vehicleName.text = thisData.GetData().GetName(); 
        }
        else
        {
            vehicleName.text = "\u2714" + thisData.GetData().GetName(); 
        }
        
       
        //kindnessText.text = thisData.GetKindness() + "";
        //cuddlynessText.text = thisData.GetCuddliness() + "";
        //friendliness.text = thisData.GetFriendliness() + "";

        //meltName.text = thisData.GetName();
    }
}
