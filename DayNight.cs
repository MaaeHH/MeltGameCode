using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DayNight : MonoBehaviour
{

    [SerializeField] private Material daybreakBox;
    [SerializeField] private Material eveningBox;
    [SerializeField] private Material middayBox;
    [SerializeField] private Material midnightBox;
    [SerializeField] private Material sunsetBox;
    [SerializeField] private MeltSpawner ms;
    [SerializeField] private TileObjectController toc;
    [SerializeField] private Light theSun;

    private TimeOfDay currentTime;
    private bool workingHours;

    public event TimeOfDayChanged OnTimeChanged;
    public delegate void TimeOfDayChanged(TimeOfDay newTimeOfDay);

    public event WorkingHoursChanged OnWorkingHoursChanged;
    public delegate void WorkingHoursChanged(bool currentlyWorkingHours);

    [SerializeField] private List<GameObject> dayObjs;
    [SerializeField] private List<GameObject> nightObjs;
    [SerializeField] private List<GameObject> lateNightObjs;


    // Start is called before the first frame update
    void Start()
    {
        prevCheckIsDay = IsDaytime();
        UpdateSkybox();
        StartCoroutine(DayCheck());
        





    }

    // Update is called once per frame
    void Update()
    {
        //TimeOfDayChecker.TimeOfDay currentTimeOfDay = timeOfDayChecker.GetCurrentTimeOfDay();

        //makeMeltsSleep();

    }
    public TimeOfDay getCurrentTime()
    {
        return currentTime;
    }


    private void makeMeltsSleep()
    {
        if (ms && toc != null)
        {
            foreach (ClickableGridObject cgo in toc.GetAllObjectsOfType(GridObjectData.ObjectType.Bed))
            {
                //Debug.Log("a");
                foreach (MeltScript melt in ms.GetSpawnedMelts())
                {

                    //Debug.Log("There are currently " + ms.GetSpawnedMelts().Count +  " spawned melts.");
                    if (!cgo.FullyOccupied())//if the bed is not occupied
                    {
                        //Debug.Log("c");
                        cgo.AddOccupiedMelt(melt);
                        //melt.SetGridObj(cgo);



                    }
                }

            }
        }
    }


    private IEnumerator DayCheck()
    {
        float delay = 0.5f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            //Debug.Log("aasd");
            yield return wait;
            UpdateSkybox();
        }
    }
    private bool prevCheckIsDay = true;

    public bool IsDaytime()
    {
        float dayStartTime = 6.0f;   // Define your daytime start hour (24-hour format)
        float nightStartTime = 18.0f;
        // Get the current time
        float currentTime = System.DateTime.Now.Hour + System.DateTime.Now.Minute / 60f;

        // Check if the current time is between dayStartTime and nightStartTime

        //return false;
        return currentTime >= dayStartTime && currentTime < nightStartTime;
    }
    public bool IsLate()
    {
        float dayStartTime = 4.0f;   // Define your daytime start hour (24-hour format)
        float nightStartTime = 21.0f;
        // Get the current time
        float currentTime = System.DateTime.Now.Hour + System.DateTime.Now.Minute / 60f;

        // Check if the current time is between dayStartTime and nightStartTime

        //return false;
        return currentTime >= dayStartTime && currentTime < nightStartTime;
    }


    public void UpdateDayNightObjs()
    {


        bool isDay = IsDaytime();
        bool isLate = IsLate();
        if(isDay != prevCheckIsDay)
        {
            //its changed
        }
        if (dayObjs != null) {
            foreach (GameObject obj in dayObjs)
            {
                obj.SetActive(isDay);
            }
        }
        if (nightObjs != null) {
            foreach (GameObject obj in nightObjs)
            {
                obj.SetActive(!isDay);
            }
        }
        if (lateNightObjs != null) {
            foreach (GameObject obj in lateNightObjs)
            {
                obj.SetActive(!isLate);
            }
        }
        prevCheckIsDay = isDay;
    }

    public void UpdateSkybox()
    {
        //makeMeltsSleep();
        currentTime = GetCurrentTimeOfDay();
        UpdateDayNightObjs();
        //currentTime = TimeOfDay.Midnight;
        workingHours = GetIsDuringWorkingHours(currentTime);
        OnTimeChanged?.Invoke(currentTime);
        OnWorkingHoursChanged?.Invoke(workingHours);
        switch (currentTime)
        {
            case TimeOfDay.Morning:
                if (theSun != null)
                {
                    theSun.intensity = 1.0f;
                }
                RenderSettings.skybox = middayBox;
                //Debug.Log("It's morning!");
                break;
            case TimeOfDay.Midday:
                if (theSun != null)
                {
                    theSun.intensity = 1.0f;
                }
                RenderSettings.skybox = middayBox;
                //Debug.Log("It's midday!");
                break;
            case TimeOfDay.Afternoon:
                if (theSun != null)
                {
                    theSun.intensity = 1.0f;
                }
                RenderSettings.skybox = middayBox;
                //Debug.Log("It's afternoon!");
                break;
            case TimeOfDay.Evening:
                
                RenderSettings.skybox = eveningBox;
                if (theSun != null)
                {
                    theSun.intensity = 0.75f;
                }

                //Debug.Log("It's evening!");
                break;
            case TimeOfDay.Night:
                if(theSun != null)
                {
                    theSun.intensity = 0.5f;
                }
                makeMeltsSleep();
                RenderSettings.skybox = midnightBox;
                //Debug.Log("It's night!");
                break;
            case TimeOfDay.Midnight:
                if (theSun != null)
                {
                    theSun.intensity = 0.5f;
                }
                makeMeltsSleep();
                RenderSettings.skybox = midnightBox;
                //Debug.Log("It's midnight!");
                break;
            case TimeOfDay.Sunset:
                if (theSun != null)
                {
                    theSun.intensity = 0.5f;
                }
                makeMeltsSleep();
                RenderSettings.skybox = sunsetBox;
                //Debug.Log("It's sunset!");
                break;
            case TimeOfDay.Daybreak:
                if (theSun != null)
                {
                    theSun.intensity = 0.75f;
                }
                RenderSettings.skybox = daybreakBox;
                //Debug.Log("It's daybreak!");
                break;
        }
    }

    public enum TimeOfDay { Morning, Afternoon, Night, Midday, Evening, Midnight, Sunset, Daybreak }

    public TimeOfDay GetCurrentTimeOfDay()
    {
        DateTime now = DateTime.Now;
        TimeSpan currentTime = now.TimeOfDay;

        TimeSpan morningStartTime = new TimeSpan(6, 0, 0);
        TimeSpan middayStartTime = new TimeSpan(12, 0, 0);
        TimeSpan afternoonStartTime = new TimeSpan(15, 0, 0);
        TimeSpan eveningStartTime = new TimeSpan(18, 0, 0);
        TimeSpan nightStartTime = new TimeSpan(22, 0, 0);
        TimeSpan midnightStartTime = new TimeSpan(0, 0, 0);
        TimeSpan sunsetStartTime = new TimeSpan(19, 0, 0);
        TimeSpan daybreakStartTime = new TimeSpan(5, 0, 0);

        if (currentTime >= morningStartTime && currentTime < middayStartTime)
        {
            
            return TimeOfDay.Morning;
        }
        else if (currentTime >= middayStartTime && currentTime < afternoonStartTime)
        {
            
            return TimeOfDay.Midday;
        }
        else if (currentTime >= afternoonStartTime && currentTime < eveningStartTime)
        {
            
            return TimeOfDay.Afternoon;
        }
        else if (currentTime >= eveningStartTime && currentTime < nightStartTime)
        {
            
            return TimeOfDay.Evening;
        }
        else if (currentTime >= nightStartTime || currentTime < daybreakStartTime)
        {
            return TimeOfDay.Night;
        }
        else if (currentTime >= midnightStartTime && currentTime < daybreakStartTime)
        {
            
            return TimeOfDay.Midnight;
        }
        else if (currentTime >= sunsetStartTime && currentTime < eveningStartTime)
        {
            
            return TimeOfDay.Sunset;
        }
        else
        {
           
            return TimeOfDay.Daybreak;
        }
    }

    public bool GetIsDuringWorkingHours(TimeOfDay time)
    {
        bool dayout = false;
        switch (time)
        {
            case TimeOfDay.Morning:
                dayout = true;
                break;
            case TimeOfDay.Midday:
                dayout = true;
                break;
            case TimeOfDay.Afternoon:
                dayout = true;
                break;
            case TimeOfDay.Evening:
                dayout = true;

                break;
            case TimeOfDay.Night:
                
                break;
            case TimeOfDay.Midnight:
                
                break;
            case TimeOfDay.Sunset:
                
                break;
            case TimeOfDay.Daybreak:
                
                break;

            
        }
        return dayout;
    }
}
