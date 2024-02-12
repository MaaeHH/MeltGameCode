using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
public class MeltDataMenu : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;
    private MeltData data;
    private MeltScript meltScript;
    //private GameObject gameObj;
    [SerializeField] private GameObject friendshipReqTemplate;
    [SerializeField] private Slider cheerSlider;
    [SerializeField] private Slider hungerSlider;
    [SerializeField] private Slider energySlider;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Text meltName;
    [SerializeField] private GameObject ejectButton;
    [SerializeField] private Transform freqTransform;
    [SerializeField] private LevelStuff ls;
    [SerializeField] private List<GameObject> friendStuff;
    [SerializeField] private List<GameObject> nonFriendStuff;
    [SerializeField] private Text meowmeltRarity;
   

    //private 
    // Start is called before the first frame update
    void Start()
    {
    }
  
    void Update()
    {
    }

    /*public void ClearMenu()
    {
        foreach (Transform thing in transform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }*/

    public void TalkToMeltButtonPressed()
    {
        if (meltScript != null) 
            meltScript.TalkToPlayer();
    }
    
    public void ClearFReqs()
    {
        foreach (Transform thing in freqTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

    public void MakeFReqs()
    {
        ClearFReqs();
        foreach (FriendshipRequirement fr in data.GetFriendshipReqs())
        {
            MakeFReq(fr);
        }
    }

    private void MakeFReq(FriendshipRequirement fr)
    {
        GameObject currentSpawned = Instantiate(friendshipReqTemplate);

        currentSpawned.transform.SetParent(freqTransform);

        currentSpawned.GetComponent<FriendshipReqTemplate>().SetFriendshipReq(fr);

        currentSpawned.SetActive(true);

    }

    public MeltDataMenu GenerateMenu(GameObject go)
    {
        MeltScript ms = go.GetComponent<MeltScript>();
        //_menuAnim.SetBool("onScreen", false);
        SetMeltScript(ms);
        SetSubjectGameObject(go);
        //SetBackGroundColour(md.GetColour());
        //ejectButton.SetActive(ms.GetMode() == 3);
      
        //ClearMenu();
        //makeText(0, ad.GetActorName());

        RefreshMenu();
        return this;
    }

    private void ShowFriendStuff(bool x)
    {
        foreach (GameObject obj in friendStuff)
        {
            obj.SetActive(x);
        }
        foreach (GameObject obj in nonFriendStuff)
        {
            obj.SetActive(!x);
        }
    }

    public override void RefreshMenu()
    {
        
        if (data != null)
        {
            ejectButton.SetActive(meltScript.GetMode() == 3);
            meltName.text = data.GetName();
            if (data.IsFriend())
            {
                ShowFriendStuff(true);
                

                Debug.Log("Refreshing sliders");
                cheerSlider.value = 1 - (data.GetCheer() / 10);
                hungerSlider.value = 1 - (data.GetHunger() / 10);
                energySlider.value = 1 - (data.GetEnergy() / 10);
                healthSlider.value = 1 - (data.GetHealth() / 10);
                
              
                ls.SetData(data);
            }
            else
            {
                ShowFriendStuff(false);

                MakeFReqs();
                meowmeltRarity.text = "This melt is " + data.GetRarityString();
            }
        }
    }

    public void EjectButtonPressed()
    {
        meltScript.GetGridObj().RemoveMelt(meltScript);
        meltScript.SetMode(0);
        RefreshMenu();
    }

    public override void CloseMenu()
    {
        UnSetMeltData();
    }
    private void SetMeltScript(MeltScript x)
    {
        meltScript = x;
        data = x.GetMeltData();
        
        //GetSelectedController().AddMenu(this);
        _menuAnim.SetBool("onScreen", true);
        //_titleText.text = x.GetActorName();
    }
  
  
    public void UnSetMeltData()
    {
        data = null;
        meltScript = null;

        SetSubjectGameObject(null);
       
        //ClearMenu();
        _menuAnim.SetBool("onScreen", false);
    }

    private List<string> GetAvatars()
    {
        //string path = Application.dataPath + "/Resources/Characters;
        // Load all the assets in the "MyFolder" folder within the "Resources" folder
        Object[] assets = Resources.LoadAll("Characters", typeof(GameObject));

        // Create a list to store the names of the assets
        List<string> assetNames = new List<string>();

        // Loop through each asset and add its name to the list
        foreach (Object asset in assets)
        {
            assetNames.Add(asset.name);
        }
        return assetNames;
    }

    private List<string> GetAnims()
    {
        //string path = Application.dataPath + "/Resources/Characters;
        // Load all the assets in the "MyFolder" folder within the "Resources" folder
        Object[] assets = Resources.LoadAll("Controllers");

        // Create a list to store the names of the assets
        List<string> assetNames = new List<string>();

        // Loop through each asset and add its name to the list
        foreach (Object asset in assets)
        {
            assetNames.Add(asset.name);
        }
        return assetNames;
    }


        private void SetBackGroundColour(Color x)
    {
        //_bgImg.color = x;
    }



}
