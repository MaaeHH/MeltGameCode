using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class GridObjectsSave
{
    public List<GridObjectInstance> gridObjects;

    public GridObjectsSave(List<GridObjectInstance> gridObjects)
    {
        this.gridObjects = gridObjects;
    }
}
