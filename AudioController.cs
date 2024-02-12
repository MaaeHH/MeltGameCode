using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource buttonClick1;
    [SerializeField] private AudioSource buttonClick2;
    [SerializeField] private AudioSource sliderSound;
    [SerializeField] private AudioSource menuOpenSound;
    [SerializeField] private AudioSource menuClosedSound;
    [SerializeField] private AudioSource dialogueSound;
    [SerializeField] private AudioSource buySellSound;
    public void PlayButtonClick1()
    {
        buttonClick1.Play(0);
    }
    public void PlayButtonClick2()
    {
        buttonClick2.Play(0);
    }
    public void PlaySliderSound()
    {
        sliderSound.Play(0);
    }
    public void PlayMenuOpenSound()
    {
        menuOpenSound.Play(0);
    }
    public void PlayMenuClosedSound()
    {
        menuClosedSound.Play(0);
    }
    public void PlayDialogueSound()
    {
        dialogueSound.Play(0);
    }
    public void PlayDialogueEndSound()
    {
        menuClosedSound.Play(0);
    }
    public void PlayBuySound()
    {
        buySellSound.Play(0);
    }
}
