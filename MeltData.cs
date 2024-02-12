using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Melt", menuName = "Meltagochi/Meltdata", order = 1)]
public class MeltData : ScriptableObject
{
    [SerializeField] private float restlessness = 4;
    [SerializeField] private float restlessnessConsistency = 5;
    [SerializeField] private float speed = 1;
    [SerializeField] private float rotationSpeed = 1.4f;
    [SerializeField] private string meltName = "New Melt";
    [SerializeField] private string meltDesc = "This is a description of this meowmelt, it usually appears during the day.";
    [SerializeField] private Color startingColour = Color.yellow;
    [SerializeField] private Vector3 startingScale = new Vector3(1,1,1);
    [SerializeField] private string startingTextureName = "";
    private bool friend = false;
    public enum rarity { Common, Uncommon, Rare, UltraRare}
    [SerializeField] private rarity meltRarity;
 
    [SerializeField] private float startingPitch = 1.0f;
    [SerializeField] private float startingVolume = 0.25f;

    [SerializeField] private List<InterractionListing> interractionList = new List<InterractionListing>();


    //[SerializeField] private int meltID;
    private string meltID;

    [SerializeField] private List<FriendshipReqData> frds;

    [SerializeField] private List<string> onBefriend = new List<string> { "You want to be friends?", "Lets be friends!", "we're friends now!"};
    [SerializeField] private List<string> onBefriendAgain = new List<string> { "Hmm, you want to become friends again?", "fiiiine, but look after me this time!" };
    [SerializeField] private List<string> onUnfriend = new List<string> { "You've been mean to me!", "I'm leaving!" };


