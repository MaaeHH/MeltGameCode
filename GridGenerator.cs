using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GridGenerator : MonoBehaviour
{
    private int _width, _height;
    [SerializeField] private Tile _tilePrefab;
    [SerializeField] private BoundsScript boundss;
    [SerializeField] private MeltSpawner meltSpawn;
    [SerializeField] private TileObjectController toc;
    [SerializeField] private FoodSpawner fs;
    [SerializeField] private InventoryScript invs;
    [SerializeField] private GridObjPreview previewObj;
    [SerializeField] private SelectedController sc;
    private int currentDirection;
    private InventoryObjectData toPlace;
    private GridObjectInstance toMove;
    private Tile[,] tileArr;
    private bool rotationDelay = false;
    private int mode = 0; //Mode 0 is no grid, mode 1 is place, mode 2 is move

    private float tileWidth, tileHeight;
    //RectTransform rt;
    // Start is called before the first frame update
    void Start()
    {
        _width = SaveSystem.GetWallSizeX();
        _height = SaveSystem.GetWallSizeY();
        GenerateGrid();
        boundss.SetSquarePosition(GetCorner(), GetOppositeCorner());
        meltSpawn.SetBounds(boundss.GetBounds());
        meltSpawn.ResetVariables();
        meltSpawn.SpawnMelts();
        toc.MakeObjects();
        Active(false);
    }

    void GenerateGrid()
    {
        tileArr = new Tile[_width, _height];
        //rt = (RectTransform)_tilePrefab.transform;
        //tileWidth = rt.rect.width;
        //tileHeight = rt.rect.height;

        Transform tileTransform = _tilePrefab.transform;
        tileWidth = tileTransform.GetComponent<Renderer>().bounds.size.x;
        tileHeight = tileTransform.GetComponent<Renderer>().bounds.size.z;

        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                Tile spawnedTile = Instantiate(_tilePrefab, new Vector3(x * tileWidth, 0, y * tileHeight), Quaternion.identity);
                spawnedTile.name = $"Tile {x} {y}";
                spawnedTile.SetXY(x, y);
                spawnedTile.SetGridGen(this);
                spawnedTile.gameObject.transform.SetParent(this.gameObject.transform);
                spawnedTile.gameObject.SetActive(true);
                tileArr[x, y] = spawnedTile;

            }
        }
        GenerateWindows();
    }

    public void GenerateWindows()
    {
        /*for (int x = 0; x < _width; x++)
        {
            tileArr[x, 0].MakeWall(SaveSystem.GetWalls(2)[x], 2);
            //tileArr[x, 0].MakeWall("DefaultWall", 2);
            tileArr[x, _height - 1].MakeWall(SaveSystem.GetWalls(0)[x], 0);
            //tileArr[x, _height - 1].MakeWall("DefaultWall", 0);
        }



        for (int y = 0; y < _height; y++)
        {
            tileArr[0, y].MakeWall(SaveSystem.GetWalls(3)[y], 3);
            //tileArr[0, y].MakeWall("DefaultWall", 3);
            tileArr[_width-1, y].MakeWall(SaveSystem.GetWalls(1)[y], 1);
            //tileArr[_width-1, y].MakeWall("DefaultWall", 1);
        }*/
    }

    public Vector3 GetOppositeCorner()
    {
        //Debug.Log(w)
        //return tileArr[_width-1, _height-1].gameObject.GetComponent<Renderer>().bounds.max;

        GameObject planeObject = tileArr[_width - 1, _height - 1].gameObject;

        // Get the size of the plane
        Vector3 planeSize = planeObject.GetComponent<Renderer>().bounds.size;

        // Get the position of the plane
        Vector3 planePosition = planeObject.transform.position;

        // Calculate the bottom right corner
        Vector3 bottomRightCorner = planePosition + new Vector3(planeSize.x / 2, 0, planeSize.z / 2);

        return bottomRightCorner;
    }

    public Vector3 GetCorner()
    {
        //return tileArr[0,0].gameObject.GetComponent<Renderer>().bounds.min;

        GameObject planeObject = tileArr[0, 0].gameObject;

        // Get the size of the plane
        Vector3 planeSize = planeObject.GetComponent<Renderer>().bounds.size;

        // Get the position of the plane
        Vector3 planePosition = planeObject.transform.position;

        // Calculate the top left corner
        Vector3 topLeftCorner = planePosition + new Vector3(-planeSize.x / 2, 0, -planeSize.z / 2);
        return topLeftCorner;
    }

    public Tile GetTile(int x, int y)
    {
        return tileArr[x, y];
    }

    void ColourGrid(int playerX, int playerY)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int y = 0; y < _height; y++)
            {
                tileArr[x, y].AltColour(true);
            }
        }

    }
    public void TileHovered(Tile hoveredTile)
    {

        switch (mode)
        {
            case 0:
                //previewObj.gameObject.SetActive(false);
                break;
            case 1:
                if (toPlace != null && toPlace is GridObjectData)
                {
                    previewObj.SetObjectData((GridObjectData)toPlace);
                    previewObj.SetTile(hoveredTile);
                }
                break;
            case 2:
                if (toMove != null)
                {
                    previewObj.SetObjectInstance(toMove);
                    previewObj.SetTile(hoveredTile);

                }
                break;
        }




    }

    public void TileClicked(Tile clickedTile)
    {
        switch (mode)
        {
            case 0:
                break;
            case 1:
                PlaceItem(clickedTile);
                break;
            case 2:
                MoveItem(clickedTile);
                break;
        }
     
    }
    private void PlaceItem(Tile clickedTile)
    {
        toMove = null;
        bool successful = false;
        if (toPlace == null && toMove == null)
        {
            Active(false);
        }
        else if (toPlace is GridObjectData)
        {
            GridObjectData obj = (GridObjectData)toPlace;

            GridObjectInstance ins = new GridObjectInstance(clickedTile.GetX(), clickedTile.GetY(), currentDirection, obj);

            ins.SetLastUpdate(DateTime.Now);
            ins.SetData(obj);
            //ins.InitTileFromCoOrds(this);//Kinda unnecessary, should be done when loaded
            //obj.SetTile(clickedTile);

            //if (ins == null) Debug.Log("null instance");
            if (toPlace != null) Debug.Log("null toPlace");

            toc.MakeObject(ins, true);
            successful = true;
        }
        else if (toPlace is FoodData)
        {
            FoodData food = (FoodData)toPlace;
            fs.SpawnFoodAtTile(clickedTile, food);
            successful = true;
        }
        if (successful)
        {
            previewObj.gameObject.SetActive(false);
            Active(false);
            invs.RemoveFromInventory(toPlace);
            toPlace = null;
            toMove = null;
        }
    }
    private void MoveItem(Tile clickedTile)
    {
        toPlace = null;
        bool successful = false;
        if (toPlace == null && toMove == null)
        {
            Active(false);
        }
        toMove.SetX(clickedTile.GetX());
        toMove.SetY(clickedTile.GetY());
        toMove.SetDirection(currentDirection);
        toMove.InitTileFromCoOrds(this);
        //GridObjectInstance ins = new GridObjectInstance(clickedTile.GetX(), clickedTile.GetY(), currentDirection, 0, obj);

        //ins.SetLastUpdate(DateTime.Now);
        //ins.SetData(obj);
        //ins.InitTileFromCoOrds(this);//Kinda unnecessary, should be done when loaded
        //obj.SetTile(clickedTile);
        //toc.MakeObject(ins, true);

        successful = true;

        //toc.Save();
        TileObjectSaver.SaveData();
        

        if (successful)
        {
            previewObj.gameObject.SetActive(false);
            Active(false);

            sc.EscapePressed();
            //invs.RemoveFromInventory(toPlace);
            toMove = null;
            toPlace = null;
            toc.RefreshObjects();
        }
    }





    public void SetItemToPlace(InventoryObjectData data){
        currentDirection = 0;
        previewObj.SetDirection(currentDirection);
        Active(true);
        previewObj.gameObject.SetActive(true);
        toPlace = data;
        mode = 1;
    }

    public void SetItemToMove(GridObjectInstance data)
    {
        
        Active(true);
        previewObj.gameObject.SetActive(true);
        toMove = data;
        currentDirection = 0;
        previewObj.SetDirection(currentDirection);
        
        mode = 2;

        //currentDirection = toMove.GetDirection();
        //previewObj.SetDirection(currentDirection);

    }

    

    public Tile[,] GetTiles(){
        return tileArr;
    }

    public void rotateRight()
    {
        if (!rotationDelay)
        {
            currentDirection++;
            if (currentDirection >= 4)
            {
                currentDirection = 0;
            }
            previewObj.SetDirection(currentDirection);
            rotationDelay = true;
            StartCoroutine(RotateDelay());
        }
        
    }

    private IEnumerator RotateDelay()
    {
        yield return new WaitForSeconds(0.4f);
        rotationDelay = false;
    }

    public void rotateLeft()
    {
        if (!rotationDelay)
        {
            currentDirection--;
            if (currentDirection < 0)
            {
                currentDirection = 3;
            }
            previewObj.SetDirection(currentDirection);
            rotationDelay = true;
            StartCoroutine(RotateDelay());
        }
    }

    public void Active(bool show)
    {
        foreach (Transform child in this.transform)
        {
            child.gameObject.SetActive(show);
        }
    }
}