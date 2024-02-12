using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlertMenu : MonoBehaviour
{
    [SerializeField] private Text alertTitleText;
    [SerializeField] private Text alertBodyText;

    public void MakeAlert(string x, string y)
    {
        alertTitleText.text = x;
        alertBodyText.text = y;
        gameObject.SetActive(true);
    }

    public void Closed()
    {
        gameObject.SetActive(false);
    }
}
