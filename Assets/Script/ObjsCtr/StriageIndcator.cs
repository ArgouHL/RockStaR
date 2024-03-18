using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StriageIndcator : MonoBehaviour
{

    [SerializeField] private Material mat;
    private Material _mat;
    private CanvasGroup canvasGroup;
    private void Awake()
    {
        _mat = new Material(mat);
        GetComponent<RawImage>().material = _mat;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    internal void StartRange(float maxRange)
    {
        _mat.SetFloat("_volume", 0);
        GetComponent<RectTransform>().sizeDelta = new Vector2(1, maxRange);
    }

    internal void UpdateRange(float range)
    {
        _mat.SetFloat("_volume", range);
        Debug.Log(range);
    }
}
