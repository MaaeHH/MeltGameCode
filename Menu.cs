using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;


public abstract class Menu : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CameraControl _camCont;
    [SerializeField] private SelectedController sc;
    
    private GameObject gameObj;
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hovered");
        _camCont.SetUseScreenEdge(false);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("unhovered");
        _camCont.SetUseScreenEdge(true);
    }

    private protected SelectedController GetSelectedController()
    {
        return sc;
    }
    public GameObject GetSubjectGameObj()
    {
        return gameObj;
    }
    public void SetSubjectGameObject(GameObject x)
    {
        gameObj = x;
    }

    public abstract void CloseMenu();

    public Transform GetParentTransform()
    {
        return this.transform.parent;
    }
    public abstract void RefreshMenu();

    public virtual bool GetLockCam()
    {
        return false ;
    }

}
