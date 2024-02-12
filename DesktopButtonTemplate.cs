using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopButtonTemplate : MonoBehaviour
{
    [SerializeField] private CheerInc cheerIncScript;
    [SerializeField] private CheerIncMenu associatedMenu;

 
    public void ButtonPressed()
    {
        cheerIncScript.ButtonPressed(associatedMenu);
    }
}
