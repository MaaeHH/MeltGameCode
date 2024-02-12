using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketMeltSpawner : MonoBehaviour
{
    [SerializeField] private List<MeltData> meltsToSpawn;
    [SerializeField] private GameObject meltTemplate;
    [SerializeField] private Transform spawnedMeltsTransform;
    [SerializeField] private DayNight dayNightScript;
    [SerializeField] private int noSlots = 3;
    //[SerializeField] private MeltAssigner ma;
    private bool isWorkingHours = true;
    private List<MarketMelt> spawnedMelts;
    private RaycastHit hit;
    private int count = 0;
    void Start()
    {
        dayNightScript.OnWorkingHoursChanged += DayNight_WorkingHoursChanged;
    }

    private void OnDestroy()
    {
        dayNightScript.OnWorkingHoursChanged -= DayNight_WorkingHoursChanged;
    }

    private void DayNight_WorkingHoursChanged(bool working)
    {
        isWorkingHours = working;
        //SetAllActive(isWorkingHours);
    }

    public void SpawnMelts()
    {
        spawnedMelts = new List<MarketMelt>();

        meltsToSpawn = MeltAssigner.GetCurrentMelts(MeltRecord.Location.Market, noSlots);

        foreach (MeltData melt in meltsToSpawn)
        {
            SpawnMelt(melt);
        }
        //SetAllActive(isWorkingHours);
    }

    private void SetAllActive(bool x)
    {
        //Debug.Log(spawnedMelts.Count);
        foreach (MarketMelt melt in spawnedMelts)
        {
            melt.gameObject.SetActive(x);
        }
    }

    public void SpawnMelt(MeltData melt)
    {
        if (melt != null)
        {
            melt.InitSaveData();
            MarketFlowBound[] children = gameObject.GetComponentsInChildren<MarketFlowBound>();
            MarketFlowBound randomObject = children[Random.Range(0, children.Length)];

            Bounds bound = randomObject.GetBounds();
            Vector3 aaa = new Vector3(
            UnityEngine.Random.Range(bound.min.x, bound.max.x),
            UnityEngine.Random.Range(bound.min.y, bound.max.y),
            UnityEngine.Random.Range(bound.min.z, bound.max.z)
            );




            //Vector3 castFrom = aaa;
            //castFrom.y = castFrom.y + 1;

            /* if ((Physics.Raycast(castFrom, -Vector3.up, out hit, 10f)))
             {
                 aaa = new Vector3(aaa.x, transform.position.y - (hit.distance - 1), aaa.z);
             }*/

            GameObject currentSpawned = Instantiate(meltTemplate, aaa, Quaternion.identity);



            currentSpawned.transform.SetParent(spawnedMeltsTransform);
            spawnedMelts.Add(currentSpawned.GetComponent<MarketMelt>());
            currentSpawned.GetComponent<MarketMelt>().SetMeltData(melt);
            //currentSpawned.GetComponent<MarketMelt>().SetColour(Color.green);

            currentSpawned.GetComponent<MarketMelt>().SetMarketFlowBound(randomObject);
            //Debug.Log(data.GetActorName());
            currentSpawned.SetActive(true);
        }

    }

    public void Ready()
    {
        count++;
        if(count == gameObject.GetComponentsInChildren<MarketFlowBound>().Length)
        {
            SpawnMelts();
        }
    }

    public List<MarketMelt> GetSpawnedMelts()
    {
        return spawnedMelts;
    }

}
