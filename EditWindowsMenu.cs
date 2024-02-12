using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EditWindowsMenu : BMWMenu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Text titleText;
    [SerializeField] private GameObject windowListingTemplate;
    [SerializeField] private Text windowTitle;
    [SerializeField] private Slider xSlider;
    [SerializeField] private Slider ySlider;
    [SerializeField] private Transform listingsTransform;
    [SerializeField] private int currentWall = 0;
    [SerializeField] private GameObject placementSelect;
    private WindowInstance currentWindowInstance;
    private int currentListingIndex;

    [SerializeField] private WindowSelect ws;
    [SerializeField] private BMWMenu subMenu;

    [SerializeField] private WindowPreview wp;

    private bool slidersActive = true;
    //private 
    // Start is called before the first frame update
    void Start()
    {
        UpdateShowSliders();
    }

    void Update()
    {

    }

    private void UpdateShowSliders()
    {
        placementSelect.SetActive(currentWindowInstance != null);
        wp.SetPointerVisibility(currentWindowInstance != null);
        if (currentWindowInstance != null) 
        wp.SetPointerLocation(xSlider.value, ySlider.value);
    }

    public void OnSliderUpdate()
    {
        if (slidersActive)
        {

            if (currentWindowInstance != null)
            {
                currentWindowInstance.height = ySlider.value;
                currentWindowInstance.xPos = xSlider.value;
                wp.MakeWindows(currentWall);
            }
        }
        wp.SetPointerLocation(xSlider.value, ySlider.value);
    }

    public void PlusPressed()
    {
        subMenu = ws.GenerateMenu();
    }

    public void WindowSelected(WindowInstance winIns)
    {
        if (winIns != null)
        {
            SaveColours.RemoveWindow(winIns);
            SaveSystem.AddWindow(currentWall, winIns);
        }
        wp.MakeWindows(currentWall);
        ws.CloseMenu();
        UpdateShowSliders();
        RefreshMenu();
    }

    public void MinusPressed()
    {
        if(currentWindowInstance != null)
        {
            SaveColours.AddWindow(currentWindowInstance);
            SaveSystem.RemoveWindow(currentWall, currentWindowInstance);
            RefreshMenu();
            wp.MakeWindows(currentWall);
        }
        UpdateShowSliders();
    }

    public void NextWall()
    {
        currentWall++;
        if(currentWall > 3)
        {
            currentWall = 0;
        }
        currentWindowInstance = null;
        currentListingIndex = -1;
        wp.MakeWindows(currentWall);
        RefreshMenu();
        UnclickListings();
        UpdateShowSliders();
    }
    public void PrevWall()
    {
        currentWall--;
        if(currentWall < 0)
        {
            currentWall = 3;
        }
        currentWindowInstance = null;
        currentListingIndex = -1;
        wp.MakeWindows(currentWall);
        RefreshMenu();
        UnclickListings();
        UpdateShowSliders();
    }

    public EditWindowsMenu GenerateMenu()
    {
        RefreshMenu();
        UnclickListings();
        wp.MakeWindows(currentWall);
        UpdateShowSliders();
        if (previewObject != null) previewObject.SetActive(true);
        _menuAnim.SetBool("onScreen", true);

        return this;
    }

    private void ClearMenu()
    {
        foreach(Transform t in listingsTransform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }
   
    private void MakeListing(int index, WindowInstance theInstance)
    {
        GameObject theListing = GameObject.Instantiate(windowListingTemplate);
        theListing.transform.SetParent(listingsTransform);
        WindowListing wl = theListing.GetComponent<WindowListing>();

        wl.SetData(index, this, theInstance);
        theListing.transform.localScale = new Vector3(1, 1, 1);
        theListing.SetActive(true);
        
        if (currentWindowInstance != null)
        {
            if (wl.GetInstance().GetData() == currentWindowInstance.GetData())
            {
                wl.ListingClicked();
            }
            else
            {
                wl.UnClick();
            }
        }
    }

    private void UnclickListings()
    {
        foreach (Transform t in listingsTransform)
        {
            t.GetComponent<WindowListing>().UnClick();
        }
    }


    public void ListingClicked(int x, WindowInstance y)
    {

        slidersActive = false;
        UnclickListings();
        
        ySlider.value = y.height;
        xSlider.value = y.xPos;

        currentWindowInstance = y;
        currentListingIndex = x;

       

        Debug.Log(y.xPos);
        windowTitle.text = currentListingIndex + ": " + currentWindowInstance.GetData().name;
        slidersActive = true;
        UpdateShowSliders();
    }

    public override void RefreshMenu()
    {
        if (currentWindowInstance == null)
            windowTitle.text = "Click a window";
        titleText.text = "Windows menu (wall " + currentWall + ")";
        ClearMenu();
        int index = 0;
        foreach (WindowInstance dat in SaveSystem.GetWindows(currentWall))
        {
            MakeListing(index, dat);
            index++;
        }
    }


    public override void CloseMenu()
    {
        if (subMenu != null) subMenu.CloseMenu();
        if (previewObject != null) previewObject.SetActive(false);
        _menuAnim.SetBool("onScreen", false);
    }

    public void SaveWindows()
    {
        SaveSystem.Save();
    }
   


    private void SetBackGroundColour(Color x)
    {
        //_bgImg.color = x;
    }

    /*public void SetCurrentWindowInstance(WindowData x)
    {

    }*/

}
