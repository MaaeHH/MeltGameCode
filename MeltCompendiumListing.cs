using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeltCompendiumListing : MonoBehaviour
{
    private MeltCompendium meltComp;
    private MeltData thisData;
    [SerializeField] private Text theText;
    [SerializeField] private Text theDescription;
    [SerializeField] private Text rarityText;
    [SerializeField] private MeltAppearance thisAppearance;
    [SerializeField] private MeltAccessoriesController thisAccCont;
    [SerializeField] private RenderTexture sourceRenderTexture;
    [SerializeField] private RawImage targetRawImage;
    [SerializeField] private Camera captureCamera;
    [SerializeField] private GameObject cameraBit;
    public void SetData(MeltData x, MeltCompendium y)
    {
        meltComp = y;

        thisData = x;

        theText.text = thisData.GetName();
        theDescription.text = thisData.GetDescription();
        rarityText.text = thisData.GetRarityString();

        RefreshAppearance();

        // Create a new RenderTexture
        renderTexture = new RenderTexture(width, height, 24);

        // Assign the RenderTexture to the camera
        captureCamera.targetTexture = renderTexture;

        // Render the camera's view into the RenderTexture
        captureCamera.Render();

        // Set the captured image to the RawImage
        SetImageToTargetRawImage();

        cameraBit.SetActive(false);
        if (!thisData.HasGeneratedFreqs())
        {
            targetRawImage.color = new Color(0, 0, 0);
        }
    }

    private void RefreshAppearance()
    {
        thisAppearance.SetMeltData(thisData);
        thisAccCont.SetAccessoriesFromData(thisData, "UICamera");
    }
    public int width = 1000; // Width of the new RenderTexture
    public int height = 1000;
    private RenderTexture renderTexture;

   
    void SetImageToTargetRawImage()
    {
        // Assign the captured Texture2D to the RawImage's texture
        targetRawImage.texture = renderTexture;
    }

}
