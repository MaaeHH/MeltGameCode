using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsScript : MonoBehaviour
{
    [SerializeField] private GridGenerator gridGen;
    [SerializeField] private GameObject plane;
    [SerializeField] private GameObject testObj1;
    [SerializeField] private GameObject testObj2;
    [SerializeField] private float BoundsHeight;
    [SerializeField] private GameObject meltObj;
    [SerializeField] private List<GameObject> walls;
    [SerializeField] private Transform cieling;
    [SerializeField] private Transform terrainTransform;
    [SerializeField] private Transform foundationTransform;
    private Vector3 corner1;
    private Vector3 corner2;
    private Bounds thisBounds;

    /*public GameObject floor; // Reference to the floor GameObject
    public GameObject[] walls; // Array of wall GameObjects (size should be 4)
    public float wallHeight = 3.0f;*/
    private void Start()
    {
        
        
    }

    public void SetSquarePosition(Vector3 closeCorner, Vector3 farCorner)
    {
       
        if (testObj1 != null){
            testObj1.transform.position = closeCorner;
        }

        if (testObj2 != null)
        {
            testObj2.transform.position = farCorner;
        }

        Vector3 bottomRightCorner = closeCorner;
        Vector3 topLeftCorner = farCorner;
        // Calculate the size of the square
        // Calculate the size of the square
        Vector3 size = topLeftCorner - bottomRightCorner;

        // Calculate the position of the plane's center
        Vector3 center = (topLeftCorner + bottomRightCorner) / 2f;

        // Move the existing plane to the center position
        plane.transform.position = center;

        // Adjust the plane's scale to fill the square
        plane.transform.localScale = new Vector3(size.x, 1f, size.z)/10;

        thisBounds = new Bounds(center, size);
        meltObj.transform.position = new Vector3(center.x,meltObj.transform.position.y,center.z);

        //MakeWalls(closeCorner, farCorner);
        MakeWalls();
        MakeWindows();
        SetCielingHeight(); 
        SetTerrainPos();
        SetFoundationPos();
    }

    public void SetCielingHeight()
    {
        cieling.localPosition = new Vector3(cieling.localPosition.x, wallHeight*10, cieling.localPosition.z);
    }

    public void SetTerrainPos()
    {
        terrainTransform.position = new Vector3(plane.transform.position.x, terrainTransform.position.y, plane.transform.position.z);
    }

    public void SetFoundationPos()
    {
        foundationTransform.position = new Vector3(plane.transform.position.x, foundationTransform.position.y, plane.transform.position.z);
        foundationTransform.localScale = new Vector3(plane.transform.localScale.x*10 + 2f, foundationTransform.localScale.y,2f+ 10*plane.transform.localScale.z);
    }

    public void MakeWalls()
    {
        int index = 0;
        foreach(GameObject wall in walls)
        {
            MakeWall(wall, index);
            index++;
        }
        ColourWalls();
    }
    float wallHeight = 10f;
    float wallThickness = 0.3f;
    public void MakeWindows()
    {
        int index = 0;
        
        foreach (GameObject wall in walls)
        {
            foreach(WindowInstance winData in SaveSystem.GetWindows(index))
            {
                //string path = "Windows/" + winData.GetData().objName;
                //GameObject windowPrefab = Resources.Load<GameObject>(path);
                GameObject instantiated = GameObject.Instantiate(winData.GetData().winObj, new Vector3(0,0,0), new Quaternion());
                instantiated.SetActive(true);
                instantiated.transform.localScale = new Vector3(instantiated.transform.localScale.x, wallThickness, instantiated.transform.localScale.z);
                instantiated.transform.eulerAngles = new Vector3(90, 90 + (90 * -(3 - index)), 0);
                instantiated.transform.SetParent(wall.transform.GetChild(0));
                //instantiated.transform.eulerAngles = new Vector3(90,0, 0);
                instantiated.transform.localPosition = new Vector3((0.5f-winData.xPos), -(0.5f-winData.height), 0);
                //instantiated.transform.localScale = new Vector3(transform.localScale.x*1f, transform.localScale.y, transform.localScale.z * 1f);
                instantiated.transform.localScale = new Vector3(instantiated.transform.localScale.x, 0.5f, instantiated.transform.localScale.z);

            }       
            index++;
        }
        
    }

    


    public void MakeWall(GameObject theWall, int edgeToFit)
    {
        theWall.SetActive(true);
        


         
            Vector3 newPosition = plane.transform.GetComponent<Renderer>().bounds.center;

        if (edgeToFit == 0 || edgeToFit == 2)
        {
            theWall.transform.localScale = new Vector3(plane.transform.localScale.x * 10, wallHeight, wallThickness);
        }
        else
        {
            theWall.transform.localScale = new Vector3(plane.transform.localScale.z * 10 + wallThickness*2, wallHeight, wallThickness);
        }
            
            theWall.transform.eulerAngles = new Vector3(0, 90 + (90 * -(3 - edgeToFit)), 0);

        switch (edgeToFit)
            {
                case 0:
                    newPosition.z += (wallThickness/2) + 5f * plane.transform.localScale.z; //+ secondRenderer.bounds.extents.z;
                    break;
                case 1:
                    newPosition.x += (wallThickness / 2) + 5f * plane.transform.localScale.x; //+ secondRenderer.bounds.extents.x;
                    break;
                case 2:
                    newPosition.z -= (wallThickness / 2) + 5f * plane.transform.localScale.z; //+ secondRenderer.bounds.extents.z;
                    break;
                case 3:
                    newPosition.x -= (wallThickness / 2) + 5f * plane.transform.localScale.x; //+ secondRenderer.bounds.extents.x;
                    break;
                default:
                    Debug.LogError("Invalid edgeToAlign value. Use 0 for top, 1 for right, 2 for bottom, or 3 for left.");
                    return;
            }
            theWall.transform.position = newPosition;


            theWall.transform.position += new Vector3(0, wallHeight/2, 0);
         
  
    }


    public void ColourWalls()
    {
        foreach (GameObject wall in walls)
        {
            Renderer temp = wall.transform.GetChild(0).GetComponent<Renderer>();
            if (temp != null)
            {
                //Debug.Log("NON NULL");
                //temp.material = wallMaterial;
                //temp.material.SetColor("_BaseColor", SaveSystem.GetWallColour());
                //temp.material.shader = Shader.Find(SaveSystem.shaderName);
            }
        }
    }

    public Bounds GetBounds()
    {
        if (thisBounds == null) { Debug.Log("Warning, null bounds!"); }

        return thisBounds;
        
    }
}
