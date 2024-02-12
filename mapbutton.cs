using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapbutton : MonoBehaviour
{
    [SerializeField] private SceneLoader sl;
    public void MapButtonClicked()
    {
        sl.ChangeLevel(0, "Map");
    }
  
}
