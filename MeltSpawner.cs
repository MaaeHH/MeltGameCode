using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.InputSystem;
public class MeltSpawner : MonoBehaviour
{
    [SerializeField] private List<MeltData> visitingToSpawn;
    [SerializeField] private List<MeltData> stayingToSpawn;
   
    [SerializeField] private GameObject meltTemplate;
    [SerializeField] private MeltTimeController mtc;
    [SerializeField]private List<MeltScript> spawnedMelts;
    [SerializeField]private List<MeltScript> spawnedVisiting;
    [SerializeField]private List<MeltScript> spawnedStaying;
    private MeltScript draggingMelt;
    [SerializeField] private ScreenDimmer sd;
    [SerializeField] private GameObject dragAndDropMarker;
    private bool dragging;
    //[SerializeField] private MeltAssigner ma;
    private RaycastHit hit;

    private Bounds bound;

    void Start()
    {
        //noSlots = SaveSystem.GetMeltHouseSlots();
        sd.OnMaxDim += MaxDim;
    }
    private void OnDestroy()
    {
        sd.OnMaxDim -= MaxDim;
    }
    private MeltData toClick;
    public void ChangeMelts(MeltData x)
    {
        toClick = x;

        Debug.Log("Old VISITING...");
        foreach(MeltData data in visitingToSpawn)
        {
            if (data != null)
            {
                Debug.Log(data.GetName());
            }
            else
            {
                Debug.Log("null");
            }
        }
        Debug.Log("New VISITING...");
        foreach (MeltData data in MeltAssigner.GetCurrentMelts(MeltRecord.Location.House, SaveSystem.GetMeltHouseSlots()))
        {
            if (data != null)
            {
                Debug.Log(data.GetName());
            }
            else
            {
                Debug.Log("null");
            }
        }

        Debug.Log("Old STAYING...");
        foreach (MeltData data in stayingToSpawn)
        {
            if (data != null)
            {
                Debug.Log(data.GetName());
            }
            else
            {
                Debug.Log("null");
            }
        }

        Debug.Log("New STAYING...");
        foreach (MeltData data in MeltAssigner.GetLockedMelts(MeltRecord.Location.House))
        {
            if (data != null)
            {
                Debug.Log(data.GetName());
            }
            else
            {
                Debug.Log("null");
            }
        }


        
        if (!CompareMeltDataLists(visitingToSpawn, MeltAssigner.GetCurrentMelts(MeltRecord.Location.House, SaveSystem.GetMeltHouseSlots())) ||
            !CompareMeltDataLists(stayingToSpawn, MeltAssigner.GetLockedMelts(MeltRecord.Location.House)))
        {
            Debug.Log("ChangeMeltsDimming");
            sd.StartDim();
            return;
        }
        ClickMeltFromData(x);
    }

    public void ClickMeltFromData(MeltData x)
    {
        foreach(MeltScript ms in spawnedMelts)
        {
            if(ms.GetMeltData() == x)
            {
                ms.thisClicked();
            }
        }
    }
    private bool CompareMeltDataLists(List<MeltData> x, List<MeltData> y)
    {
        List<String> xnames = new List<String>();
        List<String> ynames = new List<String>();
        foreach (MeltData data in x)
        {
            if(data == null) {
                xnames.Add("NULLNULLNULL");
            }
            else
            {
                xnames.Add(data.GetName());
            }
           
        }
        foreach (MeltData data in y)
        {
            if (data == null)
            {
                ynames.Add("NULLNULLNULL");
            }
            else
            {
                ynames.Add(data.GetName());
            }
        }


        foreach (String xname in xnames)
        {
            if (ynames.Contains(xname)){
                ynames.Remove(xname);
            }else
            {
                return false;
            }
        }
        return (ynames.Count == 0);
    }


    public void MaxDim()
    {
        ClearMelts();
        ResetVariables();
        SpawnMelts();
        if(toClick != null)
        {
            ClickMeltFromData(toClick);
          
            toClick = null;
        }
    }

