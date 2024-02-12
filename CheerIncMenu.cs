using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CheerIncMenu : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    public abstract void OpenMenu();

    public abstract void CloseMenu();

    public abstract void RefreshMenu();

    public virtual bool SubMenuOpen()
    {
        return false;
    }

    public virtual Transform GetCameraTransform()
    {
        return cameraTransform;
    }
  }
