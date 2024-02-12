using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MeltListing : MonoBehaviour
{
    private MeltScript ms;
    [SerializeField] private Text theText;
    [SerializeField] private Button theButton;
    public void SetMeltData(MeltScript x)
    {
        ms = x;
        theText.text = ms.GetMeltData().GetName();

    }
    public MeltScript GetMeltData()
    {
        return ms;
    }
    public void ButtonPressed()
    {
        ms.thisClicked();
    }
    
}
