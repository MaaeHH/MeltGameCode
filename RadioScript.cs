using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    [SerializeField] private DayNight dayNightScript;
    [SerializeField] private GameObject particleGO;
    [SerializeField] private Animator anims;

    // Start is called before the first frame update
    void Start()
    {
        dayNightScript.OnWorkingHoursChanged += DayNight_WorkingHoursChanged;
    }
    private void OnDestroy()
    {
        dayNightScript.OnWorkingHoursChanged -= DayNight_WorkingHoursChanged;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void DayNight_WorkingHoursChanged(bool working)
    {
        SetOnOff(working);

    }

    private void SetOnOff(bool x)
    {
        particleGO.SetActive(x);
        anims.SetBool("Playing", x);
    }
}
