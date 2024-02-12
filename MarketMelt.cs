using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class MarketMelt : MonoBehaviour
{
    [SerializeField] private MeltData meltData;
    [SerializeField] private ParticleController ps;
    [SerializeField] private MarketFlowBound mfb;
    //[SerializeField] private Screamscript screamScript;
    private int combo = 0;
    private bool next = true;
    private float minTime = 0.5f;
    private float maxTime = 3f;


    private MeltAppearance appearanceScript;
    [SerializeField] private Animator animator;
    private Bounds actorBounds;
    bool relocating = false;
    //primarily random movement associated variables
    private Vector3 targetLocation;
    private Vector3 toLookAtLocation;
    private Vector3 vector;
    float timer;
    float randomHigh;
    float randomLow;
    private float distance;
    private Vector3 startingPos;
    private float movementRadius;
    private RaycastHit hit;
    private RaycastHit ahit;
    private bool rcDone;
    [SerializeField] private Screamscript screms;
    [SerializeField] private GameObject theMelt;
    [SerializeField] private Animator umbrella;
    public bool talking = false;
    private bool moving = false;
    private int movesUntilMarket;
    [SerializeField] private RainTimekeeper rtk;
    private FoodScript foodTarget;
    private MeltScript linkedMelt;
    private MeltScript firstLink;
    //private Vector3 targetLocation;
    [SerializeField] private ClickableGridObject targetGridObj;
    [SerializeField] private Renderer meltRenderer;

    private bool talkDelayRunning;
    //[SerializeField] private ModeController _mc;
    //private GameObject avatarGameObj;

    void Start()
    {
        
        if (meltData != null)
        {
            meltData.OnLoadComplete += InitSaveComplete;
            
            meltData.InitSaveData();
        }
        
        //animator = transform.GetChild(0).GetComponent<Animator>();
        //RefreshFace();
        RefreshVariables();
        RelocateTarget();

        //rtk.OnRainChanged += SetRainStuff;
        //actorBounds = boundsS.GetBounds();
    }
    public void RefreshVariables()
    {
        if(meltData != null)
        {
            movementRadius = 4 + 2 * meltData.GetCheer() / 10.0f;
            randomHigh = meltData.GetRestlessness() + meltData.GetRestlessnessConsistency();
            randomLow = meltData.GetRestlessness();
            timer = UnityEngine.Random.Range(randomLow, randomHigh);
            if (randomLow < 0) { randomLow = 0; }
        }
        
    }
    public void InitSaveComplete()
    {
        //Debug.Log("Colour r" + meltData.GetColour().r + " g" + meltData.GetColour().g + " b" + meltData.GetColour().b + " a" + meltData.GetColour().a);

        appearanceScript.RefreshAppearance();
        //SetColour(meltData.GetColour());


        //meltRenderer.material.SetColor("_Color", currentColour);
        Debug.Log("SAVE ASDASDASD");
    }


    void Awake()
    {
        umbrella.SetBool("Open", rtk.GetIsRaining());
        rtk.OnRainChanged += SetRainStuff;
    }

    private void OnDestroy()
    {
        rtk.OnRainChanged -= SetRainStuff;
        meltData.OnLoadComplete -= InitSaveComplete;
    }

    private void SetRainStuff(bool x)
    {
        Debug.Log("open");
        umbrella.SetBool("Open", x);
    }

    /*public void SetColour(Color x)
    {
        Debug.Log("Colour r" + x.r + " g" + x.g + " b" + x.b + " a" + x.a);
        //currentColour = x;
        meltRenderer.material.SetColor("_Color", x);

    }*/




    public bool GetMoving()
    {
        return moving;
    }

    public void Update()
    {
        if (meltData != null)
        {
            if (animator != null)
            {
                animator.SetBool("isWalking", moving);
                animator.SetBool("isTalking", talking);
                animator.SetFloat("cheer", meltData.GetCheer());
            }

            MoveToTargetLocation();
        }
        //Debug.Log(meltData.GetHunger());
       
    }

    private void MoveToTargetLocation()
    {
        //targetLocation = targetGridObj.transform.position;
        if (Vector3.Distance(transform.position, targetLocation) > 0.2)
        {
            startingPos = transform.position;
            float step = meltData.GetSpeed() * 1.4f * Time.deltaTime;
            Vector3 aaa = Vector3.MoveTowards(transform.position, targetLocation, step);

            Vector3 castFrom = transform.position;
            castFrom.y = castFrom.y + 1;

            moving = (Vector3.Distance(startingPos, aaa) < step);

            transform.position = aaa;

            moving = true;
        }
        else
        {
            moving = false;
            if (!relocating){
                float randomTime = UnityEngine.Random.Range(minTime, maxTime);

                if(mfb.GetFocalPoint() != null)
                {
                    randomTime = randomTime + 10.0f;
                }

                Invoke("RelocateTarget", randomTime);
                relocating = true;
            }
            
        }

        if (toLookAtLocation != null)
        {

            Vector3 temp = toLookAtLocation;
            var dir = toLookAtLocation - transform.position;
            dir.y = 0; // kill height differences
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
        }
    }




    private void RelocateTarget()
    {
        movesUntilMarket--;
        relocating = false;
        int chance;
        bool done = false;
        if (mfb.GetStoreFront() != null)
        {
            if(movesUntilMarket <= 0)
            {
                chance = 75;
                if (UnityEngine.Random.Range(0, 100) < chance)
                {
                    mfb = mfb.GetStoreFront();
                    done = true;
                    movesUntilMarket = 5;
                }
            }
            
        }
        if (!done) {
            chance = 50 + (5 - combo) * 10;
            if (UnityEngine.Random.Range(0, 100) < chance)
            {
                if (next)
                {

                    mfb = mfb.GetNext();
                }
                else
                {

                    mfb = mfb.GetPrevious();
                }
                if (combo < 4)
                {
                    combo++;
                }
            }
            else
            {
                if (next)
                {
                    mfb = mfb.GetPrevious();
                }
                else
                {
                    mfb = mfb.GetNext();
                }
                combo = 0;
            }
        }
        
        Vector3 point = GetRandomPointWithinBounds(mfb.GetBounds());
        targetLocation = point;
        if(mfb.GetFocalPoint() != null)
        {
            toLookAtLocation = mfb.GetFocalPoint().position;
        }
        else
        {
            toLookAtLocation = point;
        }
        
    }

    private Vector3 GetRandomPointWithinBounds(Bounds bounds)
    {
        Vector3 randomPoint = new Vector3(
            UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
            UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
            UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
        );

        return randomPoint;
    }


    public void SetMeltData(MeltData data)
    {
        meltData = data;
        appearanceScript = GetComponent<MeltAppearance>();
        appearanceScript.SetMeltData(data);
        screms.SetMeltData(data);
        //meltData.InitSaveData();
    }

    public bool IsNamed(string name)
    {
        return name.Equals(meltData.GetName());
    }
    public MeltData GetMeltData()
    {
        return meltData;
    }
    public void SetMarketFlowBound(MarketFlowBound x)
    {
        mfb = x;
    }
}
