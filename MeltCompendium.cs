using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeltCompendium : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;

    [SerializeField] private GameObject meltCompendiumTemplate;

    [SerializeField] private Transform meltListingsTransform;

    //[SerializeField] private InviteMeltsSubMenu IMM;

    [SerializeField] private MeltSpawner ms;
    private List<MeltData> friendMeltsVisiting;
    public override void CloseMenu()
    {
        _menuAnim.SetBool("onScreen", false);

        ms.ChangeMelts(null);
        UnsetData();
    }

    public MeltCompendium GenerateMenu()
    {
        _menuAnim.SetBool("onScreen", true);
        RefreshMenu();
        return this;
    }
    public override void RefreshMenu()
    {
        //ms.ResetVariables();
        MakeMeltListings();

    }

    public override bool GetLockCam()
    {
        return true;
    }

    private void MakeMeltListings()
    {
        ClearMeltListings();

        MeltData[] meltDataObjects = Resources.LoadAll<MeltData>("MeltDatas");

        foreach(MeltData data in meltDataObjects)
        {
            MakeCompendiumListing(data);
        }

    }

    private void MakeCompendiumListing(MeltData melt)
    {
        GameObject currentSpawned = Instantiate(meltCompendiumTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        

        currentSpawned.SetActive(true);
        currentSpawned.GetComponent<MeltCompendiumListing>().SetData(melt, this);
        currentSpawned.transform.localScale = new Vector3(1, 1, 1);
    }



    public void ClearMeltListings()
    {
        foreach (Transform thing in meltListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

    public void UnsetData()
    {
        //_menuAnim.SetBool("onScreen", false);
    }

    private void SetBackGroundColour(Color x)
    {
        _bgImg.color = x;
    }


    public void ByeClicked(MeltData x)//this is bye clicked
    {
        MeltAssigner.UnlockMelt(x);
        RefreshMenu();
    }

    public void Clicked(MeltData x)
    {
        _menuAnim.SetBool("onScreen", false);
        ms.ChangeMelts(x);
    }

    public void EmptyClicked()
    {
        GetSelectedController().InvMeltSubMenuOpened();
    }
}
