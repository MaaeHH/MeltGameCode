using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterestController : MonoBehaviour
{
    [SerializeField] private List<PointOfInterest> pois;

    private float checkInterval = 0.3f;

    [SerializeField] private float focalPointDistanceLimit;
    [SerializeField] private MapCameraControl mcc;
    [SerializeField] private SceneLoader sl;
    [SerializeField] private Transform locationMenuTransform;
    [SerializeField] private GameObject locationMenuTemplate;
    [SerializeField] private Animator locationMenuAnimator;
    private bool buttonHovered;
    PointOfInterest activeFocalPoint;
    // Start is called before the first frame update
    void Start()
    {
        MakeMenu();
        foreach (PointOfInterest poi in pois)
        {
            poi.SetMCC(mcc);
        }
        InvokeRepeating("CheckDistance", 0, checkInterval); // Start the repeating check
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void MakeMenu()
    {
        ClearMenu();
        foreach (PointOfInterest poi in pois)
        {
            GameObject currentSpawned = Instantiate(locationMenuTemplate);

            currentSpawned.transform.SetParent(locationMenuTransform);

            currentSpawned.GetComponent<LocationButton>().SetPoi(poi);
            currentSpawned.GetComponent<LocationButton>().SetPoic(this);

            currentSpawned.SetActive(true);
        }
    }
    public void ToggleMenu()
    {
        locationMenuAnimator.SetTrigger("ToggleMenu");
    }

    public void ButtonPressed(PointOfInterest x)
    {
        sl.ChangeLevel(x.GetTransition(), x.GetScene());
    }
    public void ButtonHovered(PointOfInterest x)
    {
        mcc.MakeLookAt(x.GetPoint());
        mcc.SetUseScreenEdge(false);
        buttonHovered = true;
    }
    public void ButtonExit(PointOfInterest x)
    {
        mcc.SetUseScreenEdge(true);
        buttonHovered = false;
    }
    public void ButtonHovered()
    {
        //mcc.MakeLookAt(x.transform);
        mcc.SetUseScreenEdge(false);
        buttonHovered = true;
    }
    public void ButtonExit()
    {
        mcc.SetUseScreenEdge(true);
        buttonHovered = false;
    }

    private void ClearMenu()
    {
        foreach(Transform x in locationMenuTransform)
        {
            GameObject.Destroy(x.gameObject);
        }
    }


    private void CheckDistance()
    {
        float distance = -1;
        PointOfInterest closest = null;
        if (pois.Count > 0) { //If we have more than 0 possible focal points (we should)
            distance = pois[0].GetDistanceToFocalPoint();
            //Debug.Log(distance);
            closest = pois[0];//Then store the first focal point and the distance to it

            foreach (PointOfInterest poi in pois)//For each point of interest 
            {
                float poiDistance = poi.GetDistanceToFocalPoint();

                if (poiDistance < distance)//compare the distance, if its closer
                {
                    closest = poi;//it becomes the new closest
                    distance = poiDistance;
                }
            }
        }

        if (distance < focalPointDistanceLimit) //If it is less than the focal point limit
        {
            if (closest != activeFocalPoint) //If its not already the active focalpoint
            {
                if (activeFocalPoint != null) //If its not the first time the focalpoint was set,
                {
                    NewPOI(activeFocalPoint, closest); 
                    activeFocalPoint = closest;
                }
                else
                {
                    NewPOI(closest);
                    activeFocalPoint = closest;
                }

            }
        }
        else
        {
            if (activeFocalPoint != null) //If its not the first time the focalpoint was set,
            {
                activeFocalPoint.deactivateUIElement();
                activeFocalPoint = null;
            }
        }

    }

    private void NewPOI(PointOfInterest prevpoi, PointOfInterest newpoi)
    {
        prevpoi.deactivateUIElement();
        newpoi.activateUIElement();

    }

    private void NewPOI(PointOfInterest newpoi)
    {
        
        newpoi.activateUIElement();

    }

    public void Activated()
    {
        if(activeFocalPoint != null && buttonHovered == false)
        {
            sl.ChangeLevel(activeFocalPoint.GetTransition(), activeFocalPoint.GetScene());
        }
    }
   
}
