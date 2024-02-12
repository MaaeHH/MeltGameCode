using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class DoorwayScript : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private List<MeltData> visitors;
    [SerializeField] private Animator doorAnim;
    [SerializeField] private MeltAppearance appearance;
    [SerializeField] private Text arrivalText;
    [SerializeField] private Screamscript scremS;
    [SerializeField] private SceneLoader sl;
    [SerializeField] private AudioSource shimmering;

    [SerializeField] private List<ParticleSystem> rareParticles;

    [SerializeField] private Image background;
    [SerializeField] private Image otherBackground;
    public float fadeDuration = 2.0f; // Duration of the fade in seconds
   
    private float targetVolume = 1.0f;
    private float startVolume = 0.0f;
    private float startTime;

    private int currentMeltIndex = 0;
    private bool asd = false;
    MeltData visitor;
    // Start is called before the first frame update
    void Start()
    {
        if (SaveSystem.GetHouseColour(0).colour != null)
        {
            background.color = SaveSystem.GetHouseColour(0).colour;
            otherBackground.color = SaveSystem.GetHouseColour(0).colour;
        }
        StopParticles();
        visitors = DoorScenePassover.newMelts;
        
        visitor = visitors[currentMeltIndex];
        appearance.SetMeltData(visitor);
        scremS.SetMeltData(visitor);
        arrivalText.text = visitor.GetName() + " is visiting!";
    }


    bool AnimatorIsPlaying()
    {
        return doorAnim.GetCurrentAnimatorStateInfo(0).length >
               doorAnim.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public void NextMelt()
    {
        Debug.Log("Next melt");
        currentMeltIndex++;
        if(currentMeltIndex == visitors.Count)
        {
            doorAnim.SetBool("Done", true);
        }
        else
        {
            visitor = visitors[currentMeltIndex];
            appearance.SetMeltData(visitor);
            scremS.SetMeltData(visitor);
            arrivalText.text = visitor.GetName() + " is visiting!";
        }
    }

    private void StopParticles()
    {
        foreach(ParticleSystem rareParticle in rareParticles)
        {
            rareParticle.enableEmission = false;
        }
    }

    public void ParticlesStart()
    {
        //StopParticles();
        shimmering.Play();
    
        rareParticles[visitor.GetRarityInt()].enableEmission = true;
    }

    public void ParticlesEnd()
    {
        shimmering.Stop();

       
        rareParticles[visitor.GetRarityInt()].enableEmission = false;
    }

    public void DoorClosed()
    {
        sl.ChangeLevel(0, "MainScene");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (!AnimatorIsPlaying())
        {
            Debug.Log("Going next");
            doorAnim.SetTrigger("Next");
            asd = !asd;
            doorAnim.SetBool("Left", asd);
        }
    }
}

