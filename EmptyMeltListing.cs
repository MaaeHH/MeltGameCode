using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EmptyMeltListing : MonoBehaviour
{
    //private MeltScript ms;
    //[SerializeField] private Text theText;
    [SerializeField] private Button theButton;
    private GridObjectMenu gom;
    public void ButtonPressed()
    {
        //ms.thisClicked();
        gom.OpenMeltSelector();
    }
    public void SetGridObjectMenu(GridObjectMenu x)
    {
        gom = x;
    }
}
