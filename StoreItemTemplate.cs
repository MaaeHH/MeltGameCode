using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StoreItemTemplate : MonoBehaviour
{
    [SerializeField] private Text titleText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text onOrderText;
    [SerializeField] private Image iconImage;
    [SerializeField] private Button plusButton;
    [SerializeField] private Button minusButton;

    private InventoryObjectData data;
    private int amountOnOrder;
    private StorefrontMenu sfm;
    // Start is called before the first frame update
    void Start()
    {
        plusButton.onClick.AddListener(PlusButtonPressed);
        minusButton.onClick.AddListener(MinusButtonPressed);

    }

    public void SetData(InventoryObjectData x)
    {
        data = x;
        titleText.text = data.GetName() + " (" + data.GetCost() + ") screamcoin";
        descriptionText.text = data.GetDescription();
        iconImage.sprite = data.GetIcon();
        amountOnOrder = 0;
        UpdateOnOrderText();
    }

    public void SetStorefrontMenu(StorefrontMenu x)
    {
        sfm = x;
    }

    public void MinusButtonPressed()
    {
        if(amountOnOrder > 0)
        {
            amountOnOrder--;
        }
        sfm.UpdateSummaryText();
        UpdateOnOrderText();
    }

    public void PlusButtonPressed()
    {
        amountOnOrder++;
        sfm.UpdateSummaryText();
        UpdateOnOrderText();
    }

    public void Clear()
    {
        amountOnOrder = 0;
        UpdateOnOrderText();
    }

    private void UpdateOnOrderText()
    {
        onOrderText.text = "On order: " + amountOnOrder + '\n' +"Cost: "+ GetTotalCost();
    }

    public int GetAmount()
    {
        return amountOnOrder;
    }

    public int GetTotalCost()
    {
        return amountOnOrder * data.GetCost();
    }

    public InventoryObjectData GetData()
    {
        return data;
    }
}
