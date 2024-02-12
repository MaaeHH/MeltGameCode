using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class MissionInProgressListing : Listing
{
    InProgressMission thisData;

    [SerializeField] private Text missionTitle;
    //[SerializeField] private Text missionDuration;
    [SerializeField] private Text missionDesc;

    [SerializeField] private Text timeLeftText;
    [SerializeField] private Text usingVehicleText;
    [SerializeField] private Text usingMeltsText;


    private InProgressMenu theMenu;


    public void CompleteMissionButtonPressed()
    {
        if(!thisData.GetRewardsRecieved() && DateTime.Now > thisData.GetCompletionTime())
        {
            if (theMenu.CompleteMission(thisData)) thisData.SetRewardsRecieved(true);
        }
    }
   

    public void SetData(InProgressMission x, InProgressMenu z)
    {
        theMenu = z;
        thisData = x;
        if (thisData != null)
        {
            Refresh();
        }

    }

   

    public void Refresh()
    {
        ColourStuff(thisData.GetData().GetColour());
        missionTitle.text = thisData.GetData().GetTitle();
        missionDesc.text = thisData.GetData().GetCompleteText();

        if (thisData.GetRewardsRecieved() && DateTime.Now > thisData.GetCompletionTime())
        {
            timeLeftText.text = "This mission was completed on " + thisData.GetCompletionTime().ToString() + " by ";
            string onMissionString = "On mission: ";
            foreach (MeltData data in thisData.GetOccupiedMelts())
            {
                onMissionString = onMissionString + data.GetName();
            }


        }
        else if (!thisData.GetRewardsRecieved() && DateTime.Now > thisData.GetCompletionTime())
        {

        }
        else
        {
            timeLeftText.text = "Time remainign until completion: " + thisData.GetTimeRemaining() + ".";

            string onMissionString = "On mission: ";
            foreach(MeltData data in thisData.GetOccupiedMelts())
            {
                onMissionString = onMissionString + data.GetName();
            }
            usingMeltsText.text = onMissionString;

            usingVehicleText.text = "Using vehicle: " + thisData.GetVehicle().GetData().GetName() + ".";


        }
    }
}
