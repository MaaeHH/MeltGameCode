using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuilderMeltsShop : MonoBehaviour
{
    [SerializeField] private HouseSizeMenu houseSizeMenu;
    [SerializeField] private BuyColoursMenu buyColoursMenu;
    [SerializeField] private DecorateHouseMenu decorateHouseMenu;
    [SerializeField] private EditWindowsMenu editWindowsMenu;
    [SerializeField] private BuyWindowsMenu buyWindowsMenu;
    private BMWMenu currentMenu;
    [SerializeField] private Animator menuSelectAnimator;
    [SerializeField] private Animator backButtonAnimator;
    [SerializeField] private Transform defaultLocation;
    [SerializeField] private GameObject cameraGameObj;
    private float cameraSpeed = 10.0f;
    public float rotationSpeed = 5f;
    bool moving = false;
    public void OpenHouseSizeMenu()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        currentMenu = houseSizeMenu.GenerateMenu();
        menuSelectAnimator.SetBool("onScreen", false);
        backButtonAnimator.SetBool("onScreen", true);
    }
    public void OpenBuyColoursMenu()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        currentMenu = buyColoursMenu.GenerateMenu();
        menuSelectAnimator.SetBool("onScreen", false);
        backButtonAnimator.SetBool("onScreen", true);
    }
    public void OpenDecorateHouseMenu()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        currentMenu = decorateHouseMenu.GenerateMenu();
        menuSelectAnimator.SetBool("onScreen", false);
        backButtonAnimator.SetBool("onScreen", true);
    }
    public void OpenEditWindows()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        currentMenu = editWindowsMenu.GenerateMenu();
        menuSelectAnimator.SetBool("onScreen", false);
        backButtonAnimator.SetBool("onScreen", true);
    }
    public void OpenBuyWindows()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        currentMenu = buyWindowsMenu.GenerateMenu();
        menuSelectAnimator.SetBool("onScreen", false);
        backButtonAnimator.SetBool("onScreen", true);
    }
    public void CloseMenu()
    {
        if (currentMenu != null) currentMenu.CloseMenu();
        menuSelectAnimator.SetBool("onScreen", true);
        backButtonAnimator.SetBool("onScreen", false);
        currentMenu = null;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveCamToTargetLocation();
        RotateCamToTarget();
    }

    private void MoveCamToTargetLocation()
    {
        Vector3 targetLocation = defaultLocation.position;
        if (currentMenu != null) {
            targetLocation = currentMenu.GetCameraLocation().position;
        }

        //Vector3 targetLocation = storefronts[currentIndex].GetCameraTransform().position;
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
        }
    }

    private void RotateCamToTarget()
    {
        Quaternion targetRotation = defaultLocation.rotation;
        if (currentMenu != null)
        {
            targetRotation = currentMenu.GetCameraLocation().rotation;
        }
        // Calculate the desired rotation as a quaternion
        //Quaternion targetRotation = storefronts[currentIndex].GetCameraTransform().rotation;

        // Smoothly rotate towards the target rotation
        cameraGameObj.transform.rotation = Quaternion.Slerp(cameraGameObj.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
