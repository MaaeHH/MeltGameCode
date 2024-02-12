using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class InProgressMenu : CheerIncMenu
{
    private List<InProgressMission> missions;
    //[SerializeField] private int noSlots;
    [SerializeField] private Animator _menuAnim;
    //[SerializeField] private Image _bgImg;
    //private MeltScript meltScript;

    [SerializeField] private GameObject missionInProgressTemplate;
    [SerializeField] private GameObject missionCompleteTemplate;
    [SerializeField] private GameObject oldMissionTemplate;
    [SerializeField] private Transform missionListingsTransform;

    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        gameObject.SetActive(true);
        MakeListings();
    }
    public override void RefreshMenu()
    {
        ClearListings();
        MakeListings();

    }

    public void SetMissions(List<InProgressMission> mis)
    {
        missions = mis;
    }


    private void MakeListings()
    {
        missions = MissionAssigner.GetInProgressMissions();
        foreach (InProgressMission mission in missions)
        {

            MakeMissionListing(mission);
        }
       

    }

    public bool CompleteMission(InProgressMission x)
    {
        Debug.Log("asd");
        return true;
    }

    private void MakeMissionListing(InProgressMission mission)
    {
        GameObject currentSpawned;
        if (mission.GetRewardsRecieved() && DateTime.Now > mission.GetCompletionTime())
        {
            currentSpawned = Instantiate(oldMissionTemplate);
        }
        else if (!mission.GetRewardsRecieved() && DateTime.Now > mission.GetCompletionTime())
        {
            currentSpawned = Instantiate(missionCompleteTemplate);
        }
        else
        {
            currentSpawned = Instantiate(missionInProgressTemplate);
        }
                
       
        currentSpawned.GetComponent<MissionInProgressListing>().SetData(mission, this);
        currentSpawned.transform.SetParent(missionListingsTransform);
        currentSpawned.transform.localScale = (new Vector3(1, 1, 1));
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
