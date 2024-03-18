using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PullChargeUI : MonoBehaviour
{
    [SerializeField] private Image bar;
    [SerializeField] private Material mat;
    private Material _mat;
    private void Awake()
    {
        _mat = new Material(mat);
        bar.material = _mat;
        _mat.SetFloat("_volume", 0);
    }

    internal void UpdateCharge(float percentage)
    {
        _mat.SetFloat("_volume", percentage);
    }
}
