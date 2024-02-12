using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class GridObjectInstance
{
    [SerializeField] private int xCoOrd;
    [SerializeField] private int yCoOrd;
    //[SerializeField] private int level = 0;
    [SerializeField] private long lastUpdate;
    private Tile thisTile;
    [SerializeField] private int direction = 0;
    //[SerializeField] private GridObjectData gridData;
    [SerializeField] private int gridObjectDataIndex;
    public GridObjectInstance(int x, int y,int direction, GridObjectData data)
    {
        SetX(x);
        SetY(y);
        SetData(data);
        SetDirection(direction);
        //SetLastUpdate(DateTime.Now);
    }
    public void SetDirection(int x)
    {
        direction = x;
    }
    public int GetDirection()
    {
        return direction;
    }
    public void SetData(GridObjectData x)
    {
        gridObjectDataIndex = (TileObjectSaver.GetIDFromData(x));
        
    }
    public GridObjectData GetData()
    {
        return (GridObjectData)TileObjectSaver.GetDataFromID(gridObjectDataIndex);
        //return gridData;
    }

    public int GetX()
    {
        return thisTile.GetX();
    }
    public int GetY()
    {
        return thisTile.GetY();
    }

    public void SetX(int x)
    {
        xCoOrd = x;
    }
    public void SetY(int y)
    {
        yCoOrd = y;
    }

    public void InitTileFromCoOrds(GridGenerator ggen)
    {
        thisTile = ggen.GetComponent<GridGenerator>().GetTile(xCoOrd, yCoOrd);
    }

    //public void SetTile(Tile x)
    //{
    //    thisTile = x;
    //}

    public Tile GetTile()
    {
        return thisTile;
    }

    public DateTime GetLastUpdate()
    {
        return new DateTime(lastUpdate);
    }
    public void SetLastUpdate(DateTime x)
    {
        lastUpdate = x.Ticks;
    }

}
