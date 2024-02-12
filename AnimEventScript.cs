using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimEventScript : MonoBehaviour
{
    [SerializeField] private MeltScript ms;
    [SerializeField] private Screamscript scrs;
    [SerializeField] private ParticleController pc;
    public void SetData(MeltScript x)
    {
        ms = x;
    }

    public void AnimationEventTriggered()
    {
        if(ms != null && pc != null)
        if (ms.GetInteraction() != null)
        {
            switch (ms.GetInteraction().GetData().animationID)
            {
                case 0:
                    scrs.PlayScream();
                    pc.FireParticle();
                    break;
                default:
                    break;
            }
        }
    }
}
