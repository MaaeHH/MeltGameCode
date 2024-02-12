using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/FoodData", order = 1)]
public class FoodData : InventoryObjectData
{
    [SerializeField]private float restorationAmount;
    [SerializeField] private int skipFoodTicks;
    
    public float GetRestorationAmount()
    {
        return restorationAmount;
    }
    public int GetFoodSkips()
    {
        return skipFoodTicks;
    }
  
}
