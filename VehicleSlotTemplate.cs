using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VehicleSlotTemplate : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private VehicleInstance vehIns;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform vehiclePointTransform;
    [SerializeField] private CheerInc cInc;
    private GameObject myObj = null;
    public void SetVehicleInstance(VehicleInstance x)
    {
        vehIns = x;
        if (vehIns != null)
        {
            ShowGameObj();
        }
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        
        cInc.EnvironmentElementClicked(1);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void ShowGameObj()
    {
        VehicleData obj = vehIns.GetData();

        if (obj.GetFileName() != "" && obj.GetFileName() != null)
        {
            if (myObj != null)
                GameObject.Destroy(myObj);
            if (vehIns.GetMission() == null)
            {
                
                string path = "VehicleFiles/" + obj.GetFileName();
                //Debug.Log(path);

                GameObject gamObj = Resources.Load<GameObject>(path);
                if (gamObj != null)
                {

                    gamObj = Instantiate(gamObj, vehiclePointTransform.position, Quaternion.identity);
                    gamObj.transform.localScale = obj.GetObjectScale();
                    gamObj.transform.eulerAngles = obj.GetObjectRotation();

                    gamObj.transform.SetParent(vehiclePointTransform);
                    Renderer theRenderer = gamObj.GetComponent<Renderer>();

                    if (theRenderer != null)
                    {
                        theRenderer.material.shader = Shader.Find("FlexibleCelShader/Cel Outline");
                        gamObj.transform.position = new Vector3(gamObj.transform.position.x, gamObj.transform.position.y + theRenderer.bounds.size.y / 2, gamObj.transform.position.z);
                    }
                    myObj = gamObj;
                }
            }
           
        }
    }

    public Transform GetCameraTransform()
    {
        return cameraTransform;
    }
    public VehicleInstance GetVehicleInstance()
    {
        return vehIns;
    }

}
