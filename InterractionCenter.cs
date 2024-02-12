using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterractionCenter : MonoBehaviour
{
    [SerializeField] private Animator interractionAnim;
    public void StartAnim()
    {
        
    }

    public void EndAnim()
    {
        interractionAnim.SetTrigger("Finish");
    }

    public void AnimOver()
    {
        GameObject.Destroy(gameObject);
    }
}
