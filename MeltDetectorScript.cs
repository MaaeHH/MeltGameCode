using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeltDetectorScript : MonoBehaviour
{
    [SerializeField] private float radius = 10;
    [SerializeField] private float angle = 30;
    private MeltScript parentMS;
    public LayerMask targetMask;
    public LayerMask obstructionMask;

    

    //[SerializeField] private Color meshColor = Color.red;
    [SerializeField] private MeltScript ms;
    Mesh mesh;
    // Start is called before the first frame update
    void Start()
    {
        parentMS = gameObject.transform.parent.gameObject.GetComponent<MeltScript>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        float delay = 0.4f;
        WaitForSeconds wait = new WaitForSeconds(delay);
        while (true)
        {
            //Debug.Log("aasd");
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck() {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius);
        //Debug.Log(rangeChecks.Length);
        if (rangeChecks.Length != 0)
        {
            //Debug.Log("a");
            foreach(Collider coll in rangeChecks)
            {
                //Transform target = coll.transform;
                Vector3 directionToTarget = (coll.transform.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward,directionToTarget) < angle / 2){
                    //Debug.Log("b");
                    float distanceToTarget = Vector3.Distance(transform.position, coll.transform.position);
                    if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    {
                        //Debug.Log("c");
                        
                        if (coll.gameObject.GetComponent<FoodScript>() != null)
                        {
                            //Debug.Log("d");
                            ms.SetFoodTarget(coll.GetComponent<FoodScript>());
                            //ms.SetMode(1);
                        }
                        if (coll.gameObject.GetComponent<MeltScript>() != null && coll.gameObject.GetComponent<MeltScript>() != parentMS)
                        {
                            ms.StartInterraction(coll.GetComponent<MeltScript>());
                            //ms.SetTalkingTarget(coll.GetComponent<MeltScript>());
                        }
                    }
                }
            }
            Transform target = rangeChecks[0].transform;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    /*private void OnTriggerEnter(Collider other)
    {
        FoodScript food = other.gameObject.GetComponent<FoodScript>();
        if (food != null)
        {
            ms.SetFoodTarget(food);
            //ms.SetMode(1);
            // Object has the specific script attached
            //Debug.Log("Object has the script attached!");
        }

       
    }*/

    /*Mesh CreateWedgeMesh()
    {
        mesh = new Mesh();
        int segments = 10;
       
        int numTriangles = (segments*4) + 4;
        int numVertices = numTriangles * 3;

        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;
        int vert = 0;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;
        for(int i = 0; i < segments; ++i)
        {
           
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle+ deltaAngle, 0) * Vector3.forward * distance;

            topRight = bottomRight + Vector3.up * height;
            topLeft = bottomLeft + Vector3.up * height;
            
           

            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomLeft;
            vertices[vert++] = topLeft;

            vertices[vert++] = topLeft;
            vertices[vert++] = topCenter;
            vertices[vert++] = bottomCenter;

            vertices[vert++] = bottomCenter;
            vertices[vert++] = topCenter;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomCenter;

            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;

            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;

            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;

            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
            currentAngle += deltaAngle;
        }

       

        for(int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;
    }
    private void OnValidate()
    {
        mesh = CreateWedgeMesh();
    }

    private void OnDrawGizmos()
    {
        if (mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(mesh, transform.position, transform.rotation);
        }
    }*/
}
