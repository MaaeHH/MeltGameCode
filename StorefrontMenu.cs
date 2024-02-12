using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorefrontMenu : MonoBehaviour
{
    private Storefront data;
    [SerializeField] private Text titleText;
    [SerializeField] private Text summaryText;
    [SerializeField] private Button buyButton;
    [SerializeField] private Button clearButton;
    [SerializeField] private GameObject storeItemTemplate;
    [SerializeField] private Transform itemsTransform;
    [SerializeField] private InventorySaveSystem invSaveSys;
    //private int screamCoinAmount = 10;
    private List<StoreItemTemplate> spawnedItemTemplates;
    //[SerializeField] private SaveSystem saveSys;
    
    //bool isWorkingHours = false;
    //InventorySaveData invSave;
    public void SetStorefront(Storefront x)
    {
        
        ClearItems();
        data = x;
        titleText.text = data.GetName();

        MakeMenuItems();
        UpdateSummaryText();

        //invSave = SaveMeltSystem.LoadInventory("InventorySave");
    }

  


   
    public void UpdateSummaryText()
    {
        summaryText.text = "Total order amount: " + GetTotalOrderCost() + " screamcoin. You have " + SaveSystem.GetScreamcoin() + " screamcoin.";
    }

    private int GetTotalOrderCost()
    {
        int total = 0;

        foreach (StoreItemTemplate template in spawnedItemTemplates)
        {
            total = total + template.GetTotalCost();
        }
        return total;
    }
  

    public void ClearOrder()
    {
        foreach (StoreItemTemplate template in spawnedItemTemplates)
        {
            template.Clear();
        }

        UpdateSummaryText();
    }

    // Start is called before the first frame update
    void Start()
    {
        
        //invSave = SaveMeltSystem.LoadInventory("InventorySave");
        spawnedItemTemplates = new List<StoreItemTemplate>();
        buyButton.onClick.AddListener(BuyButtonPressed);
        clearButton.onClick.AddListener(ClearOrder);
    }

    private void MakeMenuItems()
    {
        foreach(InventoryObjectData iod in data.GetItemsToSell())
        {
            MakeMenuItem(iod);
        }
    }

    private void MakeMenuItem(InventoryObjectData iod)
    {
        GameObject currentSpawned = Instantiate(storeItemTemplate);
        currentSpawned.transform.SetParent(itemsTransform);
        spawnedItemTemplates.Add(currentSpawned.GetComponent<StoreItemTemplate>());
        currentSpawned.GetComponent<StoreItemTemplate>().SetData(iod);
        currentSpawned.GetComponent<StoreItemTemplate>().SetStorefrontMenu(this);
        //Debug.Log(data.GetActorName());
        currentSpawned.SetActive(true);
    }

    public void ClearItems()
    {
        foreach (Transform thing in itemsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

    private void BuyButtonPressed(){
        int total = GetTotalOrderCost();
        if (SaveSystem.GetScreamcoin() >= total)
        {
            SaveSystem.SubtractScreamcoin(total);
        }
        foreach (StoreItemTemplate template in spawnedItemTemplates)
        {

            invSaveSys.AddToInventory(template.GetData(), template.GetAmount());
            //for (int x = 0; x < template.GetAmount(); x++)
            //{
            //    invSaveSys.GetInventory().Add(template.GetData());
            //}
            template.Clear();
        }
        UpdateSummaryText();
        //ClearItems();
        invSaveSys.SaveInventory();
        //SaveMeltSystem.SaveInventory("InventorySave", invSave);
    }
}
