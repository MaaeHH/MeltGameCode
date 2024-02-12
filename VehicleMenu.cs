using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleMenu : CheerIncMenu
{
    [SerializeField] private List<VehicleInstance> vehicles;
    private int noSlots = SaveSystem.GetNumberOfVehicleSlots();
    [SerializeField] private Animator _menuAnim;
    //[SerializeField] private Image _bgImg;
    //private MeltScript meltScript;



    [SerializeField] private GameObject vehicleListingTemplate;
    [SerializeField] private GameObject vehicleListingTemplateNoMission;
    [SerializeField] private GameObject emptySlotTemplate;
    [SerializeField] private Transform vehicleListingsTransform;

    [SerializeField] private VehicleData vehicleToTest;

    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        gameObject.SetActive(true);
        //vehicles = SaveVehicleScript.GetVehicles();
        RefreshMenu();
    }
    public override void RefreshMenu()
    {
        vehicles = SaveVehicleScript.GetVehicles();
        ClearListings();
        MakeListings();
        
    }

    public void AddVehicleToTest()
    {
        SaveVehicleScript.AddVehicle(vehicleToTest);
        RefreshMenu();
    }

    private void MakeListings()
    {
        int x = 0;
        foreach (VehicleInstance vehicle in vehicles)
        {

            MakeVehicleListing(vehicle, x + 1);
            x++;
        }
        for (int i = vehicles.Count; i < noSlots; i++)
        {
            MakeVehicleListing(null, x + 1);
            x++;
        }

    }

    public void VehiclePressed(VehicleInstance x)
    {

    }

    public void MissionPressed(InProgressMission x)
    {

    }

    private void MakeVehicleListing(VehicleInstance vehicle, int x)
    {
        GameObject currentSpawned;
        if (vehicle == null)
        {
            currentSpawned = Instantiate(emptySlotTemplate);
        }
        else if (vehicle.GetMission() != null)
        {
            currentSpawned = Instantiate(vehicleListingTemplate);
        }
        else
        {
            currentSpawned = Instantiate(vehicleListingTemplateNoMission); 
        }
        currentSpawned.GetComponent<VehicleListing>().SetData(vehicle, x, this);
        currentSpawned.transform.SetParent(vehicleListingsTransform);
        currentSpawned.transform.localScale = (new Vector3(1, 1, 1));
        currentSpawned.SetActive(true);

    }

    public void ClearListings()
    {
        foreach (Transform thing in vehicleListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }
   
}
