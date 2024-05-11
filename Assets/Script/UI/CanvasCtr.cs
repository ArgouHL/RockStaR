using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasCtr : MonoBehaviour
{
    protected CanvasGroup canvas;
    protected bool isShown = true;
    protected float showAlpha=1;
    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    internal void Show(bool b)
    {
        isShown = b;
    }

    internal void SetAlpha(float alpha)
    {
        isShown = true;
        showAlpha = alpha;
    }

}
