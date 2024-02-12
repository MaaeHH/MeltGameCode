using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeltInterractionController
{
    private static List<MeltInterractionInstance> ongoingInterractions = new List<MeltInterractionInstance>();

    public static void StartInterraction(MeltScript x,MeltScript y, MeltInterractionData z)
    {
        Debug.Log("starting interraction");
        MeltInterractionInstance currentInterraction = GetInterractionFromMelt(y);

        if (currentInterraction != null)
        {
            currentInterraction.AddMelt(x);
            
        }
        else
        {
            StartNewInterraction(x, y,z);
        }
    }

    public static void StartNewInterraction(MeltScript x, MeltScript y, MeltInterractionData z)
    {
        MeltInterractionInstance temp = new MeltInterractionInstance(z,x,y);
        //temp.AddMelt(x);
        //temp.AddMelt(y);
        ongoingInterractions.Add(temp);
        Debug.Log(ongoingInterractions.Count);
    }

    private static MeltInterractionInstance GetInterractionFromMelt(MeltScript x)
    {
        List<MeltInterractionInstance> toRemove = new List<MeltInterractionInstance>();
        foreach(MeltInterractionInstance ins in ongoingInterractions)
        {
            if (ins.Contains(x))
            {
                return ins;
            }

            if (ins.IsDone())
            {
                toRemove.Add(ins);
            }
        }
        return null;
    }

    public static void RemoveFromInterractions(MeltScript x)
    {
        MeltInterractionInstance instance = GetInterractionFromMelt(x);
        if(instance != null)
        instance.RemoveMelt(x);
    }
    
}
