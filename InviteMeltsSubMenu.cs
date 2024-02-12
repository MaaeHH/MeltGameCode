using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InviteMeltsSubMenu : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;
   
    [SerializeField] private GameObject meltListingFriendTemplate;
  
    [SerializeField] private Transform meltListingsTransform;


    [SerializeField] private MeltSpawner ms;
    private List<MeltData> friendMeltsVisiting;

    public override bool GetLockCam()
    {
        return true;
    }

    public override void CloseMenu()
    {
        _menuAnim.SetBool("onScreen", false);
        UnsetData();
    }

    public InviteMeltsSubMenu GenerateMenu()
    {
        _menuAnim.SetBool("onScreen", true);
        RefreshMenu();
        return this;
    }
    public override void RefreshMenu()
    {
      
        MakeMeltListings();
        
    }



    private void MakeMeltListings()
    {
        ClearMeltListings();

        foreach(MeltData melt in MeltAssigner.GetAllFriendMeltNoMission())
        {
            if (!IsStaying(melt))
            {
                MakeFriendMeltListing(melt);
            }
        }
    }

    private bool IsStaying(MeltData x)
    {
        foreach(MeltData md in MeltAssigner.GetLockedMelts(MeltRecord.Location.House))
        {
            if (md == x)
                return true;
        }
        return false;
    }
    

  
    private void MakeFriendMeltListing(MeltData melt)
    {
        GameObject currentSpawned = Instantiate(meltListingFriendTemplate);

        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<InviteMeltsSubListing>().SetData(melt, this);

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
        _menuAnim.SetBool("onScreen", false);
    }

    private void SetBackGroundColour(Color x)
    {
        _bgImg.color = x;
    }


    public void ListingClicked(MeltData x)
    {
        MeltAssigner.LockMeltToLocation(x, MeltRecord.Location.House);

        GetSelectedController().EscapePressed();
    }


}
