using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseMatTemplate : MonoBehaviour
{
    [SerializeField] private DecorateHouseMenu theMenu;
    private UnlockableMaterial myMat;
    [SerializeField] private Image theImage;
    private int ID;
    public void SetData(int x, DecorateHouseMenu y, UnlockableMaterial z)
    {
        ID = x;
        theMenu = y;
        myMat = z;

        //theImage.material =  z.material;
        theImage.sprite = Sprite.Create((Texture2D)z.material.mainTexture, new Rect(0, 0, z.material.mainTexture.width, z.material.mainTexture.height), new Vector2(0.5f, 0.5f));
    }

    public void ThisClicked()
    {
        theMenu.ButtonClicked(ID, myMat);
    }
}
