using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MeltTimeController : MonoBehaviour
{
    [SerializeField] private List<MeltScript> allMelts;
    [SerializeField] private float hungerRate = 0.3f;
    [SerializeField] private float cheerRate = 0.1f;
    [SerializeField] private float energyRate = 0.3f;
    [SerializeField] private float healthRate = 0.1f;
    [SerializeField] private int intervalsLimit = 100;
    //[SerializeField] private MeltSpawner ms;
    [SerializeField] private MeltDataMenu mdm;
    [SerializeField] private float timer = 0f;
    [SerializeField] private float interval = 180.0f; // 3 minutes in seconds
    // Start is called before the first frame update
  

    /*public void RestoreMeltUpdates(List<MeltScript> melts)
    {
        allMelts = melts;
        foreach (MeltScript melt in allMelts)
        {
            RestoreLastMeltUpdate(melt.GetMeltData());
        }
        //UpdateMelts();
    }*/

    public void SetMelts(List<MeltScript> x)
    {
        allMelts = x;
        //A melt restores its update when the data is loaded from the melt, no need to iterate
        //This is for the progressive updates during runtime

    }

    private void UpdateMelts()
    {
        //Debug.Log("UPDATING MELTS");
        foreach (MeltScript melt in allMelts)
        {
            UpdateMelt(melt.GetMeltData());
            melt.GetMeltData().SaveData();
        }
        mdm.RefreshMenu();
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer >= interval)
        {
            UpdateMelts();
            timer = 0f;
        }

    }

    public void RestoreLastMeltUpdate(MeltData data)
    {
        Debug.Log("Restoring last update for " + data.GetName());
        DateTime startTime = data.GetLastUpdate(); // Specify the starting time here
        
        DateTime currentTime = DateTime.Now;
        TimeSpan timePassed = currentTime - startTime;
        int intervalsPassed = (int)(timePassed.TotalMinutes / (interval / 60.0f));
        //Debug.Log("Intervals passed since " + startTime.ToString("yyyy-MM-dd") +": "+ intervalsPassed);
        if(intervalsPassed > intervalsLimit)
        {
            intervalsPassed = intervalsLimit;
        }
        for(int x = 0; x < intervalsPassed; x++)
        {
            UpdateMeltNoSave(data);
        }
        data.SetIsSleeping(false);
        data.SaveData();
    }

    

    private void UpdateMelt(MeltData data)
    {
        //Debug.Log(data.GetName() + " updated.");
        if (data.IsFriend())
        {
            data.DecreaseHunger(hungerRate);
            data.DecreaseCheer(cheerRate);
            data.DecreaseHealth(healthRate);
            data.SetLastUpdate(DateTime.Now);
            data.AddExp(1.1f);
            if (data.GetIsSleeping())
            {
                data.IncreaseEnergy(energyRate * 1.5f);
            }
            else
            {
                data.DecreaseEnergy(energyRate);
            }
            data.SaveData();
        }
    }

    private void UpdateMeltNoSave(MeltData data)
    {
        if (data.IsFriend())
        {
            //Debug.Log(data.GetName() + " updated.");
            data.DecreaseHunger(hungerRate);
            data.DecreaseCheer(cheerRate);
            data.DecreaseHealth(healthRate);
            data.SetLastUpdate(DateTime.Now);
            data.AddExp(1.1f);
            if (data.GetIsSleeping())
            {
                data.IncreaseEnergy(energyRate * 1.5f);
            }
            else
            {
                data.DecreaseEnergy(energyRate);
            }
        }
    }

    
}
