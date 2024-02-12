using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoursMenu : MonoBehaviour
{
    [SerializeField] private GameObject colourTemplate;
    [SerializeField] private Transform holder;
    [SerializeField] private Renderer meltRenderer;
    [SerializeField] private GameObject meltGamObj;
    private MeltScript meltUsing = null;
    [SerializeField] private Animator sprayAnimator;
    [SerializeField] private Renderer canRenderer;
    [SerializeField] private MeltAppearance appearanceScript;
    public void ButtonPressed(UnlockableColour x)
    {
        StartCoroutine(Transition(x.colour));
    }


    IEnumerator Transition(Color x)
    {
        canRenderer.material.SetColor("_Color", x);
        sprayAnimator.SetTrigger("Start");
      
        yield return new WaitForSeconds(4f);

        meltUsing.SaveNewColour(x);
        appearanceScript.RefreshAppearance();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (Passover.PassedGOD == null)
        {
            Debug.Log("NullPassed");
        }
        else
        {
            Debug.Log("Occupants: " + Passover.PassedOccupants.Count);
        }
       


        if(Passover.PassedOccupants == null || Passover.PassedOccupants.Count == 0)
        {
            //meltGamObj.SetActive(false);
            appearanceScript.SetInvisible(true);
        }
        else
        {

            meltUsing = Passover.PassedOccupants[0];
            
            appearanceScript.SetMeltData(meltUsing.GetMeltData());
                //SetAColour(meltUsing.GetLastColour());
        }
        
    }

  
    public void MakeMenu(List<UnlockableColour> unlockableColours)
    {
        ClearMenu();
        foreach (UnlockableColour uc in unlockableColours)
        {
            GameObject currentSpawned = Instantiate(colourTemplate);



            currentSpawned.transform.SetParent(holder);
            ColourTemplate ct = currentSpawned.GetComponent<ColourTemplate>();
            ct.SetColoursMenu(this);
            ct.SetUnlockedColour(uc);

            currentSpawned.SetActive(true);
        }
    }

    private void ClearMenu()
    {
        foreach(Transform child in holder.transform)
        {
            Destroy(child);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
