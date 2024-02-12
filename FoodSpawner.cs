using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foodTemplate;
    private List<FoodScript> spawnedFood;

    // Start is called before the first frame update
    void Start()
    {
        spawnedFood = new List<FoodScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnFoodAtTile(Tile theTile, FoodData data)
    {
        //theTile.SetObject(obj);
        GameObject gamObj = Instantiate(foodTemplate, theTile.gameObject.transform.position, Quaternion.identity);

        FoodScript fs = gamObj.GetComponent<FoodScript>();
        fs.SetData(data);


        gamObj.transform.SetParent(this.transform);
        //gamObj.transform.position = new Vector3(gamObj.transform.position.x, gamObj.transform.position.y + gamObj.GetComponent<Renderer>().bounds.size.y / 2, gamObj.transform.position.z);
        gamObj.SetActive(true);
        
    }

    public List<FoodScript> GetSpawnedFood()
    {
        return spawnedFood;
    }
}
