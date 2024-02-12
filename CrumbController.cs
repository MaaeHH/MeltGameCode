using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumbController : MonoBehaviour
{

    [SerializeField] private BoundsScript boundsS;
    [SerializeField] private GameObject _crumbPrefab;
    private RaycastHit hit;
    Vector3 targetLocation;
    private List<GameObject> crumbs;
    void Start()
    {
        crumbs = new List<GameObject>();
       
    }
    public void MakeCrumbAt(Vector3 pos)
    {
        Vector3 vector = UnityEngine.Random.insideUnitCircle * 1.5f;
        vector = Quaternion.AngleAxis(-90, Vector3.right) * vector;
        Vector3 aaa = pos + vector;



        Vector3 castFrom = transform.position;
        castFrom.y = castFrom.y + 1;

        if ((Physics.Raycast(castFrom, -Vector3.up, out hit, 10f)))
        {
            aaa = new Vector3(aaa.x, transform.position.y - (hit.distance - 1), aaa.z);
        }


        aaa = boundsS.GetBounds().ClosestPoint(aaa);
        targetLocation = aaa;

        float randomAngle = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0f, randomAngle, 0f);

        GameObject spawnedCrumb = Instantiate(_crumbPrefab, targetLocation, randomRotation);
        //spawnedCrumb.gameObject.transform.position = targetLocation;
        spawnedCrumb.transform.SetParent(this.gameObject.transform);
        spawnedCrumb.SetActive(true);
        spawnedCrumb.GetComponent<CrumbScript>().SetCC(this);
        crumbs.Add(spawnedCrumb);
    }
    public int GetNoOfCrumbs()
    {
        return crumbs.Count;
    }

    public void DeleteCrumb(GameObject toDel)
    {
        crumbs.Remove(toDel);
        Object.Destroy(toDel);
    }
}
