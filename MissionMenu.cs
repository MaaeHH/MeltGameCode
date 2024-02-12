using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionMenu : CheerIncMenu
{
    [SerializeField] private List<MissionInstance> missions;
    //[SerializeField] private int noSlots;
    [SerializeField] private Animator _menuAnim;
    //[SerializeField] private Image _bgImg;
    //private MeltScript meltScript;

    [SerializeField] private GameObject missionListingTemplate;
    [SerializeField] private Transform missionListingsTransform;

    [SerializeField] private MissionSubMenu subMenu;


    //private bool subMenuOpen = false;
    //[SerializeField] private MissionAssigner missionAssign;

    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        gameObject.SetActive(true);
        RefreshMenu();
        // MakeListings();
    }
    public override void RefreshMenu()
    {
        ClearListings();
        MakeListings();

    }

    public void SetMissions(List<MissionInstance> mis)
    {
        missions = mis;
    }

    void Update()
    {
        //MakeListings();
    }

    private void MakeListings()
    {
        missions = MissionAssigner.GetCurrentMissions(3);
        //missions = missionAssign.GetCurrentMissions(3);
        foreach (MissionInstance mission in missions)
        {
            if(mission.GetData() != null)
            MakeMissionListing(mission);
            //Debug.Log(mission.GetData().GetTitle());
        }


    }

    

    public void MissionPressed(MissionInstance x)
    {
        subMenu.SetMission(x);
        subMenu.OpenMenu();
        
        CloseMenu();
    }
    public void CloseSubMenu()
    {
        subMenu.CloseMenu();
        OpenMenu();
    }

    private void MakeMissionListing(MissionInstance mission)
    {
        GameObject currentSpawned = Instantiate(missionListingTemplate);
       
        currentSpawned.GetComponent<MissionListing>().SetData(mission, this);
        currentSpawned.transform.SetParent(missionListingsTransform);
        currentSpawned.transform.localScale = (new Vector3(1,1,1));
        currentSpawned.SetActive(true);

    }

    public void ClearListings()
    {
        foreach (Transform thing in missionListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }
}
