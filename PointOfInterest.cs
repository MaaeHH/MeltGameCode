using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PointOfInterest : MonoBehaviour
{
    
    public Camera targetCamera;



    
    private Transform mainCameraTransform;
    private Vector3 screenCenter;
    [SerializeField] private GameObject gamObj;
    [SerializeField] private string locationScene;
    [SerializeField] private int locationTransition;
    [SerializeField] private Animator poiAnim;
    [SerializeField] private Text titleText;
    [SerializeField] private string title;
    [SerializeField] private Transform thePoint;
    private MapCameraControl mcc;

    void Start()
    {
        titleText.text = title;
    }

    public int GetTransition()
    {
        return locationTransition;
    }

    public string GetScene()
    {
        return locationScene;
    }

    public string GetTitle()
    {
        return title;
    }

    public void SetMCC(MapCameraControl x)
    {
        mcc = x;
    }

    public float GetDistanceToFocalPoint()
    {
        return Vector3.Distance(mcc.GetFocalPoint(), thePoint.position);
    }

    public Transform GetPoint()
    {
        return thePoint;
    }

    private void Update()
    {
        
        
        /*if (targetCamera != null)
        {
            // Calculate the direction from the GameObject to the camera
            Vector3 lookDirection = targetCamera.transform.position - thePoint.position;

            // Calculate the rotation to look at the camera
            Quaternion rotation = Quaternion.LookRotation(lookDirection, Vector3.up);

            // Apply the rotation to the GameObject, restricting y-axis rotation
            thePoint.rotation = Quaternion.Euler(-rotation.eulerAngles.x, thePoint.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
        }
        else
        {
            Debug.LogWarning("Target camera is not assigned!");
        }
        */

    }

    public void activateUIElement()
    {
        //gamObj.SetActive(true);
        poiAnim.SetBool("Appear", true);
    }
    public void deactivateUIElement()
    {
        //gamObj.SetActive(false);
        poiAnim.SetBool("Appear", false);
    }
    
}
