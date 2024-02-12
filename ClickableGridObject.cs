using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class ClickableGridObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GridObjectData obj;
    private GridObjectInstance ins;
    [SerializeField] private GameObject sleepParticles;
    [SerializeField] private GridObjectMenu goMenu;
    private List<MeltScript> occupiedMelts;
    private GameObject myObj;
    [SerializeField] private SelectedController sc;
    public bool AddOccupiedMelt(MeltScript x)
    {
        Debug.Log("Attempting to add " + x.GetMeltData().GetName());
        if(!occupiedMelts.Contains(x))
        {
            if (FullyOccupied())
            {
                return false;
            }
            else
            {

                if (x.SetMode(3))
                {
                    //Debug.Log("Adding " + x.GetMeltData().GetName());
                    occupiedMelts.Add(x);
                    //Debug.Log("Post add count " + occupiedMelts.Count);
                    x.SetGridObj(this);
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        return false;
        
    }

    public List<MeltScript> GetOccupiedMelts()
    {
        return occupiedMelts;
    }

    public bool HasOccupants()
    {
        return !(occupiedMelts.Count == 0);
    }

    public bool FullyOccupied()
    {
        if(occupiedMelts == null)
        {
            return true;
        }

        CheckMeltsStillOccupy();
        //Debug.Log("max occupants" + obj.GetMaxOccupants() + " count " + occupiedMelts.Count);
        return !(obj.GetMaxOccupants() > occupiedMelts.Count);        
    }
    public void CheckMeltsStillOccupy()
    {
        
            //Debug.Log("CheckmeltsStillOccupy pre count: " + occupiedMelts.Count);
            foreach (MeltScript melt in occupiedMelts)
            {
                if (melt.GetGridObj() != this)
                {
                    //occupiedMelts.Remove(melt);

                    RemoveMelt(melt);
                }
            }
        
        //Debug.Log("CheckmeltsStillOccupy post count: " + occupiedMelts.Count);
    }

    public void SetObjectData(GridObjectInstance x)
    {
        ins = x;
        obj = ins.GetData();
        
        string path = "GridObjects/" + obj.GetFileName();
        //Debug.Log(path);

        GameObject gamObj = Resources.Load<GameObject>(path);
        if (gamObj != null)
        {
          
            Tile theTile = ins.GetTile();
            theTile.SetOccupiedBy(this);
            gamObj = Instantiate(gamObj, transform.position, Quaternion.identity);
            gamObj.transform.localScale = obj.GetObjectScale();
            gamObj.transform.eulerAngles = obj.GetObjectRotation();

            gamObj.transform.SetParent(this.transform);
            Renderer theRenderer = gamObj.GetComponent<Renderer>();

            if(theRenderer != null)
            {
                theRenderer.material.shader = Shader.Find(SaveSystem.shaderName);
                gamObj.transform.position = new Vector3(gamObj.transform.position.x, gamObj.transform.position.y + theRenderer.bounds.size.y / 2, gamObj.transform.position.z);
            }
            myObj = gamObj;
        }
        //Debug.Log("Direction: " + ins.GetDirection());
        
    }

    public void RefreshAppearance()
    {
        GameObject.Destroy(myObj);
        SetObjectData(ins);
    }

    public GridObjectData GetObjectData()
    {
        return obj;
    }
    public GridObjectInstance GetObjectInstance()
    {
        return ins;
    }

    public int GetNoOfOccupants()
    {
        CheckMeltsStillOccupy();
        return occupiedMelts.Count;
    }
    
    //void Update()
    //{
        //Debug.Log("Count: " + occupiedMelts.Count);
    //}

    void Start()
    {
        occupiedMelts = new List<MeltScript>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.4f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            //Debug.Log("aasd");
            yield return wait;
            ObjectCheck();
        }
    }

    private void ObjectCheck()
    {
        CheckMeltsStillOccupy();
        if (obj != null && HasOccupants())
        {
            foreach(MeltScript melt in occupiedMelts)
            {

                if (obj.GetGridObjectType() == GridObjectData.ObjectType.Bed)
                {
                    if (!(melt.GetMoving()))
                    {
                        sleepParticles.SetActive(true);
                        melt.GetMeltData().SetIsSleeping(true);
                        melt.SetInvisible(true);
                    }
                }

            }
            
        }
    }

    public void RemoveMelt(MeltScript toRemove)
    {
        Debug.Log("Removing " + toRemove.GetMeltData().GetName());
        occupiedMelts.Remove(toRemove);
        //sleepParticles.SetActive(false);

        if (obj.GetGridObjectType() == GridObjectData.ObjectType.Bed && !HasOccupants())
        {
            sleepParticles.SetActive(false);
            toRemove.GetMeltData().SetIsSleeping(false);
            toRemove.SetInvisible(false);
        }

        
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
       
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
       
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //goMenu.GenerateMenu(this);
        ObjectClicked();
        //Debug.Log("CLICKED!");
    }
    private void ObjectClicked()
    {
        sc.GridObjectClicked(gameObject);
    }

    private void OnDrawGizmos()
    {
    
       
            Gizmos.color = Color.blue;
        

        Gizmos.DrawWireCube(this.transform.position, this.transform.localScale.x * GetComponent<BoxCollider>().size);
    }
}
