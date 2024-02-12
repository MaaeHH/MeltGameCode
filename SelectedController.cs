using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class SelectedController : MonoBehaviour
{
  
    [SerializeField] private FloatScript _selectionCone;
    [SerializeField] private MeltDataMenu _MDM;
    [SerializeField] private GridObjectMenu _GOM;
    [SerializeField] private InviteMeltsMenu _IMM;
    [SerializeField] private MeltSelectorMenu _MSM;
    [SerializeField] private InventoryScript _INV;
    [SerializeField] private MeltCompendium _MC;
    [SerializeField] private CameraControl _camCont;
    [SerializeField] private InviteMeltsSubMenu _IMSM;
    [SerializeField] private AudioController _audioCont;
    private List<Menu> openMenus;

    //[SerializeField] private MainScript _ms;
    // Start is called before the first frame update
    void Start()
    {
        openMenus = new List<Menu>();
    }

    // Update is called once per frame
    void Update()
    {

        //if (Input.GetKeyDown(KeyCode.Escape)){ Deselected(); }
    } 

    public void OnEscapePressed(InputAction.CallbackContext input)
    {
        //Debug.Log("escape");
        if (input.performed) {
            EscapePressed();
          
           
            //Debug.Log("escape");
        }
    }

    public void MeltClicked(GameObject go)
    {

        


        if (openMenus.Count != 0)
        {
            if(openMenus[openMenus.Count - 1] == _MSM)
            {
                if (!_MSM.GetCGO().FullyOccupied())
                {
                    //Debug.Log("c");
                    if (_MSM.GetCGO().AddOccupiedMelt(go.GetComponent<MeltScript>()))
                    {
                        EscapePressed();
                    }
                    else
                    {
                        //MELT BUSY STUFF GOES HERE
                    }
                    //melt.SetGridObj(cgo);




                }
                return;
            }
        }
       
            AddMenu(_MDM.GenerateMenu(go));
        
        //Debug.Log(openMenus.Count);
    }

    public void GridObjectClicked(GameObject go)
    {
        AddMenu(_GOM.GenerateMenu(go));
        //Debug.Log(openMenus.Count);
    }

    public void InventoryOpened()
    {
        AddMenu(_INV.GenerateMenu());
    }

    public void InviteMeltsMenuOpened()
    {
        AddMenu(_IMM.GenerateMenu());
    }

    public void MeltSelectorOpened(GameObject obj)
    {
        AddMenu(_MSM.GenerateMenu(obj));
    }

    public void MeltCompendiumOpened()
    {
        AddMenu(_MC.GenerateMenu());
    }


    public void InvMeltSubMenuOpened()
    {
        AddMenu(_IMSM.GenerateMenu());
    }
    /*public MeltScript GetSelectedMeltScript()
    {
        return clickedMeltScript;
    }*/
   
    private void SetConeForCurrentMenu()
    {
        if (openMenus.Count != 0)
        {
            GameObject go = openMenus[openMenus.Count - 1].GetSubjectGameObj();
            openMenus[openMenus.Count - 1].RefreshMenu();
            if (go != null)
            {
                _selectionCone.gameObject.SetActive(true);
                _selectionCone.SetTarget(go);

                _camCont.MakeLookAt(go.transform);
                //_camCont.SetRotateTarget(go);
                //_camCont.SetMode(1);
            }else if(openMenus[openMenus.Count - 1].GetLockCam())
            {
                _selectionCone.gameObject.SetActive(false);
                _camCont.SetMode(3);
            }else
            {
                _selectionCone.gameObject.SetActive(false);
                _camCont.SetMode(0);
            }
            
          
        }
        else
        {
            _selectionCone.gameObject.SetActive(false);
            _camCont.SetMode(0);
        }
    }
  

    public void AddMenu(Menu toAdd)
    {
        
        if (openMenus.Contains(toAdd))
        {
            openMenus.Remove(toAdd);
        }
        else
        {
            _audioCont.PlayMenuOpenSound();
        }
        toAdd.GetParentTransform().SetAsLastSibling();
        openMenus.Add(toAdd);
        SetConeForCurrentMenu();
    }

    public void EscapePressed()
    {
        if (openMenus.Count != 0)
        {
            _audioCont.PlayMenuClosedSound();
            openMenus[openMenus.Count - 1].CloseMenu();
            openMenus.Remove(openMenus[openMenus.Count - 1]);
            if (openMenus.Count > 0) openMenus[openMenus.Count - 1].RefreshMenu();
            SetConeForCurrentMenu();
        }

    }
}
