using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionListing : Listing
{
    [SerializeField] MissionInstance thisData;

    [SerializeField] private Text missionTitle;
    //[SerializeField] private Text missionDuration;
    [SerializeField] private Text missionDesc;
    [SerializeField] private Text sillynessreq;
    [SerializeField] private Text kindnessreq;
    [SerializeField] private Text cuddlinessreq;
    [SerializeField] private Text friendlinessreq;
    [SerializeField] private Text timeLeftText;
    [SerializeField] private Text typesOfVehicle;

    private MissionMenu theMenu;


    public void SetData(MissionInstance x)
    {
        thisData = x;
        if (thisData != null)
        {
            if (thisData.GetData() != null)
            {
                Refresh();
            }
        }
        else
        {
            Debug.Log("Null data");
            gameObject.SetActive(false);
        }
    }

    public void SetData(MissionInstance x,MissionMenu z)
    {

        
        theMenu = z;
        thisData = x;
        if (thisData != null)
        {
            if (thisData.GetData() != null)
            {
                Refresh();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    public void MissionPressed()
    {
        theMenu.MissionPressed(thisData);
    }

    public void Refresh()
    {

        ColourStuff(thisData.GetData().GetColour());

        sillynessreq.text = thisData.GetData().GetSillynessReq() + "";
        kindnessreq.text = thisData.GetData().GetKindnessReq() + "";
        cuddlinessreq.text = thisData.GetData().GetCuddlinessReq() + "";
        friendlinessreq.text = thisData.GetData().GetFriendlinessReq() + "";

        missionTitle.text = thisData.GetData().GetTitle();
        missionDesc.text = thisData.GetData().GetDescription();

        
        //Debug.Log("testtest");
        timeLeftText.text = "Expires in " + thisData.GetTimeUntilExpire().ToString() + ", takes " + thisData.GetData().GetDuration() + ".";
        MakeTypes();
    }

    private void MakeTypes()
    {
        string temp = "";
        if (thisData.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Air))
        {
            temp = temp + "air, ";
        }
        if (thisData.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Land))
        {
            temp = temp + "land, ";
        }
        if (thisData.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Sea))
        {
            temp = temp + "sea, ";
        }
        temp = temp.Substring(0, temp.Length - 2);
        temp = temp + " vehicles only.";
        typesOfVehicle.text = temp;
    }
}
