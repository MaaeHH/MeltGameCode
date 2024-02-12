using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WallpaperButton : MonoBehaviour
{
    [SerializeField] private Image thisImage;
    [SerializeField] private Text thisText;
    private WallpaperMenuScript wms;
    private int thisInt;
    public void SetData(Sprite x, int y, WallpaperMenuScript z)
    {
        thisImage.sprite = x;
        thisInt = y;
        thisText.text = "Wallpaper " + y + 1;
        wms = z;
    }

    public void ButtonPressed()
    {
        wms.SetWallpaper(thisInt);
    }
}
