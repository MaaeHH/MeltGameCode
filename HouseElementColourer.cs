using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseElementColourer : MonoBehaviour
{
    [SerializeField] private List<ListWrapper<Renderer>> elementsToColour;
    // Start is called before the first frame update
    void Start()
    {
        ColourAll();

        SaveSystem.OnHouseColourChanged += ColourElements;
    }

    void OnDestroy()
    {
        SaveSystem.OnHouseColourChanged -= ColourElements;
    }

    public void ColourElements(int x)
    {
        Debug.Log("COLOURING" + x);
        foreach (Renderer rend in elementsToColour[x].list)
        {
            if (rend != null)
            {
                if (x != 2)
                    rend.material = new Material(SaveSystem.GetHouseMaterial(x).material);
                if (SaveSystem.GetHouseColour(x) != null)
                {
                    rend.material.SetColor("_BaseColor", SaveSystem.GetHouseColour(x).colour);
                    rend.material.SetColor("_Color", SaveSystem.GetHouseColour(x).colour);
                }
            }
        }
    }

    public void ColourAll()
    {
        
        if (elementsToColour != null)
        {
            int index = 0;
            foreach (ListWrapper<Renderer> listWrap in elementsToColour)
            {

                foreach (Renderer rend in listWrap.list)
                {
                    if (rend != null)
                    {
                        if (index != 2)
                            rend.material = new Material(SaveSystem.GetHouseMaterial(index).material);

                        if (SaveSystem.GetHouseColour(index) != null)
                        {
                            rend.material.SetColor("_BaseColor", SaveSystem.GetHouseColour(index).colour);
                            rend.material.SetColor("_Color", SaveSystem.GetHouseColour(index).colour);
                        }
                    }
                    //Debug.Log("index: " + index + " colour: " + SaveSystem.GetHouseColour(index));
                }


                index++;
            }

        }
    }
}
