using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
public class InventoryScript : Menu
{
    //[SerializeField] private List<InventoryObjectData> invSaveSys.GetInventory();
    [SerializeField] private GameObject invTemplate;
    [SerializeField] private Transform itemsTransform;
    [SerializeField] private GridGenerator gridGen;
    [SerializeField] private InventorySaveSystem invSaveSys;
    [SerializeField] private Animator _menuAnim;
    //private int screamCoinAmount = 10;
    [SerializeField] private Text screamcoinText;
    //[SerializeField] private SaveSystem saveSys;

    //private InventorySaveData invSave;
    // Start is called before the first frame update
    void Start()
    {
        invSaveSys.Initialize();

        
        //SaveMeltSystem.DeleteInventorySaveData("InventorySave");
        //invSave = SaveMeltSystem.LoadInventory("InventorySave");
        //if (invSave == null){
        //Debug.Log("No save data found, making new save...");
        //invSave = new InventorySaveData();
        //invSaveSys.GetInventory() = new Dictionary<InventoryObjectData, int>();
        //}
        //SaveMeltSystem.SaveInventory("InventorySave", invSave);
        //invSaveSys.GetInventory() = new List<InventoryObjectData>();
        MakeInventoryList();
        
    }

    // Update is called once per frame
    void Update()
    {
        UpdateScreamcoinText();
    }

    public void UpdateScreamcoinText()
    {
        screamcoinText.text = ""+ SaveSystem.GetScreamcoin();
    }

    public void MakeInventoryList()
    {
        foreach(KeyValuePair<InventoryObjectData, int> invobj in invSaveSys.GetInventory())
        {
            MakeInvItem(invobj.Key, invobj.Value);
        }
    }
    public void RemoveFromInventory(InventoryObjectData toRemove)
    {
        invSaveSys.RemoveFromInventory(toRemove,1);
        ClearMenu();
        MakeInventoryList();
    }

    public void ButtonClicked(Transform buttonTrans, InventoryObjectData objData)
    {
        //RemoveFromInventory(objData);
        if (objData == null) Debug.Log("Null button data");
        gridGen.SetItemToPlace(objData);
    }
    private void MakeInvItem(InventoryObjectData data, int value)
    {
        GameObject currentSpawned = Instantiate(invTemplate);

        currentSpawned.transform.SetParent(itemsTransform);
        //spawnedMelts.Add(currentSpawned.GetComponent<MeltScript>());
        InventoryListing temp = currentSpawned.GetComponent<InventoryListing>();
        temp.SetInventoryScript(this);
        temp.SetData(data);
        temp.SetAmount(value);
        //Debug.Log(data.GetActorName());
        currentSpawned.SetActive(true);
    }
    public void ClearMenu()
    {
        foreach (Transform thing in itemsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }
    public InventoryScript GenerateMenu()
    {
        ClearMenu();
        MakeInventoryList();
        _menuAnim.SetBool("onScreen", true);

        return this;
    }

    public override void RefreshMenu()
    {
        ClearMenu();
        MakeInventoryList();
        UpdateScreamcoinText();
    }
    public override void CloseMenu()
    {
        _menuAnim.SetBool("onScreen", false);
        ClearMenu();
    }
}
