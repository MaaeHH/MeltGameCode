using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyWindowsMenu : BMWMenu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] List<WindowData> windowsToSell = new List<WindowData>();
   

    private List<WindowData> buyableWindows;


    [SerializeField] private Text windowName;
    [SerializeField] private Text windowDesc;
    [SerializeField] private Text windowPrice;
    [SerializeField] private Text inInventory;

    //[SerializeField] private Image windowIcon;
    [SerializeField] private GameObject buyWindowTemplate;
    [SerializeField] private Transform theContentTransform;
    [SerializeField] private BuyWindowPreview bwp;
    private WindowData currentWindow;

    public BuyWindowsMenu GenerateMenu()
    {
        RefreshMenu();
        _menuAnim.SetBool("onScreen", true);
        if (previewObject != null) previewObject.SetActive(true);
        return this;
    }

    public void ButtonPressed(WindowData x)
    {
        currentWindow = x;
        bwp.SetPreview(x.winObj);
        RefreshMenu();
    }


    private List<WindowData> MakeBuyableWindows()
    {
        List<WindowData> theList = new List<WindowData>();

        foreach(WindowData uc in windowsToSell)
        {
            if(SaveSystem.GetCurrentLevel() >= uc.levelRequired)
            {
                
                    theList.Add(uc);
            }
        }
        return theList;
    }

    
    public override void RefreshMenu()
    {
        buyableWindows = MakeBuyableWindows();
        ClearWindowListings();
        foreach (WindowData dat in buyableWindows)
        {
            MakeWindowListing(dat);
        }

        if(currentWindow != null)
        {
            windowName.text = currentWindow.name;
            windowDesc.text = currentWindow.description;
            windowPrice.text = currentWindow.price + "";
            inInventory.text = "In inventory: "  + SaveColours.GetCountOf(currentWindow);
        }
        else
        {
            windowName.text = "";
            windowDesc.text = "";
            windowPrice.text = "";
            inInventory.text = "";
        }
      

    }

    public void Buy()
    {
        Debug.Log("A");
        if (currentWindow != null)
        {
            Debug.Log("B");
            if (SaveSystem.GetScreamcoin() >= currentWindow.price)
            {
                Debug.Log("C");
                SaveSystem.SubtractScreamcoin(currentWindow.price);
           
                SaveColours.AddWindow(new WindowInstance(currentWindow));

            }

            RefreshMenu();
        }
    }

    private void ClearWindowListings()
    {
        foreach (Transform child in theContentTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    private void MakeWindowListing(WindowData x)
    {
        GameObject theObj = GameObject.Instantiate(buyWindowTemplate);

        BuyWindowListing theScript = theObj.GetComponent<BuyWindowListing>();
        theScript.SetData(this, x);
        theObj.transform.SetParent(theContentTransform);
        theObj.transform.localScale = new Vector3(1, 1, 1);
        theObj.SetActive(true);
        
    }

    public List<WindowData> GetBuyableWindows()
    {
        buyableWindows = MakeBuyableWindows();
        return buyableWindows;
    }
   

    public override void CloseMenu()
    {
        if (previewObject != null) previewObject.SetActive(false);
        _menuAnim.SetBool("onScreen", false);
    }


   


    private void SetBackGroundColour(Color x)
    {
        //_bgImg.color = x;
    }


}