    public void ClearMelts()
    {
        foreach(Transform melt in this.transform)
        {
            GameObject.Destroy(melt.gameObject);
        }
        spawnedMelts = null;
    }
    public void ResetVariables()
    {
        visitingToSpawn = MeltAssigner.GetCurrentMelts(MeltRecord.Location.House, SaveSystem.GetMeltHouseSlots());
        stayingToSpawn = MeltAssigner.GetLockedMelts(MeltRecord.Location.House);
    }
    public void SetBounds(Bounds x)
    {
        bound = x;
    }
    public void SpawnMelts()
    {
        
        spawnedMelts = new List<MeltScript>();
        spawnedVisiting = new List<MeltScript>();
        spawnedStaying = new List<MeltScript>();
        foreach (MeltData melt in stayingToSpawn)
        {
            if (melt != null)
            {
                MeltScript temp = SpawnMelt(bound, melt);
                spawnedStaying.Add(temp);
                spawnedMelts.Add(temp);

            }
                
        }
        foreach (MeltData melt in visitingToSpawn)
        {
            if(melt != null && !stayingToSpawn.Contains(melt))
            {
                MeltScript temp = SpawnMelt(bound, melt);
                spawnedVisiting.Add(temp);
                spawnedMelts.Add(temp);
            }
        }
  
        foreach (MeltScript melt in spawnedMelts)
        {
            //melt.SetMaterial(melt.GetMaterial());
            melt.gameObject.SetActive(true);
            melt.RefreshAppearance();
        }
        mtc.SetMelts(spawnedMelts);
    }

    public void SetDraggingMelt(MeltScript x)
    {
        if(dragging && x == null)
        {
            CheckRaycast(draggingMelt);
        }

        draggingMelt = x;
        dragging = draggingMelt != null;
        dragAndDropMarker.SetActive(dragging);

        
    }

    
    private ClickableGridObject lastHoveredObject;
    private void CheckRaycast(MeltScript x)
    {
        // Create a ray from the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // Check if the hit object has a ClickableGridObject script
            ClickableGridObject clickableGridObject = hit.collider.GetComponent<ClickableGridObject>();

            if (clickableGridObject != null)
            {
                clickableGridObject.AddOccupiedMelt(x);
            }
            
        }
        
    }

 

    void Update()
    {
        if (dragging)
        {
            dragAndDropMarker.transform.position = new Vector3(draggingMelt.gameObject.transform.position.x, 0, draggingMelt.gameObject.transform.position.z);
        }
    }
    public MeltScript SpawnMelt(Bounds bound, MeltData melt)
    {
        Vector3 aaa = new Vector3(
        UnityEngine.Random.Range(bound.min.x, bound.max.x),
        UnityEngine.Random.Range(bound.min.y, bound.max.y),
        UnityEngine.Random.Range(bound.min.z, bound.max.z)
        );
        
        
        

        Vector3 castFrom = aaa;
        castFrom.y = castFrom.y + 1;

       /* if ((Physics.Raycast(castFrom, -Vector3.up, out hit, 10f)))
        {
            aaa = new Vector3(aaa.x, transform.position.y - (hit.distance - 1), aaa.z);
        }*/

        GameObject currentSpawned = Instantiate(meltTemplate, aaa, Quaternion.identity);

        
        
        currentSpawned.transform.SetParent(this.transform);
        
       
        MeltScript currentScript = currentSpawned.GetComponent<MeltScript>();
        currentScript.SetMeltData(melt);

        return currentScript;
    }

    public List<MeltScript> GetSpawnedMelts()
    {
        return spawnedMelts;
    }

    public List<MeltScript> GetSpawnedVisitors()
    {
        return spawnedVisiting;
    }
    public List<MeltScript> GetSpawnedStaying()
    {
        return spawnedStaying;
    }


}
