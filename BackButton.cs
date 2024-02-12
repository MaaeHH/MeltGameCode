using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    [SerializeField] private SceneLoader sl;

    public void BackButtonPressed()
    {
        sl.ChangeLevel(0, "MainScene");
    }
}
