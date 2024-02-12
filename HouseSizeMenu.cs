using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseSizeMenu : BMWMenu
{
    [SerializeField] private Animator _menuAnim;

    [SerializeField] private Slider currentXSlider;
    [SerializeField] private Button increaseXButton;
    [SerializeField] private Text xMaxText;
    [SerializeField] private Text xCurrentText;
    [SerializeField] private Text xIncreaseButtonText;


    [SerializeField] private Slider currentYSlider;
    [SerializeField] private Button increaseYButton;
    [SerializeField] private Text yMaxText;
    [SerializeField] private Text yCurrentText;
    [SerializeField] private Text yIncreaseButtonText;


    [SerializeField] private int minHouseSize = 10;

    public HouseSizeMenu GenerateMenu()
    {

        currentXSlider.value = SaveSystem.GetWallSizeX();
        currentYSlider.value = SaveSystem.GetWallSizeY();

        RefreshMenu();
        _menuAnim.SetBool("onScreen", true);

        if (previewObject != null) previewObject.SetActive(true);
        return this;
    }

   

    public override void RefreshMenu()
    {
        RefreshX();
        RefreshY();
    }

    public void RefreshX()
    {
        currentXSlider.minValue = minHouseSize;
        currentXSlider.maxValue = SaveSystem.GetWallSizeLimitX();
        

        xMaxText.text = "X maximum: " + SaveSystem.GetWallSizeLimitX();
        xCurrentText.text = "Current X (" + SaveSystem.GetWallSizeX() + ")";
        xIncreaseButtonText.text = "Increase( " + SaveSystem.GetHouseUpgradeCost() + " screamcoin)";
    }
    public void RefreshY()
    {
        currentYSlider.minValue = minHouseSize;
        currentYSlider.maxValue = SaveSystem.GetWallSizeLimitY();


        yMaxText.text = "Y maximum: " + SaveSystem.GetWallSizeLimitY();
        yCurrentText.text = "Current Y (" + SaveSystem.GetWallSizeY() + ")";
        yIncreaseButtonText.text = "Increase( " + SaveSystem.GetHouseUpgradeCost() + " screamcoin)";
    }

    public void OnXSliderChanged()
    {
        RefreshX();
        SaveSystem.SetWallSizeX(Mathf.RoundToInt(currentXSlider.value));
    }
    public void OnYSliderChanged()
    {
        RefreshY();
        SaveSystem.SetWallSizeY(Mathf.RoundToInt(currentYSlider.value));
    }

    public void IncreaseMaxXPressed()
    {
        Debug.Log(SaveSystem.GetCurrentLevel());
        if(SaveSystem.GetScreamcoin() >= SaveSystem.GetHouseUpgradeCost())
        {
            SaveSystem.SubtractScreamcoin(SaveSystem.GetHouseUpgradeCost());
                SaveSystem.IncreaseMaxWallSizeX();
        }
        RefreshMenu();
    }
    public void IncreaseMaxYPressed()
    {
        if (SaveSystem.GetScreamcoin() >= SaveSystem.GetHouseUpgradeCost())
        {
            SaveSystem.SubtractScreamcoin(SaveSystem.GetHouseUpgradeCost());
                SaveSystem.IncreaseMaxWallSizeY();
        }
        RefreshMenu();
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
