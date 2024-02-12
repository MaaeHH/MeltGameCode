using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseSizePreview : MonoBehaviour
{
    [SerializeField] private GameObject thePreview;
    // Start is called before the first frame update
    void Start()
    {
        thePreview.transform.localScale = new Vector3(SaveSystem.GetWallSizeX(), 1, SaveSystem.GetWallSizeY());
        SaveSystem.OnHouseSizeChanged += RefreshPreview;
    }

    void OnDestroy()
    {
        SaveSystem.OnHouseSizeChanged -= RefreshPreview;
    }

   
    public void RefreshPreview() {
        thePreview.transform.localScale = new Vector3(SaveSystem.GetWallSizeX(),1, SaveSystem.GetWallSizeY());
    }
}
