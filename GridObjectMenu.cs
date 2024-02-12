using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
public class GridObjectMenu : Menu
{
    [SerializeField] private Animator _menuAnim;
    [SerializeField] private Image _bgImg;
    private ClickableGridObject data;
    //private MeltScript meltScript;

    //[SerializeField] private SelectedController sc;
    [SerializeField] private Text objectName;
    [SerializeField] private Text occupiedMelts;
    [SerializeField] private GameObject meltListingTemplate;
    [SerializeField] private Transform meltListingsTransform;
    [SerializeField] private GameObject unoccupiedTemplate;
    [SerializeField] private GridGenerator gridGen;
    [SerializeField] private InventorySaveSystem invSave;
    [SerializeField] private TileObjectController toc;
    
    [SerializeField] private SceneLoader sceneLoad;
    //[SerializeField] private SaveSystem saveSys;

    [SerializeField] private Button upgradeButton;
    [SerializeField] private Button openAssociatedMenuButton;
    //private 
    // Start is called before the first frame update

    public override void CloseMenu()
    {
        UnsetData();
    }

    public GridObjectMenu GenerateMenu(GameObject obj)
    {
        SetCGO(obj.GetComponent<ClickableGridObject>());
        SetSubjectGameObject(obj);
        RefreshMenu();
        return this;
    }
    public override void RefreshMenu()
    {
        if(data != null)
        {
            Debug.Log("Refreshing menu");
            openAssociatedMenuButton.interactable = (data.GetObjectData().GetAssocMenuName() != "" && data.GetObjectData().GetAssocMenuName() != null);
            upgradeButton.interactable = (data.GetObjectData().GetUpgradeData() != null);
            objectName.text = data.GetObjectData().GetName() + " Level " +data.GetObjectData().GetLevel();
            occupiedMelts.text = "Occupied melts: " + data.GetNoOfOccupants();
            MakeMeltListings();
        }
    }
    public void MoveButtonPressed()
    {
        if(data.GetOccupiedMelts().Count == 0)
        {
            gridGen.SetItemToMove(data.GetObjectInstance());
        }
       
    }
    public void UpgradeButtonPressed()
    {
        if(SaveSystem.GetScreamcoin() >= data.GetObjectData().GetUpgradeCost())
        {
            SaveSystem.SubtractScreamcoin(data.GetObjectData().GetUpgradeCost());
            data.GetObjectInstance().SetData(data.GetObjectData().GetUpgradeData());
            data.RefreshAppearance();
            RefreshMenu();
        }
        
    }

    public void ReturnToInvPressed()
    {
        invSave.AddToInventory(data.GetObjectData());
        invSave.SaveInventory();
        SaveSystem.PreventCount(data.GetObjectData());
        toc.RemoveObject(data.GetObjectInstance());
        GetSelectedController().EscapePressed();
    }

    private void MakeMeltListings()
    {
        ClearMeltListings();
        int x = data.GetObjectData().GetMaxOccupants();
        foreach (MeltScript melt in data.GetOccupiedMelts())
        {
            MakeMeltListing(melt);
            x--;
        }
        for (int i = x; i >0; i--)
        {
            MakeEmptyMeltListing();
        }
    }

    public void OpenMeltSelector()
    {
        GetSelectedController().MeltSelectorOpened(data.gameObject);
    }
    public void OpenAssociatedMenu()
    {
        Passover.PassedGOD = data.GetObjectData();
        Passover.PassedOccupants = data.GetOccupiedMelts();
        sceneLoad.ChangeLevel(data.GetObjectData().GetTransitionNumber(), data.GetObjectData().GetAssocMenuName());
        //PUT CODE HERE
    }

    private void MakeEmptyMeltListing()
    {
        GameObject currentSpawned = Instantiate(unoccupiedTemplate);



        currentSpawned.transform.SetParent(meltListingsTransform);

        currentSpawned.GetComponent<EmptyMeltListing>().SetGridObjectMenu(this);

        currentSpawned.SetActive(true);

    }

    private void MakeMeltListing(MeltScript melt)
    {
        GameObject currentSpawned = Instantiate(meltListingTemplate);



        currentSpawned.transform.SetParent(meltListingsTransform);
       
        currentSpawned.GetComponent<MeltListing>().SetMeltData(melt);
  
        currentSpawned.SetActive(true);

    }

    public void ClearMeltListings()
    {
        foreach (Transform thing in meltListingsTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }

    private void SetCGO(ClickableGridObject x)
    {
        data = x;
        if (GetSelectedController() == null)
        {
            Debug.Log("asd");
        }
        
        this.GetSelectedController().AddMenu(this);
        _menuAnim.SetBool("onScreen", true);
        //_titleText.text = x.GetActorName();
    }
   
    public void UnsetData()
    {
        data = null;
        SetSubjectGameObject(null);
        //ClearMenu();
        _menuAnim.SetBool("onScreen", false);
    }

    private void SetBackGroundColour(Color x)
    {
        _bgImg.color = x;
    }



}
