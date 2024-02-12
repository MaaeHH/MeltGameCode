using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeltIndicator : MonoBehaviour
{
    [SerializeField] private Material lowEnergyIcon;
    [SerializeField] private Material lowFoodIcon;
    [SerializeField] private Material lowCheerIcon;
    [SerializeField] private Material lowHealthIcon;

    [SerializeField] private Renderer thePlane;

    [SerializeField] private Transform thePlaneTransform;
    private MeltData myData;
    private Transform mainCameraTransform;
    private float threshold = 3.0f;
    private float delay = 1.0f;
    private bool inProgress = false;

    [SerializeField] private float minHeight = 1.0f;
    // Start is called before the first frame update
    void Start()
    {
        
        mainCameraTransform = Camera.main.transform;

    }

    void Update()
    {
        if (mainCameraTransform != null)
        {

            transform.LookAt(mainCameraTransform);
        }
    }

    public void SetMeltData(MeltData x)
    {
        myData = x;
        float height = minHeight;
        if(height < myData.GetScale().y)
        {
            height = myData.GetScale().y;
        }
        thePlaneTransform.position = new Vector3(thePlaneTransform.position.x, height, thePlaneTransform.position.z);
    }

    public void ShowIndicator()
    {
        if(!inProgress)
        StartCoroutine(ShowEachIndicator());
    }

    private IEnumerator ShowEachIndicator()
    {
        inProgress = true;
        if (myData != null)
        {
            if (myData.GetCheer() < threshold)
            {
                thePlane.gameObject.SetActive(true);
                thePlane.material = lowCheerIcon;
                yield return new WaitForSeconds(delay);
                thePlane.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }

            if (myData.GetHunger() < threshold)
            {
                thePlane.gameObject.SetActive(true);
                thePlane.material = lowFoodIcon;
                yield return new WaitForSeconds(delay);
                thePlane.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }
            if (myData.GetEnergy() < threshold)
            {
                thePlane.gameObject.SetActive(true);
                thePlane.material = lowEnergyIcon;
                yield return new WaitForSeconds(delay);
                thePlane.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }
            if (myData.GetHealth() < threshold)
            {
                thePlane.gameObject.SetActive(true);
                thePlane.material =  lowHealthIcon;
                yield return new WaitForSeconds(delay);
                thePlane.gameObject.SetActive(false);
                yield return new WaitForSeconds(delay);
            }
        }
        inProgress = false;
    }
}
