using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryListing : MonoBehaviour
{
    [SerializeField] private Button buttonComponent;
    [SerializeField] private Text amountText;
    [SerializeField] private GameObject amountGradient;
    private InventoryScript invs;
    private InventoryObjectData data;
    private int amount;
    // Start is called before the first frame update
    void Start()
    {
        //buttonComponent = transform.GetChild(0).GetComponent<Button>();
        buttonComponent.onClick.AddListener(() => invs.ButtonClicked(this.transform, data));
    }

    public void SetData(InventoryObjectData x)
    {
        data = x;
        /*if(buttonComponent.GetComponent<Image>() != null)
        {
            Debug.Log("asdasd");
        }*/

        buttonComponent.GetComponent<Image>().sprite = data.GetIcon();
    }

    public void SetInventoryScript(InventoryScript x)
    {
        invs = x;
    }

    public void SetAmount(int x)
    {
        amount = x;
        if(amount == 1)
        {
            amountGradient.SetActive(false);
        }
        else
        {
            amountText.text = "(" + amount + ")";
        }
    }

    public int GetAmount()
    {
        return amount;
    }
}
