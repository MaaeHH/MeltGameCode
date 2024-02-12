using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarketFlowBound : MonoBehaviour
{
    [SerializeField] private MarketFlowBound nextMFB;
    [SerializeField] private MarketFlowBound previousMFB;
    [SerializeField] private MarketFlowBound storeFrontMFB;
    [SerializeField] private Transform focalPoint;

    public float length = 3f; // The length of each side of the square bounds
    public float width = 3f;
    private Bounds squareBounds;

    private void Start()
    {
        CalculateSquareBounds();
        GetComponentInParent<MarketMeltSpawner>().Ready();
    }

    private void CalculateSquareBounds()
    {
        // Calculate the bounds based on the length
        Vector3 center = transform.position;
        Vector3 size = new Vector3(length, 0f, width);
        squareBounds = new Bounds(center, size);
    }

    private void OnDrawGizmos()
    {
        CalculateSquareBounds();
        // Draw the square bounds using Gizmos
        if(focalPoint == null)
        {
            Gizmos.color = Color.green;
        }
        else
        {
            Gizmos.color = Color.blue;
        }
        
        Gizmos.DrawWireCube(squareBounds.center, squareBounds.size);
    }

    public MarketFlowBound GetNext()
    {
        return nextMFB;
    }

    public MarketFlowBound GetPrevious()
    {
        return previousMFB;
    }
    public MarketFlowBound GetStoreFront()
    {
        return storeFrontMFB;
    }
    public Transform GetFocalPoint()
    {
        return focalPoint;
    }

    public Bounds GetBounds()
    {
        return squareBounds;
    }

}
