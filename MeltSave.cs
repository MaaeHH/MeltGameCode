using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[System.Serializable]
public class MeltSave
{
    [SerializeField] public long lastUpdateTicks = DateTime.Now.Ticks;
    [SerializeField] public float cheer = 10;
    [SerializeField] public float hunger = 10;
    [SerializeField] public float energy = 10;
    [SerializeField] public float health = 10;

    [SerializeField] public bool restoreLastUpdate = false;
    [SerializeField] public bool wasSleeping = false;

    [SerializeField] public float r;
    [SerializeField] public float g;
    [SerializeField] public float b;
    [SerializeField] public float a;
    //[SerializeField] public float rate = 1.0f;
    [SerializeField] public float pitch = 1.0f;
    [SerializeField] public float volume = 1.0f;
    [SerializeField] public Vector3 scale;
    //[SerializeField] public int level;
    [SerializeField] public float exp;

    [SerializeField] public List<string> accessoriesDataNames = new List<string>();

    [SerializeField] public List<LocalFR> localFrs;
    [SerializeField] public List<GlobalFR> globalFrs;
    [SerializeField] public bool frsMade = false;
    [SerializeField] public InventoryObjectCounter invObC = new InventoryObjectCounter();

    [SerializeField] public float unfriendTicks = 25;

    [SerializeField] public bool hasBeenFriends = false;

    [SerializeField] public string textureName;
    
}
