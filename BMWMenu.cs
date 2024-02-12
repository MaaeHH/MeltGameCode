using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;



public abstract class BMWMenu : MonoBehaviour
{
    [SerializeField] private Transform cameraLocationTransform;
    [SerializeField] protected GameObject previewObject;

    public Transform GetCameraLocation()
    {
        return cameraLocationTransform;
    }
  
    public abstract void CloseMenu();


    public abstract void RefreshMenu();

    
 
}