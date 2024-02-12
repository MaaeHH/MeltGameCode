using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperMenuScript : CheerIncMenu
{
    [SerializeField] private Transform theTransform;
    private List<Sprite> loadedWallpapers;
    [SerializeField] private Image wallpaper;
    [SerializeField] private GameObject wallpaperButtonTemplate;
    public void Start()
    {
        if (loadedWallpapers == null)
            LoadWallpapers();

        wallpaper.sprite = loadedWallpapers[SaveSystem.GetCurrentDesktopWallpaper()];
    }

    public override void CloseMenu()
    {
        gameObject.SetActive(false);
    }

    public override void OpenMenu()
    {
        gameObject.SetActive(true);
        RefreshMenu();
        
    }
    public override void RefreshMenu()
    {
        if (loadedWallpapers == null)
            LoadWallpapers();
        ClearListings();
        MakeListings();
        wallpaper.sprite = loadedWallpapers[SaveSystem.GetCurrentDesktopWallpaper()];
    }

    public void SetWallpaper(int x)
    {
        SaveSystem.SetCurrentDesktopWallpaper(x);
        wallpaper.sprite = loadedWallpapers[SaveSystem.GetCurrentDesktopWallpaper()];
    }

    private void MakeListings()
    {
        int tempInt = 0;
        foreach(Sprite wallpaper in loadedWallpapers)
        {
            
            MakeWallpaperListing(wallpaper, tempInt);
            tempInt++;
        }
    }

    private void MakeWallpaperListing(Sprite wallpaper, int x)
    {
        GameObject currentSpawned = Instantiate(wallpaperButtonTemplate);

        currentSpawned.GetComponent<WallpaperButton>().SetData(wallpaper,x, this);
        currentSpawned.transform.SetParent(theTransform);
        currentSpawned.transform.localScale = (new Vector3(1, 1, 1));
        currentSpawned.SetActive(true);

    }


    private void LoadWallpapers()
    {
        Sprite[] wallpapers = Resources.LoadAll<Sprite>("Wallpapers");
        loadedWallpapers = new List<Sprite>();

        // You can now access and use the found Scriptable Objects
        foreach (Sprite wallpaper in wallpapers)
        {
            //Debug.Log(meltData.GetName());
            loadedWallpapers.Add(wallpaper);

        }
    }

    private void ClearListings()
    {
        foreach (Transform thing in theTransform)
        {
            GameObject.Destroy(thing.gameObject);
        }
    }
}
