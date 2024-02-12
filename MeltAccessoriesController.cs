using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltAccessoriesController : MonoBehaviour
{
    public List<AccessoryData> accessoryDatas = new List<AccessoryData>();
    
    public void SetAccessories(List<AccessoryData> x)
    {
        accessoryDatas = x;
        RefreshAppearance();
    }

    public void SetAccessoriesFromData(MeltData x)
    {
        accessoryDatas = x.GetAccessories();
        RefreshAppearance();
    }

    public void SetAccessoriesFromData(MeltData x, string layerName)
    {
        accessoryDatas = x.GetAccessories();
        RefreshAppearance(layerName);
    }

    private void MakeAccessory(AccessoryData x)
    {
       
  

        GameObject gamObj = x.gameObj;
        //GameObject gamObj = Resources.Load<GameObject>(path);
        if (gamObj != null)
        {

          
            gamObj = Instantiate(gamObj, transform.position, Quaternion.identity);

            
            gamObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            gamObj.transform.SetParent(this.transform);
          
            gamObj.transform.localScale = new Vector3(1f, 1f, 1f);
            gamObj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);


        }
    }

    private void MakeAccessoryOnLayer(AccessoryData x, string layerName)
    {



        GameObject gamObj = x.gameObj;
        //GameObject gamObj = Resources.Load<GameObject>(path);
        if (gamObj != null)
        {


            gamObj = Instantiate(gamObj, transform.position, Quaternion.identity);


            gamObj.transform.eulerAngles = new Vector3(0f, 0f, 0f);
            gamObj.transform.SetParent(this.transform);

            gamObj.transform.localScale = new Vector3(1f, 1f, 1f);
            gamObj.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);

            int theLayer = LayerMask.NameToLayer(layerName);
            gamObj.layer = theLayer;

            foreach (Transform child in gamObj.transform)
            {
                child.gameObject.layer = theLayer;
            }
        }
    }

    public void RefreshAppearance(string layerName)
    {
        foreach(Transform thing in transform)
        {
            GameObject.Destroy(thing.gameObject);
        }

        foreach(AccessoryData ad in accessoryDatas)
        {
            MakeAccessoryOnLayer(ad, layerName);
        }
    }
    public void RefreshAppearance()
    {
        foreach (Transform thing in transform)
        {
            GameObject.Destroy(thing.gameObject);
        }

        foreach (AccessoryData ad in accessoryDatas)
        {
            MakeAccessory(ad);
        }
    }



}
