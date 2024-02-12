using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStuff : MonoBehaviour
{
    [SerializeField] private GameObject expSlidAndTitleObject;

    [SerializeField] private Text expSliderTitle;
    [SerializeField] private Slider expSlider;
    [SerializeField] private Text expSliderText;

    [SerializeField] private GameObject nextLevelGameObj;
    [SerializeField] private Text currentText;

    [SerializeField] private Text currentLevelSillyness;
    [SerializeField] private Text currentLevelKindness;
    [SerializeField] private Text currentLevelCuddlyness;
    [SerializeField] private Text currentLevelFriendliness;

    [SerializeField] private Text nextLevelSillyness;
    [SerializeField] private Text nextLevelKindness;
    [SerializeField] private Text nextLevelCuddlyness;
    [SerializeField] private Text nextLevelFriendliness;

    private MeltData thisData;


    public void SetData(MeltData x)
    {
        thisData = x;
        UpdateText();
    }

    private void UpdateText()
    {
        

        currentLevelSillyness.text = thisData.GetSillyness() + "";
        currentLevelKindness.text = thisData.GetKindness() + "";
        currentLevelCuddlyness.text = thisData.GetCuddliness() + "";
        currentLevelFriendliness.text = thisData.GetFriendliness() + "";

        if (thisData.GetIsMaxLevel())
        {
            currentText.text = "level max (" + thisData.GetCurrentLevel() + ")";
            nextLevelGameObj.SetActive(false);
            expSlidAndTitleObject.SetActive(false);
        }
        else
        {
            expSlidAndTitleObject.SetActive(true);
            expSliderTitle.text = "Friendship level " + thisData.GetCurrentLevel();
            expSliderText.text = thisData.GetExpOnLevel() +"/"+ thisData.GetExpReqForNextLevel() ;
            expSlider.value = (thisData.GetExpReqForNextLevel()-thisData.GetExpOnLevel()) / thisData.GetExpReqForNextLevel();
            currentText.text = "current";
            nextLevelGameObj.SetActive(true);
            nextLevelSillyness.text = thisData.GetAllSillyness()[thisData.GetCurrentLevel() + 1] +"";
            nextLevelKindness.text = thisData.GetAllKindness()[thisData.GetCurrentLevel() + 1] + "";
            nextLevelCuddlyness.text = thisData.GetAllCuddliness()[thisData.GetCurrentLevel() + 1] + "";
            nextLevelFriendliness.text = thisData.GetAllFriendliness()[thisData.GetCurrentLevel() + 1] + "";
        }
        
    }
    
}
