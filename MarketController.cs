using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketController : MonoBehaviour
{
    [SerializeField] private List<Storefront> storefronts;
    [SerializeField] private StorefrontMenu sfm;
    [SerializeField] private GameObject cameraGameObj;
    [SerializeField] private Button leftButton;
    [SerializeField] private Button rightButton;
    [SerializeField] private Animator menuAnim;
    [SerializeField] private InventorySaveSystem invSaveSys;
    [SerializeField] private DayNight dayNightScript;
    private float cameraSpeed = 10.0f;
    public float rotationSpeed = 5f;
    private int currentIndex = 0;
    private bool isWorkingHours = false;
    bool moving = false;
    // Start is called before the first frame update
    void Start()
    {
        invSaveSys.Initialize();
        //if (SaveMeltSystem.LoadInventory("InventorySave") == null)
        //{
        //    Debug.Log("Null save");
        //}
        //sfm.SetSave(SaveMeltSystem.LoadInventory("InventorySave"));
        sfm.SetStorefront(storefronts[currentIndex]);
        leftButton.onClick.AddListener(PreviousStall);
        rightButton.onClick.AddListener(NextStall);
        dayNightScript.OnWorkingHoursChanged += DayNight_WorkingHoursChanged;

        //StartCoroutine(StartEnum());
    }
    private void OnDestroy()
    {
        dayNightScript.OnWorkingHoursChanged -= DayNight_WorkingHoursChanged;
    }

    private void DayNight_WorkingHoursChanged(bool working)
    {
        isWorkingHours = working;
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamToTargetLocation();
        RotateCamToTarget();
    }

    public void NextStall()
    {
        //Debug.Log("next");
        if(currentIndex != storefronts.Count - 1)
        {
            currentIndex++;
            //menuAnim.SetBool("onScreen", false);

            if (isWorkingHours == storefronts[currentIndex].GetOpensDuringDay())
            {
                menuAnim.SetBool("onScreen", false);
            }

            sfm.SetStorefront(storefronts[currentIndex]);
        }        
    }



    public void PreviousStall()
    {
        //Debug.Log("previous");
        if (currentIndex != 0)
        {
            currentIndex--;

            if (isWorkingHours == storefronts[currentIndex].GetOpensDuringDay())
            {
                menuAnim.SetBool("onScreen", false);
            }
            //menuAnim.SetBool("onScreen", false);
           
            sfm.SetStorefront(storefronts[currentIndex]);
        }
    }

    private void MoveCamToTargetLocation()
    {
        Vector3 targetLocation = storefronts[currentIndex].GetCameraTransform().position;
        if (Vector3.Distance(cameraGameObj.transform.position, targetLocation) > 0.1)
        {
            Vector3 startingPos = cameraGameObj.transform.position;
            float step = cameraSpeed * 1.4f * Time.deltaTime;
            Vector3 aaa = Vector3.MoveTowards(cameraGameObj.transform.position, targetLocation, step);

            moving = (Vector3.Distance(startingPos, aaa) < step);

            cameraGameObj.transform.position = aaa;

            moving = true;
        }
        else
        {
            moving = false;

            if (isWorkingHours == storefronts[currentIndex].GetOpensDuringDay())
            {
                menuAnim.SetBool("onScreen", true);
            }

            
        }
    }

    private void RotateCamToTarget()
    {
        
        // Calculate the desired rotation as a quaternion
        Quaternion targetRotation = storefronts[currentIndex].GetCameraTransform().rotation;

        // Smoothly rotate towards the target rotation
        cameraGameObj.transform.rotation = Quaternion.Slerp(cameraGameObj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
