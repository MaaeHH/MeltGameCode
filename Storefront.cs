using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storefront : MonoBehaviour
{
    [SerializeField] private string storeFrontName;
    [SerializeField] private List<InventoryObjectData> itemsToSell;
    [SerializeField] private Transform cameraLocation;
    [SerializeField] private Animator shopKeeperAnim;
    //[SerializeField] private GameObject meltGameObj;
    [SerializeField] private DayNight dayNightScript;
    //[SerializeField] private GameObject closedSign;
    //[SerializeField] private GameObject openSign;

    [SerializeField] private List<GameObject> gamObjsWhenOpen;
    [SerializeField] private List<GameObject> gamObjsWhenClosed;

    private bool opensDuringDay = true;
    public string GetName()
    {
        return storeFrontName;
    }
    public void SetName(string x)
    {
        storeFrontName = x;
    }
    public Transform GetCameraTransform()
    {
        return cameraLocation;
    }

    public List<InventoryObjectData> GetItemsToSell()
    {
        return itemsToSell;
    }
    public bool GetOpensDuringDay()
    {
        return opensDuringDay;
    }

    void Start()
    {
        if(dayNightScript != null)
        dayNightScript.OnWorkingHoursChanged += DayNight_WorkingHoursChanged;
    }
    private void OnDestroy()
    {
        if (dayNightScript != null)
            dayNightScript.OnWorkingHoursChanged -= DayNight_WorkingHoursChanged;
    }

    private void DayNight_WorkingHoursChanged(bool x)
    {
        foreach(GameObject obj in gamObjsWhenOpen)
        {
            obj.SetActive(x);
        }
        foreach (GameObject obj in gamObjsWhenClosed)
        {
            obj.SetActive(!x);
        }
        
    }

}
