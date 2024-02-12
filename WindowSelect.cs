using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSelect : BMWMenu
{
    [SerializeField] GameObject windowSelectTemplate;
    [SerializeField] private Transform listingsTransform;
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private EditWindowsMenu ewm;
  

    public void ButtonClicked(WindowInstance data)
    {
        ewm.WindowSelected(data);
    }
    public void Cancel()
    {
        ewm.WindowSelected(null);
    }

    public override void RefreshMenu()
    {
        ClearMenu();
       
        foreach (WindowInstance dat in SaveColours.GetInventory())
        {
            MakeListing(dat);
        }
    }


    public override void CloseMenu()
    {
        _menuAnim.SetBool("onScreen", false);
    }

    public WindowSelect GenerateMenu()
    {
        RefreshMenu();

        _menuAnim.SetBool("onScreen", true);
        return this;
    }

    private void ClearMenu()
    {
        foreach (Transform t in listingsTransform)
        {
            GameObject.Destroy(t.gameObject);
        }
    }

    private void MakeListing(WindowInstance theInstance)
    {
        GameObject theListing = GameObject.Instantiate(windowSelectTemplate);
        theListing.transform.SetParent(listingsTransform);
        WindowSelectTemplate wl = theListing.GetComponent<WindowSelectTemplate>();

        wl.SetData(this, theInstance);

        theListing.SetActive(true);
    }
}
