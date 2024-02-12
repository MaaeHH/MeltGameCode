using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyWindowPreview : MonoBehaviour
{
    private GameObject myObj;
    private float rotationSpeed = 10.0f;
    private float floatAmplitude = 0.5f;
    private float floatSpeed = 0.7f;

    private Vector3 startPosition;
    public void SetPreview(GameObject x)
    {
        if (myObj != null) {
            GameObject.Destroy(myObj);
            myObj = null;
        }

        myObj = GameObject.Instantiate(x);
        myObj.transform.parent = this.transform;
        myObj.SetActive(true);
        myObj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        startPosition = myObj.transform.position;
    }
 

    // Update is called once per frame
    void Update()
    {
        if (myObj != null)
        {
            // Rotate the GameObject
            myObj.transform.Rotate(new Vector3(0,0,1) * rotationSpeed * Time.deltaTime);

            // Float the GameObject up and down
            float newY = startPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            myObj.transform.position = new Vector3(transform.position.x, newY, transform.position.z);
        }
    }
}
