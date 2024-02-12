using System.Collections.Generic;
using UnityEngine;
using System.Threading.Tasks;

public class MeltInterractionInstance
{
    private List<MeltScript> melts;
    private MeltInterractionData interractionData;

    private bool done = false;

    /* public MeltInterractionInstance(MeltInterractionData x)
     {
         melts = new List<MeltScript>();
         interractionData = x;
         done = false;
         SetTimer();
     }*/

    GameObject centerGO;
    InterractionCenter centerGOI;

    public MeltInterractionInstance(MeltInterractionData x, MeltScript meltOne, MeltScript meltTwo)
    {
        melts = new List<MeltScript>();
        melts.Add(meltOne);
        melts.Add(meltTwo);
        
        interractionData = x;
        CalculateAvgLocation();
        CalculateCirclePoints();
        CalculatePositions();
        
        done = false;

        meltOne.SetInterraction(this);
        meltTwo.SetInterraction(this);

        SetTimer();

        if(x.centerPiece != null)
        {
            centerGO = Object.Instantiate(x.centerPiece, GetAverageLocation(), new Quaternion());
            centerGOI = centerGO.GetComponent<InterractionCenter>();
            centerGOI.StartAnim();
        }
    }


    public bool AddMelt(MeltScript x)
    {
        Debug.Log("addingMelt");
        if(melts.Count < interractionData.maxMelts)
        {
            melts.Add(x);
            x.SetInterraction(this);
            return true;
        }
        CalculateCirclePoints();
        CalculatePositions();
        return false;
    }

    public void ReleaseMelts()
    {
        foreach(MeltScript melt in melts)
        {
            melt.StopInterraction();
            melt.GetMeltData().IncreaseCheer(interractionData.cheerBonus);
            melt.GetMeltData().IncreaseHealth(interractionData.healthBonus);
            melt.GetMeltData().IncreaseEnergy(interractionData.energyBonus);
            melt.GetMeltData().IncreaseHunger(interractionData.hungerBonus);
        }
        if(centerGO != null)
        {
            centerGO = null;
            centerGOI.EndAnim();
            centerGOI = null;
        }
       
        done = true;
    }

    public Vector3 GetAverageLocation()
    {
        return avgLocation;
    }

    private Vector3 avgLocation;

    private void CalculateAvgLocation()
    {
        if (melts.Count < 2)
        {
            ReleaseMelts();
            //return new Vector3();
        }

        Vector3 avg = melts[0].transform.position;

        List<MeltScript> body = new List<MeltScript>(melts);
        body.Remove(body[0]);
        foreach (MeltScript melt in body)
        {
            avg = avg + melt.transform.position;
        }
        avg = avg / melts.Count;
        avgLocation = avg;
    }
    List<MeltScript> orderedMelts = new List<MeltScript>();
    List<Vector3> orderedPoints = new List<Vector3>();

    public void CalculatePositions()
    {
        orderedMelts = new List<MeltScript>();
        orderedPoints = new List<Vector3>();

        List<MeltScript> theMelts = new List<MeltScript>(melts);

        foreach (Vector3 point in circlePoints)
        {
            float shortestDistance = 0;
            MeltScript shortestMS = null;
            if (theMelts.Count > 0) {
                shortestDistance = Vector3.Distance(theMelts[0].transform.position, point);
                shortestMS = theMelts[0];
            }
            foreach (MeltScript melt in theMelts)
            {
                float currentDist = Vector3.Distance(theMelts[0].transform.position, point);
                if (currentDist < shortestDistance)
                {
                    currentDist = shortestDistance;
                    shortestMS = melt;
                }
            }

            orderedMelts.Add(shortestMS);
            orderedPoints.Add(point);

            theMelts.Remove(shortestMS);
            
        }
    }


    public Vector3 GetPosition(MeltScript x)
    {
        //Debug.Log(orderedMelts.Count);
        //Debug.Log(orderedPoints.Count);
        if(orderedPoints != null)
        return orderedPoints[orderedMelts.IndexOf(x)];

        return new Vector3();
    }

    Vector3[] circlePoints;
    private void CalculateCirclePoints()
    {
        Vector3 center = GetAverageLocation();
        int numPoints = melts.Count;
        float radius = interractionData.radius;
        Vector3 referencePoint = melts[0].transform.position;
        Vector3[] points = new Vector3[numPoints];
        float minDistance = float.MaxValue;

        // Calculate the closest point on the circle to the reference point.
        Vector3 closestPoint = Vector3.zero;

        for (int i = 0; i < numPoints; i++)
        {
            float angle = 360f / numPoints * i;
            float radians = angle * Mathf.Deg2Rad;

            float x = center.x + radius * Mathf.Cos(radians);
            float y = center.y;
            float z = center.z + radius * Mathf.Sin(radians);

            Vector3 currentPoint = new Vector3(x, y, z);

            // Calculate the distance to the reference point and check if it's the closest so far.
            float distance = Vector3.Distance(referencePoint, currentPoint);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPoint = currentPoint;
            }

            points[i] = currentPoint;
        }

        // Calculate the angle from the reference point to the closest point.
        Vector3 direction = closestPoint - center;
        float angleToClosestPoint = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;

        // Rotate the points array so that the closest point is the first element.
        for (int i = 0; i < numPoints; i++)
        {
            float rotatedAngle = angleToClosestPoint + (360f / numPoints) * i;
            float rotatedRadians = rotatedAngle * Mathf.Deg2Rad;
            float x = center.x + radius * Mathf.Cos(rotatedRadians);
            float z = center.z + radius * Mathf.Sin(rotatedRadians);

            points[i] = new Vector3(x, center.y, z);
        }

       
        circlePoints = points;
        //return points;
    }

    public void RemoveMelt(MeltScript x)
    {
        melts.Remove(x);
        if (melts.Count <2)
        {
            ReleaseMelts();
        }
    }

    private void SetTimer()
    {
        Timer();
        
    }

    private async void Timer()
    {
        float randomDelay = Random.Range(interractionData.minTime, interractionData.maxTime);
        Debug.Log("starting timer (" + randomDelay + ")");
        await Task.Delay((int)(randomDelay * 1000));

        ReleaseMelts();
    }

    public bool Contains(MeltScript x)
    {
        return melts.Contains(x);
    }

    public bool IsDone()
    {
        return done;
    }

    public MeltInterractionData GetData()
    {
        return interractionData;
    }
}
