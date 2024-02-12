using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleListing : Listing
{
    VehicleInstance thisData;
    [SerializeField] private Text titleText;
    [SerializeField] private Text descText;
    [SerializeField] private Text missionTitle;
    [SerializeField] private Text missionDuration;
    [SerializeField] private Text missionDesc;
    [SerializeField] private Text stat1;
    [SerializeField] private Text stat2;
    [SerializeField] private Text stat3;
    [SerializeField] private int slotNum = -1;
    private VehicleMenu theMenu;
    public void SetData(VehicleInstance x)
    {
        thisData = x;
        if(thisData != null)
        {
            Refresh();
        }
        else
        {
            titleText.text = "Slot " + slotNum + " Empty";
        }
        
    }

    public void SetData(VehicleInstance x, int y, VehicleMenu z)
    {

        SetSlot(y);
        theMenu = z;
        thisData = x;
        if (thisData != null)
        {
            Refresh();
        }
        else
        {
            titleText.text = "Slot " + slotNum + " Empty";
        }

    }

    public void VehiclePressed()
    {
        theMenu.VehiclePressed(thisData);
    }
    public void MissionPressed()
    {
        theMenu.MissionPressed(thisData.GetMission());
    }

    public void SetSlot(int x)
    {
        slotNum = x;
    }
    public void Refresh()
    {
        stat1.text = thisData.GetCheerFactor()+"";
        stat2.text = thisData.GetCapacity()+"";
        stat3.text = thisData.GetSpeed()+"";
        ColourStuff(thisData.GetData().GetColour());
        if (slotNum == -1)
        {
            titleText.text = thisData.GetData().GetName();
        }
        else
        {
            titleText.text = "Slot "+ slotNum + thisData.GetData().GetName();
        }

        
        descText.text = thisData.GetData().GetDescription();

        if(thisData.GetMission() != null)
        {
            missionTitle.text = thisData.GetMission().GetData().GetTitle();
            missionDuration.text = thisData.GetMission().GetTimeRemaining().ToString("c");
            missionDesc.text = thisData.GetMission().GetData().GetDescription();
        }
        else
        {
            missionDesc.text = "Not currently on a mission.";
        }
        
    }

}
