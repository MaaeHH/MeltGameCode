using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private List<Animator> transitionAnimators;
    [SerializeField] private List<float> transitionDelays;
    public static int lastTransition = -1;
    //[SerializeField] private GameObject theCanvas;
    // Start is called before the first frame update
    void Start()
    {
        //if(lastTransition != 0)
        //{
        //    theCanvas.SetActive(true);
        //}
        if(lastTransition != -1) 
        transitionAnimators[lastTransition].SetTrigger("Return");
        lastTransition = -1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    public void ChangeLevel(int x, string levelName)
    {
        lastTransition = x;
       
        StartCoroutine(Transition(x, levelName));
       
    }

    IEnumerator Transition(int x, string levelName)
    {
        //transitionAnimators[x].gameObject.SetActive(true);
        transitionAnimators[x].SetTrigger("Start");
        yield return new WaitForSeconds(transitionDelays[x]);
        
        SceneManager.LoadScene(levelName);
    } 


   
}
