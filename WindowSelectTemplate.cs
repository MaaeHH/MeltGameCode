using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectTemplate : MonoBehaviour
{
    private WindowSelect wsmenu;
    private WindowInstance windowInstance;
    [SerializeField] private Text theText;
    public void SetData(WindowSelect menu, WindowInstance data)
    {
        wsmenu = menu;
        windowInstance = data;
        theText.text = windowInstance.GetData().name;
    }
    public void Clicked()
    {
        wsmenu.ButtonClicked(windowInstance);
    }
}
