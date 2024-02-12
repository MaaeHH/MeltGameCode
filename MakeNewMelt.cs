using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeNewMelt : MonoBehaviour
{
    [SerializeField] private InputField nameField;
    [SerializeField] private InputField descriptionField;

    [SerializeField] private Slider speedSlider;
    [SerializeField] private Slider restlessnessSlider;
    [SerializeField] private Slider sizeSlider;

    [SerializeField] private Slider colourRSlider;
    [SerializeField] private Slider colourGSlider;
    [SerializeField] private Slider colourBSlider;

    [SerializeField] private Slider pitchSlider;
    [SerializeField] private Slider volumeSlider;

    //SerializeField] private CustomMeltLoader loader;
    public void MakeMelt()
    {
        CustomMeltLoader.SaveMeltSave(
        new CustomMeltSave(restlessnessSlider.value,
        restlessnessSlider.value, speedSlider.value, speedSlider.value,
        nameField.text, descriptionField.text, new Color(colourRSlider.value, colourGSlider.value, colourBSlider.value), new Vector3(sizeSlider.value, sizeSlider.value, sizeSlider.value), "",
        pitchSlider.value, volumeSlider.value)
        );
        
    }

}
