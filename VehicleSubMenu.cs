using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VehicleSubMenu : CheerIncMenu
{
    [SerializeField] VehicleInstance thisVehicle;

    [SerializeField] private Text title;
    [SerializeField] private Text types;
    [SerializeField] private Text description;


    [SerializeField] private Text capacityUpgradeText;
    [SerializeField] private Text capacityBarText;
    [SerializeField] private Slider capacityBar;
    [SerializeField] private Text capacityUpgradeButtonText;
    [SerializeField] private Button capacityUpgradeButton;

    [SerializeField] private Text cheerFactorUpgradeText;
    [SerializeField] private Text cheerFactorBarText;
    [SerializeField] private Slider cheerFactorBar;
    [SerializeField] private Text cheerFactorUpgradeButtonText;
    [SerializeField] private Button cheerFactorUpgradeButton;

    [SerializeField] private Text speedUpgradeText;
    [SerializeField] private Text speedBarText;
    [SerializeField] private Slider speedBar;
    [SerializeField] private Text speedUpgradeButtonText;
    [SerializeField] private Button speedUpgradeButton;

    [SerializeField] private GameObject StatsDisplay;

    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        gameObject.SetActive(true);
    }
    public override void RefreshMenu()
    {
        if (thisVehicle != null)
        {
            
            StatsDisplay.SetActive(true);
            title.text = thisVehicle.GetData().GetName();
            description.text = thisVehicle.GetData().GetDescription();

            string temp = "(";
            if (thisVehicle.GetData().GetVehicleTypes().Contains(VehicleData.VehicleTypes.Air))
            {
                temp = temp + "air, ";
            }
            if (thisVehicle.GetData().GetVehicleTypes().Contains(VehicleData.VehicleTypes.Land))
            {
                temp = temp + "land, ";
            }
            if (thisVehicle.GetData().GetVehicleTypes().Contains(VehicleData.VehicleTypes.Sea))
            {
                temp = temp + "sea, ";
            }
            temp = temp.Substring(0, temp.Length - 2);
            temp = temp + ")";
            types.text = temp;


            UpdateSliders();

            if (thisVehicle.GetMission() != null)
            {
                description.text = thisVehicle.GetData().GetDescription() + "\n ALERT \n This vehicle is currently on a mission. It will return in " + thisVehicle.GetMission().GetTimeRemaining().ToString(@"hh\:mm");
                //StatsDisplay.SetActive(false);
                capacityUpgradeButton.interactable = false;
                capacityUpgradeButtonText.text = "On Mission";
                capacityUpgradeText.text = "";
                cheerFactorUpgradeButton.interactable = false;
                cheerFactorUpgradeButtonText.text = "On Mission";
                cheerFactorUpgradeText.text = "";
                speedUpgradeButton.interactable = false;
                speedUpgradeButtonText.text = "On Mission";
                speedUpgradeText.text = "";
            }
        }
        else
        {
            
            
                //Debug.Log("Null vehicle");
                StatsDisplay.SetActive(false);
                title.text = "Empty vehicle slot";
                types.text = "";
                description.text = "There is no vehicle in this slot, consider checking the meowmelt market for vehicles.";
            
            
        }
    }

    public void SetVehicle(VehicleInstance x)
    {
        thisVehicle = x;
        RefreshMenu();
    }

    public void UpgradeCapacity()
    {
        if (SaveSystem.GetScreamcoin() >= thisVehicle.GetCapacityCost())
        {
            SaveSystem.SubtractScreamcoin(thisVehicle.GetCapacityCost());
            
            thisVehicle.IncreaseCapacityLevel();
            SaveVehicleScript.SaveVehicleInstances();
        }

        RefreshMenu();
    }    
    public void UpgradeCheerFactor()
    {
        if (SaveSystem.GetScreamcoin() >= thisVehicle.GetCheerFactorCost())
        {
            SaveSystem.SubtractScreamcoin(thisVehicle.GetCheerFactorCost());
            thisVehicle.IncreaseCheerFactorLevel();
            SaveVehicleScript.SaveVehicleInstances();
        }

        RefreshMenu();
    }
    public void UpgradeSpeed()
    {
        if (SaveSystem.GetScreamcoin() >= thisVehicle.GetSpeedCost())
        {
            SaveSystem.SubtractScreamcoin(thisVehicle.GetSpeedCost());
            thisVehicle.IncreaseSpeedLevel();
            SaveVehicleScript.SaveVehicleInstances();
        }

        RefreshMenu();
    }


    private void UpdateSliders()
    {
        if(thisVehicle.GetCapacityLevel() < thisVehicle.GetData().GetCapacityLevels().Count -1)
        {
            capacityUpgradeText.text = thisVehicle.GetCapacity() + "=>" + thisVehicle.GetData().GetCapacityLevels()[thisVehicle.GetCapacityLevel() + 1];
            capacityUpgradeButton.interactable = true;

            
           
            capacityUpgradeButtonText.text = "Upgrade (" + thisVehicle.GetCapacityCost() + ") screamcoin.";
        }
        else
        {
            capacityUpgradeText.text = "max level";
            capacityUpgradeButton.interactable = false;

            capacityUpgradeButtonText.text = "Max level";
        }

        capacityBarText.text = thisVehicle.GetCapacity() + "/" + thisVehicle.GetData().GetCapacityLevels()[thisVehicle.GetData().GetCapacityLevels().Count - 1];
        capacityBar.value = thisVehicle.GetCapacity() / thisVehicle.GetData().GetCapacityLevels()[thisVehicle.GetData().GetCapacityLevels().Count - 1];

        if (thisVehicle.GetCheerFactorLevel() < thisVehicle.GetData().GetCheerFactorLevels().Count -1)
        {
            cheerFactorUpgradeText.text = thisVehicle.GetCheerFactor() + "=>" + thisVehicle.GetData().GetCheerFactorLevels()[thisVehicle.GetCheerFactorLevel() + 1];
            cheerFactorUpgradeButton.interactable = true;

            
           
            cheerFactorUpgradeButtonText.text = "Upgrade (" + thisVehicle.GetCheerFactorCost() + ") screamcoin.";
        }
        else
        {
            cheerFactorUpgradeText.text = "max level";
            cheerFactorUpgradeButton.interactable = false;

            cheerFactorUpgradeButtonText.text = "Max level";
        }

        cheerFactorBarText.text = thisVehicle.GetCheerFactor() + "/" + thisVehicle.GetData().GetCheerFactorLevels()[thisVehicle.GetData().GetCheerFactorLevels().Count - 1];
        cheerFactorBar.value = thisVehicle.GetCheerFactor() / thisVehicle.GetData().GetCheerFactorLevels()[thisVehicle.GetData().GetCheerFactorLevels().Count - 1];


        if (thisVehicle.GetSpeedLevel() < thisVehicle.GetData().GetSpeedLevels().Count -1)
        {
            speedUpgradeText.text = thisVehicle.GetSpeed() + "=>" + thisVehicle.GetData().GetSpeedLevels()[thisVehicle.GetSpeedLevel() + 1];
            speedUpgradeButton.interactable = true;

            
            
            speedUpgradeButtonText.text = "Upgrade (" + thisVehicle.GetSpeedCost() + ") screamcoin.";
        }
        else
        {
            speedUpgradeText.text = "max level";
            speedUpgradeButton.interactable = false;

            speedUpgradeButtonText.text = "Max level";
        }
        
        speedBarText.text = thisVehicle.GetSpeed() + "/" + thisVehicle.GetData().GetSpeedLevels()[thisVehicle.GetData().GetSpeedLevels().Count - 1];
        speedBar.value = thisVehicle.GetSpeed() / thisVehicle.GetData().GetSpeedLevels()[thisVehicle.GetData().GetSpeedLevels().Count - 1];
    }

    
    
}
