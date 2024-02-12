using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenDimmer : MonoBehaviour
{
    public event MaxDim OnMaxDim;
    public delegate void MaxDim();
   
    [SerializeField] private Animator dimmerAnimator;

    public void ReachedMaxDim()
    {
        OnMaxDim?.Invoke();
    }
    public void StartDim()
    {
        dimmerAnimator.SetTrigger("StartAnim");
    }
}
