using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/MeltInterraction", order = 1)]
public class MeltInterractionData : ScriptableObject
{
    [SerializeField] public int animationID;
    [SerializeField] public float minTime;
    [SerializeField] public float maxTime;
    [SerializeField] public float radius;
    [SerializeField] public float minActivationDelay;
    [SerializeField] public float maxActivationDelay;
    [SerializeField] public int maxMelts;
    [SerializeField] public bool talkingFreq;

    [SerializeField] public float cheerBonus;
    [SerializeField] public float healthBonus;
    [SerializeField] public float energyBonus;
    [SerializeField] public float hungerBonus;

    [SerializeField] public GameObject centerPiece;
}
