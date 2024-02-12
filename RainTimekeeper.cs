using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public class RainTimekeeper : MonoBehaviour
{

    private const string fileName = "RainTime.txt";
    private DateTime futureTime = default(DateTime);
    private int HourDelay = 1;
    int rainChance = 10;
    private WaitForSeconds hourWait = new WaitForSeconds(3600);

    public event RainingEvent OnRainChanged;
    public delegate void RainingEvent(bool isRaining);

    private void Start()
    {
        LoadFutureTime();
    
            
        CheckAndRegenerateFutureTime();
            


        StartCoroutine(CallFunctionEveryHour());
    }

    //private void Awake()
    //{
    //    StartCoroutine(CallFunctionEveryHour());
    //}

    private void Update()
    {
        //HourlyFunction();
    }

    private IEnumerator CallFunctionEveryHour()
    {
        //HourlyFunction();
        while (true)
        {
            // Call your function here
            

            yield return hourWait;
            HourlyFunction();
        }
    }

    private void HourlyFunction()
    {

        CheckAndRegenerateFutureTime();
        //SaveFutureTime();
        
    }

    private void GenerateFutureTime()
    {

        if (HasTimePassed())
        {
            //if (true)
            if (UnityEngine.Random.Range(0, rainChance) == 1)
            {
                int randomHours = UnityEngine.Random.Range(1, 4);
                futureTime = DateTime.Now.AddHours(randomHours);
                //futureTime = DateTime.Now.AddSeconds(10);
            }
            else
            {
                futureTime = DateTime.Now;
            }
            //OnRainChanged?.Invoke(GetIsRaining());
        }
        OnRainChanged?.Invoke(GetIsRaining());
    }

    private void SaveFutureTime()
    {
        using (StreamWriter writer = new StreamWriter(Path.Combine(Application.persistentDataPath, fileName)))
        {
            writer.Write(futureTime.Ticks);
        }
        //Debug.Log("Saving new time: " + futureTime.ToString());
    }

    private void LoadFutureTime()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                long ticks;
                if (long.TryParse(reader.ReadToEnd(), out ticks))
                {
                    futureTime = new DateTime(ticks);
                    //Debug.Log("Rain loaded. Rain stop date: " + futureTime.ToString());
                }
            }
        }
    }


    public bool GetIsRaining()
    {
        return futureTime > DateTime.Now;
        //return true;
    }

    private bool HasTimePassed()
    {
        TimeSpan timeDifference = DateTime.Now - futureTime;
        return timeDifference.TotalHours >= HourDelay;
    }

    private void DestroyTimeFile()
    {
        string filePath = Path.Combine(Application.persistentDataPath, fileName);
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
    }

    private void CheckAndRegenerateFutureTime()
    {
        
        GenerateFutureTime();
        SaveFutureTime();
        
    }

}
