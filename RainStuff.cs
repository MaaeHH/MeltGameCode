using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RainStuff : MonoBehaviour
{
    [SerializeField] private RainTimekeeper rtk;
    [SerializeField] private GameObject rainstuffGO;
    // Start is called before the first frame update
    void Awake()
    {
        rtk.OnRainChanged += setRainStuff;
    }

    private void OnDestroy()
    {
        rtk.OnRainChanged -= setRainStuff;
    }

    private void setRainStuff(bool x)
    {
        if (x)
        {
            //Debug.Log("ITS RAINING");
        }
        else
        {
            //Debug.Log("ITS NOT RAINING");
        }
        rainstuffGO.SetActive(x);
    }

}
