using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class SaveData 
{
    public int screamCoinAmount = 10000;

    public int currentDesktopWallpaperIndex = 0;

    public InventoryObjectCounter invObC = new InventoryObjectCounter();

    public float exp = 10000000;

    public int currentVehicleSlots = 3;
   
    public int currentWallSizeX = 10;
    public int currentWallSizeY = 10;
    public int currentMaxWallSizeX = 10;
    public int currentMaxWallSizeY = 10;

    public List<WindowInstance> houseWindowsX1 = new List<WindowInstance>();
    public List<WindowInstance> houseWindowsX2 = new List<WindowInstance>();
    public List<WindowInstance> houseWindowsY1 = new List<WindowInstance>();
    public List<WindowInstance> houseWindowsY2 = new List<WindowInstance>();

    //0 = wall colour, 1 = floor colour, 2 = door colour, 3 = skirting colour
    public List<string> houseColours = new List<string> {"","","",""};
    public List<string> houseMaterials = new List<string> {"","","",""};
  

}
