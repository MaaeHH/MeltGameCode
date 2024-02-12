using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Tile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private GameObject particles;
    [SerializeField] private GameScript gs;
    [SerializeField] private Color _normalColour, _rangeColour, _projectileRangeColour;
    [SerializeField] private Renderer tileRenderer;
    [SerializeField] private TileObjectController toc;
    [SerializeField] private Transform windows;
    //[SerializeField] private Material wallMaterial;
    private List<GameObject> myWindows = new List<GameObject>();
    private ClickableGridObject occupiedBy;
    private GridGenerator gridGen;
    private int xCoOrd;
    private int yCoOrd;
    // Start is called before the first frame update
    void Start()
    {
        particles.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetOccupiedBy(ClickableGridObject x)
    {
        occupiedBy = x;
    }
    public ClickableGridObject GetOccupied()
    {
        return occupiedBy;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        gs.Hovered(xCoOrd, yCoOrd);
        particles.SetActive(true);
        gridGen.TileHovered(this);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        gs.UnHovered(xCoOrd, yCoOrd);
        particles.SetActive(false);
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
            gs.Clicked(xCoOrd, yCoOrd);
        gridGen.TileClicked(this);
        //Debug.Log("asdasda");
    }

    public void AltColour(bool isColour)
    {

        //tileRenderer.material.color = isColour ? _rangeColour : _normalColour;

        if (isColour)
        {


            switch (gs.GetClickMode())
            {
                case 0:
                    tileRenderer.material.color = _rangeColour;
                    break;
                case 1:
                    tileRenderer.material.color = _projectileRangeColour;
                    break;
                default:
                    Debug.Log("Tile colour error");
                    break;
            }
        }
        else
        {
            tileRenderer.material.color = _normalColour;
        }
    }

    public void DestroyWindows()
    {
        if (myWindows != null)
        {
            foreach (GameObject window in myWindows)
            {
                GameObject.Destroy(window);
                //wall.GetComponent<Renderer>().material.SetColor("_Color", SaveSystem.GetWallColour());
            }
        }
    }
    
    /*public void MakeWindow(string windowName, int edgeToFit)
    {
        //if (myWall != null) GameObject.Destroy(myWall);
        float wallHeight = 0.65f;
        string path = "Windows/" + windowName;
   

        GameObject planePrefab = Resources.Load<GameObject>(path);
        if (planePrefab != null)
        {
            
            BoxCollider firstCollider = GetComponent<BoxCollider>();
            //Renderer secondRenderer = secondPlane.GetComponent <Renderer>();

            // Calculate the position for the second plane's pivot
            Vector3 newPosition = firstCollider.bounds.center;

            switch (edgeToFit)
            {
                case 0:
                    newPosition.z += firstCollider.size.z * 0.5f * this.transform.localScale.z; //+ secondRenderer.bounds.extents.z;
                    break;
                case 1:
                    newPosition.x += firstCollider.size.x * 0.5f * this.transform.localScale.x; //+ secondRenderer.bounds.extents.x;
                    break;
                case 2:
                    newPosition.z -= firstCollider.size.z * 0.5f * this.transform.localScale.z; //+ secondRenderer.bounds.extents.z;
                    break;
                case 3:
                    newPosition.x -= firstCollider.size.x * 0.5f * this.transform.localScale.x; //+ secondRenderer.bounds.extents.x;
                    break;
                default:
                    Debug.LogError("Invalid edgeToAlign value. Use 0 for top, 1 for right, 2 for bottom, or 3 for left.");
                    return;
            }
            GameObject instantiated = GameObject.Instantiate(planePrefab, newPosition, new Quaternion());

            instantiated.transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, wallHeight);
            instantiated.transform.eulerAngles = new Vector3(90, 90+(90 * -(3-edgeToFit)), 0);

            

            instantiated.transform.SetParent(windows);
            myWindows.Add(instantiated);
            instantiated.transform.position += new Vector3(0, 5*wallHeight, 0);
            SetWindowColour();
            // Set the new position on the X and Z axes
            //secondPlane.transform.position = new Vector3(newPosition.x, secondPlane.transform.position.y, newPosition.z);
        }
    }
    
    public void SetWindowColour()
    {
        if (myWindows != null)
        {
            foreach (GameObject window in myWindows)
            {
                Renderer temp = window.transform.GetChild(0).GetComponent<Renderer>();
                if (temp != null)
                {
                    //temp.material = wallMaterial;
                    temp.material.SetColor("_BaseColor", SaveSystem.GetWallColour());
                    //temp.material.shader = Shader.Find(SaveSystem.shaderName);
                }
            }
        }
    }*/
    
    public int GetX()
    {
        return xCoOrd;
    }

    public int GetY()
    {
        return yCoOrd;
    }

    public void SetXY(int x, int y)
    {
        //Debug.Log(x);  
        xCoOrd = x;
        yCoOrd = y;

        
        //MakeWall("testWall",1);
        //MakeWall("testWall",2);
        //MakeWall("testWall",3);
    }

    public void SetGridGen(GridGenerator x)
    {
        gridGen = x;
    }
 

    public void SetX(int x)
    {
        xCoOrd = x;
    }

    public void SetY(int y)
    {
        yCoOrd = y;
    }


}
