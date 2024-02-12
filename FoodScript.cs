using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour
{
    [SerializeField] private FoodData thisData;
    [SerializeField] private CrumbController crumbCont;
    public float shrinkSpeed = 0.5f;
    private MeltScript eatenBy;
    void Start()
    {
        if(thisData != null)
        {
            InitData();
        }

    }
    public void SetData(FoodData data)
    {
        thisData = data;
        InitData();
    }

    private void InitData()
    {
        string path = "FoodObjects/" + thisData.GetFileName();
        //Debug.Log(path);

        GameObject gamObj = Resources.Load<GameObject>(path);
        if (gamObj != null)
        {

            gamObj = Instantiate(gamObj, this.transform.position, gameObject.transform.rotation);

            gamObj.transform.SetParent(this.transform);
           
        }
    }
    public void Shrink(MeltScript ms)
    {
        eatenBy = ms;
        transform.localScale -= Vector3.one * shrinkSpeed * Time.deltaTime;

        // Check if the object is small enough
        if (transform.localScale.x <= 0f)
        {
            ms.FoodEaten(thisData);
            // Destroy the object
            Destroy(gameObject);
        }
        if (Random.Range(0, 1500) > 1496)
        {
            crumbCont.MakeCrumbAt(this.transform.position);
        }
    }
    void LateUpdate()
    {
        eatenBy = null;
    }
    public MeltScript GetEatenBy()
    {
        return eatenBy;
    }
}
