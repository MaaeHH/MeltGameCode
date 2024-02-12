using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class TileObjectController : MonoBehaviour
{
    //private int houseXLength;
    //private int houseYLength;
    //private string fName = "gridObjects.json";
    [SerializeField] private List<GridObjectInstance> objects;
    [SerializeField] private List<ClickableGridObject> spawnedObjects;
    [SerializeField] private GridGenerator gridGen;
    [SerializeField] private GameObject cot;
    
    public void MakeObject(GridObjectInstance obj, bool count) 
    {
        
        //Debug.Log(obj.GetData().GetFileName());
        obj.InitTileFromCoOrds(gridGen);
        Tile theTile = obj.GetTile();
        
        GameObject gamObj = Instantiate(cot, theTile.gameObject.transform.position, Quaternion.identity);
        ClickableGridObject temp = gamObj.GetComponent<ClickableGridObject>();
        spawnedObjects.Add(temp);
        temp.SetObjectData(obj);
        theTile.SetOccupiedBy(temp);
        gamObj.transform.SetParent(this.transform);
        gamObj.SetActive(true);
        gamObj.transform.eulerAngles = new Vector3(0, 90 * obj.GetDirection(), 0);
       
        TileObjectSaver.AddGridObjectInstance(obj);
        
        if(count) SaveSystem.IncreaseCount(obj.GetData());
        //if(isnew) 
    }
  

    public List<ClickableGridObject> GetAllObjectsOfType(GridObjectData.ObjectType type)
    {
        List<ClickableGridObject> outObj = new List<ClickableGridObject>();
        foreach (ClickableGridObject obj in spawnedObjects)
        {
            if(obj.GetObjectData().GetGridObjectType() == type)
            {
                outObj.Add(obj);
            }
        }
        return outObj;
    }

    public void RemoveObject(GridObjectInstance obj)
    {
        
        TileObjectSaver.RemoveGridObjectInstance(obj);
        
        RefreshObjects();
    }

    public void MakeObjects()
    {
        objects = TileObjectSaver.GetGridObjects();
        //Debug.Log("there are " + objects.Count + " objects");
        foreach (GridObjectInstance obj in objects)
        {
            MakeObject(obj, false);
        }
       
    }
    public void RefreshObjects()
    {
        spawnedObjects = new List<ClickableGridObject>();
        foreach (Transform obj in transform)
        {
            Destroy(obj.gameObject);
        }
        MakeObjects();
    }



  
}
