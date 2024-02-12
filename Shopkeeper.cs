using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shopkeeper : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private MeltData ms;
    private float minInterval = 10f;
    private float maxInterval = 15f;
    //[SerializeField] private AudioSource screamSound;
    // Start is called before the first frame update
    void Start()
    {
        //animator = GetComponent<Animator>();
        //ms = transform.parent.parent.GetComponent<MeltScript>();
        ActivateMethodAtRandomInterval();
    }

    // Update is called once per frame
    void Update()
    {
        

    }

 


    private void ActivateMethodAtRandomInterval()
    {
        float randomInterval = Random.Range(minInterval, maxInterval);
        Invoke("ActivateMethod", randomInterval);
    }

    private void ActivateMethod()
    {
        animator.SetBool("pointToSign", true);
        Invoke("ResetVariable", 4.8f);
        Debug.Log("Playing");
        
        //PlayScream();
        // Perform the method activation here
        // Your code logic goes here

        ActivateMethodAtRandomInterval(); // Activate the method again at a new random interval
    }


    private void ResetVariable()
    {
        animator.SetBool("pointToSign", false);
    }
}
