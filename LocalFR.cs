using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LocalFR : FriendshipRequirement
{
    public LocalFR(FriendshipReqData dat)
    {
        SetData(dat);

        startingNumber = ms.GetMeltData().GetCountOf(dat.GetData());
        requiredNumber = ms.GetMeltData().GetCountOf(dat.GetData()) + dat.GetTarget();
       

    }

    public override float GetProgress()
    {
        int progress = ms.GetMeltData().GetCountOf(GetData().GetData()) - startingNumber;
        return progress * 1.0f / GetData().GetTarget();
        //return 1.0f;
    }

    public override bool IsComplete()
    {
        //return false;
        return (ms.GetMeltData().GetCountOf(GetData().GetData()) >= requiredNumber);
    }
}
