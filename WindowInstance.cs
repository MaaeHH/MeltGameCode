using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WindowInstance
{
    [SerializeField] private string windowDataName;
    [SerializeField] public float height;
    [SerializeField] public float xPos;

    public WindowInstance(WindowData wd)
    {
        SetData(wd);
    }

    public void SetData(WindowData wd)
    {
        windowDataName = wd.name;
    }

    public WindowData GetData()
    {
        return Resources.Load<WindowData>("WindowDatas/" + windowDataName);
    }
}