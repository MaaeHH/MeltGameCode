using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;
public class MeltScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    [SerializeField] private MeltData meltData;
    [SerializeField] private ParticleController ps;
    [SerializeField] private BoundsScript boundsS;
    [SerializeField] private MeltDataMenu mdm;
    [SerializeField] private SelectedController sc;
    [SerializeField] private Animator animator;
    [SerializeField] private Screamscript screamScript;
    private Bounds actorBounds;
    [SerializeField] private MeltSpawner ms;
    //primarily random movement associated variables
    private Vector3 targetLocation;
    private Vector3 vector;
    float timerCount = 0;
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
    //[SerializeField] private GameObject theMelt;
    //[SerializeField] private GameObject meltScaler;
    public bool talking = false;
    private bool moving = false;
    [SerializeField] private int meltMode = 0;
    //[SerializeField] private Renderer meltRenderer;
    private MeltAppearance appearanceScript;
    private FoodScript foodTarget;

    //private Vector3 gridObjLocation;
    [SerializeField] private ClickableGridObject targetGridObj;
    [SerializeField] private Color lastSetColour;
    //[SerializeField] private Color currentColour;

    [SerializeField] private bool started = false;
    //[SerializeField] private Material templateMaterial;
    [SerializeField] private Material temp;
    [SerializeField] private MeltTimeController timeCont;
    Material newMaterial;

    [SerializeField] private AudioSource levelUpAudio;
    [SerializeField] private ParticleSystem levelUpParticle;

    [SerializeField] private MeltIndicator meltInd;
    [SerializeField] private AnimEventScript aes;

    [SerializeField] private MeltAccessoriesController mac;
    private Camera mainCamera;
    private bool friend = false;
    void Awake()
    {
        if (meltData != null)
        {
            meltData.OnLoadComplete += InitSaveComplete;
            meltData.OnLevelUp += LevelUp;
            meltData.OnMakePointers += ShowIndicator;
            meltData.OnUnfriend += NoLongerFriends;
            meltData.OnMakeDialogue += MakeDialogue;
            SaveSystem.OnIncreasedCount += GlobalCountIncreased;
        }
        //gameObject.SetActive(true);
        StartMelt();


        //SetColour(Color.green);
    }

    public void StartMelt()
    {
        //gameObject.SetActive(true);

        if (started == false)
        {
            mainCamera = Camera.main;
            if (meltData != null)
            {
                
                //Debug.Log("NON NULL MELT DATA");
                meltData.SetMeltScript(this);
               
                //meltData.InitFriendshipReqs();
                meltData.InitSaveData();

                friend = meltData.IsFriend();
            }
            else
            {
                gameObject.SetActive(false);
            }
           


            actorBounds = boundsS.GetBounds();
            //animator = transform.GetChild(0).GetComponent<Animator>();

            
        }
    }

    void OnDestroy()
    {
        meltData.OnLoadComplete -= InitSaveComplete;
        meltData.OnLevelUp -= LevelUp;
        meltData.OnMakePointers -= ShowIndicator;
        SaveSystem.OnIncreasedCount -= GlobalCountIncreased;
        meltData.OnUnfriend -= NoLongerFriends;
        meltData.OnMakeDialogue -= MakeDialogue;
    }
    private bool likedLastObject = false;
    public void GlobalCountIncreased(InventoryObjectData x)
    {
        bool nowFriend = meltData.IsFriend();
        if (nowFriend != friend && nowFriend)
        {
            nowFriend = friend;
            //We've just become friends...
            JustBecameFriends(meltData.GetHasBeenFriends());// If we were previously friends
            likedLastObject = meltData.LikesObject(x);
            meltData.BefriendingComplete();
        }
    }

    public void TalkToPlayer()
    {
        db.MakeDialogue(meltData.GetDialogue(likedLastObject), this);
        likedLastObject = false;
    }

    [SerializeField] private DialogueBox db;
    public void JustBecameFriends(bool previouslyFriends)
    {
        Debug.Log("Just became friends");
        GetMeltData().SetRestoreLastUpdate(true);
        if (previouslyFriends)
        {
            db.MakeDialogue(meltData.GetDialogueOnBefriendAgain(), this);
        }
        else
        {
            db.MakeDialogue(meltData.GetDialogueOnBefriend(), this);
        }
    }

    public void NoLongerFriends()
    {
        GetMeltData().SetRestoreLastUpdate(false);
        db.MakeDialogue(meltData.GetDialogueOnUnfriend(), this);
    }

    public void LevelUp()
    {
        levelUpAudio.Play();
        levelUpParticle.Emit(30);
    }

    public void SaveNewColour(Color x)
    {
        meltData.SetColour(x);
        meltData.SaveData();
        appearanceScript.RefreshAppearance();
    }

    public void SetInvisible(bool invis)
    {
        appearanceScript.SetInvisible(invis);
    }

    public void SaveNewScale(Vector3 x)
    {
        meltData.SetScale(x);
        meltData.SaveData();
        appearanceScript.RefreshAppearance();
    }

    public void RefreshAppearance()
    {
        appearanceScript.RefreshAppearance();
    }

    public void MakeDialogue(List<string> x)
    {
        db.MakeDialogue(x, this);
    }

    public void InitSaveComplete()
    {
        //Debug.Log("init asd");
        RefreshVariables();
        RelocateTargetLocation();
        
        //SetScale(meltData.GetScale());
        //SetAColour(new Color(meltData.GetColour().r, meltData.GetColour().g, meltData.GetColour().b, meltData.GetColour().a));
        if (meltData.GetRestoreLastUpdate())
        {
            timeCont.RestoreLastMeltUpdate(meltData);
        }
        else
        {
            
            meltData.ResetLastUpdate();
        }
        //Debug.Log(meltData.GetName() + " READY");

        appearanceScript.RefreshAppearance();
        mac.SetAccessories(meltData.GetAccessories());
        started = true;
      
    
    }

   

    public Vector3 GetGridObjLocation()
    {
        return targetGridObj.transform.position;
        //return gridObjLocation;
    }
    public ClickableGridObject GetGridObj()
    {
        return targetGridObj;
        //return gridObjLocation;
    }
    public bool SetGridObj(ClickableGridObject x)
    {
        targetGridObj = x;
        return SetMode(3);
    }

    public void RefreshVariables()
    {
        if(meltData != null)
        {
            //Debug.Log("Refreshing");
            movementRadius = 4 + 2 * meltData.GetCheer() / 10.0f;
            randomHigh = meltData.GetRestlessness() + meltData.GetRestlessnessConsistency();
            randomLow = meltData.GetRestlessness();
            timer = UnityEngine.Random.Range(randomLow, randomHigh);
            if (randomLow < 0) { randomLow = 0; }
        }
        
    }

    public bool GetMoving()
    {
        return moving;
    }

   

    public void Update()
    {
        //meltRenderer.material = temp;
        //Debug.Log("We updating");
        if (started)
        {
            //Debug.Log("We started");
            //SetColour(meltData.GetColour());
            //RefreshColour();
            if (meltData != null)
            {
                //Debug.Log("We got the data");
                if (animator != null)
                {
                    animator.SetBool("isWalking", moving);
                    bool interacting = currentInteraction != null;
                    animator.SetBool("Interracting", interacting);
                    if (interacting) animator.SetInteger("InterractAnimation", currentInteraction.GetData().animationID);
                    animator.SetFloat("cheer", meltData.GetCheer());

                }

                switch (meltMode)
                {
                    case 0://random movement
                        SetInvisible(false);
                        MoveTowards();
                        RotateTowards();

                        distance = Vector3.Distance(transform.position, targetLocation);
                        //animator.SetBool("Moving", moving);

                        if (timerCount >= timer)
                        {

                            RelocateTargetLocation();
                            RefreshVariables();
                            timerCount = 0;
                            timer = UnityEngine.Random.Range(randomLow, randomHigh);
                        }
                        else
                        {
                            timerCount = timerCount + Time.deltaTime;
                        }
                        break;
                    case 1://eating
                        if (foodTarget != null)
                        {
                            MoveToFoodTarget();
                        }
                        else
                        {
                            SetMode(0);
                        }
                        break;
                    case 2://Talking
                        InterractScript();
                        break;
                    case 3://Moving to grid object
                        MoveToGridObject();
                        break;
                    case 4://Being dragged
                        moving = false;
                        talking = false;
                        break;
                    case 5://Talking to camera
                        moving = false;
                        talking = true;
                        LookAtCamera();

                        break;
                    default:
                        break;
                }
            }
        }
        //Debug.Log(meltData.GetHunger());
    }

    private void LookAtCamera()
    {
        if (mainCamera != null)
        {
            // Calculate the direction from the GameObject to the camera
            Vector3 lookAtDirection = mainCamera.transform.position - transform.position;

            // Calculate the target rotation
            Quaternion targetRotation = Quaternion.LookRotation(lookAtDirection);

            // Gradually rotate towards the target rotation
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, meltData.GetRotationSpeed() * Time.deltaTime);
        }
    }

    private void MoveToGridObject()
    {
        Vector3 gridObjLocation = targetGridObj.transform.position;
        if (Vector3.Distance(transform.position, gridObjLocation) > 0.2)
        {
            startingPos = transform.position;
            float step = meltData.GetSpeed() * 1.4f * Time.deltaTime;
            Vector3 aaa = Vector3.MoveTowards(transform.position, gridObjLocation, step);

            Vector3 castFrom = transform.position;
            castFrom.y = castFrom.y + 1;

            moving = (Vector3.Distance(startingPos, aaa) < step);

            transform.position = aaa;

            moving = true;
        }
        else
        {
            moving = false;
            
        }

        if (targetGridObj != null)
        {

            Vector3 temp = gridObjLocation;
            var dir = gridObjLocation - transform.position;
            dir.y = 0; // kill height differences
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
        }
    }


    public bool SetMode(int x)
    {
        //Debug.Log("Attempting to set mode to " + x);
        bool successful = false;

        if (x == 0 || x == 4)//Reverting to state 0 is always successful, but we must revert all associated variables
        {
            //linkedMelt = null;
            foodTarget = null;
            //firstLink = null;
            targetGridObj = null;
            successful = true;
            //gridObjLocation = null;
        }
        else if (x == meltMode)//if attempting to set mode to the current mode, does nothing but doesn't revert associated variables
        {
            return false;
        }else if(meltMode == 0)//It is always a success if the melt is in state 0
        {
            
            successful = true;
        }
        else if (meltMode == 2)
        {
            MeltInterractionController.RemoveFromInterractions(this);
        }
        else if(meltMode == 5)
        {
            
            //linkedMelt = null;
            foodTarget = null;
            //firstLink = null;
            targetGridObj = null;
            successful = true;
            
        }
        if (successful){// If we are successful, switch the mode
            meltMode = x;
            //Debug.Log("Success");
        }
        else
        {
            switch (x)//If it was not successful, revert the associated variable
            {
                case 0:
                    break;
                case 1:
                    foodTarget = null;
                    break;
                case 2:
                    
                    break;
                case 3:
                    targetGridObj = null;
                    break;
                default:
                    break;
            }
            //Debug.Log("Fail");
        }
        return successful;
    }
    
    public int GetMode()
    {
        return meltMode;
    }

    public void SetFoodTarget(FoodScript f)
    {
        if (foodTarget == null)
        {
            foodTarget = f;
            SetMode(1);
        }
        
    }

    MeltInterractionInstance currentInteraction = null;
    public void SetInterraction(MeltInterractionInstance x)
    {
        currentInteraction = x;
        SetMode(2);
    }

    public void StopInterraction()
    {
        SetMode(0);
        currentInteraction = null;
        StartCoroutine(InterractCooldown());
    }

    private float interractChance = 1.0f;
    public void StartInterraction(MeltScript x)
    {
        if (canInteract && canTryInteract && currentInteraction == null)
        {
            if (UnityEngine.Random.Range(0f, 1f)<= interractChance)
            {
                MeltInterractionController.StartNewInterraction(this, x, SelectInterractionData());
                //StartCoroutine(InterractAttemptCooldown());
            }
            else
            {
                StartCoroutine(InterractAttemptCooldown());
            }
        }
        
    }

    private MeltInterractionData SelectInterractionData()
    {
        float totalchance = 0f;
        foreach(InterractionListing ilist in meltData.GetInterractionListings())
        {
            totalchance += ilist.chance;
        }
        float selectedFloat = UnityEngine.Random.Range(0f, totalchance);
        foreach (InterractionListing ilist in meltData.GetInterractionListings())
        {
            selectedFloat -= ilist.chance;

            if (selectedFloat <= 0) return ilist.data;
        }
        return null;
    }

    [SerializeField] bool canInteract = true;
    private IEnumerator InterractCooldown()
    {
        canInteract = false;
        float randomTime = UnityEngine.Random.Range(25f, 30f);
        yield return new WaitForSeconds(randomTime);
        canInteract = true;
    }

    [SerializeField] bool canTryInteract = true;
    private IEnumerator InterractAttemptCooldown()
    {
        canTryInteract = false;
        float randomTime = UnityEngine.Random.Range(15f, 20f);
        yield return new WaitForSeconds(randomTime);
        canTryInteract = true;
    }
    /*public MeltScript GetLinkedMelt()
    {
        return linkedMelt;
    }
    public bool GetIsTalking()
    {
        return meltMode == 2;
    }
    public MeltScript GetLastLink()
    {
        if(linkedMelt != null)
        {
            return linkedMelt.GetLastLink();
        }
        else
        {
            return this;
        }
    }

    public void SetLinkedMelt(MeltScript x)
    {
        linkedMelt = x;
    }
    public void SetFirstLink(MeltScript x)
    {
        firstLink = x;
    }
    public MeltScript GetFirstLink()
    {
        return firstLink;
    }

    public int GetLinkSize()
    {
        return GetFirstLink().SizeLoop();
    }

    public int SizeLoop()
    {
        if (linkedMelt != null)
        {
            return linkedMelt.SizeLoop() + 1;
        }
        else
        {
            return 1;
        }
    }*/

    private void GiveCheer(float cheerAmount)
    {
        meltData.IncreaseCheer(cheerAmount);
        meltData.SaveData();
    }

    /*public MeltScript LinksLoop()
    {
        MeltScript temp = linkedMelt;
        SetMode(0);
        GiveCheer(0.5f);
        if (temp == null)
        { 
            return this;
        }
        else
        {
            return temp.LinksLoop();
            
        }
    }

    public Vector3 GetAverageLocation()
    {
        if(GetFirstLink() == null) {
            return gameObject.transform.position;
        }
        else
        {
            
            return GetFirstLink().LocationLoop() / GetLinkSize() * 1.0f;
        }
        
    }

    public Vector3 LocationLoop()
    {
        if (linkedMelt == null)
        {
            return (gameObject.transform.position);
            
        }
        else
        {
            return gameObject.transform.position + linkedMelt.LocationLoop();
        }
        //return gameObject.transform.position;
    }*/

    public void ShowIndicator()
    {
        meltInd.ShowIndicator();
    }

    /*public void SetTalkingTarget(MeltScript ms)
    {
        if (canTalkAgain)//if we can talk
        {
            if (SetMode(2))//and we're not doing anything else,
            {
                bool successful = false;
                if (ms.GetIsTalking())//First check if the target melt is talking already or not
                {
                    //if the melt is talking,
                    ms.GetLastLink().SetLinkedMelt(this);
                    SetFirstLink(ms.GetFirstLink());
                    //Set linked melt to this
                    successful = true;
                }
                else
                // if the melt is not talking, 
                {
                    if (ms.GetCanTalkAgain())//If the other melt can talk again
                    {
                        if (ms.SetMode(2))//If we successfully set their mode to 2
                        {
                            SetFirstLink(this);
                            ms.SetFirstLink(this); //make sure we're the first link for both
                            SetLinkedMelt(ms); //make sure we link to the next melt
                            StartCoolDown(); 
                            ms.StartCoolDown(); //start cooldown on both melts
                           
                            StartCoroutine(TalkDuration());//start the talk timer                   

                            successful = true;
                        }
                    }
                }
                if (!successful)
                {
                    SetMode(0);
                }
            }
        }
        
    }
    public bool GetCanTalkAgain()
    {
        return canTalkAgain;
    }

    public void StartCoolDown()
    {
        //SetFirstLink(ms.GetFirstLink());
        StartCoroutine(TalkCooldown());
    }

    private IEnumerator TalkCooldown()
    {
        canTalkAgain = false;
        float randomTime = UnityEngine.Random.Range(25f, 30f);
        yield return new WaitForSeconds(randomTime);
        canTalkAgain = true;
    }

    private IEnumerator TalkDuration()
    {
        float randomTime = UnityEngine.Random.Range(5f, 10f);
        yield return new WaitForSeconds(randomTime);

        //Debug.Log("talkduration over");

        LinksLoop();
    }*/

    public void FoodEaten(FoodData data)
    {
        
        foodTarget = null;
        float newHunger = meltData.GetHunger() + data.GetRestorationAmount();
        if (newHunger > 10.0f)
        {
            newHunger = 10.0f;
        }
        Debug.Log("FOOD EATEN, NEW HUNGER: " + newHunger);

        meltData.SetHunger(newHunger);
        meltData.SaveData();
        mdm.RefreshMenu();
    }

    private void MoveToFoodTarget()
    {
        if (Vector3.Distance(transform.position, foodTarget.gameObject.transform.position) > 0.2)
        {
            startingPos = transform.position;
            float step = meltData.GetSpeed() * 1.4f * Time.deltaTime;
            Vector3 aaa = Vector3.MoveTowards(transform.position, foodTarget.gameObject.transform.position, step);

            Vector3 castFrom = transform.position;
            castFrom.y = castFrom.y + 1;

            moving = (Vector3.Distance(startingPos, aaa) < step);

            transform.position = aaa;

            moving = true;
        }
        else { moving = false;
            foodTarget.Shrink(this);
        }

        if (foodTarget != null)
        {

            Vector3 temp = foodTarget.gameObject.transform.position;
            var dir = foodTarget.gameObject.transform.position - transform.position;
            dir.y = 0; // kill height differences
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
        }
    }

    private void RelocateTargetLocation()
    {
        vector = UnityEngine.Random.insideUnitCircle * movementRadius;
        vector = Quaternion.AngleAxis(-90, Vector3.right) * vector;
        vector.y = 0;
        Vector3 aaa = transform.position + vector;

        //Vector3 aaa = new Vector3(0,0,0);
        int x = 0;

        while (!boundsS.GetBounds().Contains(aaa) && x < 1000)
        {
            //(!boundsS.GetBounds().Contains(aaa))

            vector = UnityEngine.Random.insideUnitCircle * movementRadius;
            vector = Quaternion.AngleAxis(-90, Vector3.right) * vector;
            vector.y = 0;
            aaa = transform.position + vector;

            x++;
        }
        if (x == 1000) Debug.Log("relocation limit exceeded");
            /*Vector3 castFrom = transform.position;
            castFrom.y = castFrom.y + 1;

            if ((Physics.Raycast(castFrom, -Vector3.up, out hit, 10f)))
            {
                Vector3 bbb = new Vector3(aaa.x, transform.position.y - (hit.distance - 1), aaa.z);
            }*/

        
            aaa = boundsS.GetBounds().ClosestPoint(aaa);


        targetLocation = aaa;
    }
    

    private void MoveTowards()
    {
        if (distance > 0.2)
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
        else { moving = false;}
    }

    private void RotateTowards()
    {

        /*Vector3 targetDirection = targetLocation - transform.position;

        float singleStep = rotationSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);

        Debug.DrawRay(transform.position, newDirection, Color.red);
        */
        Vector3 temp = targetLocation;
        if(targetLocation != null)
        {
            var dir = targetLocation - transform.position;
            dir.y = 0; // kill height differences
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
        }
       



        //transform.rotation = Quaternion.LookRotation(newDirection);

    }

    private void InterractScript()
    {
        //Vector3 targetLoc = GetAverageLocation();
        //Vector3 targetLoc = currentInteraction.GetAverageLocation();

        Vector3 targetLoc = currentInteraction.GetPosition(this);

        if (Vector3.Distance(transform.position, targetLoc) > 0.1)
        {
            startingPos = transform.position;
            float step = meltData.GetSpeed() * 1.4f * Time.deltaTime;
            Vector3 aaa = Vector3.MoveTowards(transform.position, targetLoc, step);

            Vector3 castFrom = transform.position;
            castFrom.y = castFrom.y + 1;

            moving = (Vector3.Distance(startingPos, aaa) < step);

            transform.position = aaa;

            moving = true;


            if (targetLoc != null)
            {
                var dir = targetLoc - transform.position;
                dir.y = 0; // kill height differences
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
            }
        }
        else { moving = false;

            if (!talkDelayRunning) {
                StartCoroutine(InterractTriggerDelay());
            }



            targetLoc = currentInteraction.GetAverageLocation();
            Vector3 temp = targetLoc;
            if (targetLoc != null)
            {
                var dir = targetLoc - transform.position;
                dir.y = 0; // kill height differences
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), meltData.GetRotationSpeed() * Time.deltaTime);
            }

        }

        
    }

    private bool talkDelayRunning = false;
    private IEnumerator InterractTriggerDelay()
    {
        if (currentInteraction != null)
        {
            talkDelayRunning = true;
            float randomTime = UnityEngine.Random.Range(currentInteraction.GetData().minActivationDelay, currentInteraction.GetData().maxActivationDelay);
           

            // Code to execute after the random wait time
            //if(GetMode() == 2 || GetMode() == 5)
            //{
            if (animator != null)
            {
                //screms.PlayScream();
                animator.SetTrigger("ActivateInterract");
                //yield return new WaitForSeconds(0.5f);
                //ps.FireParticle();
                //animator.SetBool("jumpUpAndDown", false);
            }
            yield return new WaitForSeconds(randomTime);
            //}
            //Debug.Log("Jump wait time is over!");        
            talkDelayRunning = false;
        }
    }

    public MeltInterractionInstance GetInteraction()
    {
        return currentInteraction;
    }

    public void SetMeltData(MeltData data)
    {
        meltData = data;
        appearanceScript = GetComponent<MeltAppearance>();
        appearanceScript.SetMeltData(data);
        meltInd.SetMeltData(data);
        screms.SetMeltData(data);
        aes.SetData(this);
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
    

    private bool isDragging = false;
    private Vector3 originalPosition;
    private float yOffset;


    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            originalPosition = transform.position;
            yOffset = transform.position.y + 3f;

            if (currentInteraction != null) MeltInterractionController.RemoveFromInterractions(this);

            //if (GetMode() == 2) LinksLoop();
            SetMode(4);
            ms.SetDraggingMelt(this);
            isDragging = true;
            originalPosition = transform.position;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            isDragging = false;
            SetMode(0);
            ms.SetDraggingMelt(null);
            // Return the box to its original Y coordinate
            Vector3 newPosition = transform.position;
            newPosition.y = originalPosition.y;
            transform.position = newPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (isDragging)
            {
                // Create a ray from the camera to the ground plane
                Ray ray = Camera.main.ScreenPointToRay(eventData.position);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, Mathf.Infinity))
                {
                    // Update the box's position while keeping it at the desired Y coordinate
                    Vector3 newPosition = hit.point + Vector3.up * yOffset;
                    transform.position = newPosition;
                }
            }
        }

    }


    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("hovered");
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("unhovered");
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log("clicked");
        thisClicked();
    }
    public void thisClicked()
    {
        sc.MeltClicked(gameObject);
    }

    private void OnDrawGizmos()
    {
        if(targetLocation != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(targetLocation, 0.3f);
        }
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, movementRadius);

    }
}
