using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
public class MeltSelectorMenu : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;
    private ClickableGridObject data;
    //private MeltScript meltScript;


   
    [SerializeField] private GameObject meltListingTemplate;
    [SerializeField] private Transform meltListingsTransform;
    [SerializeField] private MeltSpawner ms;
    //private ClickableGridObject cgo;
    //private 
    // Start is called before the first frame update

    public override void CloseMenu()
    {
        UnsetData();
    }

    public MeltSelectorMenu GenerateMenu(GameObject obj)
    {
        SetCGO(obj.GetComponent<ClickableGridObject>());
        SetSubjectGameObject(obj);
        RefreshMenu();
        return this;
    }
    public override void RefreshMenu()
    {
        if (data != null)
        {
            Debug.Log("Refreshing menu");

            //objectName.text = data.GetObjectData().GetName();
            //occupiedMelts.text = "There are currently " + data.GetNoOfOccupants() + " melts using this.";
            MakeMeltListings();
        }
    }

    private void MakeMeltListings()
    {
        ClearMeltListings();
        Debug.Log("total melts: "+ ms.GetSpawnedMelts().Count);
        List<MeltScript> result = new List<MeltScript>(ms.GetSpawnedMelts());

        // Iterate through the elements of the second list
        foreach (MeltScript meltScript in data.GetOccupiedMelts())
        {
            // Remove elements from the result list that are also present in the second list
            result.Remove(meltScript);
        }

        
        foreach (MeltScript melt in result)
        {
            MakeMeltListing(melt);
            //x--;
        }
    
    }


    private void MakeMeltListing(MeltScript melt)
    {
        GameObject currentSpawned = Instantiate(meltListingTemplate);



        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<MeltListing>().SetMeltData(melt);

        currentSpawned.SetActive(true);

    }

    public void ClearMeltListings()
    {
        foreach (Transform thing in meltListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

    private void SetCGO(ClickableGridObject x)
    {
        data = x;
        if (GetSelectedController() == null)
        {
            Debug.Log("asd");
        }

        this.GetSelectedController().AddMenu(this);
        _menuAnim.SetBool("onScreen", true);
        //_titleText.text = x.GetActorName();
    }

    public ClickableGridObject GetCGO()
    {
        return data;
    }

    public void UnsetData()
    {
        data = null;
        SetSubjectGameObject(null);
        //ClearMenu();
        _menuAnim.SetBool("onScreen", false);
    }

    private void SetBackGroundColour(Color x)
    {
        _bgImg.color = x;
    }
}
