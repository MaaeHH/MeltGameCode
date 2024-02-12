using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class CheerInc : MonoBehaviour
{
    [SerializeField] private List<CheerIncMenu> menus;
    //[SerializeField] int currentMenu = 0;
    [SerializeField] private Texture2D cursorTexture;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private DayNight dayNightScript;
    [SerializeField] private GameObject cameraGameObj;
    private CheerIncMenu currentComputerMenu;
    private CheerIncMenu vehicleSubMenu;
    bool moving = false;
    public Transform targetTransform;
    private float rotationSpeed = 10.0f;
    private float movementSpeed = 10.0f;
    [SerializeField] private Text timeText;
    [SerializeField] private WallpaperMenuScript wms;

    [SerializeField] private List<GameObject> toShow;
    [SerializeField] private List<GameObject> toHide;
    [SerializeField] private Transform cameraPointOfOrigin;
    
    [SerializeField] private ScreenDimmer sd;
    //[SerializeField] private GameObject cameraGameObj;
    
    [SerializeField] private Transform computerCam;

    [SerializeField] private VehicleSubMenu vehSubMenu;

    [SerializeField] private GameObject endOfWall;
    // Start is called before the first frame update
    void Start()
    {
        vehicles = SaveVehicleScript.GetVehicles();
        cameraGameObj.transform.position = cameraPointOfOrigin.transform.position;
        cameraGameObj.transform.rotation = cameraPointOfOrigin.transform.rotation;

        //wallpaper.sprite = loadedWallpapers[SaveSystem.GetCurrentDesktopWallpaper()];
        Cursor.SetCursor(cursorTexture,new Vector2(0,0), CursorMode.Auto);
        wms.Start();

        InstantiateObjects();
        //OpenMenu();
        //leftButton.onClick.AddListener(PreviousMenu);
        //rightButton.onClick.AddListener(NextMenu);
        dayNightScript.OnWorkingHoursChanged += DayNight_WorkingHoursChanged;
        sd.OnMaxDim += MaxDim;
        timeText.text = DateTime.Now.ToString("HH:mm");
        InvokeRepeating("UpdateClockText", 1f, 1f);
        //StartCoroutine(StartEnum());
    }
    private void OnDestroy()
    {
        dayNightScript.OnWorkingHoursChanged -= DayNight_WorkingHoursChanged;
        sd.OnMaxDim -= MaxDim;
    }



    private void DayNight_WorkingHoursChanged(bool working)
    {
        //isWorkingHours = working;
    }
    // Update is called once per frame
    void Update()
    {
        MoveCamToTargetLocation();
        RotateCamToTarget();
        //MoveCamToTargetLocation();
        //RotateCamToTarget();
    }

    private void UpdateClockText()
    {
        timeText.text = DateTime.Now.ToString("HH:mm");
    }
 

    public void ButtonPressed(CheerIncMenu x)
    {
        if (currentComputerMenu == null)
        {
            currentComputerMenu = x;
            currentComputerMenu.OpenMenu();
        }
    }

    public void CloseMenu()
    {
        if(currentComputerMenu != null)
        {
            currentComputerMenu.CloseMenu();
            currentComputerMenu = null;
        }
    }


    private void showShowStuff(bool x)
    {
        foreach (GameObject obj in toShow)
        {
            obj.SetActive(x);
        }
        foreach (GameObject obj in toHide)
        {
            obj.SetActive(!x);
        }
        if (!x)
        {
            targetTransform = cameraPointOfOrigin;
            lastClicked = -1;
        }
    }
    public int lastClicked;
    public void EnvironmentElementClicked(int clickableID)
    {
        Debug.Log(clickableID + "clicked");
        lastClicked = clickableID;
        switch (clickableID)
        {
            case 0:
                targetTransform = computerCam;


                // code block
                break;
            case 1:
                targetTransform = vehicleSlotList[vehicleSlot].GetCameraTransform();
                
                // code block
                break;
            default:
                // code block
                break;
        }
        
    }
    private bool dimmerBool = false;

    public void CameraDone()
    {
        StartCoroutine(WaitDelay());
    }

    IEnumerator WaitDelay()
    {
        yield return new WaitForSeconds(0.1f);
        
        switch (lastClicked)
        {
            case 0:
                
                TransitionToShowStuff(true);
                // code block
                break;
            case 1:
                //SetCurrentVehicle();
                vehSubMenu.OpenMenu();
                lastClicked = 1;
                if (vehicleSlot < vehicles.Count)
                {
                    vehSubMenu.SetVehicle(vehicles[vehicleSlot]);
                }
                else
                {
                    vehSubMenu.SetVehicle(null);
                }

                // code block
                break;
            default:
                // code block
                break;
        }
        
    }

    public void TurnOffComputer()
    {
        InstantiateObjects();
        TransitionToShowStuff(false);
        //showShowStuff(false);
    }

    public void TransitionToShowStuff(bool x)
    {
        sd.StartDim();
        dimmerBool = x;
        
    }
    public void MaxDim()
    {
        showShowStuff(dimmerBool);
    }

   
    private void MoveCamToTargetLocation()
    {
       
        if (targetTransform != null && cameraGameObj != null)
        {

            // Calculate the target position
            Vector3 targetPosition = targetTransform.position;

            cameraGameObj.transform.position = Vector3.MoveTowards(cameraGameObj.transform.position, targetPosition, movementSpeed * Time.deltaTime);
            // Check if the object needs to move
            if (Vector3.Distance(cameraGameObj.transform.position, targetPosition) < 0.00000000001f)
            {
                targetTransform = null;
                CameraDone();
            }
           
        }
    }
    [SerializeField] private GameObject vehicleSlotTemplate;
    private List<VehicleInstance> vehicles;
    private int vehicleSlot = 0;


    public void NextVehicleSlot()
    {
        vehSubMenu.CloseMenu();
        if (vehicleSlot + 1 < menus.Count )
        {
            vehicleSlot++;
        }
        else
        {
            vehicleSlot = 0;
        }
        SetCurrentVehicle();
    }
    public void PreviousVehicleSlot()
    {
        vehSubMenu.CloseMenu();
        if (vehicleSlot - 1 >=0)
        {
            vehicleSlot--;
        }
        else
        {
            vehicleSlot = menus.Count - 1;
        }
        SetCurrentVehicle();
    }

    private void SetCurrentVehicle()
    {
        lastClicked = 1;
        if(vehicleSlot < vehicles.Count)
        {
            vehSubMenu.SetVehicle(vehicles[vehicleSlot]);
        }
        else
        {
            vehSubMenu.SetVehicle(null);
        }

        targetTransform = vehicleSlotList[vehicleSlot].GetCameraTransform();
    }

    //public GameObject template;        // The template GameObject to instantiate.
    [SerializeField] private Transform firstVehicleSlot; // The starting location where the first object will be placed.
    private int numberOfInstances = SaveSystem.GetNumberOfVehicleSlots();

    private List<VehicleSlotTemplate> vehicleSlotList;
    
    void InstantiateObjects()
    {
        vehicles = SaveVehicleScript.GetVehicles();
        vehicleSlotList = new List<VehicleSlotTemplate>();
        float templateWidth = GetTemplateWidth();
        Vector3 spawnPosition = firstVehicleSlot.position;
       
        for (int i = 0; i < numberOfInstances; i++)
        {
            GameObject newObj = Instantiate(vehicleSlotTemplate, spawnPosition, Quaternion.identity);
            newObj.SetActive(true);
            newObj.transform.parent = gameObject.transform;
            VehicleSlotTemplate temp = newObj.GetComponent<VehicleSlotTemplate>();
            if (i < vehicles.Count)
                temp.SetVehicleInstance(vehicles[i]);
            if (temp != null)
            
                vehicleSlotList.Add(temp);
            
            spawnPosition.z += templateWidth;
            
        }
        endOfWall.transform.position = new Vector3(endOfWall.transform.position.x, endOfWall.transform.position.y, spawnPosition.z + 0.8f);
        vehicleSlotTemplate.SetActive(false);
    }

    float GetTemplateWidth()
    {
        vehicleSlotTemplate.SetActive(true);
        Collider templateCollider = vehicleSlotTemplate.GetComponent<Collider>();
        if (templateCollider != null)
        {
            return templateCollider.bounds.size.z;
        }
        else
        {
            Debug.LogError("Template object does not have a Renderer component.");
            return 1.0f; // Default spacing if the template doesn't have a Renderer.
        }
        vehicleSlotTemplate.SetActive(false);
    }


    private void RotateCamToTarget()
    {
        if (targetTransform != null && cameraGameObj != null)
        {

            // Calculate the desired rotation as a quaternion
            Quaternion targetRotation = targetTransform.rotation;

            // Smoothly rotate towards the target rotation
            cameraGameObj.transform.rotation = Quaternion.Slerp(cameraGameObj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void BackButtonPressed()
    {
        vehSubMenu.SetVehicle(null);
        vehSubMenu.CloseMenu();
        lastClicked = -1;
        targetTransform = cameraPointOfOrigin;
    }
}
