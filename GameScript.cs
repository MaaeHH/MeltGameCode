using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    [SerializeField] private GridGenerator gridGen;
    //[SerializeField] private GameObject selectTargetScreen;
    private List<Tile> tempList;
    private List<Transform> transforms;
    
    private Tile lastClicked = null;
    private int clickMode = 0;//0 = Move, 1 = target
    
    
    Tile temp;
    // Start is called before the first frame update
    void Start()
    {
        //commands = (CommandsDragAndDrop)GameObject.FindObjectOfType(typeof(CommandsDragAndDrop));
        
        //selectTargetScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Clicked(int x, int y)
    {
        Debug.Log("x:" + x + " y:" + y);
     
        switch (clickMode)
        {
            case 0:
                
                break;
            case 1:
                lastClicked = gridGen.GetTile(x, y);
                
                break;
            default:
                Debug.Log("Invalid click mode!( " + clickMode + ")");
                break;
        }


    }

    public void Hovered(int x, int y)
    {
        switch (clickMode)
        {
            case 0:

                break;
            case 1:
                

                break;
            default:
                Debug.Log("Invalid click mode!( " + clickMode + ")");
                break;
        }
    }

    public void UnHovered(int x, int y)
    {

    }


    public Tile GetSelectedTile()
    {
        return lastClicked;
    }

    public void SetClickMode(int x)
    {
        clickMode = x;

        lastClicked = null;
        //selectTargetScreen.SetActive(false);

        switch (clickMode)
        {
            case 0:
                gridGen.Active(false);
                //GenerateMoves(pX, pY, distance, false);
                break;
            case 1:
                gridGen.Active(true);
                //lastClicked = gridGen.GetTile(x, y);
                lastClicked = null;
                //selectTargetScreen.SetActive(true);
                break;
            default:
                Debug.Log("Invalid click mode!( " + clickMode + ") Defaulting to 0");
                clickMode = 0;
                break;
        }


    }
    public int GetClickMode()
    {
        return clickMode;
    }

 
    
   
    
}
