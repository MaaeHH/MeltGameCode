using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MissionSubMenu : CheerIncMenu
{

    //[SerializeField] private List<MissionInstance> missions;
    //[SerializeField] private int noSlots;
    [SerializeField] private Animator _menuAnim;
    //[SerializeField] private Image _bgImg;
    //private MeltScript meltScript;
    [SerializeField] private AlertMenu _alertMenu;

    private MissionInstance mission;
    private List<VehicleInstance> vehicles;
    private List<MeltData> melts;

    [SerializeField] private MissionMenu missionMenu;

    [SerializeField] private GameObject vehicleSubListingTemplate;
    [SerializeField] private GameObject meltSubListingTemplate;
    [SerializeField] private Transform vehicleSubListingsTransform;
    [SerializeField] private Transform meltSubListingsTransform;

    [SerializeField] private Text missionTitle;
    [SerializeField] private Text missionDescription;
    [SerializeField] private Text missionRewards;

    [SerializeField] private Text vehicleMenuHeader;
    [SerializeField] private Text meltMenuHeader;
    [SerializeField] private Text startMissionButtonText;
    [SerializeField] private Text clearMissionButtonText;

    [SerializeField] private Text sillynessReq;
    [SerializeField] private Text kindnessReq;
    [SerializeField] private Text friendlinessReq;
    [SerializeField] private Text cuddlinessReq;

    [SerializeField] private Slider sillynessReqSlider;
    [SerializeField] private Slider kindnessReqSlider;
    [SerializeField] private Slider friendlinessReqSlider;
    [SerializeField] private Slider cuddlinessReqSlider;

    private VehicleSubListing selectedVehicleListing;

    private int currentSillyness = 0;
    private int currentKindness = 0;
    private int currentFriendliness = 0;
    private int currentCuddliness = 0;

    private List<MeltData> selectedMelts;
    private VehicleInstance selectedVehicle;
    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }


    public void VehicleSelected(VehicleSubListing x)
    {
        if (selectedVehicleListing != null && selectedVehicleListing != x)
        {
            selectedVehicleListing.Deselected();
        }

        selectedVehicleListing = x;
        selectedVehicle = x.GetData();

        RefreshText();
    }

    public override void OpenMenu()
    {
        melts = MeltAssigner.GetMissionSelectableMelts();
        vehicles = SaveVehicleScript.GetVehicles();
        selectedMelts = new List<MeltData>();
        gameObject.SetActive(true);
        
        MakeListings();
        RefreshMenu();
    }
    public override void RefreshMenu()
    {

        RefreshText();
        RefreshSliders();
        ClearVehicles();
        ClearMelts();
        MakeListings();

    }
    public void SetMission(MissionInstance x)
    {
        mission = x;
        //selectedMelts = new List<MeltData>();
        //RefreshMenu();
    }

    private void RefreshText()
    {

    
        missionTitle.text = mission.GetData().GetTitle();
        missionDescription.text = mission.GetData().GetDescription();
        startMissionButtonText.text = "Start mission! (will take " + mission.GetData().GetDuration() + " minutes)";
        string temp = "Rewards: ";

        foreach (InventoryObjectData invobj in mission.GetData().GetItemRewards())
        {
            temp = temp + invobj.GetName() + ". ";
        }
        temp = temp + " EXP: "+  mission.GetData().GetExpReward() + ". ";
        temp = temp + " Screamcoin: "+  mission.GetData().GetScreamcoinReward() + ". ";

        missionRewards.text = temp;

        temp = "Vehicles (";
        if (mission.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Air))
        {
            temp = temp + "air, ";
        }
        if (mission.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Land))
        {
            temp = temp + "land, ";
        }
        if (mission.GetData().GetTypes().Contains(VehicleData.VehicleTypes.Sea))
        {
            temp = temp + "sea, ";
        }
        temp = temp.Substring(0, temp.Length - 2);
        temp = temp + ")";
        vehicleMenuHeader.text = temp;

        if(selectedVehicle != null)
        {
            meltMenuHeader.text = "Melts (" + selectedMelts.Count +"/" +selectedVehicle.GetCapacity() + ")";
        }
        else
        {
            meltMenuHeader.text = "Melts";
        }
        

    }

    public void StartMission()
    {
        bool successful = true;
        string errorText = "Error: ";
        if(selectedVehicle != null)
        {
            if (currentSillyness < mission.GetData().GetSillynessReq())
            {
                successful = false;
                errorText = errorText + " Mission sillyness requirement not met.";
            }
            if (currentKindness < mission.GetData().GetKindnessReq())
            {
                successful = false;
                errorText = errorText + " Mission kindness requirement not met.";
            }
            if (currentFriendliness < mission.GetData().GetFriendlinessReq())
            {
                successful = false;
                errorText = errorText + " Mission friendliness requirement not met.";
            }
            if (currentCuddliness < mission.GetData().GetCuddlinessReq())
            {
                successful = false;
                errorText = errorText + " Mission cuddliness requirement not met.";
            }
            if (selectedMelts.Count > selectedVehicle.GetCapacity())
            {
                successful = false;
                errorText = errorText + " Vehicle capacity exceeded.";
            }
        }
        else
        {
            successful = false;
            errorText = errorText + " No vehicle selected";
        }
        
        //startMissionButtonText.text = errorText;
        _alertMenu.MakeAlert("Error!", errorText);
        if (successful)
        {
            


            Debug.Log("success");
            //ClearSelected();
            missionMenu.CloseSubMenu();
            missionMenu.RefreshMenu();
            _alertMenu.MakeAlert("Alert", "Mission allocation successful. Notifiying meowmelts");
            MissionAssigner.AddInProgressMission(new InProgressMission(mission.GetData(), selectedVehicle, selectedMelts));
            
            ClearSelected();
        }
    }

    public void ClearSelected()
    {
        selectedMelts = new List<MeltData>();
        selectedVehicle = null;
        selectedVehicleListing = null;
        RefreshMenu();
    }

    private void RefreshSliders()
    {
        currentSillyness = 0;
        currentKindness = 0;
        currentFriendliness = 0;
        currentCuddliness = 0;
        foreach (MeltData melt in selectedMelts)
        {
            currentSillyness = currentSillyness + melt.GetSillyness();
            currentKindness = currentKindness + melt.GetKindness();
            currentFriendliness = currentFriendliness + melt.GetFriendliness();
            currentCuddliness = currentCuddliness + melt.GetCuddliness();
        }

        sillynessReq.text = "(" + currentSillyness + "/" + mission.GetData().GetSillynessReq() + ")";
        kindnessReq.text = "(" + currentKindness + "/" + mission.GetData().GetKindnessReq() + ")";
        friendlinessReq.text = "(" + currentFriendliness + "/" + mission.GetData().GetFriendlinessReq() + ")";
        cuddlinessReq.text = "(" + currentCuddliness + "/" + mission.GetData().GetCuddlinessReq() + ")";

        sillynessReqSlider.value = currentSillyness * 1.0f / mission.GetData().GetSillynessReq();
        kindnessReqSlider.value = currentKindness * 1.0f / mission.GetData().GetKindnessReq();
        friendlinessReqSlider.value = currentFriendliness * 1.0f / mission.GetData().GetFriendlinessReq();
        cuddlinessReqSlider.value = currentCuddliness * 1.0f / mission.GetData().GetCuddlinessReq();

    }

    public bool MeltSelected(MeltData x)
    {
        if ((selectedVehicle != null && selectedMelts.Count < selectedVehicle.GetCapacity()))
        {

            if (!selectedMelts.Contains(x))
                selectedMelts.Add(x);
            RefreshSliders();
            RefreshText();
            return true;
        }
        return false;
    }

    public void MeltDeselected(MeltData x)
    {
        if (selectedMelts.Contains(x))
            selectedMelts.Remove(x);
        RefreshSliders();
        RefreshText();
    }


    private void MakeListings()
    {
        MakeMelts();
        MakeVehicles();



    }
    private void MakeMelts()
    {
        foreach (MeltData meltData in melts)
        {

            MakeMeltSubListing(meltData);
        }
    }


    private void MakeVehicles()
    {
        bool flag = false;
        foreach (VehicleInstance vehicle in vehicles)
        {
            flag = false;
            foreach(VehicleData.VehicleTypes type in mission.GetData().GetTypes())
            {
                
                if (vehicle.GetData().GetVehicleTypes().Contains(type))
                {
                    Debug.Log("MAKEVEHICLE a");
                    if (vehicle.GetMission() == null)
                    {
                        Debug.Log("MAKEVEHICLE b");
                        flag = true;
                    }
                    
                    
                }
            }
            if(flag == true)
            {
                MakeVehicleSubListing(vehicle);
            }
            
        }
    }


    public void MissionPressed(MissionInstance x)
    {
        Debug.Log("asd");
    }

    private void MakeVehicleSubListing(VehicleInstance vehicle)
    {
        GameObject currentSpawned = Instantiate(vehicleSubListingTemplate);

        currentSpawned.GetComponent<VehicleSubListing>().SetData(vehicle, this);
        currentSpawned.transform.SetParent(vehicleSubListingsTransform);
        currentSpawned.transform.localScale = (new Vector3(1, 1, 1));
        currentSpawned.SetActive(true);

    }

    private void MakeMeltSubListing(MeltData melt)
    {
        GameObject currentSpawned = Instantiate(meltSubListingTemplate);

        currentSpawned.GetComponent<MeltSubListing>().SetData(melt, this);
        currentSpawned.transform.SetParent(meltSubListingsTransform);
        currentSpawned.transform.localScale = (new Vector3(1, 1, 1));
        currentSpawned.SetActive(true);

    }

    public void ClearVehicles()
    {
        foreach (Transform thing in vehicleSubListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }
    public void ClearMelts()
    {
        foreach (Transform thing in meltSubListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

}
