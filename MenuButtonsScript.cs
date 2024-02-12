using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuButtonsScript : MonoBehaviour
{
    [SerializeField] private Animator dropDownAnims;
    bool open = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void ToggleDropdowns()
    {
        open = !open;
        dropDownAnims.SetBool("open", open);
        
    }
}
