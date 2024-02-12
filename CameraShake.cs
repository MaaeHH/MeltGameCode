using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public float swayAmount = 0.25f; // Adjust the amount of sway
    public float swaySpeed = 0.85f;   // Adjust the speed of the sway

    private Quaternion initialRotation;

    void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Calculate sway based on Perlin noise for smooth random movement
        float swayX = Mathf.PerlinNoise(Time.time * swaySpeed, 0) * 2 - 1;
        float swayY = Mathf.PerlinNoise(0, Time.time * swaySpeed) * 2 - 1;

        // Apply the sway to the camera rotation
        Quaternion swayRotation = Quaternion.Euler(new Vector3(swayY, swayX, 0) * swayAmount);
        transform.localRotation = initialRotation * swayRotation;
    }

}
