using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FriendshipReqTemplate : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descText;
    [SerializeField] private Text barText;
    [SerializeField] private Slider progressSlider;

    private FriendshipRequirement fr;

    public void SetFriendshipReq(FriendshipRequirement x)
    {
        fr = x;
        Refresh();
    }
    
    public void Refresh()
    {
        titleText.text = fr.GetData().GetName();
        descText.text = fr.GetData().GetDescription();
        barText.text = fr.GetProgress() + "/" + (fr.GetRequiredNum() - fr.GetStartingNum());
        if ((fr.GetRequiredNum() - fr.GetStartingNum()) == 0)
        {
            progressSlider.value = 1.0f;
        }
        else
        {
            progressSlider.value = fr.GetProgress() / (fr.GetRequiredNum() - fr.GetStartingNum());
        }
       
    }
}
