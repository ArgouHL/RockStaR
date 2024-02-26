using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharaterUICtr : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private CanvasGroup confirmUI;
    [SerializeField] private CanvasGroup uI;

    private void Awake()
    {
        uI.alpha = 0;
        confirmUI.alpha = 0;
    }
    internal void ShowChara()
    {
        uI.alpha = 1;
    }

    internal void ChangeColor(Color c)
    {
        image.color = c;
    }

    internal void ShowConfirm()
    {
        confirmUI.alpha = 1;
        Debug.Log("confirm");
    }
}
