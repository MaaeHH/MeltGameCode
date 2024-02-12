using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Screamscript : MonoBehaviour
{
    private Animator animator;
    //private MeltScript ms;
    private MeltData md;
    private float minInterval = 10f;
    private float maxInterval = 15f;
    //[SerializeField] private AudioSource screamSound;
    [SerializeField] private List<AudioSource> screamSounds;

    private int currentInt = 0;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //md = transform.parent.parent.parent.GetComponent<MeltScript>().GetMeltData();
        //if(md == null) md = transform.parent.parent.parent.GetComponent<MarketMelt>().GetMeltData();
        ActivateMethodAtRandomInterval();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("MouthOpen", screamSounds[currentInt].isPlaying);
        
    }

    public void PlayScream()
    {
        currentInt = Random.Range(0, screamSounds.Count);

        screamSounds[currentInt].pitch = Random.Range(md.GetPitch() - 0.1f, md.GetPitch() +0.25f); // Set random pitch between 0.5 and 2.0
        screamSounds[currentInt].volume = md.GetVolume(); // Set random pitch between 0.5 and 2.0
        //screamSounds[currentInt].pitch = Random.Range(ms.GetMeltData().GetPitch() - 0.1f, ms.GetMeltData().GetPitch() +0.25f); // Set random pitch between 0.5 and 2.0
        screamSounds[currentInt].Play();
        //screamSound.Play();

    }
    
    public void SetMeltData(MeltData x)
    {
        md = x;
        md.InitSaveData();
    }

    private void ActivateMethodAtRandomInterval()
    {
        float randomInterval = Random.Range(minInterval, maxInterval);
        Invoke("ActivateMethod", randomInterval);
    }

    private void ActivateMethod()
    {
        PlayScream();
        // Perform the method activation here
        // Your code logic goes here

        ActivateMethodAtRandomInterval(); // Activate the method again at a new random interval
    }

}
