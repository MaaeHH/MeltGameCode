using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorScript : MonoBehaviour
{

    private Transform cameraTransform;
    private float distanceToCam;
    [SerializeField] private float threshold = 10.0f;
    // Start is called before the first frame update
    void Start()
    {
        cameraTransform = Camera.main.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        distanceToCam  = Vector3.Distance(transform.position, cameraTransform.position);
    }
    public bool GetFarEnough()
    {
        return distanceToCam > threshold;
    }
}
