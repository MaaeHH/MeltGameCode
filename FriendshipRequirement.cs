using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class FriendshipRequirement
{
    [SerializeField] protected int startingNumber;
    [SerializeField] protected int friendshipReqID;
    //[SerializeField] protected FriendshipReqData data;
    [SerializeField] protected int requiredNumber;
    //private SaveSystem sav;
    [SerializeField] protected MeltScript ms;

    public abstract float GetProgress();

    public FriendshipReqData GetData()
    {
        return TileObjectSaver.GetFReqFromID(friendshipReqID);
    }

    public void SetData(FriendshipReqData x)
    {
        friendshipReqID = TileObjectSaver.GetIDFromFReq(x);
    }

    public abstract bool IsComplete();

    public int GetStartingNum()
    {
        return startingNumber;
    }
    public int GetRequiredNum()
    {
        return requiredNumber;
    }

    public void SetMeltScript(MeltScript x)
    {
        ms = x;
    }

   
}
