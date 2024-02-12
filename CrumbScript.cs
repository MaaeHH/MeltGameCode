using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class CrumbScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    private CrumbController cc;
    public void SetCC(CrumbController crumb)
    {
        cc = crumb;
    }
    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("crumb clicked");
        transform.localScale -= Vector3.one * 0.2f;

        // Check if the object is small enough
        if (transform.localScale.x <= 0.3f)
        {

            cc.DeleteCrumb(this.gameObject);
        }
    }
    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("a");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("b");
    }

  
}
