using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowPreview : MonoBehaviour
{
    [SerializeField] private GameObject wall;
    [SerializeField] private GameObject pointer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MakeWindows(int wallNum)
    {
        //Debug.Log("asd");
        foreach(Transform child in wall.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        if(wallNum != 3 || wallNum != 1){
            wall.transform.localScale = new Vector3(SaveSystem.GetWallSizeX()*1.5f, 1, 10);
        }else
        {
            wall.transform.localScale = new Vector3(SaveSystem.GetWallSizeY() * 1.5f, 1, 10);
        }
        foreach (WindowInstance winData in SaveSystem.GetWindows(wallNum))
        {
            //string path = "Windows/" + winData.GetData().objName;
            //GameObject windowPrefab = Resources.Load<GameObject>(path);
            GameObject instantiated = GameObject.Instantiate(winData.GetData().winObj, new Vector3(0, 0, 0), new Quaternion());
            instantiated.SetActive(true);
            instantiated.transform.localScale = new Vector3(instantiated.transform.localScale.x, 1, instantiated.transform.localScale.z);
            instantiated.transform.eulerAngles = new Vector3(90, 0, 90);
            instantiated.transform.SetParent(wall.transform);
            //instantiated.transform.eulerAngles = new Vector3(90,0, 0);
            instantiated.transform.localPosition = new Vector3((0.5f - winData.xPos)*10f, 0, (0.5f - winData.height)*10f);
            //instantiated.transform.localScale = new Vector3(transform.localScale.x*1f, transform.localScale.y, transform.localScale.z * 1f);
            instantiated.transform.localScale = new Vector3(instantiated.transform.localScale.x*0.25f, 0.1f, instantiated.transform.localScale.z * 0.25f);

        }
    }

    public void SetPointerLocation(float x, float y)
    {

        //pointer.transform.eulerAngles = new Vector3(90,0, 0);
        pointer.transform.SetParent(this.transform);
        pointer.transform.localPosition = new Vector3((0.5f - x) * 10f, 0, (0.5f - y) * 10f);
        pointer.transform.SetParent(null);
        //instantiated.transform.localScale = new Vector3(transform.localScale.x*1f, transform.localScale.y, transform.localScale.z * 1f);
        //pointer.transform.localScale = new Vector3(pointer.transform.localScale.x * 0.25f, 0.1f, instantiated.transform.localScale.z * 0.25f);
    }

    public void SetPointerVisibility(bool visibility)
    {
        pointer.SetActive(visibility);
    }
}
