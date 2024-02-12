using System.Collections;
using UnityEngine;
public class HideIfBehind : MonoBehaviour
{
    private Transform cameraTransform; // Assign the camera's transform in the Inspector.
    private float rotationThreshold = 60.0f; // Adjust the threshold as needed.

    private GameObject childObjectToHide;
    [SerializeField] private GameObject backOfWall;
    [SerializeField] private FloorScript theFloor;

    int fade = 0;

    private void Start()
    {
        targetMaterial = backOfWall.GetComponent<Renderer>().material;
        cameraTransform = Camera.main.transform.parent;
        if (transform.childCount > 0)
        {
            childObjectToHide = transform.GetChild(0).gameObject;
        }
    }

    bool prevBool = false;


    void Update()
    {
        if (childObjectToHide == null || cameraTransform == null)
        {
            Debug.LogWarning("Make sure to assign the camera and the child object in the Inspector.");
            return;
        }

        // Calculate the angle between the camera's forward vector and the object's forward vector.
        float angle = Vector3.Angle(transform.forward, cameraTransform.forward);
      
        /*childObjectToHide.SetActive(true);
        if (theFloor.GetFarEnough())
        {
            childObjectToHide.SetActive(true);
        }
        else
        {
            
        }*/
        childObjectToHide.SetActive(!(angle < rotationThreshold));
        /*if (theFloor.GetFarEnough() != prevBool && theFloor.GetFarEnough())
        {
            FadeToOpaque();
            prevBool = true;
        }
        else if (theFloor.GetFarEnough() != prevBool && !theFloor.GetFarEnough())
        {
            FadeToTransparent();
            
            prevBool = false;
        }*/




    }
    public Material targetMaterial;
    public float fadeDuration = 1.0f;
    private float currentAlpha = 1.0f;
    private Coroutine fadeCoroutine;

    public void FadeToTransparent()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToAlpha(0.0f));
    }

    public void FadeToOpaque()
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        fadeCoroutine = StartCoroutine(FadeToAlpha(1.0f));
    }

    private IEnumerator FadeToAlpha(float targetAlpha)
    {
        float startAlpha = currentAlpha;
        float elapsedTime = 0.0f;

        while (elapsedTime < fadeDuration)
        {
            currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);

            Color color = targetMaterial.color;
            color.a = currentAlpha;
            targetMaterial.color = color;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        currentAlpha = targetAlpha;
        fadeCoroutine = null;
    }

}