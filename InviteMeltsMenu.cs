using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteMeltsMenu : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;
   

    [SerializeField] private GameObject meltListingNonFriendTemplate;
    [SerializeField] private GameObject meltListingFriendTemplate;
    [SerializeField] private GameObject emptyTemplate;
    [SerializeField] private GameObject dividerTemplate;
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

    public InviteMeltsMenu GenerateMenu()
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


        foreach (MeltData melt in MeltAssigner.GetLockedMelts(MeltRecord.Location.House))
        {

            if (melt.IsFriend())
            {
                MakeFriendMeltListing(melt);
            }
            else
            {
                MakeNonFriendMeltListing(melt);
            }
        }

        for (int x = MeltAssigner.GetLockedMelts(MeltRecord.Location.House).Count; x < SaveSystem.GetFriendMeltHouseSlots(); x++)
        {
            MakeEmptyMeltListing();
        }
        MakeDivider();
        foreach (MeltData melt in MeltAssigner.GetCurrentMelts(MeltRecord.Location.House, SaveSystem.GetMeltHouseSlots()))
        {
            if (melt != null)
            {
                if (melt.IsFriend())
                {
                    MakeFriendMeltListing(melt);
                }
                else
                {
                    MakeNonFriendMeltListing(melt);
                }
            }
        }

    }


    private void MakeNonFriendMeltListing(MeltData melt)
    {
        GameObject currentSpawned = Instantiate(meltListingNonFriendTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<InviteMeltListingNotFriend>().SetData(melt, this);

        currentSpawned.SetActive(true);

        currentSpawned.transform.localScale = new Vector3(1, 1, 1);
    }

    private void MakeFriendMeltListing(MeltData melt)
    {
        GameObject currentSpawned = Instantiate(meltListingFriendTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<InviteMeltListing>().SetData(melt, this);

        currentSpawned.SetActive(true);

        currentSpawned.transform.localScale = new Vector3(1, 1, 1);
    }

    private void MakeEmptyMeltListing()
    {
        GameObject currentSpawned = Instantiate(emptyTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<InviteMeltListingEmpty>().SetData(this);

        currentSpawned.SetActive(true);

        currentSpawned.transform.localScale = new Vector3(1, 1, 1);
    }



    private void MakeDivider()
    {
        GameObject currentSpawned = Instantiate(dividerTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.SetActive(true);

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
