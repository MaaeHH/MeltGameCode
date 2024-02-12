using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GlobalFR : FriendshipRequirement
{
    public GlobalFR(FriendshipReqData dat)
    {
        SetData(dat);
 
        startingNumber = SaveSystem.GetCounter().GetCountOf(dat.GetData());
        requiredNumber = SaveSystem.GetCounter().GetCountOf(dat.GetData()) + dat.GetTarget();
      
    }

    public override float GetProgress()
    {
        int progress = SaveSystem.GetCounter().GetCountOf(GetData().GetData()) - startingNumber;
        return (progress * 1.0f / GetData().GetTarget());
    }

    public override bool IsComplete()
    {
        return (SaveSystem.GetCounter().GetCountOf(GetData().GetData()) >= requiredNumber);
    }
}
