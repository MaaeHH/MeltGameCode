using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DateAndTimeUI : MonoBehaviour
{
    [SerializeField] private Text date;
    [SerializeField] private Text time;
    [SerializeField] private DayNight dn;
    [SerializeField] private Image nightImg;
    [SerializeField] private GameObject sun;
    [SerializeField] private GameObject stars;
    [SerializeField] private GameObject rain;
    [SerializeField] private Image bgImage;
    [SerializeField] private RainTimekeeper rtk;
    private DayNight.TimeOfDay currentTimeOfDay = DayNight.TimeOfDay.Morning;
    private bool currentRain = false;

    // Start is called before the first frame update
    void Start()
    {
        UpdateUI();
        RainTimekeeper_OnRainChanged(rtk.GetIsRaining());
        //UpdateUI(DayNight.TimeOfDay.Night);
        dn.OnTimeChanged += DayNight_OnTimeChanged;
        rtk.OnRainChanged += RainTimekeeper_OnRainChanged;
    }

    
    private void OnDestroy()
    {
        dn.OnTimeChanged -= DayNight_OnTimeChanged;
        rtk.OnRainChanged -= RainTimekeeper_OnRainChanged;
    }

    private void DayNight_OnTimeChanged(DayNight.TimeOfDay newTimeOfDay)
    {
        currentTimeOfDay = newTimeOfDay;
        UpdateUI();
    }

    private void RainTimekeeper_OnRainChanged(bool isRaining)
    {
        currentRain = isRaining;

        //currentRain = true;
        UpdateUI();
    }

    private void UpdateUI()
    {
        string temp = "";
        bool day = false;


        switch (currentTimeOfDay)
        {
            case DayNight.TimeOfDay.Morning:
                day = true;
                temp = "Morning";
                break;
            case DayNight.TimeOfDay.Midday:
                day = true;

                temp = "Midday";
                break;
            case DayNight.TimeOfDay.Afternoon:
                day = true;
                temp = "Afternoon";
                break;
            case DayNight.TimeOfDay.Evening:
                temp = "Evening";
                break;
            case DayNight.TimeOfDay.Night:
                temp = "Night";
                break;
            case DayNight.TimeOfDay.Midnight:
                temp = "Midnight";
                break;
            case DayNight.TimeOfDay.Sunset:
                temp = "Sunset";
                break;
            case DayNight.TimeOfDay.Daybreak:
                day = true;
                temp = "Daybreak";
                break;
        }

        rain.SetActive(currentRain);

        if (!currentRain)
        {
            sun.SetActive(day);
            stars.SetActive(!day);
        }
        else
        {
            sun.SetActive(false);
            stars.SetActive(false);
        }
        


        if (day)
        {
            bgImage.sprite = Resources.Load<Sprite>("DateandtimeUI/Day");
        }
        else
        {
            bgImage.sprite = Resources.Load<Sprite>("DateandtimeUI/Night");
        }
        time.text = temp;
        date.text = DateTime.Now.ToString("dd/MM/yy");

    }
}
