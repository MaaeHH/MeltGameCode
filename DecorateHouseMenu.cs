using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecorateHouseMenu : BMWMenu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;

    [SerializeField] private GameObject houseColourTemplate;
    [SerializeField] private GameObject houseMaterialTemplate;
    
    [SerializeField] private Transform wallColourTransform;
    [SerializeField] private Transform skirtingColourTransform;
    [SerializeField] private Transform floorColourTransform;
    [SerializeField] private Transform doorColourTransform;

    [SerializeField] private Transform wallMaterialTransform;
    [SerializeField] private Transform skirtingMaterialTransform;
    [SerializeField] private Transform floorMaterialTransform;

    //private 
    // Start is called before the first frame update
    void Start()
    {
    }

    void Update()
    {
    }

    public void ButtonClicked(int buttonID, UnlockableColour theColour)
    {
        SaveSystem.SetHouseColour(theColour, buttonID);
    }
    public void ButtonClicked(int buttonID, UnlockableMaterial theMat)
    {
        SaveSystem.SetHouseMaterial(theMat, buttonID);
    }

    private void MakeButton(int id, UnlockableColour theColour, Transform trns)
    {
        if (theColour != null)
        {
            GameObject theObj = GameObject.Instantiate(houseColourTemplate);

            HouseColourTemplate theScript = theObj.GetComponent<HouseColourTemplate>();
            theScript.SetData(id, this, theColour);
            theObj.transform.SetParent(trns);
            theObj.transform.localScale = new Vector3(1, 1, 1);
            theObj.SetActive(true);
        }
        else { Debug.Log("null colour"); }
    }


    private void MakeMaterialButton(int id, UnlockableMaterial theMat, Transform trns)
    {
        if (theMat != null)
        {
            GameObject theObj = GameObject.Instantiate(houseMaterialTemplate);

            HouseMatTemplate theScript = theObj.GetComponent<HouseMatTemplate>();
            theScript.SetData(id, this, theMat);
            theObj.transform.SetParent(trns);
            theObj.transform.localScale = new Vector3(1, 1, 1);
            theObj.SetActive(true);
        }
    }

    public DecorateHouseMenu GenerateMenu()
    {
        RefreshMenu();
        _menuAnim.SetBool("onScreen", true);
        previewObject.SetActive(true);
        return this;
    }

   

    public override void RefreshMenu()
    {
        ClearTransform(wallColourTransform);
        ClearTransform(floorColourTransform);
        ClearTransform(doorColourTransform);
        ClearTransform(skirtingColourTransform);
        ClearTransform(wallMaterialTransform);
        ClearTransform(floorMaterialTransform);
        ClearTransform(skirtingMaterialTransform);
        Debug.Log(SaveColours.GetUnlockedColours().Count);
        foreach (UnlockableColour col in SaveColours.GetUnlockedColours())
        {
            
            MakeButton(0, col, wallColourTransform);
            MakeButton(1, col, floorColourTransform);
            MakeButton(2, col, doorColourTransform);
            MakeButton(3, col, skirtingColourTransform);
        }
      
        foreach (UnlockableMaterial mat in SaveColours.GetUnlockedMaterials())
        {
            MakeMaterialButton(0, mat, wallMaterialTransform);
            MakeMaterialButton(1, mat, floorMaterialTransform);
            MakeMaterialButton(3, mat, skirtingMaterialTransform);
        }
    }

    private void ClearTransform(Transform theTransform)
    {
        foreach(Transform child in theTransform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }


    public override void CloseMenu()
    {
        previewObject.SetActive(false);
        _menuAnim.SetBool("onScreen", false);
    }


   


    private void SetBackGroundColour(Color x)
    {
        //_bgImg.color = x;
    }


}
