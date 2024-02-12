using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatScript : MonoBehaviour
{
    private GameObject aGO;
    public Vector3 targetPosition;  // the position to float above
    public float floatHeight = 1;  // the height above the target to float at
    public float floatSpeed = 1f;   // the speed at which to float

    private void Update()
    {
        if(aGO != null)
        {
            targetPosition = aGO.transform.position;
            // Calculate the desired position by adding the target position and the float height
            Vector3 desiredPosition = targetPosition;
            desiredPosition.y = desiredPosition.y + floatHeight;
            // Use Mathf.Sin to oscillate the object up and down over time
            float y = Mathf.Sin(Time.time * floatSpeed) * 0.5f;  // multiply by 0.5 to reduce the amplitude
            transform.position = desiredPosition + (Vector3.up * y);
        }
        
    }
    public void SetTarget(GameObject g)
    {
        aGO = g;
    }
}
