using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObjPreview : MonoBehaviour
{
    GridObjectData obj;
    GameObject gamObj = null;
    Tile theTile;
    GridObjectInstance ins;
    private int direction;
    [SerializeField] private Material theMaterial;
    [SerializeField] private AudioController _audioCont;
    public void SetObjectData(GridObjectData x)
    {
        if (obj != x)
        {
            if (gamObj != null)
            {

                GameObject.Destroy(gamObj);
                gamObj = null;
            }
            obj = x;

            string path = "GridObjects/" + obj.GetFileName();
            //Debug.Log(path);

            gamObj = Resources.Load<GameObject>(path);
            if (gamObj != null)
            {

                //Tile theTile = ins.GetTile();
                //theTile.SetOccupiedBy(this);
                gamObj = Instantiate(gamObj, transform.position, Quaternion.identity);
                gamObj.transform.eulerAngles = obj.GetObjectRotation();
                gamObj.transform.localScale = obj.GetObjectScale();
                gamObj.transform.SetParent(this.transform);
                if(gamObj.GetComponent<Renderer>() != null)
                gamObj.transform.position = new Vector3(gamObj.transform.position.x, gamObj.transform.position.y + gamObj.GetComponent<Renderer>().bounds.size.y / 2, gamObj.transform.position.z);
                
                if(gamObj.GetComponent<Renderer>() != null)
                gamObj.GetComponent<Renderer>().material = theMaterial;
                //BoxCollider coll = transform.GetChild(0).gameObject.GetComponent<BoxCollider>();
                //coll.enabled = false;
                //Destroy(coll);

                //Collider colla = transform.GetChild(0).gameObject.GetComponent<Collider>();
                //colla.enabled = false;
                //Destroy(colla);
                //GameObject.Destroy(gamObj.GetComponent<BoxCollider>());
                BoxCollider.Destroy(transform.GetChild(0).gameObject.GetComponent<BoxCollider>());
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                gamObj.layer = LayerIgnoreRaycast;

                //if (coll != null) Debug.Log("ASDASDASD");
                //Destroy(gamObj.GetComponent<BoxCollider>());
                //gamObj.raycastTarget = false;
                UpdateDirection();


            }
        }
    }
    public void SetDirection(int x)
    {
        direction = x;
        
        UpdateDirection();
    }
    public void Update()
    {
        if (transform.childCount != 0)
        {
            BoxCollider.Destroy(transform.GetChild(0).gameObject.GetComponent<BoxCollider>());
        }
    }

    public void SetObjectInstance(GridObjectInstance x)
    {
        if (ins != x)
        {

            if (gamObj != null)
            {

                GameObject.Destroy(gamObj);
                gamObj = null;
            }
            obj = x.GetData();
            ins = x;
            string path = "GridObjects/" + obj.GetFileName();
            //Debug.Log(path);

            gamObj = Resources.Load<GameObject>(path);
            if (gamObj != null)
            {

                //Tile theTile = ins.GetTile();
                //theTile.SetOccupiedBy(this);
                gamObj = Instantiate(gamObj, transform.position, Quaternion.identity);
                gamObj.transform.eulerAngles = obj.GetObjectRotation();
                gamObj.transform.localScale = obj.GetObjectScale();
                gamObj.transform.SetParent(this.transform);
                if (gamObj.GetComponent<Renderer>() != null)
                {
                    gamObj.transform.position = new Vector3(gamObj.transform.position.x, gamObj.transform.position.y + gamObj.GetComponent<Renderer>().bounds.size.y / 2, gamObj.transform.position.z);

                    gamObj.GetComponent<Renderer>().material = theMaterial;
                }
                int LayerIgnoreRaycast = LayerMask.NameToLayer("Ignore Raycast");
                gamObj.layer = LayerIgnoreRaycast;
                Destroy(gamObj.GetComponent<BoxCollider>());
                GameObject.Destroy(gamObj.GetComponent<BoxCollider>());
                //gamObj.raycastTarget = false;
                UpdateDirection();
            }
        }
    }
    public void SetTile(Tile x)
    {
        _audioCont.PlaySliderSound();
        theTile = x;
        this.transform.position = theTile.transform.position;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        
            this.transform.eulerAngles = new Vector3(0, 90 * direction, 0);
        
        
    }
   
}
