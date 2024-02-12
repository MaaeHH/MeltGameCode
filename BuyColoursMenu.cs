using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuyColoursMenu : BMWMenu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] List<UnlockableColour> coloursToSell = new List<UnlockableColour>();
    [SerializeField] List<UnlockableMaterial> materialsToSell = new List<UnlockableMaterial>();

    private List<UnlockableColour> buyableColours;
    private List<UnlockableMaterial> buyableMaterials;

    private int buyableColourIndex = 0;
    private int buyableMaterialIndex = 0;

    [SerializeField] private Text coloursPrice;
    [SerializeField] private Text coloursDescription;
    [SerializeField] private Text coloursTitle;
    [SerializeField] private Image coloursImage;

    [SerializeField] private Text materialsPrice;
    [SerializeField] private Text materialsDescription;
    [SerializeField] private Text materialsTitle;
    [SerializeField] private Image materialsImage;

    [SerializeField] private Button buyColours;
    [SerializeField] private Button buyMaterials;

    public BuyColoursMenu GenerateMenu()
    {
        RefreshMenu();
        _menuAnim.SetBool("onScreen", true);
        return this;
    }

    private void SetColourDisplay()
    {
        if (buyableColours.Count != 0) {
            if (buyableColourIndex >= buyableColours.Count) buyableColourIndex = buyableColours.Count - 1;

            UnlockableColour currentColour = buyableColours[buyableColourIndex];
            coloursPrice.text = "$" + currentColour.price;
            coloursDescription.text = currentColour.description;
            coloursTitle.text = currentColour.name;
            coloursImage.color = currentColour.colour;

            buyColours.interactable = true;
            coloursImage.gameObject.SetActive(true);
        }
        else
        {
            buyColours.interactable = false;
            coloursImage.gameObject.SetActive(false);
            coloursDescription.text = "You have bought everything.";
            coloursTitle.text = "Sold out";
        }
    }

    private void SetMaterialDisplay()
    {
        if (buyableMaterials.Count != 0) {

            if (buyableMaterialIndex >= buyableMaterials.Count) buyableMaterialIndex = buyableMaterials.Count - 1;

            UnlockableMaterial currentMaterial = buyableMaterials[buyableMaterialIndex];
            materialsPrice.text = "$" + currentMaterial.price;
            materialsDescription.text = currentMaterial.description;
            materialsTitle.text = currentMaterial.name;


            materialsImage.sprite = Sprite.Create((Texture2D)currentMaterial.material.mainTexture, new Rect(0, 0, currentMaterial.material.mainTexture.width, currentMaterial.material.mainTexture.height), new Vector2(0.5f, 0.5f));

            materialsImage.gameObject.SetActive(true);
            buyMaterials.interactable = true;
        }
        else
        {
            materialsImage.gameObject.SetActive(false);
            buyMaterials.interactable = false;
            materialsDescription.text = "You have bought everything.";
            materialsTitle.text = "Sold out";
        }
    }

    public void NextColour()
    {
        buyableColourIndex++;
        if (buyableColourIndex >= buyableColours.Count)
            buyableColourIndex = 0;

        SetColourDisplay();
    }
    public void PrevColour()
    {
        buyableColourIndex--;
        if (buyableColourIndex <= 0)
            buyableColourIndex = buyableColours.Count-1;

        SetColourDisplay();
    }

    public void BuyColour()
    {
        UnlockableColour currentColour = buyableColours[buyableColourIndex];
        if (SaveSystem.GetScreamcoin() >= currentColour.price)
        {
            SaveSystem.SubtractScreamcoin(currentColour.price);
            SaveColours.UnlockColour(currentColour);
        }
        
        RefreshMenu();
    }

    public void NextMaterial()
    {
        buyableMaterialIndex++;
        if (buyableMaterialIndex >= buyableMaterials.Count)
            buyableMaterialIndex = 0;

        SetMaterialDisplay();
    }
    public void PrevMaterial()
    {
        buyableMaterialIndex--;
        if (buyableMaterialIndex <= 0)
            buyableMaterialIndex = buyableMaterials.Count - 1;

        SetMaterialDisplay();
    }
    public void BuyMaterial()
    {
        UnlockableMaterial currentMaterial = buyableMaterials[buyableMaterialIndex];
        if(SaveSystem.GetScreamcoin() >= currentMaterial.price)
        {
            SaveSystem.SubtractScreamcoin(currentMaterial.price);
            SaveColours.UnlockMaterial(currentMaterial);
        }

        RefreshMenu();
    }


    private List<UnlockableColour> MakeBuyableColours()
    {
        List<UnlockableColour> theList = new List<UnlockableColour>();

        foreach(UnlockableColour uc in coloursToSell)
        {
            if(SaveSystem.GetCurrentLevel() >= uc.levelRequired)
            {
                if (!SaveColours.IsUnlocked(uc)) 
                    theList.Add(uc);
            }
        }
        return theList;
    }
    private List<UnlockableMaterial> MakeBuyableMaterials()
    {
        List<UnlockableMaterial> theList = new List<UnlockableMaterial>();

        foreach (UnlockableMaterial um in materialsToSell)
        {
            if (SaveSystem.GetCurrentLevel() >= um.levelRequired)
            {
                if (!SaveColours.IsUnlocked(um))
                    theList.Add(um);
            }
        }
        return theList;
    }
    
    public override void RefreshMenu()
    {
        buyableColours = MakeBuyableColours();
        buyableMaterials = MakeBuyableMaterials();
        SetColourDisplay();
        SetMaterialDisplay();
    }

    public List<UnlockableColour> GetBuyableColours()
    {
        return buyableColours;
    }
    public List<UnlockableMaterial> GetBuyableMaterials()
    {
        return buyableMaterials;
    }

    public override void CloseMenu()
    {
        _menuAnim.SetBool("onScreen", false);
    }


   


    private void SetBackGroundColour(Color x)
    {
        //_bgImg.color = x;
    }


}
