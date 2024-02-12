using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteMeltListing : Listing
{
    

    [SerializeField] private Text meltName;
   
    [SerializeField] private Text sillynessText;
    [SerializeField] private Text kindnessText;
    [SerializeField] private Text cuddlynessText;
    [SerializeField] private Text friendliness;

    [SerializeField] private Slider cheerSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider healthSlider;

    //[SerializeField] private GameObject expSlidAndTitleObject;

    [SerializeField] private Text expSliderTitle;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Text expSliderText;

    private MeltData thisData;
    
    private InviteMeltsMenu theMenu;

    public void SetData(MeltData x, InviteMeltsMenu z)
    {
    
        theMenu = z;
        thisData = x;
        thisData.InitSaveData();
        if (thisData != null)
        {
            Refresh();
        }

    }

    public void Refresh()
    {

        sillynessText.text = thisData.GetSillyness() + "";
        kindnessText.text = thisData.GetKindness() + "";
        cuddlynessText.text = thisData.GetCuddliness() + "";
        friendliness.text = thisData.GetFriendliness() + "";

      
        meltName.text = thisData.GetName();
        

        if (thisData.GetColour() != null)
            ColourStuff(thisData.GetColour());

        UpdateExpBar();
        UpdateStats();
    }

    private void UpdateExpBar()
    {
        if (thisData.GetIsMaxLevel())
        {
           
            expSlider.value = 1;
            expSliderText.text = "";
            expSliderTitle.text = "Friendship level max (" + thisData.GetCurrentLevel() + ")";
        }
        else
        {
            expSliderTitle.text = "Friendship level " + thisData.GetCurrentLevel();
            expSliderText.text = thisData.GetExpOnLevel() + "/" + thisData.GetExpReqForNextLevel();
            expSlider.value = (thisData.GetExpReqForNextLevel() - thisData.GetExpOnLevel()) / thisData.GetExpReqForNextLevel();
        }
    }

    private void UpdateStats()
    {
        cheerSlider.value = 1 - (thisData.GetCheer() / 10);
        hungerSlider.value = 1 - (thisData.GetHunger() / 10);
        energySlider.value = 1 - (thisData.GetEnergy() / 10);
        healthSlider.value = 1 - (thisData.GetHealth() / 10);
    }


    public void Clicked()
    {
        //theMenu.CloseMenu();
        //thisMS.thisClicked();
        theMenu.Clicked(thisData);
    }
    public void ByeClicked()
    {
        theMenu.ByeClicked(thisData);
    }
}
