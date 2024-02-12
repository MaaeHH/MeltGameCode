using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class InventorySaveSystem : MonoBehaviour
{

    [SerializeField] private Dictionary<InventoryObjectData, int> inventoryToSave = null;

    private static Dictionary<int, InventoryObjectData> allInventoryObjectDataCodes = new Dictionary<int, InventoryObjectData>();

    private static int HashInventoryObjectData(InventoryObjectData item) => Animator.StringToHash(item.GetName());

    const char SPLIT_CHAR = '_';

    private static string FILE_PATH;

    private bool initialized = false;

    private void Awake()
    {
        FILE_PATH = Application.persistentDataPath + "/Inventory.txt";

        CreateInventoryObjectDataDictionary();
    }

    [SerializeField] private List<InventoryObjectData> startingItems;

    public void Initialize()
    {
        if (!initialized)
        {
            inventoryToSave = LoadInventory();
            if (inventoryToSave == null)
            {
                Debug.Log("New inventory made");
                inventoryToSave = new Dictionary<InventoryObjectData, int>();
                foreach(InventoryObjectData iod in startingItems)
                {
                    AddToInventory(iod, 1);
                }
            }
            SaveInventory();
            initialized = true;
            //Debug.Log("Inventory with " + inventoryToSave.Count + " items loaded.");
        }
    }

    private void OnDisable()
    {
        SaveInventory();
    }

    public Dictionary<InventoryObjectData, int> GetInventory()
    {
        return inventoryToSave;
    }

    private void CreateInventoryObjectDataDictionary()
    {
        InventoryObjectData[] allInventoryObjectDatas = Resources.FindObjectsOfTypeAll<InventoryObjectData>();

        foreach (InventoryObjectData i in allInventoryObjectDatas)
        {
            int key = HashInventoryObjectData(i);

            if (!allInventoryObjectDataCodes.ContainsKey(key))
                allInventoryObjectDataCodes.Add(key, i);
        }
    }

    public void SaveInventory()
    {
        using (StreamWriter sw = new StreamWriter(FILE_PATH))
        {
            foreach (KeyValuePair<InventoryObjectData, int> kvp in inventoryToSave)
            {
                InventoryObjectData item = kvp.Key;
                int count = kvp.Value;

                string itemID = HashInventoryObjectData(item).ToString();

                sw.WriteLine(itemID + SPLIT_CHAR + count);
            }
        }
    }

    private bool InventorySaveExists()
    {
        if (!File.Exists(FILE_PATH))
        {
            Debug.LogWarning("The file you're trying to access doesn't exist. (Try saving an inventory first).");
            return false;
        }
        return true;
    }

    //Delete all items in the inventory. Will be irreversable. Could just create a new file (ie. Change the name of the old save file and create a new one)
    public void ClearInventorySaveFile()
    {
        if (!InventorySaveExists())
            return;

        File.WriteAllText(FILE_PATH, "");
    }

    internal Dictionary<InventoryObjectData, int> LoadInventory()
    {
        if (!InventorySaveExists())
            return null;

        Dictionary<InventoryObjectData, int> inventory = new Dictionary<InventoryObjectData, int>();

        string line = "";

        using (StreamReader sr = new StreamReader(FILE_PATH))
        {
            while ((line = sr.ReadLine()) != null)
            {
                int key = int.Parse(line.Split(SPLIT_CHAR)[0]);
                InventoryObjectData item = allInventoryObjectDataCodes[key];
                int count = int.Parse(line.Split(SPLIT_CHAR)[1]);

                inventory.Add(item, count);
                //Debug.Log("adding");
            }
        }

        return inventory;
    }

    public void AddToInventory(InventoryObjectData item, int count = 1)
    {
        if (!inventoryToSave.ContainsKey(item))
            inventoryToSave.Add(item, count);

        else
            inventoryToSave[item] += count;

        //onInventoryChange?.Invoke();
    }

    public void RemoveFromInventory(InventoryObjectData item, int count)
    {
        if (!inventoryToSave.ContainsKey(item))
            throw new System.Exception("The dictionary doesn't contain that item. How did we get here?");

        if (inventoryToSave[item] >= count)
        {
            inventoryToSave[item] -= count;
            if(inventoryToSave[item] == 0)
            {
                inventoryToSave.Remove(item);
            }
        }
            
        else
        {
            Debug.LogWarning($"Inventory contains less that {count} of {item.name}. Setting count to zero");
            inventoryToSave[item] = 0;
        }

        //onInventoryChange?.Invoke();
    }

}
