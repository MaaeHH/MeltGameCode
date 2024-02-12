using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CustomMeltSave 
{
    public float restlessness = 4;
    public float restlessnessConsistency = 5;
    public float speed = 1;
    public float rotationSpeed = 1.4f;
    public string meltName = "New Melt";
    public string meltDesc = "This meowmelt was recently spotted in space";
    public Color startingColour = Color.yellow;
    public Vector3 startingScale = new Vector3(1, 1, 1);
    public string startingTextureName = "";
    public float startingPitch = 1.0f;
    public float startingVolume = 0.25f;

    public CustomMeltSave(float restless,
        float rc, float speed, float rotationSpeed,
        string meltName, string meltDesc, Color startingColour, Vector3 startingScale, string textureName,
        float pitch, float volume)
    {
        restlessness = restless;
        restlessnessConsistency = rc;
        this.speed = speed;
        this.rotationSpeed = rotationSpeed;
        this.meltName = meltName;
        this.meltDesc = meltDesc;
        this.startingColour = startingColour;
        this.startingScale = startingScale;
        this.startingTextureName = textureName;
        this.startingPitch = pitch;
        this.startingVolume = volume;
    }

    public static CustomMeltSave FromMeltData(MeltData meltData)
    {
        return meltData.GetCustomMeltSave();
    }

    public MeltData ToMeltData()
    {
        MeltData meltData = ScriptableObject.CreateInstance<MeltData>();
        meltData.SetDataFromCustomMeltSave(this);
      
        return meltData;
    }

}