    [SerializeField]
    private MultiDialogueHolder friendDialogue = new MultiDialogueHolder(
    new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "Hi there, fren!", "hows it going?" }),
        new ListWrapper<string>(new List<string> { "Good day today", "hope you are doing well" }),
        new ListWrapper<string>(new List<string> { "Hope you're as cheerful as me!" })
    });

    [SerializeField]
    private MultiDialogueHolder previousFriendDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "Hi", "..." }),
        new ListWrapper<string>(new List<string> { "Just visiting", "...friend... I mean, human." })
        });

    [SerializeField]
    private MultiDialogueHolder nonFriendDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "Hiya", "you seem nice" }),
        new ListWrapper<string>(new List<string> { "Nice place!", "did buildermelt build it?" })
        });

    [SerializeField]
    private MultiDialogueHolder itemPlacedDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "that new item really ties the place together!", "good buy, friend" }),
        new ListWrapper<string>(new List<string> { "wow, this also ties the place together!", "its double as tied together" })
        });

    [SerializeField]
    private MultiDialogueHolder lowHealthDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "Feeling a bit ill", "*cough cough*" }),
        new ListWrapper<string>(new List<string> { "Might need to visit doctormelt", "Can you take me?" })
        });

    [SerializeField]
    private MultiDialogueHolder lowCheerDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "Not very happy at the moment, friend", ":(" }),
        new ListWrapper<string>(new List<string> { "feeling kind of bored...", "......" })
        });

    [SerializeField]
    private MultiDialogueHolder lowEnergyDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "*yaaawwwwwnnn*", "sorry friend, feeling sleepy" }),
        new ListWrapper<string>(new List<string> { "zzz.zzzz.zzzzzz.zzzzzzz", "wha? what? oh! Sorry, friend, just dozed off" })
        });

    [SerializeField]
    private MultiDialogueHolder lowHungerDialogue = new MultiDialogueHolder(
        new List<ListWrapper<string>> {
        new ListWrapper<string>(new List<string> { "hmm", "i'm hungry" }),
        new ListWrapper<string>(new List<string> { "perhaps maybe do you have any...", "cookies?" })
        });


    [SerializeField] private List<int> sillyness = new List<int> { 1,2,3,4,5,6};
    [SerializeField] private List<int> kindness = new List<int> { 1, 2, 3, 4, 5,6 };
    [SerializeField] private List<int> friendliness = new List<int> { 1, 2, 3, 4, 5,6 };
    [SerializeField] private List<int> cuddliness = new List<int> { 1, 2, 3, 4, 5,6 };

    [SerializeField] private List<int> levelBoundaries = new List<int> {0, 10, 30, 50, 70, 90 };
    private int currentLevel;
    //[SerializeField] private Color currentColour = Color.yellow;
    [SerializeField] private List<float> appearanceTimes = new List<float> { 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f, 0.5f };

    [SerializeField] private List<LevelDialogue> levelDialogues = new List<LevelDialogue>();

    [SerializeField] private List<AccessoryData> startingAccessories = new List<AccessoryData>();

    private MeltSave saveData;
    
    private MeltScript ms;
    private MarketMelt mm;
    private float unfriendTicksLimit = 25;
    public event LoadComplete OnLoadComplete;
    public delegate void LoadComplete();

    public event LevelUp OnLevelUp;
    public delegate void LevelUp();

    public event Unfriend OnUnfriend;
    public delegate void Unfriend();

    public event MakeDialogue OnMakeDialogue;
    public delegate void MakeDialogue(List<string> x);

    public event PointersEvent OnMakePointers;
    public delegate void PointersEvent();

    public List<float> GetAppearanceTimes()
    {
        return appearanceTimes;
    }

    public string GetDescription()
    {
        return meltDesc;
    }

    public bool HasGeneratedFreqs()
    {
        return saveData.frsMade;
    }

    public List<FriendshipRequirement> GetFriendshipReqs()
    {
        if (!HasGeneratedFreqs())
            MakeFriendshipReqs();
        List<FriendshipRequirement> temp = new List<FriendshipRequirement>();

        foreach(LocalFR lfr in saveData.localFrs)
        {
            temp.Add((FriendshipRequirement)lfr);
        }
        foreach(GlobalFR gfr in saveData.globalFrs)
        {
            temp.Add((FriendshipRequirement)gfr);
        }
        return temp;
    }

    public string GetTextureName()
    {
        if (saveData == null) InitSaveData();

        return saveData.textureName;

    }

    public bool LikesObject(InventoryObjectData x)
    {
        bool iLike = false;
        foreach (LocalFR lfr in saveData.localFrs)
        {
            if(lfr.GetData().GetData() == x)
            {
                iLike = true;
            }
        }
        foreach (GlobalFR gfr in saveData.globalFrs)
        {
            if (gfr.GetData().GetData() == x) {
                iLike = true;
            }
        }
        return iLike;
    }

    private float threshold = 3.0f;
    public List<string> GetDialogue(bool LikedObjectPlaced)
    {
        if(IsFriend())
        {
            if(lowHealthDialogue.GetHasLooped() && lowHungerDialogue.GetHasLooped() && lowEnergyDialogue.GetHasLooped() && lowCheerDialogue.GetHasLooped())
            {
                lowHealthDialogue.ResetHasLooped();
                lowHungerDialogue.ResetHasLooped();
                lowEnergyDialogue.ResetHasLooped();
                lowCheerDialogue.ResetHasLooped();
            }

            if (GetHealth() < threshold)
            {
                if(!lowHealthDialogue.GetHasLooped())
                return lowHealthDialogue.GetNextDialogue();
            }
            else
            {
                lowHealthDialogue.ResetHasLooped();
            }


            if (GetHunger() < threshold)
            {
                if (!lowHungerDialogue.GetHasLooped())
                    return lowHungerDialogue.GetNextDialogue();
            }
            else
            {
                lowHungerDialogue.ResetHasLooped();
            }


            if (GetEnergy() < threshold)
            {
                if (!lowEnergyDialogue.GetHasLooped())
                    return lowEnergyDialogue.GetNextDialogue();
            }
            else
            {
                lowEnergyDialogue.ResetHasLooped();
            }


            if (GetCheer() < threshold)
            {
                if (!lowCheerDialogue.GetHasLooped())
                    return lowCheerDialogue.GetNextDialogue();
            }
            else
            {
                lowCheerDialogue.ResetHasLooped();
            }

            if (LikedObjectPlaced)
            {
                return itemPlacedDialogue.GetNextDialogue();
            }

            return friendDialogue.GetNextDialogue();
        }
        else
        {
            if (LikedObjectPlaced)
            {
                return itemPlacedDialogue.GetNextDialogue();
            }


            if (saveData.hasBeenFriends)
            {
                return previousFriendDialogue.GetNextDialogue();
            }
            else
            {
                return nonFriendDialogue.GetNextDialogue();
            }
        }
        return null;
    }

    public List<string> GetDialogueOnBefriend()
    {
        return onBefriend;
    }
    public List<string> GetDialogueOnBefriendAgain()
    {
        return onBefriendAgain;
    }
    public List<string> GetDialogueOnUnfriend()
    {
        return onUnfriend;
    }
    public List<LocalFR> GetLocalFriendshipReqs()
    {
        if(saveData == null)InitSaveData();
        if (!HasGeneratedFreqs())
            MakeFriendshipReqs();

        return saveData.localFrs;
    }

    public List<GlobalFR> GetGlobalFriendshipReqs()
    {
        if (saveData == null) InitSaveData();
        if (!HasGeneratedFreqs())
            MakeFriendshipReqs();

        return saveData.globalFrs;
    }

    public List<InterractionListing> GetInterractionListings()
    {
        return interractionList;
    }

    public bool GetHasBeenFriends()
    {
        return saveData.hasBeenFriends;
    }

    public void BefriendingComplete()
    {
        saveData.hasBeenFriends = true;
        SaveData();
    }
   
    public bool IsFriend()
    {
        if (saveData == null) InitSaveData();
        if (saveData.frsMade == false) return false;

        foreach (FriendshipRequirement fr in saveData.localFrs)
        {
            if (!fr.IsComplete())
            {
                return false;
            }
        }

        foreach (FriendshipRequirement fr in saveData.globalFrs)
        {
            if (!fr.IsComplete())
            {
                return false;
            }
        }
        return true;
    }

    public void AddAccessory(AccessoryData x)
    {
        saveData.accessoriesDataNames.Add(x.name);
        SaveMeltSystem.SaveMelt(meltID, saveData);
    }
   
    public List<AccessoryData> GetAccessories()
    {
        List<AccessoryData> toOutput = new List<AccessoryData>();

        foreach(string name in saveData.accessoriesDataNames)
        {
            toOutput.Add(Resources.Load<AccessoryData>("Accessories/" + name));
        }

        return toOutput;
    }

    public void SaveAccessories(List<AccessoryData> x)
    {
        List<string> toSave = new List<string>();
        foreach(AccessoryData data in x)
        {
            toSave.Add(data.name);
        }
        saveData.accessoriesDataNames = toSave;

        SaveMeltSystem.SaveMelt(meltID, saveData);
    }

    public void InitSaveData()
    {

        //Debug.Log("Initialising " + meltID);
        //if (saveData == null)
        //{
        meltID = this.name;
        MeltSave temp = SaveMeltSystem.LoadMelt(meltID);
        //saveData = 
        if (temp != null)
        {
            //Debug.Log("Loading existing save");
            saveData = temp;
            SetFreqReferences();
            //saveData = SaveMeltSystem.LoadMelt(meltID);
            //Debug.Log("Save loaded for " + meltName);
        }
        else
        {
            //Debug.Log("Making new save");
            saveData = new MeltSave();
            saveData.invObC.MakeNewRecords();


            saveData.lastUpdateTicks = DateTime.Now.Ticks;
            saveData.r = startingColour.r;
            saveData.g = startingColour.g;
            saveData.b = startingColour.b;
            saveData.a = startingColour.a;

            saveData.pitch = startingPitch;
            saveData.volume = startingVolume;
            saveData.textureName = startingTextureName;
            saveData.scale = startingScale;

            saveData.unfriendTicks = unfriendTicksLimit;

            SaveAccessories(startingAccessories);
            //Debug.Log(saveData.r + saveData.g + saveData.b + "");
            //currentColour = new Color(saveData.r,saveData.g,saveData.b,saveData.a);

            SaveMeltSystem.SaveMelt(meltID, saveData);


        }
        //frds = new List<FriendshipReqData>();

        SetCurrentLevel();


        /*if (ms != null && saveData != null)
        {
            ms.InitSaveComplete();
            PrintMelt();
        }

        if (mm != null && saveData != null)
        {
            mm.InitSaveComplete();
            PrintMelt();
        }*/
        unfriended = false;
        PrintMelt();
        OnLoadComplete?.Invoke();
        //}

    }

    public void SetRestoreLastUpdate(bool x)
    {
        saveData.restoreLastUpdate = x;
        SaveData();
    }

    public bool GetRestoreLastUpdate()
    {
        return saveData.restoreLastUpdate;
    }
  
    public void ResetLastUpdate()
    {
        saveData.restoreLastUpdate = false;
        saveData.lastUpdateTicks = DateTime.Now.Ticks;
        OnMakePointers?.Invoke();
        SaveData();
    }

    public rarity GetMeltRarity()
    {
        return meltRarity;
    }

    public int GetRarityInt()
    {
        switch (meltRarity)
        {
            case rarity.Common:
                return 0;
            break;
            case rarity.Uncommon:
                return 1;
            break;
            case rarity.Rare:
                return 2;
            break;
            case rarity.UltraRare:
                return 3;
            break;

        }
        return 0;
        
    }

    public string GetRarityString()
    {
        switch (meltRarity)
        {
            case rarity.Common:
                return "common";
                break;
            case rarity.Uncommon:
                return "uncommon";
                break;
            case rarity.Rare:
                return "rare";
                break;
            case rarity.UltraRare:
                return "ultra rare";
                break;

        }
        return "";

    }

    public void SetCurrentLevel()
    {
        for(int x =0; x<levelBoundaries.Count; x++)
        {
            if(saveData.exp >= levelBoundaries[x])
            {
                currentLevel = x;
            }
            else
            {
                return;
            }
        }
    }

    public float GetExpOnLevel()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        if(currentLevel == 0)
        {
            return saveData.exp;
        }
        else
        {
            return saveData.exp - levelBoundaries[currentLevel];
        }
        
    }

    public float GetExpReqForNextLevel()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();

        //if (currentLevel == 0) return levelBoundaries[1];
        if(!GetIsMaxLevel())
        return levelBoundaries[currentLevel + 1]- levelBoundaries[currentLevel];

        return -1.0f;
    }

    public bool GetIsMaxLevel()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return (currentLevel == levelBoundaries.Count - 1);
      
    }

    public int GetCurrentLevel()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return currentLevel;
    }

    public void AddExp(float x)
    {
        int prevLevel = currentLevel;
        saveData.exp = saveData.exp + x;
        SetCurrentLevel();
        if(currentLevel > prevLevel && currentLevel != 0)
        {
            OnLevelUp?.Invoke();

            CheckLevelDialogue();
        }
        CheckIfTimeToUnfriend();
        SaveMeltSystem.SaveMelt(meltID, saveData);
    }

    private void CheckLevelDialogue()
    {
        foreach (LevelDialogue dialogue in levelDialogues)
        {
            if(dialogue.theLevel == currentLevel)
            {
                OnMakeDialogue?.Invoke(dialogue.theDialogue);
            }
        }
    }

    public int GetCountOf(InventoryObjectData x)
    {
        return saveData.invObC.GetCountOf(x);
    }

    public List<int> GetAllSillyness()
    {
        return sillyness;
    }
    public List<int> GetAllKindness()
    {
        return sillyness;
    }
    public List<int> GetAllFriendliness()
    {
        return sillyness;
    }
    public List<int> GetAllCuddliness()
    {
        return sillyness;
    }

    public int GetSillyness()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return sillyness[currentLevel];
    }

    public int GetKindness()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return kindness[currentLevel];
    }

    public int GetFriendliness()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return friendliness[currentLevel];
    }

    public int GetCuddliness()
    {
        if (saveData == null) InitSaveData();
        SetCurrentLevel();
        return cuddliness[currentLevel];
    }

    public void MakeFriendshipReqs()
    {
        
        saveData.globalFrs = new List<GlobalFR>();
        saveData.localFrs = new List<LocalFR>();
        foreach (FriendshipReqData frd in frds)
        {
            
            if (frd.GetReadFromMelt())
            {
                LocalFR tempreq = new LocalFR(frd);
                tempreq.SetMeltScript(ms);
                saveData.localFrs.Add(tempreq);
            }
            else
            {
                GlobalFR tempreqa = new GlobalFR(frd);
                tempreqa.SetMeltScript(ms);
                saveData.globalFrs.Add(tempreqa);
            }

            
            //SaveMeltSystem.SaveFriendshipReq(temp, tempreq);
            
        }
        saveData.frsMade = true;
        SaveData();
        
    }

    private void SetFreqReferences()
    {
        if (saveData.localFrs != null)
        {
            foreach (FriendshipRequirement fr in saveData.localFrs)
            {
                fr.SetMeltScript(ms);
            }
        }
        if (saveData.globalFrs != null)
        {
            foreach (FriendshipRequirement fr in saveData.globalFrs)
            {
                fr.SetMeltScript(ms);
            }
        }
    }

    private void PrintMelt()
    {
        string x = "";
        x = x + "Melt: " + meltName;
        x = x + " cheer: " + saveData.cheer;
        x = x + " hunger: " + saveData.hunger;
        x = x + " energy: " + saveData.energy;
        x = x + " health: " + saveData.health;
        x = x + " r: " + saveData.r;
        x = x + " g: " + saveData.g;
        x = x + " b: " + saveData.b;
        x = x + " a: " + saveData.a;
        Debug.Log(x);
    }

  
    /*public void IncreaseCount(GridObjectData x)
    {
        if (!saveData.interractedWithObjectCount.ContainsKey(x))
        {
            saveData.interractedWithObjectCount.Add(x, 1);
        }
        else
        {
            saveData.interractedWithObjectCount[x]++;
        }

        if (!saveData.interractedWithCategoryCount.ContainsKey(x.GetGridObjectType()))
        {
            saveData.interractedWithCategoryCount.Add(x.GetGridObjectType(), 1);
        }
        else
        {
            saveData.interractedWithCategoryCount[x.GetGridObjectType()]++;
        }

        SaveMeltSystem.SaveMelt(meltID, saveData);
    }

    public int GetCountOf(InventoryObjectData x)
    {
        if (!saveData.interractedWithObjectCount.ContainsKey(x))
            return 0;

        else
            return saveData.interractedWithObjectCount[x];
    }
    public int GetCountOf(GridObjectData.ObjectType x)
    {
        if (!saveData.interractedWithCategoryCount.ContainsKey(x))
            return 0;

        else
            return saveData.interractedWithCategoryCount[x];
    }*/




    public void SetMeltScript(MeltScript x)
    {
        ms = x;
    }

    public void SetMarketMelt(MarketMelt x)
    {
        mm = x;
    }

    public void SetID(string x)
    {
        meltID = x;
    }
    public string GetID()
    {
        return meltID;
    }
    public void DecreaseHunger(float baseAmount)
    {
        saveData.hunger = saveData.hunger - baseAmount;
        if (saveData.hunger < 0){
            saveData.hunger = 0;
        }
    }
    public void DecreaseCheer(float baseAmount)
    {
        saveData.cheer = saveData.cheer - baseAmount;
        if (saveData.energy < 8)
        {
            saveData.cheer = saveData.cheer - 3*(1 - (saveData.energy / 10));
        }

        if (saveData.cheer < 0)
        {
            saveData.cheer = 0;
        }
    }
    public void DecreaseEnergy(float baseAmount)
    {
        saveData.energy = saveData.energy - baseAmount;
        if (saveData.hunger < 8)
        {
            saveData.energy = saveData.energy - 3 * (1 - (saveData.hunger / 10));
        }

        if (saveData.energy < 0)
        {
            saveData.energy = 0;
        }
    }
    public void DecreaseHealth(float baseAmount)
    {
        saveData.health = saveData.health - baseAmount;
        if (saveData.hunger < 8)
        {
            saveData.health = saveData.health - 3 * (1 - (saveData.hunger / 10));
        }

        if (saveData.health < 0)
        {
            saveData.health = 0;
        }
    }
    public Color GetColour()
    {
        //return Color.yellow;
        //return currentColour;
        return new Color(saveData.r, saveData.g, saveData.b, saveData.a);
    }
    public Vector3 GetScale()
    {
        return saveData.scale;
    }

    public bool GetIsSleeping()
    {
        return saveData.wasSleeping;
    }

    public void SetIsSleeping(bool x)
    {
        saveData.wasSleeping = x;
        SaveData();
    }

    public void IncreaseCheer(float baseAmount)
    {
        saveData.cheer = saveData.cheer + baseAmount;

        if (saveData.cheer > 10)
        {
            saveData.cheer = 10;
        }
        IncreaseUnfriendTicks(baseAmount);
    }

    public void IncreaseEnergy(float baseAmount)
    {
        saveData.energy = saveData.energy + baseAmount;

        if (saveData.energy > 10)
        {
            saveData.energy = 10;
        }
        IncreaseUnfriendTicks(baseAmount);
    }

    public void IncreaseHunger(float baseAmount)
    {
        saveData.hunger = saveData.hunger + baseAmount;

        if (saveData.hunger > 10)
        {
            saveData.hunger = 10;
        }
        IncreaseUnfriendTicks(baseAmount);
    }

    public void IncreaseHealth(float baseAmount)
    {
        saveData.health = saveData.health + baseAmount;

        if (saveData.health > 10)
        {
            saveData.health = 10;
        }
        IncreaseUnfriendTicks(baseAmount);
    }

    public void IncreaseUnfriendTicks(float amount)
    {
        saveData.unfriendTicks = saveData.unfriendTicks + amount;
        if(saveData.unfriendTicks > unfriendTicksLimit)
        {
            saveData.unfriendTicks = unfriendTicksLimit;
        }
        SaveData();
    }

    public void CheckIfTimeToUnfriend()
    {
        Debug.Log("checking if time to unfriend");
        float avgStats = saveData.cheer + saveData.hunger + saveData.energy + saveData.health;
        avgStats = avgStats / 4;

        if (avgStats < 1.3)
        {
            saveData.unfriendTicks--;
        }
        if(saveData.unfriendTicks <= 0)
        {
            UnfriendMelt();
        }
    }

    bool unfriended = false;
    public void UnfriendMelt()
    {
        if (!unfriended)
        {
            saveData.unfriendTicks = unfriendTicksLimit;
            saveData.cheer = 10;
            saveData.hunger = 10;
            saveData.energy = 10;
            saveData.health = 10;
            MakeFriendshipReqs();
            saveData.frsMade = false;
            SetFreqReferences();
            MeltAssigner.UnFriendMelt(this);
            OnUnfriend?.Invoke();
            SaveData();
            unfriended = true;
        }
    }

    public void SaveData()
    {
        SaveMeltSystem.SaveMelt(meltID, saveData); 
    }

    public void SetPitch(float x)
    {
        saveData.pitch = x;
    }
    public void SetVolume(float x)
    {
        saveData.volume = x;
    }
 
    public float GetPitch()
    {
        return saveData.pitch;
    }
    public float GetVolume()
    {
        return saveData.volume;
    }
    public void SetCheer(float x)
    {
        saveData.cheer = x;
    }
    public float GetCheer()
    {
        return saveData.cheer;
    }
    public void SetHunger(float x)
    {
        saveData.hunger = x;
    }
    public float GetHunger()
    {
        return saveData.hunger;
    }
    public float GetEnergy()
    {
        return saveData.energy;
    }
    public void SetEnergy(float x)
    {
        saveData.energy = x;
    }
    public float GetHealth()
    {
        return saveData.health;
    }
    public void SetHealth(float x)
    {
        saveData.health = x;
    }
    public void SetLastUpdate(DateTime x)
    {
        saveData.lastUpdateTicks = x.Ticks;

    }
    public DateTime GetLastUpdate()
    {
        return new DateTime(saveData.lastUpdateTicks);
    }


    public void SetRestlessness(float r)
    {
        restlessness = r;
    }
    public void SetColour(Color c)
    {
        saveData.r = c.r;
        saveData.g = c.g;
        saveData.b = c.b;
        saveData.a = c.a;
    }
    public void SetScale(Vector3 x)
    {
        saveData.scale = x;
    }

    public void SetSpeed(float s)
    {
        speed = s;
    }
    public void SetRestlessnessConsistency(float r)
    {
        restlessnessConsistency = r;
    }
    public void SetRotationSpeed(float r)
    {
        rotationSpeed = r;
    }

    public void SetName(string name) {
        meltName = name;
    }


    public float GetRestlessness()
    {
        return restlessness - (2*saveData.cheer)/100;
    }
    public float GetRestlessnessConsistency()
    {
        return restlessnessConsistency;
    }
    public float GetSpeed()
    {
        return speed - (((2 * saveData.hunger) / 100)+  ((2 * saveData.cheer) / 100))*((10-saveData.energy)/10);
    }
    public float GetRotationSpeed()
    {
        return rotationSpeed - ((2 * saveData.hunger) / 100) * ((10 - saveData.energy) / 10);
    }

    public string GetName()
    {
        return meltName;
    }

    public CustomMeltSave GetCustomMeltSave()
    {
        return new CustomMeltSave(restlessness,
            restlessnessConsistency, speed, rotationSpeed,
            meltName, meltDesc, startingColour, startingScale, startingTextureName,
            startingPitch, startingVolume);
    
    }

    private bool custom = false;
    public void SetDataFromCustomMeltSave(CustomMeltSave x)
    {
        this.custom = true;
        this.friend = true;
        this.restlessness = x.restlessness;
        this.restlessnessConsistency = x.restlessnessConsistency;
        this.speed = x.speed;
        this.rotationSpeed = x.rotationSpeed;
        this.meltName = x.meltName;
        this.meltDesc = x.meltDesc;
        this.startingColour = x.startingColour;
        this.startingScale = x.startingScale;
        this.startingTextureName = x.startingTextureName;
        this.startingPitch = x.startingPitch;
        this.startingVolume = x.startingVolume;
    }

}
