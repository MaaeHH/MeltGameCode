using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class MeltAppearance : MonoBehaviour
{
    [SerializeField] private MeltData meltData;

   
    [SerializeField] private GameObject theMelt;
    [SerializeField] private GameObject meltScaler;

    [SerializeField] private Renderer meltRenderer;

   
    [SerializeField] private Color lastSetColour;


    [SerializeField] private Material temp;
 
    Material newMaterial;

    /*public Material GetMaterial()
    {
        return temp;
    }

    public void SetMaterial(Material x)
    {
        //transform.GetChild(0).GetChild(0).GetComponent<Renderer>().material = x;
        meltRenderer.material = x;
    }*/

    public void SetAColour(Color x)
    {

        //meltData.SetColour(x);
        //meltData.SaveData();

        lastSetColour = x;


   //     temp = new Material(meltRenderer.material);

        //temp.SetColor("_Color", lastSetColour);

        meltRenderer.material.SetColor("_BaseColor", lastSetColour);
        //meltRenderer.material = temp;

    }

    public void SaveNewColour(Color x)
    {
        meltData.SetColour(x);
        meltData.SaveData();
    }

    public Color GetLastColour()
    {
        return lastSetColour;
    }

    public void RefreshColour()
    {
        SetAColour(lastSetColour);
    }

    public void SetScale(Vector3 scale)
    {
       
        meltData.SetScale(scale);
        meltScaler.transform.localScale = scale;
        //Debug.Log("Setting scale to " + scale.x );

    }

    public void SetTexture(string textureName)
    {
        if (textureName != "")
        {
            Texture theImg = Resources.Load<Texture>("MeltTextures/" + textureName);

            meltRenderer.material.SetTexture("_BaseMap", theImg);
        }
    }
 

    public void RefreshAppearance()
    {
        //meltData.InitSaveData();
        try
        {
            //Debug.Log(meltData.GetName());
            SetTexture(meltData.GetTextureName());
            SetScale(meltData.GetScale());
            SetAColour(new Color(meltData.GetColour().r, meltData.GetColour().g, meltData.GetColour().b, meltData.GetColour().a));
        }
        catch
        {
            Debug.Log("something wrong");
        }
        
    }

  

    public void SetInvisible(bool invis)
    {
        theMelt.SetActive(!invis);
    }

 

    public void SetMeltData(MeltData data)
    {
        meltData = data;
      
        meltData.InitSaveData();
        RefreshAppearance();
    }

   
    public MeltData GetMeltData()
    {
        return meltData;
    }


   


}
